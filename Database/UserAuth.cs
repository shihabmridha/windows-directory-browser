using Project.Controller;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Database {
    class UserAuth {
        public void AddUserCred(string _Username, string _Password) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("insert into User_Cred values ('" + _Username + "','" + _Password + "')");
            con.CloseConnection();
        }
        public void UpdateUserCred(string _Username, string _Password) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("UPDATE User_Cred SET username='" + _Username + "',passwords='" + _Password + "' where username='"+ Session.sec.UserName +"'");
            con.CloseConnection();
        }
        public void AddUserInfo(string _FirstName, string _LastName, string _Username, string _Email, string _ShareCode) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("insert into Users (first_name,last_name,username,email,share_code) values ('" + _FirstName + "','" + _LastName + "','" + _Username + "','" + _Email + "','" + _ShareCode + "')");
            con.CloseConnection();
        }

        public void UpdateUserInfo(string _FirstName, string _LastName, string _Username, string _Email, string _ShareCode) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("UPDATE Users SET first_name='" + _FirstName + "',last_name='" + _LastName + "',username='" + _Username + "',email='" + _Email + "',share_code='" + _ShareCode + "' where username='" + Session.sec.UserName + "'");
            con.CloseConnection();
        }
        public bool SelectEmail(string email) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader rd = con.DataReader("select * from Users where email='" + email + "'");
            if (rd.HasRows) {
                rd.Close();
                con.CloseConnection();
                return true; 
            }
            return false;
        }
        public bool SelectUsername(string username) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader rd = con.DataReader("select * from User_Cred where username='" + username + "'");
            if (rd.HasRows) {
                rd.Close();
                con.CloseConnection();
                return true;
            }
            return false;
        }
        public bool CheckLoginValidity(string username, string password) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();            
            SqlDataReader rd = con.DataReader("select * from User_Cred where username='"+ username +"' and passwords='"+ password +"'");
            if (rd.Read()) {
                rd.Close();
                con.CloseConnection();
                return true;
            }
            return false;
        }
    }
}
