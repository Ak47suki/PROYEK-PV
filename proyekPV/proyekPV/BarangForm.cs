using MySql.Data.MySqlClient;
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
    public partial class BarangForm : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        DataTable dt;

        string user;
        string username;

        public BarangForm(string user, string username)
        {
            Connection.openConn();
            conn = Connection.getConn();

            InitializeComponent();
            this.user = user; this.username = username;

            LoadBarang();

        }

        private void LoadBarang()
        {
            try
            {
                // Query untuk mengambil semua data dari tabel barang
                string query = "SELECT * FROM barang";

                // Membuat DataTable untuk menyimpan data
                dt = new DataTable();

                // Menggunakan MySqlCommand untuk menjalankan query
                using (cmd = new MySqlCommand(query, conn))
                {
                    // Membuka koneksi ke database
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    // Menggunakan MySqlDataAdapter untuk mengisi DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }

                // Menampilkan data ke DataGridView
                dgvBarang.DataSource = dt;
            }
            catch (Exception ex)
            {
                // Menampilkan pesan error jika terjadi kesalahan
                MessageBox.Show("Error: " + ex.Message, "Load Barang", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Menutup koneksi jika masih terbuka
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }





        private void btnTambah_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(txtNamaBarang.Text) ||
                    string.IsNullOrWhiteSpace(txtHargaBarang.Text) ||
                    string.IsNullOrWhiteSpace(txtJumlahBarang.Text) ||
                    string.IsNullOrWhiteSpace(txtKategori.Text))
                {
                    MessageBox.Show("Semua field harus diisi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string newBarangID = GenerateBarangID();
                if (newBarangID == null) return;

                string query = "INSERT INTO barang (barang_id, nama_barang, jumlah_barang, harga_barang, kategori_barang) VALUES (@id, @nama, @jumlah, @harga, @kategori)";

                using (cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", newBarangID);
                    cmd.Parameters.AddWithValue("@nama", txtNamaBarang.Text);
                    cmd.Parameters.AddWithValue("@jumlah", int.Parse(txtJumlahBarang.Text));
                    cmd.Parameters.AddWithValue("@harga", int.Parse(txtHargaBarang.Text));
                    cmd.Parameters.AddWithValue("@kategori", txtKategori.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Barang berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadBarang();
                ClearTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Tambah Barang", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validasi jika tidak ada barang yang dipilih
                if (txtNamaBarang.Tag == null)
                {
                    MessageBox.Show("Pilih barang yang akan diupdate dengan double-click pada datagrid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Query untuk update barang
                string query = "UPDATE barang SET nama_barang = @nama, jumlah_barang = @jumlah, harga_barang = @harga, kategori_barang = @kategori WHERE barang_id = @id";

                using (cmd = new MySqlCommand(query, conn))
                {
                    // Menambahkan parameter ke query
                    cmd.Parameters.AddWithValue("@id", txtNamaBarang.Tag.ToString());
                    cmd.Parameters.AddWithValue("@nama", txtNamaBarang.Text);
                    cmd.Parameters.AddWithValue("@jumlah", int.Parse(txtJumlahBarang.Text));
                    cmd.Parameters.AddWithValue("@harga", int.Parse(txtHargaBarang.Text));
                    cmd.Parameters.AddWithValue("@kategori", txtKategori.Text);

                    // Membuka koneksi dan menjalankan query
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Barang berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Refresh DataGridView
                LoadBarang();
                ClearTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update Barang", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Validasi jika tidak ada barang yang dipilih
                if (txtNamaBarang.Tag == null)
                {
                    MessageBox.Show("Pilih barang yang akan dihapus dengan double-click pada datagrid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Konfirmasi penghapusan
                var confirm = MessageBox.Show("Apakah Anda yakin ingin menghapus barang ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                // Query untuk delete barang
                string query = "DELETE FROM barang WHERE barang_id = @id";

                using (cmd = new MySqlCommand(query, conn))
                {
                    // Menambahkan parameter ID barang
                    cmd.Parameters.AddWithValue("@id", txtNamaBarang.Tag.ToString());

                    // Membuka koneksi dan menjalankan query
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Barang berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Mengosongkan textbox
                ClearTextBox();

                // Refresh DataGridView
                LoadBarang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Delete Barang", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim();

                // Jika kata kunci kosong, tampilkan semua barang
                string query = string.IsNullOrWhiteSpace(keyword)
                    ? "SELECT * FROM barang"
                    : "SELECT * FROM barang WHERE nama_barang LIKE @keyword OR kategori_barang LIKE @keyword";

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    dt = new DataTable();
                    adapter.Fill(dt);
                    dgvBarang.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat mencari barang: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e); // Panggil logika pencarian
        }


        private string GenerateBarangID()
        {
            try
            {
                string query = "SELECT barang_id FROM barang ORDER BY barang_id DESC LIMIT 1";
                using (cmd = new MySqlCommand(query, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    var lastID = cmd.ExecuteScalar()?.ToString();
                    if (string.IsNullOrEmpty(lastID))
                    {
                        return "BRG001"; // Jika tidak ada barang, mulai dari BRG001
                    }

                    // Ambil angka dari barang_id terakhir, lalu tambahkan 1
                    int numericPart = int.Parse(lastID.Substring(3)) + 1;
                    return $"BRG{numericPart:D3}"; // Format ke 3 digit, contoh: 003
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Generate Barang ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }


        private void ClearTextBox()
        {
            // Mengosongkan semua textbox
            txtNamaBarang.Clear();
            txtJumlahBarang.Clear();
            txtHargaBarang.Clear();
            txtKategori.Clear();
            txtNamaBarang.Tag = null;
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm(user,username);
            mainForm.ShowDialog();
            this.Close();
        }

        private void dgvBarang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validasi jika tidak ada row yang diklik
            if (e.RowIndex < 0) return;

            // Mengisi textbox berdasarkan data dari row yang dipilih
            txtNamaBarang.Text = dgvBarang.Rows[e.RowIndex].Cells["nama_barang"].Value.ToString();
            txtJumlahBarang.Text = dgvBarang.Rows[e.RowIndex].Cells["jumlah_barang"].Value.ToString();
            txtHargaBarang.Text = dgvBarang.Rows[e.RowIndex].Cells["harga_barang"].Value.ToString();
            txtKategori.Text = dgvBarang.Rows[e.RowIndex].Cells["kategori_barang"].Value.ToString();

            // ID barang hanya untuk referensi saat update/delete (tidak ditampilkan)
            txtNamaBarang.Tag = dgvBarang.Rows[e.RowIndex].Cells["barang_id"].Value.ToString();
        }

        
    }
}
