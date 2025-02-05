﻿
namespace proyekPV
{
    partial class TransaksiForm
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
            this.Text = "TransaksiForm";
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonCart = new System.Windows.Forms.Button();
            this.buttonAddCart = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSimul = new System.Windows.Forms.Button();
            this.txtTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbtrans = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();

            //AdjustFormSizeAndLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCart
            // 
            this.dgvCart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCart.Location = new System.Drawing.Point(8, 31);
            this.dgvCart.Margin = new System.Windows.Forms.Padding(2);
            this.dgvCart.Name = "dgvCart";
            this.dgvCart.ReadOnly = true;
            this.dgvCart.RowHeadersVisible = false;
            this.dgvCart.RowHeadersWidth = 51;
            this.dgvCart.RowTemplate.Height = 24;
            this.dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCart.Size = new System.Drawing.Size(510, 532);
            this.dgvCart.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(580, 420);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(125, 20);
            this.txtSearch.TabIndex = 1;
            //this.txtSearch.Visible = false;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(531, 56);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(66, 26);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Visible = false;
            // 
            // buttonCart
            // 
            this.buttonCart.Location = new System.Drawing.Point(720, 500);
            this.buttonCart.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCart.Name = "buttonCart";
            this.buttonCart.Size = new System.Drawing.Size(107, 40);
            this.buttonCart.TabIndex = 6;
            this.buttonCart.Text = "BuatNota";
            this.buttonCart.UseVisualStyleBackColor = true; 
            this.buttonCart.Click += new System.EventHandler(this.bayar_Click);
            // 
            // buttonAddCart
            // 
            this.buttonAddCart.Location = new System.Drawing.Point(525, 512);
            this.buttonAddCart.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAddCart.Name = "buttonAddCart";
            this.buttonAddCart.Size = new System.Drawing.Size(96, 26);
            this.buttonAddCart.TabIndex = 5;
            this.buttonAddCart.Text = "Add to Cart";
            this.buttonAddCart.UseVisualStyleBackColor = true;
            this.buttonAddCart.Visible = false;
           
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(20, 500);
            this.btnBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(69, 28);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            //this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSimul
            // 
            this.btnSimul.Location = new System.Drawing.Point(600, 500);
            this.btnSimul.Margin = new System.Windows.Forms.Padding(2);
            this.btnSimul.Name = "btnSimul";
            this.btnSimul.Size = new System.Drawing.Size(107, 40);
            this.btnSimul.TabIndex = 4;
            this.btnSimul.Text = "hapus barang";
            this.btnSimul.UseVisualStyleBackColor = true;
            this.btnSimul.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(650, 630);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(165, 220);
            this.txtTotal.TabIndex = 7;
            this.txtTotal.Text = "TOTAL Belanja";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(720, 400);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(720, 450);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 24);
            this.label2.TabIndex = 11;
            //this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(720, 420);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 24);
            this.label3.TabIndex = 11; 
            this.txtSearch.KeyDown += txtSearch_KeyDown;

            // 
            // cmbtrans
            // 
            this.cmbtrans.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtrans.FormattingEnabled = true;
            this.cmbtrans.Location = new System.Drawing.Point(30, 0);
            this.cmbtrans.Name = "cmbtans";
            this.cmbtrans.Size = new System.Drawing.Size(400, 24);
            this.cmbtrans.TabIndex = 8; 
            this.cmbtrans.Click += cmbtrans_SelectedIndexChanged;
            // 
            // CartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 569);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSimul);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonAddCart);
            this.Controls.Add(this.buttonCart);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dgvCart);
            this.Controls.Add(this.cmbtrans);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CartForm";
            this.Text = "BuyerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label txtTotal;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonAddCart;
        private System.Windows.Forms.Button buttonCart;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSimul;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbtrans;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}