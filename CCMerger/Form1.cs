using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FSO.Files.Formats.DBPF;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Configuration;

namespace CCMerger
{
    
    public partial class CCMergerForm : Form
    {
        List<PackageError> errors = new List<PackageError>();
        bool failed = false;
        string trace = "";
        long packageMaxSize = 100000000;
        uint packageMaxFileAmount = 1000;
        Thread thread;
        bool startedWriting = false;
        uint currentFile = 0;
        uint fileAmount = 0;
        string target;
        bool isFinished = false;
        string downloadsDir = "";
        private System.Windows.Forms.Timer timer1;
        PleaseWaitForm waitForm = new PleaseWaitForm();
        Configuration config;

        public CCMergerForm()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            InitializeComponent();
            if (ConfigurationManager.AppSettings.Get("folder") == "")
            {
                var dirTest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EA Games/The Sims™ 2 Ultimate Collection/Downloads");
                if (Directory.Exists(dirTest))
                    downloadsDir = dirTest;

            }
            else
                downloadsDir = ConfigurationManager.AppSettings.Get("folder");
            DownloadsInputBox.Text = downloadsDir;
            long fsiz = packageMaxSize;
            uint famount = packageMaxFileAmount;
            long.TryParse(ConfigurationManager.AppSettings.Get("filesize"),out fsiz);
            uint.TryParse(ConfigurationManager.AppSettings.Get("filecount"), out famount);
            //fsiz = Math.Min(packSize.Maximum, Math.Max(0, fsiz));
            fsiz /= 1000000;
            packSize.Value = Math.Min(packSize.Maximum, Math.Max(0, fsiz));
            packFiles.Value = Math.Min(packFiles.Maximum, Math.Max(0, famount));
        }

