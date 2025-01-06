using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace proyekPV
{
    public partial class SimulasiForm : Form
    {
        private MySqlConnection conn;
        private MySqlCommand cmd;

        public SimulasiForm()
        {
            Connection.openConn();
            conn = Connection.getConn();
            InitializeComponent();
            LoadBrand();
            LoadDataForComboBoxes();
        }

        private void LoadBrand()
        {
            cmbBrand.Items.Add("INTEL");
            cmbBrand.Items.Add("AMD");
        }

        private void LoadDataForComboBoxes()
        {
            LoadComboBoxData("RAM", cmbRAM);
            LoadComboBoxData("Casing", cmbCasing);
            LoadComboBoxData("SSD", cmbSSD);
            LoadComboBoxData("HDD", cmbHDD);
            LoadComboBoxData("VGA", cmbVGA);
            LoadComboBoxData("PSU", cmbPSU);
        }

        private void LoadComboBoxData(string category, ComboBox comboBox)
        {
            try
            {
                string query = "SELECT nama_barang FROM barang WHERE kategori_barang = @kategori";
                using (cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kategori", category);

                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBox.Items.Clear();
                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader["nama_barang"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void cmbBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBrand = cmbBrand.SelectedItem.ToString();

            // Load motherboard and processor based on selected brand
            LoadMotherboard(selectedBrand);
            LoadProcessor(selectedBrand);
        }

        private void LoadMotherboard(string brand)
        {
            string query = "";

            if (brand == "INTEL")
            {
                query = "SELECT nama_barang FROM barang WHERE kategori_barang = 'Motherboard' AND (nama_barang LIKE 'H%' OR nama_barang LIKE 'B%60%' OR nama_barang LIKE 'Z%')";
            }
            else if (brand == "AMD")
            {
                query = "SELECT nama_barang FROM barang WHERE kategori_barang = 'Motherboard' AND (nama_barang LIKE 'A%' OR nama_barang LIKE 'B%50%' OR nama_barang LIKE 'X%')";
            }

            try
            {
                using (cmd = new MySqlCommand(query, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cmbMobo.Items.Clear();
                        while (reader.Read())
                        {
                            cmbMobo.Items.Add(reader["nama_barang"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading motherboard data: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void LoadProcessor(string brand)
        {
            string query = "";

            if (brand == "INTEL")
            {
                query = "SELECT nama_barang FROM barang WHERE kategori_barang = 'Processor' AND nama_barang LIKE 'I%'";
            }
            else if (brand == "AMD")
            {
                query = "SELECT nama_barang FROM barang WHERE kategori_barang = 'Processor' AND nama_barang LIKE 'A%'";
            }

            try
            {
                using (cmd = new MySqlCommand(query, conn))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cmbProc.Items.Clear();
                        while (reader.Read())
                        {
                            cmbProc.Items.Add(reader["nama_barang"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading processor data: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void CalculateTotal(TextBox jumlahTextBox, ComboBox comboBox, TextBox totalTextBox)
        {
            if (comboBox.SelectedItem != null && int.TryParse(jumlahTextBox.Text, out int jumlah))
            {
                string query = "SELECT harga_barang FROM barang WHERE nama_barang = @nama";
                try
                {
                    using (cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", comboBox.SelectedItem.ToString());

                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            int harga = Convert.ToInt32(result);
                            totalTextBox.Text = (harga * jumlah).ToString(); // Tidak menggunakan format Rp. atau koma
                            UpdateGrandTotal();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error calculating total: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void UpdateGrandTotal()
        {
            int grandTotal = 0;

            grandTotal += GetTotalFromTextBox(txtTotalproc);
            grandTotal += GetTotalFromTextBox(txtTotalMobo);
            grandTotal += GetTotalFromTextBox(txtTotalRAM);
            grandTotal += GetTotalFromTextBox(txtTotalCasing);
            grandTotal += GetTotalFromTextBox(txtTotalSSD);
            grandTotal += GetTotalFromTextBox(txtTotalHDD);
            grandTotal += GetTotalFromTextBox(txtTotalVGA);
            grandTotal += GetTotalFromTextBox(txtTotalPSu);

            lblGrandtot.Text = grandTotal.ToString(); // Menampilkan total tanpa format Rp.
        }

        private int GetTotalFromTextBox(TextBox textBox)
        {
            if (int.TryParse(textBox.Text, out int total))
            {
                return total;
            }
            return 0;
        }

        private void txtJmlProc_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal(txtJmlProc, cmbProc, txtTotalproc);
        }

        private void txtJmlMobo_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal(txtJmlMobo, cmbMobo, txtTotalMobo);
        }

        private void txtJmlRAM_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal(txtJmlRAM, cmbRAM, txtTotalRAM);
        }

        private void txtJmlCasing_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal(txtJmlCasing, cmbCasing, txtTotalCasing);
        }

        private void txtJmlSSD_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal(txtJmlSSD, cmbSSD, txtTotalSSD);
        }

        private void txtJmlHDD_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal(txtJmlHDD, cmbHDD, txtTotalHDD);
        }

        private void txtJmlVGA_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal(txtJmlVGA, cmbVGA, txtTotalVGA);
        }

        private void txtJmlPSU_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal(txtJmlPSU, cmbPSU, txtTotalPSu);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
