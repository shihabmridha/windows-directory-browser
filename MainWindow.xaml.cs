using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using System.IO;
using System.Collections.ObjectModel;
using Project.Controller;
using System.Windows.Media.Animation;
using System.Data.SQLite;

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : MetroWindow
    {

        private void AddDrives() {
            StackPanel stack;
            Image img;
            Label label;
            try {
                string[] drives = Directory.GetLogicalDrives();
                foreach (string str in drives) {
                    stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;
                    stack.Style = (Style)FindResource("LeftMenuHover");
                    stack.Tag = str;
                    stack.MouseLeftButtonDown += LogicalDriveClick;
                    label = new Label();
                    img = new Image();
                    img.Width = 20;
                    img.Source = new BitmapImage(new Uri("Resources/Icons/hdd.png", UriKind.Relative));
                    label.Content = str;
                    stack.Children.Add(img);
                    stack.Children.Add(label);
                    StaticFolder.Children.Add(stack);
                }
            } catch (IOException) {
                Console.WriteLine("An I/O error occurs.");
            } catch (System.Security.SecurityException) {
                Console.WriteLine("The caller does not have the required permission.");
            }
        }
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

        private void PopulateDataGrid(string path) {
            try {
                Version.Visibility = Visibility.Hidden;
                FileProperties file;
                ObservableCollection<FileAndDirAttributes> FileList = new ObservableCollection<FileAndDirAttributes>();
                FileList.Add(new FileAndDirAttributes { Type = true, FileName = "Up", FileIcon = "Resources/Icons/back.png", FileDirectory = GetParent(path) });

                // Get directories from the path
                string[] dirs = Directory.GetDirectories(@path, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs) {
                    FileList.Add(new FileAndDirAttributes { Type = true, FileIcon = "Resources/Icons/folder.png", FileName = Path.GetFileName(dir), FileDirectory = dir });
                }

                // Get files from the path
                string[] files = Directory.GetFiles(@path, "*", SearchOption.TopDirectoryOnly);
                foreach (string f in files) {
                    file = new FileProperties(f);
                    FileList.Add(new FileAndDirAttributes { Type = false, FileIcon = "Resources/Icons/file.png", FileName = file.GetFileName(), FileSize = file.GetFileSize(), FileDirectory = file.GetDirectory(), FileCreated = file.GetFileCreated(), FileAccessed = file.GetFileLastAccess(), FileModified = file.GetFileLastModify() });
                }
                DirsAndFiles.ItemsSource = FileList;
                items.Content = (FileList.Count - 1) + " items";
                size.Content = "No file selected.";
            } catch (Exception e) {
                string error = e.ToString();
                MessageBox.Show("The process failed: " + error, "Data loading error.");
            }
        }

        


        /*
         * Application starts here
         */
        //SQLiteConnection m_dbConnection;
        public MainWindow()
        {            
            //SQLiteConnection.CreateFile("Main.sqlite");
            //m_dbConnection =new SQLiteConnection("Data Source=Main.sqlite;Version=3;");
            //m_dbConnection.Open();
            
            InitializeComponent();
            AddDrives();
            PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }
                     
        

        /*
         * Logical drive click event 
         */
        private void LogicalDriveClick(object sender, MouseButtonEventArgs e) {
            string label = (sender as StackPanel).Tag.ToString();
            PopulateDataGrid(label);
        }

        /*
        * My Files populate data
        */
        private void PopulateMyFiles(object sender, MouseButtonEventArgs e) {
            SidebarFileName.Content = "";
        }

        /*
        * Sharing populate data
        */
        private void PopulateSharingFiles(object sender, MouseButtonEventArgs e) {

        }

        /*
        * Add to My files
        */
        private void AddToMyFiles(object sender, RoutedEventArgs e) {
            Console.WriteLine("New file added!");
        }

        /*
         * Left static folder actions
         */
        private void StaticFolderClick(object sender, MouseButtonEventArgs e) {
            string tag = (sender as StackPanel).Tag.ToString();
            switch (tag) {
                case "Desktop":
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                    break;
                case "Document":
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                    break;
                case "Music":
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
                    break;
                case "Videos":
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
                    break;
                case "Downloads":
                    string Downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    PopulateDataGrid(Downloads + "\\Downloads");
                    break;
                default:
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
                    break;
            }
        }
 
        /*
         * Folder DoubleClick event
         */
        private void FolderDoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                FileAndDirAttributes file = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
                if (file.Type) {
                    PopulateDataGrid(file.FileDirectory);
                }
            } else {
                Console.WriteLine("Right mouse click.");
            }            
        }


        private void FileLeftButtonClick(object sender, MouseButtonEventArgs e) {
            FileAndDirAttributes files = (FileAndDirAttributes)DirsAndFiles.SelectedItem;            
            if (!files.Type) {
                if (!Sidebar.Margin.Right.Equals(0)) {
                    Storyboard sb = Resources["SidebarSlideShow"] as Storyboard;
                    sb.Begin(Sidebar);
                }
                size.Content = files.FileSize;
                PopulateSidebarWithFileInfo(files.FileDirectory,files.FileName);              
            } else {
                if (Sidebar.Margin.Right.Equals(0)) {
                    Storyboard sb = Resources["SidebarSlideHide"] as Storyboard;
                    sb.Begin(Sidebar);
                }
                size.Content = "No file selected";
            }
        }

        /*
         * Populate sidebar data
         */
        private void PopulateSidebarWithFileInfo(string path, string name) {
            string FullPath = path + "/" + name;
            try {
                SidebarFilePreview.Source = new BitmapImage(new Uri(FullPath));
                SidebarFileName.Content = name;
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }            
        }

        /*
        * Sidebar Close
        */
        private void SidebarClose(object sender, MouseButtonEventArgs e) {
            Storyboard sb = Resources["SidebarSlideHide"] as Storyboard;
            sb.Begin(Sidebar);
        }


    }
}
