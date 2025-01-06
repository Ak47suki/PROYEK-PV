using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace proyekPV
{
    public partial class LoginForm : Form
    {

        MySqlConnection conn;
        MySqlCommand cmd;
        DataTable dt;

        public LoginForm()
        {

            Connection.openConn();
            conn = Connection.getConn();
            InitializeComponent();

            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from user";
            conn.Open();
            cmd.ExecuteReader();
            conn.Close();

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;


            if (username != "" && password != "")
            {
                bool ada = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["username"].ToString() == username)
                    {
                        ada = true;
                        if (dt.Rows[i]["password"].ToString() == password)
                        {
                            if (dt.Rows[i]["authority"].ToString() == "admin")
                            {
                                // Jika login berhasil
                                this.Hide(); // Sembunyikan form login
                                MainForm mainForm = new MainForm(dt.Rows[i]["user_id"].ToString(),dt.Rows[i]["username"].ToString());
                                mainForm.ShowDialog();
                                this.Close();



                                //TransService f = new TransService(dt.Rows[i]["us_id"].ToString());


                            }
                            else
                            {
                                // Jika login berhasil
                                this.Hide(); // Sembunyikan form login
                                MainForm mainForm = new MainForm(dt.Rows[i]["user_id"].ToString(), dt.Rows[i]["username"].ToString());
                                mainForm.ShowDialog();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Password salah!");
                        }
                    }
                }
                if (!ada)
                {
                    MessageBox.Show("Username tidak terdaftar!");
                }
            }
            else
            {
                MessageBox.Show("Field ada yang masih kosong!");
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            RoleForm roleForm = new RoleForm();
            this.Hide();
            roleForm.ShowDialog();
            this.Close();
        }
    }
}
