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
    public partial class TransaksiForm : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        DataTable dt;
        public TransaksiForm()
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
                // Query to retrieve all data from the 'cart' table
                string query = "SELECT * FROM cart";

                // Create a DataTable to store the data
                dt = new DataTable();

                using (cmd = new MySqlCommand(query, conn))
                {
                    // Open the connection to the database if it's not already open
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    // Use MySqlDataAdapter to fill the DataTable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }

                // Display data in the DataGridView
                dgvCart.DataSource = dt;

                // Hide the 'barang_id' column
                if (dgvCart.Columns["barang_id"] != null)
                {
                    dgvCart.Columns["barang_id"].Visible = false;
                }

                // Add or refresh the "No" column for row numbers
                AddRowNumberColumn();

                // Adjust the DataGridView size to match its content
                AdjustDataGridViewSize();
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                MessageBox.Show("Error: " + ex.Message, "Load Barang", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void AddRowNumberColumn()
        {
            // Check if the "No" column already exists
            if (!dgvCart.Columns.Contains("No"))
            {
                // Create a new column for row numbers
                DataGridViewTextBoxColumn rowNumberColumn = new DataGridViewTextBoxColumn
                {
                    Name = "No",
                    HeaderText = "No",
                    ReadOnly = true,
                    Width = 50 // Set column width (optional)
                };

                // Insert the column at the first position (index 0)
                dgvCart.Columns.Insert(0, rowNumberColumn);
            }

            // Populate the "No" column with row numbers
            for (int i = 0; i < dgvCart.Rows.Count; i++)
            {
                dgvCart.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }


        private void AdjustDataGridViewSize()
        {
            // Minimum height for the DataGridView
            const int minHeight = 100;

            // Calculate the total height required for the rows
            int totalRowHeight = dgvCart.Rows.Count * dgvCart.RowTemplate.Height;

            // Add the height of the column header
            int totalHeight = totalRowHeight + dgvCart.ColumnHeadersHeight;

            // Ensure the height doesn't exceed the form or container height
            int maxHeight = this.ClientSize.Height - dgvCart.Location.Y - 10; // 10 for padding
            dgvCart.Height = Math.Min(Math.Max(totalHeight, minHeight), maxHeight);

            // Adjust the width to fill the parent container
            dgvCart.Width = this.ClientSize.Width - dgvCart.Location.X - 10; // 10 for padding
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
                        UpdateTotalHarga();
                    }
                    else
                    {
                        MessageBox.Show("Item removed from the cart.", "Remove Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UpdateTotalHarga();
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
