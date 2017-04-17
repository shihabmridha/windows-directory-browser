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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Project.Database;
using Project.Controller;
namespace Project.UI {
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : MetroWindow {
        public Registration() {          
            InitializeComponent();
            rLocalIP.Text = ShareCode.LocalIPAddress();
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e) {
            bool Inputs = true, ValidIP = true;
            string[] fields = new string[] { rFirstName.Text, rLastName.Text, rUsername.Text, rEmail.Text, rPassword.Password, rLocalIP.Text };
            for (int i = 0; i < 6; i++) {
                if (fields[i] == "") {
                    Inputs = false;
                    break;
                }
            }

            if (!ShareCode.ValidateIP(fields[5])) {
                ValidIP = false;
            }

            if (Inputs && rAgree.IsChecked == true && ValidIP) {
                UsersCred cred = new UsersCred(fields[2], fields[4]);
                Users usr = new Users(fields[0], fields[1], fields[2], fields[3], ShareCode.GenerateShareCode(rLocalIP.Text));
                bool Username = cred.CheckIsUsernameExist(), Email = usr.CheckIsEmailExist(usr.Email);
                if (!Username && !Email) {
                    cred.AddCred();                    
                    usr.AddUser();
                    this.Hide();
                    Login log = new Login();
                    log.Show();
                } else {                    
                    if (Email) {
                        if (Username) {
                            rMessage.Content = "Email and Username already exist.";
                        } else {
                            rMessage.Content = "Email already exist.";                            
                        }
                    } else {
                        rMessage.Content = "Username already exist.";
                    }
                }   
            } else {               
                if (rAgree.IsChecked == true) {
                    if (!ValidIP) {
                        rMessage.Content = "Re-check the IPv4 address.";
                    } else {
                        rMessage.Content = "All fields are required.";
                    }
                } else {
                    rMessage.Content = "All fields are required and Agree with condition.";
                }
            }

        }

        private void OpenLoginWindow(object sender, MouseButtonEventArgs e) {            
            Login next = new Login();
            next.Show();
            this.Hide();
        }



    }
}
