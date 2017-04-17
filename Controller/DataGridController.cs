using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Controller {
    class DataGridController {
        public string GetParent(string path) {
            try {
                if (path.Length > 3) {
                    DirectoryInfo directoryInfo = Directory.GetParent(path);
                    return directoryInfo.FullName;
                } else {
                    return path;
                }
            } catch (ArgumentNullException) {
                return "Error: Path is a null reference.";
            } catch (ArgumentException) {
                return "Error: Path is an empty string, contains only white spaces, or contains invalid characters.";
            }
        }
        public ObservableCollection<FileAndDirAttributes> PopulateDataGrid(string path) {
            FileProperties file;
            ObservableCollection<FileAndDirAttributes> FileList = new ObservableCollection<FileAndDirAttributes>();
            FileList.Add(new FileAndDirAttributes { Type = true, FileName = "Up", FileIcon = "/Resources/Icons/back.png", FileDirectory = GetParent(path) });

            // Get directories from the path
            string[] dirs = Directory.GetDirectories(@path, "*", SearchOption.TopDirectoryOnly);
            foreach (string dir in dirs) {
                FileList.Add(new FileAndDirAttributes { Type = true, FileIcon = "/Resources/Icons/folder.png", FileName = Path.GetFileName(dir), FileDirectory = dir });
            }

            // Get files from the path
            string[] files = Directory.GetFiles(@path, "*", SearchOption.TopDirectoryOnly);
            foreach (string f in files) {
                file = new FileProperties(f);
                FileList.Add(new FileAndDirAttributes { Type = false, FileIcon = "/Resources/Icons/file.png", FileName = file.GetFileName(), FileSize = file.GetFileSize(), FileDirectory = file.GetDirectory(), FileCreated = file.GetFileCreated(), FileAccessed = file.GetFileLastAccess(), FileModified = file.GetFileLastModify() });
            }

            return FileList;
        }

    }
}
