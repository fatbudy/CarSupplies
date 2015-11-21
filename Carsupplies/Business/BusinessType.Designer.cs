namespace Carsupplies.Business
{
    partial class BusinessType
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
            this.Savebutton = new System.Windows.Forms.Button();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TypedataGridView = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.CustomCounttoolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Findbutton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TypedataGridView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Savebutton
            // 
            this.Savebutton.Location = new System.Drawing.Point(270, 2);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(75, 23);
            this.Savebutton.TabIndex = 18;
            this.Savebutton.Text = "保存";
            this.Savebutton.UseVisualStyleBackColor = true;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel1.Text = "设置：";
            // 
            // TypedataGridView
            // 
            this.TypedataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypedataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TypedataGridView.Location = new System.Drawing.Point(0, 31);
            this.TypedataGridView.Name = "TypedataGridView";
            this.TypedataGridView.RowTemplate.Height = 23;
            this.TypedataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TypedataGridView.Size = new System.Drawing.Size(561, 225);
            this.TypedataGridView.TabIndex = 13;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.CustomCounttoolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 259);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(563, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // CustomCounttoolStripStatusLabel
            // 
            this.CustomCounttoolStripStatusLabel.Name = "CustomCounttoolStripStatusLabel";
            this.CustomCounttoolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // Findbutton
            // 
            this.Findbutton.Location = new System.Drawing.Point(189, 2);
            this.Findbutton.Name = "Findbutton";
            this.Findbutton.Size = new System.Drawing.Size(75, 23);
            this.Findbutton.TabIndex = 16;
            this.Findbutton.Text = "查找";
            this.Findbutton.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(55, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(128, 21);
            this.textBox1.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "关键字：";
            // 
            // BusinessType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.Savebutton);
            this.Controls.Add(this.TypedataGridView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Findbutton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "BusinessType";
            this.Size = new System.Drawing.Size(563, 281);
            this.Load += new System.EventHandler(this.BusinessType_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.Findbutton, 0);
            this.Controls.SetChildIndex(this.statusStrip1, 0);
            this.Controls.SetChildIndex(this.TypedataGridView, 0);
            this.Controls.SetChildIndex(this.Savebutton, 0);
            ((System.ComponentModel.ISupportInitialize)(this.TypedataGridView)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Savebutton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridView TypedataGridView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel CustomCounttoolStripStatusLabel;
        private System.Windows.Forms.Button Findbutton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;

    }
}
