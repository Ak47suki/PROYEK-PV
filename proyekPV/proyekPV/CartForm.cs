using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace proyekPV
{
    public partial class CartForm : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        DataTable dt;
        public CartForm()
        {
            Connection.openConn();
            conn = Connection.getConn();

            InitializeComponent();
            LoadBarang();
            UpdateTotalHarga();
            dgvCart.CellFormatting += dgvCart_CellFormatting;

        }
        private void LoadBarang()
        {
            try
            {
                // Query untuk mengambil semua data dari tabel barang
                string query = "SELECT * FROM cart";

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
                dgvCart.DataSource = dt;

                // Menyembunyikan kolom barang_id
                if (dgvCart.Columns["barang_id"] != null)
                {
                    dgvCart.Columns["barang_id"].Visible = false;
                }
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
        private void RemoveOrReduceSelectedRow()
        {
            try
            {
                // Check if a row is selected in the DataGridView
                if (dgvCart.CurrentRow == null)
                {
                    MessageBox.Show("Please select a row to remove!", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected row's barang_id and current barang_qty
                string selectedId = dgvCart.CurrentRow.Cells["barang_id"].Value.ToString();
                int currentQty = Convert.ToInt32(dgvCart.CurrentRow.Cells["barang_qty"].Value);

                // Query to either reduce quantity or delete the row
                string query = currentQty > 1
                    ? "UPDATE cart SET barang_qty = barang_qty - 1 WHERE barang_id = @barang_id"
                    : "DELETE FROM cart WHERE barang_id = @barang_id";

                // Execute the query
                using (cmd = new MySqlCommand(query, conn))
                {
                    // Add parameter to prevent SQL injection
                    cmd.Parameters.AddWithValue("@barang_id", selectedId);

                    // Open the connection if it's not already open
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // Display a message indicating what happened
                    if (currentQty > 1)
                    {
                        MessageBox.Show("Quantity reduced by 1.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Item removed from the cart.", "Remove Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Reload the data in the DataGridView
                LoadBarang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Remove or Reduce Row", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Close the connection if it's still open
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RemoveOrReduceSelectedRow();
        }
        private void dgvCart_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the column being formatted is the "barang_harga" column
            if (dgvCart.Columns[e.ColumnIndex].Name == "barang_harga" && e.Value != null)
            {
                // Format the value with thousand separators
                if (decimal.TryParse(e.Value.ToString(), out decimal harga))
                {
                    e.Value = harga.ToString("N0"); // "N0" formats the number with commas and no decimal places
                    e.FormattingApplied = true;    // Indicate that formatting has been applied
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void UpdateTotalHarga()
        {
            try
            {
                // Query to calculate the total price from the database
                string query = "SELECT SUM(barang_harga * barang_qty) AS total_harga FROM cart";

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (cmd = new MySqlCommand(query, conn))
                {
                    // Execute the query and get the total
                    object result = cmd.ExecuteScalar();
                    decimal totalHarga = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                    // Update the label with the total formatted as currency
                    label1.Text = $"Total: {totalHarga:N0}"; // N0 formats with commas for thousands
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update Total Harga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Close the connection
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            BuyerForm buyerForm = new BuyerForm();
            buyerForm.ShowDialog();
            this.Close();
        }
    }
}
