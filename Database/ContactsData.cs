using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Database {
    class ContactsData {

        public static void AddNewContact(string name, string code) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("insert into Contacts (contact_name,contact_code) values ('"+ name +"', '"+ code +"')");
            con.CloseConnection();
        }
        public static void DeleteContact(string name) { 
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("delete from Contacts where contact_name='"+ name +"'");
            con.CloseConnection();
        }
        public static List<string> GetAllContactList() {
            List<string> contacts = new List<string>();
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select * from Contacts");
            while (read.Read()) {
                contacts.Add((string) read["contact_name"]);
            }
            read.Close();
            con.CloseConnection();
            return contacts;
        }
        public static string GetShareCode(string name) {
            string code = "";
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select contact_code from Contacts where contact_name='"+name+"'");
            while (read.Read()) {
                code = (string)read["contact_code"];
            }
            read.Close();
            con.CloseConnection();
            return code;
        }
        public static bool IsExist(string name, string code) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select * from Contacts where contact_name='"+name+"' or contact_code='"+code+"'");
            if (read.Read()) {
                read.Close();
                con.CloseConnection();
                return true;
            }
            read.Close();
            con.CloseConnection();
            return false;
        }
    }
}
