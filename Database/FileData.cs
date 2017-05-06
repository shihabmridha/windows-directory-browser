using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Database
{
    class FileData
    {
        public void AddMyFile(string path){
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("insert into my_files (file_path) values ('" + path + "')");
            con.CloseConnection();
        }

        public static void AddFileVersion(string path) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select * from my_files where file_path='"+ path +"'");
            int FileID = -1;
            while (read.Read()) {
                FileID = (int)read["file_id"];
            }
            if (FileID >= 0) {
                con.ExecuteQueries("insert into version (file_id,file_path) values ('" + FileID + "','" + path + "')");
            }
            con.CloseConnection();
        }

        public static int VersionCount(string path) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select * from my_files where file_path='" + path + "'");
            int counter = 0;
            while (read.Read()) {
                counter++;
            }
            con.CloseConnection();
            read.Close();
            return counter;
        }
        public static List<string> GetMyFilePath() {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            List<string> path = new List<string>();
            SqlDataReader read = con.DataReader("select * from my_files");
            while (read.Read()) {
                path.Add((string)read["file_path"]);
            }
            read.Close();
            con.CloseConnection();
            return path;
        }
       
    }
}
