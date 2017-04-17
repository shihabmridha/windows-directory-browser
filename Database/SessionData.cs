using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Database {
    class SessionData {
        public static void CreateSession(string username, string time, int active) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("insert into Session values('"+ username +"','"+ time +"',1)");
            con.CloseConnection();
        }
        public static string CurrentSession() {
            string username = null;
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select username from Session where active = 1");
            if (read.Read()) {
                username = (string)read["username"];
                Console.WriteLine(username);
            }
            read.Close();
            con.CloseConnection();

            return username;
        }
        public static void UpdateSession() {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("UPDATE Session SET active = 1");
            con.CloseConnection();
        }
        public static void EndSession() {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("UPDATE Session SET active = 0");            
            con.CloseConnection();
        }

        public static bool HasSession() {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select * from Session");
            if (read.Read()) {
                read.Close();
                con.CloseConnection();
                return true;
            }
            return false;
        }
    }
}
