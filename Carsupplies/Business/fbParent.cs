using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dbl;

namespace Carsupplies
{
    public partial class fbParent : UserControl
    {
        public event ClosedEventHandler Closed;
        public fbParent()
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
        }
        protected  SqlCmd _sqlcommand = null;
        public SqlCmd SQLCommand { get { return _sqlcommand; } set { _sqlcommand = value; } }
        protected DBLClass _dblClass = null;
        public DBLClass DBLClass { set { _dblClass = value; } }
        public DialogResult DialogResult { get; set; }
        public new Control Parent
        {
            get
            { return base.Parent; }
            set
            {
                base.Parent = value;
                if (this.Dock  != DockStyle.Fill)
                {
                    this.Left = (this.Parent.Width - this.Width) / 2;
                    this.Top = (this.Parent.Height - this.Height) / 2;
                }
            }
        }
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
                if (base.Parent !=null &&value != DockStyle.Fill)
                {
                    this.Left = (base.Parent .Width - this.Width) / 2;
                    this.Top = (base.Parent.Height - this.Height) / 2;
                }
            }
        }
        protected void RaiseEvent(ClosedArgs e)
        {
            if (Closed != null)
            {
                Closed.Invoke(this, e);
            }
        }
        public virtual void AcceptButton()
        {
        }
        public virtual void CancelButton()
        {
        }
        public bool CloseButtonShow
        {
            get { return Closelabel.Visible; }
            set { Closelabel.Visible = value; }
        }
        private void Closelabel_Click(object sender, EventArgs e)
        {
            RaiseEvent(new ClosedArgs() { Closing = true });
        }
        public string ModelName { get; set; }
        private void fbParent_Resize(object sender, EventArgs e)
        {
            Closelabel.Left = this.Width - Closelabel.Width - 2;
            Closelabel.Top = 2;
        }
    }
}