        void ThreadedStuff()
        {
            failed = false;
            trace = "";
            errors = new List<PackageError>();
            var pendingPackages = new List<PackageToWrite>();
            var currentPack = new PackageToWrite();
            long currentPackSize = 0;
            uint currentFileAmount = 0;
            pendingPackages.Add(currentPack);
            //var dirInfo = new DirectoryInfo(downloadsDir);
            var packages = new List<DBPFFile>();
            var entryAmount = 0;
            var gotCompression = false;
            fileAmount = 0;
            currentFile = 0;
            downloadsDir = downloadsDir.Replace("/", "\\");
            startedWriting = false;
            var packageCount = 0; //For logging purposes
            foreach (var element in Delimon.Win32.IO.Directory.GetFiles(downloadsDir, "*.package", Delimon.Win32.IO.SearchOption.AllDirectories))
            {
                try
                {
                    var pack = new DBPFFile(element);
                    currentPackSize += new Delimon.Win32.IO.FileInfo(element).Length;
                    currentFileAmount += pack.NumEntries;
                    if ((currentPackSize >= packageMaxSize && packageMaxSize != 0) || (currentFileAmount >= packageMaxFileAmount && packageMaxFileAmount != 0))
                    {
                        currentPack = new PackageToWrite();
                        currentPackSize = 0;
                        currentFileAmount = 0;
                        pendingPackages.Add(currentPack);
                    }
                    fileAmount += pack.NumEntries;
                    if (pack.hasCompression)
                    {
                        gotCompression = true;
                        currentPack.compress = true;
                        fileAmount -= 1;
                    }
                    entryAmount += (int)pack.NumEntries;
                    //packages.Add(pack);
                    currentPack.packages.Add(pack);
                    packageCount += 1;
                }
                catch(Exception e)
                {
                    var err = new PackageError()
                    {
                        file = element,
                        trace = e.ToString()
                    };
                    errors.Add(err);
                    //MessageBox.Show("There was an error reading package " + element + "." + Environment.NewLine + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (fileAmount == 0)
            {
                MessageBox.Show("There are no packages to merge in here, or they're all empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isFinished = true;
                return;
            }
            fileAmount *= 2;
            startedWriting = true;
            byte[] dirfil = null;
            try
            {
                for (var i = 0; i < pendingPackages.Count; i++)
                {
                    var packToWrite = pendingPackages[i];
                    var dirStream = new MemoryStream();
                    var dirWriter = new BinaryWriter(dirStream);
                    foreach (var packa in packToWrite.packages)
                    {
                        foreach (var element in packa.m_EntryByID)
                        {
                            if (element.Value.TypeID != 0xE86B1EEF && element.Value.uncompressedSize != 0)
                            {
                                //fileAmount += 1;
                                //TypeID
                                dirWriter.Write(element.Value.TypeID);
                                //GroupID
                                dirWriter.Write(element.Value.GroupID);
                                //InstanceID
                                dirWriter.Write(element.Value.InstanceID);
                                //ResourceID
                                dirWriter.Write(element.Value.InstanceID2);
                                //UncompressedSize
                                dirWriter.Write(element.Value.uncompressedSize);
                            }
                        }
                    }
                    dirfil = dirStream.ToArray();
                    dirWriter.Dispose();
                    dirStream.Dispose();
                    var fname = Path.Combine(Path.GetDirectoryName(target), Path.GetFileNameWithoutExtension(target) + i.ToString() + ".package");
                    var mStream = new FileStream(fname, FileMode.Create);
                    var mWriter = new BinaryWriter(mStream);
                    //HeeeADER
                    mWriter.Write(new char[] { 'D', 'B', 'P', 'F' });
                    //major version
                    mWriter.Write((int)1);
                    //minor version
                    mWriter.Write((int)2);
                    mWriter.Write(new byte[12]);
                    //Date stuff
                    mWriter.Write((int)0);
                    mWriter.Write((int)0);
                    //Index major
                    mWriter.Write((int)7);
                    //Num entries
                    var entryAmountOffset = mStream.Position;
                    mWriter.Write((int)0);
                    //Index offset
                    var indexOff = mStream.Position;
                    //Placeholder
                    mWriter.Write((int)0);
                    //Index size
                    var indexSize = mStream.Position;
                    //Placeholder
                    mWriter.Write((int)0);

                    //Trash Entry Stuff
                    mWriter.Write((int)0);
                    mWriter.Write((int)0);
                    mWriter.Write((int)0);

                    //Index Minor Ver
                    mWriter.Write((int)2);
                    //Padding
                    mWriter.Write(new byte[32]);

                    var lastPos = mStream.Position;
                    mStream.Position = indexOff;
                    mWriter.Write((int)lastPos);
                    mStream.Position = lastPos;
                    var entryAmount2 = 0;
                    var indeOf = mStream.Position;
                    long dirFilOffset = 0;
                    if (packToWrite.compress)
                    {
                        currentFile += 1;
                        //TypeID
                        mWriter.Write(0xE86B1EEF);
                        //GroupID
                        mWriter.Write(0xE86B1EEF);
                        //InstanceID
                        mWriter.Write(0x286B1F03);
                        //ResourceID
                        mWriter.Write(0x00000000);
                        //File Offset
                        dirFilOffset = mStream.Position;
                        mWriter.Write((int)0);
                        //File Size
                        mWriter.Write(dirfil.Length);
                        entryAmount2 += 1;
                    }
                    foreach (var packa in packToWrite.packages)
                    {
                        foreach (var element in packa.m_EntryByID)
                        {
                            if (element.Value.TypeID != 0xE86B1EEF)
                            {
                                currentFile += 1;
                                //TypeID
                                mWriter.Write(element.Value.TypeID);
                                //GroupID
                                mWriter.Write(element.Value.GroupID);
                                //InstanceID
                                mWriter.Write(element.Value.InstanceID);
                                //ResourceID
                                mWriter.Write(element.Value.InstanceID2);
                                //File Offset
                                element.Value.writeOff = mStream.Position;
                                mWriter.Write((int)0);
                                //File Size
                                mWriter.Write(element.Value.FileSize);
                                entryAmount2 += 1;
                            }
                        }
                    }
                    lastPos = mStream.Position;
                    mStream.Position = entryAmountOffset;
                    mWriter.Write(entryAmount2);
                    var siz = lastPos - indeOf;
                    mStream.Position = indexOff;
                    mWriter.Write(indeOf);
                    mStream.Position = indexSize;
                    mWriter.Write(siz);
                    mStream.Position = lastPos;
                    if (packToWrite.compress)
                    {
                        lastPos = mStream.Position;
                        mStream.Position = dirFilOffset;
                        mWriter.Write((int)lastPos);
                        mStream.Position = lastPos;
                        mWriter.Write(dirfil);
                    }
                    foreach (var packa in packToWrite.packages)
                    {
                        foreach (var element in packa.m_EntryByID)
                        {
                            System.GC.Collect(); // Just in case?
                            if (element.Value.TypeID != 0xE86B1EEF)
                            {
                                currentFile += 1;
                                lastPos = mStream.Position;
                                mStream.Position = element.Value.writeOff;
                                mWriter.Write((int)lastPos);
                                mStream.Position = lastPos;
                                var ent = packa.GetEntry(element.Value);
                                mWriter.Write(ent);
                            }
                        }
                    }
                    mWriter.Dispose();
                    mStream.Dispose();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("There was an error writing the merged packages:" + Environment.NewLine + e.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                failed = true;
                trace = e.ToString();
            }
            var logName = Path.Combine(Path.GetDirectoryName(target), "CCMerger.log");
            using (StreamWriter logStream = new StreamWriter(logName))
            {
                logStream.WriteLine("CCMerger log");
                logStream.WriteLine();
                logStream.WriteLine("[Settings Used]");
                logStream.WriteLine("Max files per package: " + packFiles.Value);
                logStream.WriteLine("Max package size: " + packSize.Value + " mb");
                logStream.WriteLine();
                logStream.WriteLine("[Results]");
                logStream.WriteLine("Merged "+packageCount+" packages containing "+fileAmount+" files into "+pendingPackages.Count+" packages.");
                logStream.WriteLine();
                logStream.WriteLine("[Errors]");
                logStream.WriteLine(errors.Count+" packages failed to merge.");
                foreach(var element in errors)
                {
                    logStream.WriteLine("Package name: " + element.file);
                    logStream.WriteLine(element.trace);
                }
                if (failed)
                {
                    logStream.WriteLine("Failed to merge packages :(");
                    logStream.WriteLine(trace);
                }
                else
                {
                    logStream.WriteLine("Packages were merged succesfully :)");
                }

            }
            isFinished = true;
        }

        

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            DownloadsBrowserDialog.SelectedPath = downloadsDir;
            var browserResult = DownloadsBrowserDialog.ShowDialog();
            if (browserResult == DialogResult.OK && !string.IsNullOrWhiteSpace(DownloadsBrowserDialog.SelectedPath) && Directory.Exists(DownloadsBrowserDialog.SelectedPath))
            {
                downloadsDir = DownloadsBrowserDialog.SelectedPath;
                DownloadsInputBox.Text = downloadsDir;
                config.AppSettings.Settings["folder"].Value = downloadsDir;
                config.Save(ConfigurationSaveMode.Full);
            }
        }
        private void MergeButton_Click(object sender, EventArgs e)
        {
            DoMerge();
        }

        private void DoMerge()
        {
            if (!Directory.Exists(downloadsDir))
            {
                MessageBox.Show("Invalid directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SavePackageDialog.InitialDirectory = downloadsDir;
            var saveDiag = SavePackageDialog.ShowDialog();
            if (saveDiag != DialogResult.OK || string.IsNullOrWhiteSpace(SavePackageDialog.FileName))
                return;
            target = SavePackageDialog.FileName;
            //waitForm.Show();
            //waitForm.Focus();
            BrowseButton.Enabled = false;
            DownloadsInputBox.Enabled = false;
            MergeButton.Enabled = false;
            packSize.Enabled = false;
            packFiles.Enabled = false;
            isFinished = false;
            thread = new Thread(new ThreadStart(ThreadedStuff));
            thread.Start();
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isFinished)
            {
                
                //waitForm.Close();
                this.Focus();
                BrowseButton.Enabled = true;
                DownloadsInputBox.Enabled = true;
                MergeButton.Enabled = true;
                packFiles.Enabled = true;
                packSize.Enabled = true;
                isFinished = false;
                timer1.Stop();
                if (failed)
                    MessageBox.Show("Failed to merge the packages :(. Check the log at CCMerger.log in the output directory for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (errors.Count > 0)
                        MessageBox.Show("There were errors merging " + errors.Count + " packages. Check the log at CCMerger.log in the output directory for more details.", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        MessageBox.Show("Packages merged successfully", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
                progressText.Text = "";
                progressBar.Value = 0;
            }
            else
            {
                if (startedWriting)
                {
                    var prog = Math.Floor((((float)currentFile / (float)fileAmount) * 100.0f));
                    progressText.Text = prog.ToString() + "%";
                    progressBar.Value = (int)prog;
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal, Handle);
                    TaskbarManager.Instance.SetProgressValue((int)prog, 100, Handle);
                }
                else
                {
                    progressBar.Value = 0;
                    progressText.Text = "Preparing...";
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal, Handle);
                    TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
                }
                /*
                if (startedWriting)
                    waitForm.setProgress(Math.Floor((((float)currentFile / (float)fileAmount) * 100.0f)).ToString() + "%");
                else
                    waitForm.setProgress("Reading packages...");*/
            }
        }

        private void SavePackageDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void DownloadsInputBox_TextChanged(object sender, EventArgs e)
        {
            downloadsDir = DownloadsInputBox.Text;
            config.AppSettings.Settings["folder"].Value = downloadsDir;
            config.Save(ConfigurationSaveMode.Full);
        }

        private void CCMergerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null && thread.IsAlive)
                thread.Abort();
            //waitForm.Close();
            
            Application.Exit();
        }

        private void packSize_ValueChanged(object sender, EventArgs e)
        {
            packageMaxSize = (long)packSize.Value * 1000000;
            config.AppSettings.Settings["filesize"].Value = packageMaxSize.ToString();
            config.Save(ConfigurationSaveMode.Full);
        }

        private void packFiles_ValueChanged(object sender, EventArgs e)
        {
            packageMaxFileAmount = (uint)packFiles.Value;
            config.AppSettings.Settings["filecount"].Value = packageMaxFileAmount.ToString();
            config.Save(ConfigurationSaveMode.Full);
        }
    }
    
}
