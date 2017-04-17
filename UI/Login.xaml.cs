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
using Project.Controller;
namespace Project.UI {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow {
        public Login() {
            InitializeComponent();
            Session.sec = new Session();
            Session.sec.UserName = Session.sec.GetSession();
            if (Session.sec.GetSession() != null) {
                Dashboard next = new Dashboard();
                next.Show();
                this.Close();
            }
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e) {
            if (lUsername.Text != "" && lPassword.Password != "") {
                UsersCred cred = new UsersCred(lUsername.Text, lPassword.Password);
                if (cred.CheckLoginValidity()) {                 
                    Session.sec = new Session();
                    Session.sec.UserName = lUsername.Text;
                    if (Session.sec.SessionExist()) {
                        Session.sec.UpdateSession();
                    } else {
                        Session.sec.StartSession();
                    }                    
                    Dashboard next = new Dashboard();                    
                    next.Show();
                    this.Close();
                } else {
                    lMessage.Content = "Invalid username or password.";
                }
            } else {
                lMessage.Content = "All fields are required.";
            }
        }

        private void OpenRegisterWindow(object sender, MouseButtonEventArgs e) {           
            Registration next = new Registration();
            next.Show();
            this.Close();
        }

    }
}
