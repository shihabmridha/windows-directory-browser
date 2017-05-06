using System;
using System.IO;
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
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Project.Controller;
using Project.UI;
using Project.Database;

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class Dashboard : MetroWindow
    {
        private bool InMyFiles = false;
        private void AddDrives()
        {
            StackPanel stack;
            Image img;
            Label label;
            try
            {
                string[] drives = Directory.GetLogicalDrives();
                foreach (string str in drives)
                {
                    stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;
                    stack.Style = (Style)FindResource("LeftMenuHover");
                    stack.Tag = str;
                    stack.MouseLeftButtonDown += LogicalDriveClick;
                    label = new Label();
                    img = new Image();
                    img.Width = 20;
                    img.Source = new BitmapImage(new Uri("/Resources/Icons/hdd.png", UriKind.Relative));
                    label.Content = str;
                    stack.Children.Add(img);
                    stack.Children.Add(label);
                    StaticFolder.Children.Add(stack);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("An I/O error occurs.");
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("The caller does not have the required permission.");
            }
        }

        /**********************************
         * Menu items
         *********************************/
        private void AddToMyFiles(object sender, RoutedEventArgs e)
        {
            string path = "";
            System.Windows.Forms.OpenFileDialog file = new System.Windows.Forms.OpenFileDialog();
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = file.FileName;
                FileData.AddMyFile(path);
                FileData.AddFileVersion(path,path);
                MessageBox.Show("Confirmation.", "File added successfully.");
            }
            
        }
        private void AddNewContact(object sender, RoutedEventArgs e) {
            NewContact next = new NewContact();
            next.Show();
            this.Hide();
        }
        private void UpdateProfile(object sender, RoutedEventArgs e)
        {
            Profile next = new Profile();
            next.Show();
            this.Close();
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            Session sec = new Session();
            sec.EndSession();
            Login next = new Login();
            next.Show();
            this.Close();
        }

        /*******************************
         * Application starts here
         ******************************/
        public Dashboard()
        {
            InitializeComponent();
            AddDrives();
            PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        private void PopulateDataGrid(string path)
        {
            try
            {
                DataGridController data = new DataGridController();
                ObservableCollection<FileAndDirAttributes> GridData = data.PopulateDataGrid(path);
                DirsAndFiles.ItemsSource = GridData;
                items.Content = (GridData.Count - 1) + " items";
                size.Content = "No file selected.";
            }
            catch (Exception e)
            {
                string error = e.ToString();
                MessageBox.Show("The process failed: " + error, "Data loading error.");
            }
        }

        /*****************************
         * Logical drive click event 
         ****************************/
        private void LogicalDriveClick(object sender, MouseButtonEventArgs e)
        {
            string label = (sender as StackPanel).Tag.ToString();
            InMyFiles = false;
            VersionCol.Visibility = Visibility.Hidden;
            SidebarReviseButton.Visibility = Visibility.Hidden;
            PopulateDataGrid(label);
        }

        /***************************
        * My Files populate data
        ***************************/
        private void PopulateMyFiles(object sender, MouseButtonEventArgs e)
        {
            VersionCol.Visibility = Visibility.Visible;
            InMyFiles = true;
            SidebarReviseButton.Visibility = Visibility.Visible;
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            List<string> paths = FileData.GetMyFilePath();

            DataGridController data = new DataGridController();
            ObservableCollection<FileAndDirAttributes> GridData = new ObservableCollection<FileAndDirAttributes>();
            FileProperties file;
            if (paths.Count > 0) {
                foreach (string dir in paths) {                 
                    file = new FileProperties(dir);                    
                    GridData.Add(new FileAndDirAttributes { Type = false, FileIcon = "/Resources/Icons/file.png", FileName = file.GetFileName(), FileSize = file.GetFileSize(), FileDirectory = file.GetDirectory(), FileCreated = file.GetFileCreated(), FileAccessed = file.GetFileLastAccess(), FileModified = file.GetFileLastModify(), Versions = FileData.VersionCount(dir) });
                }

                DirsAndFiles.ItemsSource = GridData;
                items.Content = (GridData.Count - 1) + " items";
                size.Content = "No file selected.";
            }
            else {
                MessageBox.Show("Error","No file found!");
            }
           
        }

        /*************************
        * Sharing populate data
        *************************/
        private void PopulateSharingFiles(object sender, MouseButtonEventArgs e)
        {
        
        }

        /******************************
         * Left static folder actions
         *****************************/
        private void StaticFolderClick(object sender, MouseButtonEventArgs e)
        {
            string tag = (sender as StackPanel).Tag.ToString();
            switch (tag)
            {
                case "Desktop":
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                    InMyFiles = false;
                    VersionCol.Visibility = Visibility.Hidden;
                    SidebarReviseButton.Visibility = Visibility.Hidden;
                    break;
                case "Document":
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                    InMyFiles = false;
                    VersionCol.Visibility = Visibility.Hidden;
                    SidebarReviseButton.Visibility = Visibility.Hidden;
                    break;
                case "Music":
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
                    InMyFiles = false;
                    VersionCol.Visibility = Visibility.Hidden;
                    SidebarReviseButton.Visibility = Visibility.Hidden;
                    break;
                case "Videos":
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
                    InMyFiles = false;
                    VersionCol.Visibility = Visibility.Hidden;
                    SidebarReviseButton.Visibility = Visibility.Hidden;
                    break;
                case "Downloads":
                    string Downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    InMyFiles = false;
                    VersionCol.Visibility = Visibility.Hidden;
                    SidebarReviseButton.Visibility = Visibility.Hidden;
                    PopulateDataGrid(Downloads + "\\Downloads");
                    break;
                default:
                    PopulateDataGrid(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
                    InMyFiles = false;
                    VersionCol.Visibility = Visibility.Hidden;
                    SidebarReviseButton.Visibility = Visibility.Hidden;
                    break;
            }
        }

        /*****************************
         * Folder DoubleClick event
         ****************************/
        private void FolderDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FileAndDirAttributes file = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
                if (file.Type)
                {
                    PopulateDataGrid(file.FileDirectory);
                }
            }
            else
            {
                Console.WriteLine("Right mouse double click.");
            }
        }

        /*****************************
         * File Click event
         ****************************/
        private void FileLeftButtonClick(object sender, MouseButtonEventArgs e)
        {
            FileAndDirAttributes files = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
            if (files != null)
            {
                if (!files.Type)
                {
                    if (Sidebar.IsOpen == false)
                    {
                        Sidebar.IsOpen = true;
                    }
                    size.Content = files.FileSize;
                    PopulateSidebarWithFileInfo(files);
                }
                else
                {
                    if (Sidebar.IsOpen == true)
                    {
                        SidebarFilePreview.Source = new BitmapImage(new Uri(@"/Resources/placeholder.png", UriKind.Relative));
                        Sidebar.IsOpen = false;
                    }
                    size.Content = "No file selected";
                }
            }
        }

        /******************************
         * Populate sidebar data
         *****************************/
        private void PopulateSidebarWithFileInfo(FileAndDirAttributes file)
        {
            string FullPath = file.FileDirectory+ "/" + file.FileName;
            FileAndDirAttributes files = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
            try
            {
                if (InMyFiles == true) {
                    VersionsList.Visibility = Visibility.Visible;
                    int counter = files.Versions;
                    List<string> item = new List<string>();
                    for (int i = 1; i <= counter; i++) {
                        item.Add("Version: " + i);
                    }
                    VersionsList.ItemsSource = item;
                }
                else {
                    VersionsList.Visibility = Visibility.Hidden;
                }          
                SidebarFileName.Content = "Name: " + file.FileName;
                SidebarFileSize.Content = "Size: " + file.FileSize;
                SidebarFileModify.Content = "Last modify: " + file.FileModified;
                SidebarFilePreview.Source = new BitmapImage(new Uri(FullPath));
            }
            catch (Exception e)
            {
                SidebarFilePreview.Source = new BitmapImage(new Uri(@"/Resources/placeholder.png", UriKind.Relative));
                Console.WriteLine(e.Message);
            }
        }       

        private void SessionLogView(object sender, RoutedEventArgs e)
        {
            DashboardMenuController dialog = new DashboardMenuController();
            dialog.SessionLogView();
        }

        private void HelpMenuClick(object sender, RoutedEventArgs e)
        {
            DashboardMenuController dialog = new DashboardMenuController();
            dialog.HelpDialogView();
        }

        ProgressDialogController result;
        private async void ReceiveFile(object sender, RoutedEventArgs e)
        {
            result = await this.ShowProgressAsync("Receiving file", "Please wait...", true);
            FileReceiver receive = new FileReceiver(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\", result);
            result.SetProgress(.7);
            result.Canceled += CancelDialog;
        }

        private async void CancelDialog(object sender, EventArgs e)
        {
            await result.CloseAsync();
        }

        private void SendFile(object sender, RoutedEventArgs e)
        {
            FileAndDirAttributes files = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
            FileSender send = new FileSender();
            Partners pr = new Partners();
            string code = "";
            code = ContactsData.GetShareCode(ContactName.Text);
            string ip = "";
            if (code != "") {
                ip = ShareCode.GetIPFromCode(code);                 
            }
            if (ip != "" && pr.IsContactAvailable(ip)) {
                if (InMyFiles == true) {
                    string path = FileData.SelectedVersionPath(files.FileDirectory + "\\" + files.FileName, VersionsList.SelectedIndex);
                    DataGridController dgc = new DataGridController();
                    send.SendFile(path, ip);
                    System.Diagnostics.Process.Start(@path);
                }
                else {
                    send.SendFile(files.FileDirectory + "\\" + files.FileName, "127.0.0.1");
                }
            }
            else {
                MessageBox.Show("Error","Couldn't connect with partner");
            }
            
            
        }

        private void ReviseFile(object sender, RoutedEventArgs e)
        {
            FileAndDirAttributes files = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
            string path = files.FileDirectory+"\\"+files.FileName;
            System.Windows.Forms.OpenFileDialog file = new System.Windows.Forms.OpenFileDialog();
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileData.AddFileVersion(path,file.FileName);
                MessageBox.Show("Confirmation.", "Version added successfully.");
            }            
        }

        private void OpenInExplorer(object sender, RoutedEventArgs e) {
            FileAndDirAttributes files = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
            if (InMyFiles == true) {
                string path = FileData.SelectedVersionPath(files.FileDirectory+"\\"+files.FileName,VersionsList.SelectedIndex);
                DataGridController dgc = new DataGridController();                
                System.Diagnostics.Process.Start(@dgc.GetParent(path));
            }
            else {              
                System.Diagnostics.Process.Start(@files.FileDirectory);
            }
            
        }

    }
    
}