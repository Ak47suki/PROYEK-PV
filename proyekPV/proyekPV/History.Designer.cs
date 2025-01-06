
namespace proyekPV
{
    partial class History
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "History";
            this.dgvBarang = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonCart = new System.Windows.Forms.Button();
            this.buttonAddCart = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSimul = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBarang)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBarang
            // 
            this.dgvBarang.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBarang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBarang.Location = new System.Drawing.Point(10, 14);
            this.dgvBarang.Name = "dgvBarang";
            this.dgvBarang.ReadOnly = true;
            this.dgvBarang.RowHeadersVisible = false;
            this.dgvBarang.RowHeadersWidth = 51;
            this.dgvBarang.RowTemplate.Height = 24;
            this.dgvBarang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBarang.Size = new System.Drawing.Size(680, 655);
            this.dgvBarang.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(708, 41);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(165, 22);
            this.txtSearch.TabIndex = 1;
            //this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(708, 69);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(88, 32);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            //this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(1015, 686);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(92, 35);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            //this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSimul
            // 
            this.btnSimul.Location = new System.Drawing.Point(937, 28);
            this.btnSimul.Name = "btnSimul";
            this.btnSimul.Size = new System.Drawing.Size(143, 49);
            this.btnSimul.TabIndex = 4;
            this.btnSimul.Text = "Simulasi RAKIT";
            this.btnSimul.UseVisualStyleBackColor = true;
            //this.btnSimul.Click += new System.EventHandler(this.btnSimul_Click);
            // 
            // buttonAddCart
            // 
            this.buttonAddCart.Location = new System.Drawing.Point(700, 630);
            this.buttonAddCart.Name = "buttonAddCart";
            this.buttonAddCart.Size = new System.Drawing.Size(128, 32);
            this.buttonAddCart.TabIndex = 2;
            this.buttonAddCart.Text = "Add to Cart";
            this.buttonAddCart.UseVisualStyleBackColor = true;
            //this.buttonAddCart.Click += new System.EventHandler(this.buttonAddCart_Click);
            // 
            // buttonCart
            // 
            this.buttonCart.Location = new System.Drawing.Point(1008, 650);
            this.buttonCart.Name = "buttonCart";
            this.buttonCart.Size = new System.Drawing.Size(88, 32);
            this.buttonCart.TabIndex = 2;
            this.buttonCart.Text = "Cart";
            this.buttonCart.UseVisualStyleBackColor = true;
            //this.buttonCart.Click += new System.EventHandler(this.buttonCart_Click);
            // 
            // BuyerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 733);
            this.Controls.Add(this.btnSimul);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonAddCart);
            this.Controls.Add(this.buttonCart);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dgvBarang);
            this.Name = "BuyerForm";
            this.Text = "BuyerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBarang)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBarang;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonAddCart;
        private System.Windows.Forms.Button buttonCart;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSimul;
    }

       
    
}