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
            ConfigureTextBoxRestrictions();
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
                string query = "SELECT harga_barang, jumlah_barang FROM barang WHERE nama_barang = @nama";
                try
                {
                    using (cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", comboBox.SelectedItem.ToString());

                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int harga = Convert.ToInt32(reader["harga_barang"]);
                                int stok = Convert.ToInt32(reader["jumlah_barang"]);

                                if (jumlah > stok)
                                {
                                    MessageBox.Show("Jumlah barang melebihi stok yang tersedia: " + stok);
                                    jumlahTextBox.Text = stok.ToString(); // Set jumlah ke stok maksimum
                                    jumlah = stok; // Update jumlah untuk kalkulasi
                                }

                                totalTextBox.Text = (harga * jumlah).ToString(); // Update total harga
                                UpdateGrandTotal();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error calculating total: " + ex.Message);
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

        private void ConfigureTextBoxRestrictions()
        {
            // Restrict jumlah TextBox input
            txtJmlProc.KeyPress += ValidateJumlahInput;
            txtJmlProc.TextChanged += txtJmlProc_TextChanged;

            txtJmlMobo.KeyPress += ValidateJumlahInput;
            txtJmlMobo.TextChanged += txtJmlMobo_TextChanged;

            txtJmlRAM.KeyPress += ValidateJumlahInput;
            txtJmlRAM.TextChanged += txtJmlRAM_TextChanged;

            txtJmlCasing.KeyPress += ValidateJumlahInput;
            txtJmlCasing.TextChanged += txtJmlCasing_TextChanged;

            txtJmlSSD.KeyPress += ValidateJumlahInput;
            txtJmlSSD.TextChanged += txtJmlSSD_TextChanged;

            txtJmlHDD.KeyPress += ValidateJumlahInput;
            txtJmlHDD.TextChanged += txtJmlHDD_TextChanged;

            txtJmlVGA.KeyPress += ValidateJumlahInput;
            txtJmlVGA.TextChanged += txtJmlVGA_TextChanged;

            txtJmlPSU.KeyPress += ValidateJumlahInput;
            txtJmlPSU.TextChanged += txtJmlPSU_TextChanged;

            // Set total TextBox to read-only
            txtTotalproc.ReadOnly = true;
            txtTotalMobo.ReadOnly = true;
            txtTotalRAM.ReadOnly = true;
            txtTotalCasing.ReadOnly = true;
            txtTotalSSD.ReadOnly = true;
            txtTotalHDD.ReadOnly = true;
            txtTotalVGA.ReadOnly = true;
            txtTotalPSu.ReadOnly = true;
        }

        private void ValidateJumlahInput(object sender, KeyPressEventArgs e)
        {
            // Hanya menerima angka dan backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
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

        private void btnAddtoCart_Click(object sender, EventArgs e)
        {
            // Cek apakah semua kategori barang telah dipilih dan jumlahnya lebih dari 0
            if (IsFormValid())
            {
                try
                {
                    // Tambahkan setiap barang ke Cart
                    AddItemToCart(cmbProc, txtJmlProc);
                    AddItemToCart(cmbMobo, txtJmlMobo);
                    AddItemToCart(cmbRAM, txtJmlRAM);
                    AddItemToCart(cmbCasing, txtJmlCasing);
                    AddItemToCart(cmbSSD, txtJmlSSD);
                    AddItemToCart(cmbHDD, txtJmlHDD);
                    AddItemToCart(cmbVGA, txtJmlVGA);
                    AddItemToCart(cmbPSU, txtJmlPSU);

                    MessageBox.Show("Barang berhasil ditambahkan ke keranjang.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saat menambahkan barang ke keranjang: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Pastikan semua kategori barang diisi dan jumlah lebih dari 0.");
            }
        }

        private bool IsFormValid()
        {
            // Pastikan semua kategori sudah dipilih dan jumlah lebih dari 0
            return (IsItemValid(cmbProc, txtJmlProc) &&
                    IsItemValid(cmbMobo, txtJmlMobo) &&
                    IsItemValid(cmbRAM, txtJmlRAM) &&
                    IsItemValid(cmbCasing, txtJmlCasing) &&
                    IsItemValid(cmbSSD, txtJmlSSD) &&
                    IsItemValid(cmbHDD, txtJmlHDD) &&
                    IsItemValid(cmbVGA, txtJmlVGA) &&
                    IsItemValid(cmbPSU, txtJmlPSU));
        }

        private bool IsItemValid(ComboBox comboBox, TextBox jumlahTextBox)
        {
            return comboBox.SelectedItem != null && int.TryParse(jumlahTextBox.Text, out int jumlah) && jumlah > 0;
        }

        private void AddItemToCart(ComboBox comboBox, TextBox jumlahTextBox)
        {
            if (comboBox.SelectedItem != null && int.TryParse(jumlahTextBox.Text, out int jumlah) && jumlah > 0)
            {
                string itemName = comboBox.SelectedItem.ToString();
                string query = "SELECT barang_id, harga_barang FROM barang WHERE nama_barang = @nama_barang";

                try
                {
                    using (cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama_barang", itemName);

                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string idBarang = reader["barang_id"].ToString();
                                int hargaBarang = Convert.ToInt32(reader["harga_barang"]);

                                reader.Close(); // Pastikan DataReader ditutup sebelum membuka koneksi lain

                                // Membuka koneksi baru untuk INSERT
                                using (var insertConn = new MySqlConnection(conn.ConnectionString))
                                {
                                    insertConn.Open();
                                    string insertQuery = "INSERT INTO cart (Barang_nama, barang_qty, Barang_id, barang_harga) " +
                                                         "VALUES (@nama_barang, @jumlah_barang, @barang_id, @harga_barang)";

                                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, insertConn))
                                    {
                                        insertCmd.Parameters.AddWithValue("@nama_barang", itemName);
                                        insertCmd.Parameters.AddWithValue("@jumlah_barang", jumlah);
                                        insertCmd.Parameters.AddWithValue("@barang_id", idBarang);
                                        insertCmd.Parameters.AddWithValue("@harga_barang", hargaBarang);

                                        insertCmd.ExecuteNonQuery(); // Menjalankan query INSERT
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saat menambahkan barang ke keranjang: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Jumlah barang tidak valid.");
            }
        }




    }
}
