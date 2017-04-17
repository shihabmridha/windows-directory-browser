using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Database {
    class DatabaseCon {
        string ConnectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=F:\Work\C#\Project\Project\FilerDB.mdf;Integrated Security=True";
        SqlConnection con;

        public void OpenConnection() {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }

        public void CloseConnection() {
            con.Close();
        }


        public void ExecuteQueries(string Query_) {
            SqlCommand cmd = new SqlCommand(Query_, con);
            cmd.ExecuteNonQuery();
        }

        public SqlDataReader DataReader(string Query_) {
            SqlCommand cmd = new SqlCommand(Query_, con);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        public object ShowDataInGridView(string Query_) {
            SqlDataAdapter dr = new SqlDataAdapter(Query_, ConnectionString);
            DataSet ds = new DataSet();
            dr.Fill(ds);
            object dataum = ds.Tables[0];
            return dataum;
        }

    }
}
