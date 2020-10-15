using System.Collections;
using System;
using System.Collections.Generic;
using FSO.Files.Formats.DBPF;
using FSO.Files.Utils;
using System.IO;
using SU2.Utils;

namespace SU2.Files.Formats.DIR
{
    /// <summary>
    /// Contains a list of all compressed resources in a DBPF package and their uncompressed sizes.
    /// </summary>
    public class DIRFile
    {
        private IoBuffer reader;

        public DIRFile()
        {
        }

        public static void Read(DBPFFile package, byte[] file)
        {
            var gGroupID = package.groupID;
            var stream = new MemoryStream(file);
            var reader = IoBuffer.FromStream(stream, ByteOrder.LITTLE_ENDIAN);
            while (stream.Position < file.Length)
            {
                var TypeID = reader.ReadUInt32();
                var GroupID = reader.ReadUInt32();
                if (GroupID == 0xFFFFFFFF && package.fname != "")
                    GroupID = gGroupID;
                var InstanceID = reader.ReadUInt32();
                uint  InstanceID2 = 0x00000000;
                if (package.IndexMinorVersion >= 2)
                    InstanceID2 = reader.ReadUInt32();
                var idEntry2 = Hash.TGIRHash(InstanceID, InstanceID2, TypeID, GroupID);
                package.GetEntryByFullID(idEntry2).uncompressedSize = reader.ReadUInt32();
            }
            reader.Dispose();
            stream.Dispose();
        }
    }
}