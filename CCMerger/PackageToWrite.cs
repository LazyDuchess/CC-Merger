using FSO.Files.Formats.DBPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMerger
{
    public class PackageToWrite
    {
        public List<DBPFFile> packages = new List<DBPFFile>();
        public bool compress = false;
    }
}
