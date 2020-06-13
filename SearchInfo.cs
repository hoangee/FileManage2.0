using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class SearchInfo
    {
        public string Name { get; set; }
        public bool IsFile { get; set; }

        public SearchInfo(string _name, bool _isFile)
        {
            this.Name = _name;
            this.IsFile = _isFile;
        }
    }
}
