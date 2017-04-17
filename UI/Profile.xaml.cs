using MahApps.Metro.Controls;
using Project.Controller;
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

namespace Project.UI {
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : MetroWindow {
        public Profile() {
            InitializeComponent();
            ProfileController pro = new ProfileController();
            Users usr = pro.GetProfileInfo();
            pFirstName.Text = usr.FirstName;
            pLastName.Text = usr.LastName;
            pUsername.Text = usr.Username;
            pEmail.Text = usr.Email;
            pLocalIP.Text = ShareCode.GetIPFromCode(usr.ShareCode);
            pShareCode.Content = "Code: " + usr.ShareCode;
        }

        private void ReGenerateCode(object sender, RoutedEventArgs e) {
            string localIP = pLocalIP.Text;
            if (ShareCode.ValidateIP(localIP)) {
                pShareCode.Content = "Code: " + ShareCode.GenerateShareCode(localIP);
            }
        }

        private void UpdateProfile(object sender, RoutedEventArgs e) {
            bool Inputs = true, ValidIP = true;
            string[] fields = new string[] { pFirstName.Text, pLastName.Text, pUsername.Text, pEmail.Text, pPassword.Password, pLocalIP.Text };
            for (int i = 0; i < 6; i++) {
                if (fields[i] == "") {
                    Inputs = false;
                    break;
                }
            }

            if (!ShareCode.ValidateIP(fields[5])) {
                ValidIP = false;
            }

            if (Inputs && ValidIP) {
                UsersCred cred = new UsersCred(fields[2], fields[4]);
                Users usr = new Users(fields[0], fields[1], fields[2], fields[3], ShareCode.GenerateShareCode(pLocalIP.Text));                
                cred.UpdateCred();
                usr.UpdateUser();
                Session.sec.UserName = fields[2];
                Dashboard next = new Dashboard();
                next.Show();
                this.Close();                             
            } else {                
                if (!ValidIP) {
                    pMessage.Content = "Re-check the IPv4 address.";
                } else {
                    pMessage.Content = "All fields are required.";
                }                
            }
        }

        private void BackToDashboard(object sender, MouseButtonEventArgs e) {
            Dashboard next = new Dashboard();
            next.Show();
            this.Close();
        }
    }
}
