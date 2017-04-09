using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.Controller {
    class FileAndDirAttributes {
        public bool Type { get; set; }
        public string FileIcon { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileDirectory { get; set; }
        public string FileCreated { get; set; }
        public string FileAccessed { get; set; }
        public string FileModified { get; set; }        
        public List<string> Versions { get; set; }
    }
}
