using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainService.Models
{
    //public class TrainDBContext:DbContext 
    //{
    //    public TrainDBContext()
    //        : base("DefaultConnection")
    //    {
    //    }

    //    //public DbSet<StudentInfo> Students { get; set; }
    //}

    public class ContextMySQL
    {
        public static MySqlDataReader GetData()
        {
            string connection = "server=10.66.160.191;user id=root;password=Zsh@19870000;database=userdb; pooling=true;";
            MySqlConnection conn = new MySqlConnection(connection);
            string sqlQuery = "SELECT * FROM provider";
            MySqlCommand comm = new MySqlCommand(sqlQuery, conn);
            conn.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            conn.Close();
            return dr;
        }
    }
}