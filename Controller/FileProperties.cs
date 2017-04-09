using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Controller {
    class FileProperties {
  
        private string FilePath;

        public FileProperties(string path) {
            this.FilePath = path;
        }

        public string GetFileName() {
            FileInfo file = new FileInfo(FilePath);
            if (file.Exists) {
                return file.Name;                
            } 
            return "Error!";
        }
        public string GetFileSize() {
            FileInfo file = new FileInfo(FilePath);
            long Length = file.Length;
            if (file.Exists) {
                if((Length / 1024)/1024 >= 1024) {
                    return (((Length / 1024) / 1024)/1024).ToString() + " GB";
                } else if((Length/1024) >= 1024) {
                    return ((Length / 1024)/1024).ToString() + " MB";
                }
                else if(Length < 1024) {
                    return Length.ToString() + " bytes";
                }
                
                return (Length/1024).ToString() + " KB";
            }
            return "Error!"; 
        }
        public string GetDirectory() {
            FileInfo file = new FileInfo(FilePath);
            if (file.Exists) {
                return file.DirectoryName;
            }
            return "Error!";
        }
        public string GetFileCreated() {
            FileInfo file = new FileInfo(FilePath);
            if (file.Exists) {
                return file.CreationTime.GetDateTimeFormats('D')[3].ToString();
            }
            return "Error!";
        }

        public string GetFileLastAccess() {
            FileInfo file = new FileInfo(FilePath);
            if (file.Exists) {
                return file.LastAccessTime.GetDateTimeFormats('D')[3].ToString();
            }
            return "Error!";
        }

        public string GetFileLastModify() {
            FileInfo file = new FileInfo(FilePath);
            if (file.Exists) {
                return file.LastWriteTime.GetDateTimeFormats('D')[3].ToString();
            }
            return "Error!";
        }
    }
}
