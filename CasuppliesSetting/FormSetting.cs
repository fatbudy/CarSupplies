using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CasuppliesSetting
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            Application.Exit();
        }

        private void 用户UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserForm.Default.MdiParent  = this;
            UserForm.Default.Show();
        }
    }
}
