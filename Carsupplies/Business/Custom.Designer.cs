namespace Carsupplies.Business
{
    partial class Custom
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CustomdataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Findbutton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.CustomCounttoolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.VIPCounttoolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Savebutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CustomdataGridView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CustomdataGridView
            // 
            this.CustomdataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustomdataGridView.Location = new System.Drawing.Point(0, 37);
            this.CustomdataGridView.Name = "CustomdataGridView";
            this.CustomdataGridView.RowTemplate.Height = 23;
            this.CustomdataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CustomdataGridView.Size = new System.Drawing.Size(598, 262);
            this.CustomdataGridView.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "关键字：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(58, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(128, 21);
            this.textBox1.TabIndex = 2;
            // 
            // Findbutton
            // 
            this.Findbutton.Location = new System.Drawing.Point(192, 8);
            this.Findbutton.Name = "Findbutton";
            this.Findbutton.Size = new System.Drawing.Size(75, 23);
            this.Findbutton.TabIndex = 3;
            this.Findbutton.Text = "查找";
            this.Findbutton.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.CustomCounttoolStripStatusLabel,
            this.toolStripStatusLabel2,
            this.VIPCounttoolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 302);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(598, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel1.Text = "客户：";
            // 
            // CustomCounttoolStripStatusLabel
            // 
            this.CustomCounttoolStripStatusLabel.Name = "CustomCounttoolStripStatusLabel";
            this.CustomCounttoolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(30, 17);
            this.toolStripStatusLabel2.Text = "VIP:";
            // 
            // VIPCounttoolStripStatusLabel
            // 
            this.VIPCounttoolStripStatusLabel.Name = "VIPCounttoolStripStatusLabel";
            this.VIPCounttoolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // Savebutton
            // 
            this.Savebutton.Location = new System.Drawing.Point(273, 8);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(75, 23);
            this.Savebutton.TabIndex = 5;
            this.Savebutton.Text = "保存";
            this.Savebutton.UseVisualStyleBackColor = true;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // Custom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.Savebutton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Findbutton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CustomdataGridView);
            this.Name = "Custom";
            this.Size = new System.Drawing.Size(598, 324);
            this.Load += new System.EventHandler(this.Custom_Load);
            this.Controls.SetChildIndex(this.CustomdataGridView, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.Findbutton, 0);
            this.Controls.SetChildIndex(this.statusStrip1, 0);
            this.Controls.SetChildIndex(this.Savebutton, 0);
            ((System.ComponentModel.ISupportInitialize)(this.CustomdataGridView)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView CustomdataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Findbutton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel CustomCounttoolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel VIPCounttoolStripStatusLabel;
        private System.Windows.Forms.Button Savebutton;
    }
}
