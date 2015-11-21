namespace Carsupplies
{
    partial class fbParent
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Closelabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Closelabel
            // 
            this.Closelabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Closelabel.AutoSize = true;
            this.Closelabel.Location = new System.Drawing.Point(536, 4);
            this.Closelabel.Name = "Closelabel";
            this.Closelabel.Size = new System.Drawing.Size(11, 12);
            this.Closelabel.TabIndex = 0;
            this.Closelabel.Text = "X";
            this.Closelabel.Click += new System.EventHandler(this.Closelabel_Click);
            // 
            // fbParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.Closelabel);
            this.DoubleBuffered = true;
            this.Name = "fbParent";
            this.Size = new System.Drawing.Size(548, 108);
            this.Resize += new System.EventHandler(this.fbParent_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Closelabel;
    }
}
