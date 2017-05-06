using MahApps.Metro.Controls;
using Project.Database;
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
using System.Windows.Shapes;

namespace Project.UI {
    /// <summary>
    /// Interaction logic for NewContactDialog.xaml
    /// </summary>
    public partial class NewContact : MetroWindow {
        public NewContact() {
            InitializeComponent();
            PopulateContactList();
        }

        private void SaveContact(object sender, RoutedEventArgs e) {
            if (ContactName.Text == "" ||  ContactCode.Text == "") {
                cMessage.Content = "All fields are required.";
            }
            else {
                if (!ContactsData.IsExist(ContactName.Text, ContactCode.Text)) {
                    ContactsData.AddNewContact(ContactName.Text, ContactCode.Text);
                    ContactList.ItemsSource = null;
                    PopulateContactList();
                    cMessage.Content = "Contact added successfully.";                    
                }
                else {
                    cMessage.Content = "Contact name or code already exist.";
                }
            }
        }

        private void DeleteContact(object sender, RoutedEventArgs e) {
            ContactsData.DeleteContact(ContactList.SelectedItem.ToString());
            ContactList.ItemsSource = null;
            PopulateContactList();
        }

        private void OpenDashboard(object sender, MouseButtonEventArgs e) {
            Dashboard next = new Dashboard();
            next.Show();
            this.Close();
        }

        private void PopulateContactList() {
            List<string> contacts = new List<string>();
            contacts = ContactsData.GetAllContactList();
            ContactList.ItemsSource = contacts;
            if (contacts.Count <= 0) {
                ContactList.Visibility = Visibility.Hidden;
            }
            else {
                ContactList.Visibility = Visibility.Visible;
            }
        }
    }
}
