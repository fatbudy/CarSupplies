namespace Carsupplies
{
    partial class SelectItemValue
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
            this.SelectItemdataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.Selectbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SelectItemdataGridView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectItemdataGridView
            // 
            this.SelectItemdataGridView.AllowUserToAddRows = false;
            this.SelectItemdataGridView.AllowUserToDeleteRows = false;
            this.SelectItemdataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectItemdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SelectItemdataGridView.Location = new System.Drawing.Point(12, 12);
            this.SelectItemdataGridView.Name = "SelectItemdataGridView";
            this.SelectItemdataGridView.ReadOnly = true;
            this.SelectItemdataGridView.RowTemplate.Height = 23;
            this.SelectItemdataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.SelectItemdataGridView.Size = new System.Drawing.Size(540, 281);
            this.SelectItemdataGridView.TabIndex = 0;
            this.SelectItemdataGridView.DoubleClick += new System.EventHandler(this.SelectItemdataGridView_DoubleClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Cancelbutton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Selectbutton, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(352, 295);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 35);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbutton.Location = new System.Drawing.Point(12, 6);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.Cancelbutton.TabIndex = 0;
            this.Cancelbutton.Text = "取消";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // Selectbutton
            // 
            this.Selectbutton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Selectbutton.Location = new System.Drawing.Point(112, 6);
            this.Selectbutton.Name = "Selectbutton";
            this.Selectbutton.Size = new System.Drawing.Size(75, 23);
            this.Selectbutton.TabIndex = 1;
            this.Selectbutton.Text = "选择";
            this.Selectbutton.UseVisualStyleBackColor = true;
            this.Selectbutton.Click += new System.EventHandler(this.Selectbutton_Click);
            // 
            // SelectItemValue
            // 
            this.AcceptButton = this.Selectbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancelbutton;
            this.ClientSize = new System.Drawing.Size(564, 331);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.SelectItemdataGridView);
            this.MinimumSize = new System.Drawing.Size(580, 370);
            this.Name = "SelectItemValue";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.SelectItemdataGridView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView SelectItemdataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.Button Selectbutton;
    }
}