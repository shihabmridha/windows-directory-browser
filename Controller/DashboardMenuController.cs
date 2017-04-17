using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Project.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Controller {
    public class DashboardMenuController : MetroWindow{
        /*
        * Add to My files
        */
        public void SessionLogView() {
            string dialogTitle = "Your session Log";
            string dialogMessage = "User: " + Session.sec.UserName + "\nStatus: Active \nLog in time: " + Session.sec.SessionStartTime;
            MessageBox.Show(dialogMessage, dialogTitle);
        }
        public void HelpDialogView() {
            string dialogTitle = "Help";
            string dialogMessage = "Contact with developer for any bug report or help. \nEmail: shihabmridha@gmail.com";
            MessageBox.Show(dialogMessage, dialogTitle);
        }
    }
}
