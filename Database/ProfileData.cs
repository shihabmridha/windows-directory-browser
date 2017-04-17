using Project.Controller;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Database {
    class ProfileData {
        public static Users GetUserInformation() {
            
            
            Users usr = new Users();
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            

            SqlDataReader read = con.DataReader("select * from Users where username='" + Session.sec.UserName + "'");
            while (read.Read()) {
                usr.FirstName = (string) read["first_name"];
                usr.LastName = (string) read["last_name"];
                usr.Username = (string) read["username"];
                usr.Email = (string) read["email"];
                usr.ShareCode = (string) read["share_code"];
                Console.WriteLine("Username: " + usr.ShareCode);
            }

            return usr;
        }
    }
}
