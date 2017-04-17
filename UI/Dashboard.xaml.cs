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

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class Dashboard : MetroWindow
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
                    img.Source = new BitmapImage(new Uri("/Resources/Icons/hdd.png", UriKind.Relative));
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
        
        /**********************************
         * Menu items
         *********************************/
        private void AddToMyFiles(object sender, RoutedEventArgs e) {
            Console.WriteLine("New file added!");
        }
        private void UpdateProfile(object sender, RoutedEventArgs e) {
            Profile next = new Profile();
            next.Show();
            this.Close();
        }
        private void LogOut(object sender, RoutedEventArgs e) {
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
                     
        private void PopulateDataGrid(string path){
            //Version.Visibility = Visibility.Hidden;
            try {
                DataGridController data = new DataGridController();
                ObservableCollection<FileAndDirAttributes> GridData = data.PopulateDataGrid(path);
                DirsAndFiles.ItemsSource = GridData;
                items.Content = (GridData.Count - 1) + " items";
                size.Content = "No file selected.";
            } catch (Exception e) {
                string error = e.ToString();
                MessageBox.Show("The process failed: " + error, "Data loading error.");
            }
        }

        /*****************************
         * Logical drive click event 
         ****************************/
        private void LogicalDriveClick(object sender, MouseButtonEventArgs e) {
            string label = (sender as StackPanel).Tag.ToString();
            PopulateDataGrid(label);
        }

        /***************************
        * My Files populate data
        ***************************/
        private void PopulateMyFiles(object sender, MouseButtonEventArgs e) {
            SidebarFileName.Content = "";
        }

        /*************************
        * Sharing populate data
        *************************/
        private void PopulateSharingFiles(object sender, MouseButtonEventArgs e) {

        }

        

        /******************************
         * Left static folder actions
         *****************************/
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
 
        /*****************************
         * Folder DoubleClick event
         ****************************/
        private void FolderDoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                FileAndDirAttributes file = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
                if (file.Type) {
                    PopulateDataGrid(file.FileDirectory);
                }
            } else {
                Console.WriteLine("Right mouse double click.");
            }            
        }

        /*****************************
         * File Click event
         ****************************/
        private void FileLeftButtonClick(object sender, MouseButtonEventArgs e) {
            FileAndDirAttributes files = (FileAndDirAttributes)DirsAndFiles.SelectedItem;
            if (files != null) {
                if (!files.Type) {
                    if (Sidebar.IsOpen == false) {
                        Sidebar.IsOpen = true;
                    }
                    size.Content = files.FileSize;
                    PopulateSidebarWithFileInfo(files.FileDirectory, files.FileName);
                } else {
                    if (Sidebar.IsOpen == true) {
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
        private void PopulateSidebarWithFileInfo(string path, string name) {
            string FullPath = path + "/" + name;
            try {
                SidebarFilePreview.Source = new BitmapImage(new Uri(FullPath));
                SidebarFileName.Content = name;
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }            
        }


        private void SessionLogView(object sender, RoutedEventArgs e) {
            DashboardMenuController dialog = new DashboardMenuController();
            dialog.SessionLogView();
        }

        private void HelpMenuClick(object sender, RoutedEventArgs e) {
            DashboardMenuController dialog = new DashboardMenuController();
            dialog.HelpDialogView();
        }
        
        ProgressDialogController result;
        private async void RecieveFile(object sender, RoutedEventArgs e) {
            result = await this.ShowProgressAsync("Recieving file", "Please wait...",true);
            result.SetProgress(.7);
            result.Canceled += CancelDialog;
        }

        private async void CancelDialog(object sender, EventArgs e) {
            await result.CloseAsync();
        }
    }
}
