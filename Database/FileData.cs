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
        public static void AddMyFile(string path){
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            con.ExecuteQueries("insert into my_files (file_path) values ('" + path + "')");
            con.CloseConnection();
        }
        public static string SelectedVersionPath(string path, int index) {
            List<string> dirs = new List<string>();
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select * from my_files where file_path='" + path + "'");
            int FileID = -1;
            while (read.Read()) {
                FileID = (int)read["file_id"];
            }
            read.Close();

            SqlDataReader readx = con.DataReader("select file_path from version where file_id="+FileID+"");
            while (readx.Read()) {
                dirs.Add((string)readx["file_path"]);
            }
            readx.Close();
            return dirs[index];
        }
        public static void AddFileVersion(string parent, string child) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select * from my_files where file_path='"+ parent +"'");
            int FileID = -1;
            while (read.Read()) {
                FileID = (int)read["file_id"];
            }
            read.Close();
            if (FileID >= 0) {
                con.ExecuteQueries("insert into version (file_id,file_path) values (" + FileID + ",'" + child + "')");
            }
            con.CloseConnection();
        }

        public static int VersionCount(string path) {
            DatabaseCon con = new DatabaseCon();
            con.OpenConnection();
            SqlDataReader read = con.DataReader("select * from version where file_id=(select file_id from my_files where file_path='" + path + "')");
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
