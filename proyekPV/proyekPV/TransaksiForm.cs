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
            //LoadBarang();
            UpdateTotalHarga();
            dgvCart.CellFormatting += dgvCart_CellFormatting;
            LoadDtransToComboBox();
        }
        private void cmbtrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtrans.SelectedItem != null)
            {
                // Get the selected dtrans_id from the combo box
                string selectedDtransId = cmbtrans.SelectedItem.ToString();
                dgvCart.CellFormatting += dgvCart_CellFormatting;
                // Load the data for the selected dtrans_id
                LoadBarang(selectedDtransId);
            }
        }

        private void LoadBarang(string selectedDtransId)
        {
            try
            {
                // Ensure a valid dtrans_id is provided
                if (string.IsNullOrEmpty(selectedDtransId))
                {
                    MessageBox.Show("Please select a valid dtrans ID.", "Load Barang Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Query to retrieve data from the 'dtrans' table for the selected dtrans_id
                string query = @"
            SELECT 
                dtrans.barang_id, 
                barang.nama_barang, 
                barang.harga_barang, 
                dtrans.barang_qty 
            FROM 
                dtrans 
            INNER JOIN 
                barang 
            ON 
                dtrans.barang_id = barang.barang_id
            WHERE 
                dtrans.dtrans_id = @dtrans_id";

                // Create a DataTable to store the data
                dt = new DataTable();

                using (cmd = new MySqlCommand(query, conn))
                {
                    // Add parameter for the selected dtrans_id to prevent SQL injection
                    cmd.Parameters.AddWithValue("@dtrans_id", selectedDtransId);

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

                // Optional: Adjust visibility of specific columns
                if (dgvCart.Columns["barang_id"] != null)
                {
                    dgvCart.Columns["barang_id"].Visible = false; // Hide 'barang_id' column
                }

                // Add or refresh the "No" column for row numbers
                AddRowNumberColumn();

                // Adjust the DataGridView size to match its content
                AdjustDataGridViewSize();
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                MessageBox.Show("Error: " + ex.Message, "Load Barang Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void RemoveOrReduceSelectedRowFromDtrans()
        {
            try
            {
                // Ensure a dtrans_id is selected in the ComboBox
                if (cmbtrans.SelectedItem == null)
                {
                    MessageBox.Show("Please select a valid dtrans ID from the combo box!", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ensure a row is selected in the DataGridView
                if (dgvCart.CurrentRow == null)
                {
                    MessageBox.Show("Please select a row to remove!", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected dtrans_id from the ComboBox
                string selectedDtransId = cmbtrans.SelectedItem.ToString();

                // Get the selected row's barang_id from the DataGridView
                string selectedBarangId = dgvCart.CurrentRow.Cells["barang_id"].Value.ToString();

                // Get the current barang_qty from the DataGridView
                int currentQty = Convert.ToInt32(dgvCart.CurrentRow.Cells["barang_qty"].Value);

                // Query to either reduce quantity or delete the row
                string query = currentQty > 1
                    ? "UPDATE dtrans SET barang_qty = barang_qty - 1 WHERE dtrans_id = @dtrans_id AND barang_id = @barang_id"
                    : "DELETE FROM dtrans WHERE dtrans_id = @dtrans_id AND barang_id = @barang_id";

                // Execute the query
                using (cmd = new MySqlCommand(query, conn))
                {
                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@dtrans_id", selectedDtransId);
                    cmd.Parameters.AddWithValue("@barang_id", selectedBarangId);

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
                        MessageBox.Show("Item removed from the transaction.", "Remove Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Reload the data in the DataGridView
                LoadDtrans();
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

        private void LoadDtrans()
        {
            try
            {
                string query = "SELECT * FROM dtrans";

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }

                dgvCart.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Load Dtrans", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RemoveOrReduceSelectedRowFromDtrans();
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
        private void LoadDtransToComboBox()
        {
            try
            {
                // Query to get distinct dtrans_id from the dtrans table where status is 1
                string query = "SELECT DISTINCT dtrans_id FROM dtrans WHERE status = 1";

                // Open the connection if it's not open
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                // Clear the combo box before loading data
                cmbtrans.Items.Clear();

                // Use MySqlCommand to execute the query
                using (cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Read each dtrans_id and add it to the combo box
                        while (reader.Read())
                        {
                            string dtransId = reader["dtrans_id"].ToString();
                            cmbtrans.Items.Add(dtransId);
                        }
                    }
                }

                // Optionally, set the first item as the selected one
                if (cmbtrans.Items.Count > 0)
                {
                    cmbtrans.SelectedIndex = 0; // Select the first item by default
                }
            }
            catch (Exception ex)
            {
                // Show error message if an exception occurs
                MessageBox.Show("Error: " + ex.Message, "Load dtrans IDs", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the connection is closed
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
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
        private void UpdateTotalkembalian()
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
                    string texto = label3.Text;
                    int res2 = int.Parse(texto) - Convert.ToInt32(result) ;
                    decimal totalHarga = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                    // Update the label with the total formatted as currency
                    label2.Text = $"Total: {res2:N0}"; // N0 formats with commas for thousands
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update Total Hargaxx", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void bayar_Click(object sender, EventArgs e)
        {
            UpdateSelectedDtransStatusToZero();
        }

        private void UpdateSelectedDtransStatusToZero()
        {
            try
            {
                // Ensure a dtrans_id is selected from the combo box
                if (cmbtrans.SelectedItem == null)
                {
                    MessageBox.Show("Please select a valid dtrans ID.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected dtrans_id from the combo box
                string selectedDtransId = cmbtrans.SelectedItem.ToString();

                // SQL query to update the status column to 0 for the selected dtrans_id
                string updateStatusQuery = "UPDATE dtrans SET status = 0 WHERE dtrans_id = @dtrans_id";

                // Open the connection if it's not open
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                // Use a transaction to ensure all queries are executed as a single atomic operation
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Step 1: Update the status in dtrans
                        using (cmd = new MySqlCommand(updateStatusQuery, conn, transaction))
                        {
                            // Add the parameter to prevent SQL injection
                            cmd.Parameters.AddWithValue("@dtrans_id", selectedDtransId);

                            // Execute the update command
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Step 2: Retrieve barang_id and barang_qty from the cart
                                string selectCartQuery = "SELECT barang_id, barang_qty FROM cart";

                                using (cmd = new MySqlCommand(selectCartQuery, conn, transaction))
                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    // Create a list to store updates
                                    var updates = new List<(string barangId, int barangQty)>();

                                    while (reader.Read())
                                    {
                                        string barangId = reader["barang_id"].ToString();
                                        int barangQty = Convert.ToInt32(reader["barang_qty"]);
                                        updates.Add((barangId, barangQty));
                                    }

                                    reader.Close();

                                    // Step 3: Update jumlah_barang in the barang table
                                    string updateBarangQuery = "UPDATE barang SET jumlah_barang = jumlah_barang - @barang_qty WHERE barang_id = @barang_id";

                                    foreach (var (barangId, barangQty) in updates)
                                    {
                                        using (MySqlCommand updateCmd = new MySqlCommand(updateBarangQuery, conn, transaction))
                                        {
                                            updateCmd.Parameters.AddWithValue("@barang_id", barangId);
                                            updateCmd.Parameters.AddWithValue("@barang_qty", barangQty);
                                            updateCmd.ExecuteNonQuery();
                                        }
                                    }
                                }

                                // Commit the transaction
                                transaction.Commit();

                                MessageBox.Show($"Status for dtrans_id '{selectedDtransId}' successfully updated to 0 and stock updated.",
                                    "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Reload the combo box to reflect the updated data
                                LoadDtransToComboBox();
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show($"No records found for dtrans_id '{selectedDtransId}'.",
                                    "Update Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Show an error message if an exception occurs
                MessageBox.Show("Error: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the connection is closed
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the pressed key is Enter
            if (e.KeyCode == Keys.Enter)
            {
                
                try
                {
                    label3.Text = txtSearch.Text;
                    UpdateTotalkembalian();
                    // Parse the text as a number and format it with commas
                    if (int.TryParse(txtSearch.Text, out int number))
                    {
                        label3.Text = "bayar: " + number.ToString("N0"); // Format with commas
                    }
                    else
                    {
                        // If input is not a valid number, show the original text
                        label3.Text = txtSearch.Text;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Formatting Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                // Suppress the beep sound on pressing Enter
                e.SuppressKeyPress = true;
            }
        }
        

    }
}
