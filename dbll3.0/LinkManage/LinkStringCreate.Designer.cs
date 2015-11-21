namespace dbll3.LinkManage
{
    partial class LinkStringCreate
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Savebutton = new System.Windows.Forms.Button();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.editDBLink1 = new dbll3.LinkManage.EditDBLink();
            this.TypecomboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Savebutton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Cancelbutton, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(73, 156);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 37);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // Savebutton
            // 
            this.Savebutton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Savebutton.Location = new System.Drawing.Point(112, 7);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(75, 23);
            this.Savebutton.TabIndex = 0;
            this.Savebutton.Text = "保存";
            this.Savebutton.UseVisualStyleBackColor = true;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbutton.Location = new System.Drawing.Point(12, 7);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.Cancelbutton.TabIndex = 1;
            this.Cancelbutton.Text = "取消";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // editDBLink1
            // 
            this.editDBLink1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editDBLink1.AutoSize = true;
            this.editDBLink1.LinkStringCreate = null;
            this.editDBLink1.Location = new System.Drawing.Point(12, 33);
            this.editDBLink1.Name = "editDBLink1";
            this.editDBLink1.Size = new System.Drawing.Size(252, 117);
            this.editDBLink1.TabIndex = 0;
            // 
            // TypecomboBox
            // 
            this.TypecomboBox.FormattingEnabled = true;
            this.TypecomboBox.Location = new System.Drawing.Point(70, 7);
            this.TypecomboBox.Name = "TypecomboBox";
            this.TypecomboBox.Size = new System.Drawing.Size(121, 20);
            this.TypecomboBox.TabIndex = 2;
            this.TypecomboBox.SelectionChangeCommitted += new System.EventHandler(this.TypecomboBox_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "类型:";
            // 
            // LinkStringCreate
            // 
            this.AcceptButton = this.Savebutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.Cancelbutton;
            this.ClientSize = new System.Drawing.Size(272, 191);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TypecomboBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.editDBLink1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "LinkStringCreate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库连接信息配置";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EditDBLink editDBLink1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Savebutton;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.ComboBox TypecomboBox;
        private System.Windows.Forms.Label label1;
    }
}