using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Carsupplies
{
    public partial class fromNoteShow : Form
    {
        public fromNoteShow()
        {
            InitializeComponent();
        }
        public void Set_NoteText(string text)
        {
            this.richTextBox1.Text = text;
        }
    }
}
