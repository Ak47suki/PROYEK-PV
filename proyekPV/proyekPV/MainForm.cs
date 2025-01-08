using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyekPV
{
    public partial class MainForm : Form
    {

        string user;
        string username;
        public MainForm(string user,string username)
        {
            InitializeComponent();
            this.user = user; this.username = username;
            lblNama.Text = username;

        }

        private void keluarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            this.Hide();
            loginForm.ShowDialog();
            this.Close();
        }

        private void kelolaBarangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();// Sembunyikan form login
            BarangForm barangForm = new BarangForm(user,username);
            barangForm.ShowDialog(); // Buka form barang
            this.Close(); 
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void buatTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            TransaksiForm transaksi = new TransaksiForm();
            transaksi.ShowDialog();
            this.Show();
        }

        private void historyTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            History histo = new History();
            histo.ShowDialog();
            this.Close();
        }
    }
}
