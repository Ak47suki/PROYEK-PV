using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyekPV
{
    internal class Connection
    {
        public static MySqlConnection conn;
        public static string server;
        public static string uid;
        public static string pass;
        public static string database;
        public static bool isValid;

        public static void openConn()
        {
            server = "localhost";
            uid = "root";
            pass = "";
            database = "proyekPV";

            conn = new MySqlConnection($"server = {server}; uid = {uid}; database = {database};");

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                isValid = true;
                //System.Windows.Forms.MessageBox.Show("Berhasil Connect DB");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Windows.Forms.MessageBox.Show("Gagal Connect DB");
                conn.Close();

            }
        }

        public static MySqlConnection getConn()
        {
            return conn;
        }

    }
}
