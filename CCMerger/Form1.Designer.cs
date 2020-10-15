namespace CCMerger
{
    partial class CCMergerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SavePackageDialog = new System.Windows.Forms.SaveFileDialog();
            this.DownloadsInputBox = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.packSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.packFiles = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressText = new System.Windows.Forms.Label();
            this.MergeButton = new System.Windows.Forms.Button();
            this.DownloadsBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.packSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.packFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // SavePackageDialog
            // 
            this.SavePackageDialog.DefaultExt = "package";
            this.SavePackageDialog.FileName = "mergedcc.package";
            this.SavePackageDialog.Filter = "Package Files | *.package";
            this.SavePackageDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.SavePackageDialog_FileOk);
            // 
            // DownloadsInputBox
            // 
            this.DownloadsInputBox.Location = new System.Drawing.Point(188, 3);
            this.DownloadsInputBox.Name = "DownloadsInputBox";
            this.DownloadsInputBox.Size = new System.Drawing.Size(195, 22);
            this.DownloadsInputBox.TabIndex = 0;
            this.DownloadsInputBox.TextChanged += new System.EventHandler(this.DownloadsInputBox_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.DownloadsInputBox);
            this.flowLayoutPanel1.Controls.Add(this.BrowseButton);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.packSize);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.packFiles);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.progressBar);
            this.flowLayoutPanel1.Controls.Add(this.progressText);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(474, 180);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Directory with CC to merge:";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(389, 3);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 3;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Max package size (MB)";
            // 
            // packSize
            // 
            this.packSize.Location = new System.Drawing.Point(163, 32);
            this.packSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.packSize.Name = "packSize";
            this.packSize.Size = new System.Drawing.Size(169, 22);
            this.packSize.TabIndex = 6;
            this.packSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.packSize.ValueChanged += new System.EventHandler(this.packSize_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Max files per package";
            // 
            // packFiles
            // 
            this.packFiles.Location = new System.Drawing.Point(154, 60);
            this.packFiles.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.packFiles.Name = "packFiles";
            this.packFiles.Size = new System.Drawing.Size(169, 22);
            this.packFiles.TabIndex = 8;
            this.packFiles.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.packFiles.ValueChanged += new System.EventHandler(this.packFiles_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(440, 34);
            this.label4.TabIndex = 11;
            this.label4.Text = "(If your game crashes with higher values, use the default ones or try smaller.)";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(3, 122);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(464, 32);
            this.progressBar.TabIndex = 9;
            // 
            // progressText
            // 
            this.progressText.AutoSize = true;
            this.progressText.Location = new System.Drawing.Point(3, 157);
            this.progressText.Name = "progressText";
            this.progressText.Size = new System.Drawing.Size(0, 17);
            this.progressText.TabIndex = 10;
            this.progressText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MergeButton
            // 
            this.MergeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MergeButton.Location = new System.Drawing.Point(492, 146);
            this.MergeButton.Name = "MergeButton";
            this.MergeButton.Size = new System.Drawing.Size(75, 46);
            this.MergeButton.TabIndex = 4;
            this.MergeButton.Text = "Merge";
            this.MergeButton.UseVisualStyleBackColor = true;
            this.MergeButton.Click += new System.EventHandler(this.MergeButton_Click);
            // 
            // CCMergerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 204);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.MergeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CCMergerForm";
            this.Text = "Custom Content Merger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CCMergerForm_FormClosing);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.packSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.packFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog SavePackageDialog;
        private System.Windows.Forms.TextBox DownloadsInputBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.FolderBrowserDialog DownloadsBrowserDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown packSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown packFiles;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label progressText;
        private System.Windows.Forms.Label label4;
    }
}

