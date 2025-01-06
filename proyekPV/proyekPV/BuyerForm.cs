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
    public partial class BuyerForm : Form
    {

        MySqlConnection conn;
        MySqlCommand cmd;
        DataTable dt;
        public BuyerForm()
        {
            Connection.openConn();
            conn = Connection.getConn();

            InitializeComponent();
            LoadBarang();
            dgvBarang.CellFormatting += dgvBarang_CellFormatting;
        }

        private void BuyerForm_Load(object sender, EventArgs e)
        {

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

                // Menyembunyikan kolom barang_id
                if (dgvBarang.Columns["barang_id"] != null)
                {
                    dgvBarang.Columns["barang_id"].Visible = false;
                }
                if (dgvBarang.Columns["jumlah_barang"] != null)
                {
                    dgvBarang.Columns["jumlah_barang"].Visible = false;
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


        private string[] GetSelectedBarang(HashSet<string> excludedIds)
{
    try
    {
        if (dgvBarang.CurrentRow != null)
        {
            // Create an array to store the values
            string[] selectedValues = new string[dgvBarang.Columns.Count];

            // Get values from the selected row
            DataGridViewRow row = dgvBarang.CurrentRow;

            // Assume the ID column is at index 0 (modify based on your DataGridView structure)
            string selectedId = row.Cells[0].Value?.ToString();

            // Check if the ID is in the excluded list
            if (excludedIds != null && excludedIds.Contains(selectedId))
            {
                MessageBox.Show("This item is already in the cart!", "Duplicate Item",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            // Loop through each column and get its value
            for (int i = 0; i < dgvBarang.Columns.Count; i++)
            {
                selectedValues[i] = row.Cells[i].Value?.ToString() ?? "";
            }

            return selectedValues;
        }
        else
        {
            MessageBox.Show("Please select a row first!", "Selection Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error: " + ex.Message, "Get Selected Barang",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
    }
}


        // Example usage - you can call this method when you need the selected values
        // For example, in a button click event:
        //private void buttonGetSelected_Click(object sender, EventArgs e)
        //{
        //    string[] selectedBarang = GetSelectedBarang();
        //    if (selectedBarang != null)
        //    {
        //        // Example: Access specific columns by index
        //        string barangId = selectedBarang[0];  // Assuming barang_id is first column
        //                                              // Use the values as needed
        //    }
        //}
        private void buttonAddCart_Click(object sender, EventArgs e)
        {
            // Example: A HashSet containing IDs of items already in the cart
            HashSet<string> excludedIds = new HashSet<string>(); // This can be dynamically generated if needed

            // Call the GetSelectedBarang function and pass the excluded IDs
            string[] selectedBarang = GetSelectedBarang(excludedIds);

            if (selectedBarang != null)
            {
                // Extract data from the selectedBarang array
                string id = selectedBarang[0];      // Index 0: ID
                string name = selectedBarang[1];    // Index 1: Item name
                string qty = "1";                   // Default quantity
                string price = selectedBarang[3];   // Index 3: Price

                try
                {
                    // Check if the item already exists in the database
                    string queryCheck = "SELECT barang_qty FROM cart WHERE Barang_id = @id AND Barang_nama = @name AND barang_harga = @price";

                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    using (cmd = new MySqlCommand(queryCheck, conn))
                    {
                        // Add parameters to the query
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@price", price);

                        // Execute the query and check if any rows are returned
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            // If the item exists, update the quantity
                            int existingQty = Convert.ToInt32(result);
                            int newQty = existingQty + 1;

                            string queryUpdate = "UPDATE cart SET barang_qty = @newQty WHERE barang_id = @id AND barang_nama = @name AND barang_harga = @price";

                            using (MySqlCommand updateCmd = new MySqlCommand(queryUpdate, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@newQty", newQty);
                                updateCmd.Parameters.AddWithValue("@id", id);
                                updateCmd.Parameters.AddWithValue("@name", name);
                                updateCmd.Parameters.AddWithValue("@price", price);

                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // If the item does not exist, insert it as a new entry
                            string queryInsert = "INSERT INTO cart (barang_id, barang_nama, barang_qty, barang_harga) VALUES (@id, @name, @qty, @price)";

                            using (MySqlCommand insertCmd = new MySqlCommand(queryInsert, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@id", id);
                                insertCmd.Parameters.AddWithValue("@name", name);
                                insertCmd.Parameters.AddWithValue("@qty", qty);
                                insertCmd.Parameters.AddWithValue("@price", price);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Reload the cart data to reflect changes
                    LoadBarang();

                    MessageBox.Show("Barang added to cart successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }


        private void dgvBarang_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the column being formatted is the "barang_harga" column
            if (dgvBarang.Columns[e.ColumnIndex].Name == "harga_barang" && e.Value != null)
            {
                // Format the value with thousand separators
                if (decimal.TryParse(e.Value.ToString(), out decimal harga))
                {
                    e.Value = harga.ToString("N0"); // "N0" formats the number with commas and no decimal places
                    e.FormattingApplied = true;    // Indicate that formatting has been applied
                }
            }
        }


        private void buttonCart_Click(object sender, EventArgs e)
        {

            this.Hide();
            CartForm cartform = new CartForm();
            cartform.ShowDialog();
            this.Show();
        }
        private Dictionary<string, string> GetSelectedBarangByColumnNames()
        {
            try
            {
                if (dgvBarang.CurrentRow != null)
                {
                    Dictionary<string, string> selectedValues = new Dictionary<string, string>();
                    DataGridViewRow row = dgvBarang.CurrentRow;

                    foreach (DataGridViewColumn column in dgvBarang.Columns)
                    {
                        selectedValues[column.Name] = row.Cells[column.Name].Value?.ToString() ?? "";
                    }

                    return selectedValues;
                }
                else
                {
                    MessageBox.Show("Please select a row first!", "Selection Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Get Selected Barang",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Example usage of the dictionary method:
        private void buttonGetSelected_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> selectedBarang = GetSelectedBarangByColumnNames();
            if (selectedBarang != null)
            {
                // Access values by column name
                string barangId = selectedBarang["barang_id"];
                // Use the values as needed
            }
        }


        private void buttonSearch_Click(object sender, EventArgs e)
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
            buttonSearch_Click(sender, e);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            RoleForm roleForm = new RoleForm();
            roleForm.ShowDialog();
            this.Close();
        }

        private void btnSimul_Click(object sender, EventArgs e)
        {
            this.Hide();
            SimulasiForm simulasiForm = new SimulasiForm();
            simulasiForm.ShowDialog();
            this.Show();
        }

    }
}
