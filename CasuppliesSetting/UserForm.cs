using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dbl;

namespace CasuppliesSetting
{
    public partial class UserForm : Form
    {
        private static UserForm _default = null;
        public static UserForm Default
        {
            get
            {
                if (_default == null || _default .IsDisposed )
                {
                    _default = new UserForm();
                }
                return _default;
            }
        }
        public UserForm()
        {
            InitializeComponent();
        }
        private DataTable udt = new DataTable();
        private SqlCmd _UM_System = null;
        private void UserForm_Load(object sender, EventArgs e)
        {
            if (!Program._dbl_Main.ActionList.ContainsKey("UM_System"))
            {
                //错误
            }
            _UM_System = Program._dbl_Main.ActionList["UM_System"];
            reload();

        }
        private void UserForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode  == Keys.F5)
            {
                reload();
            }
        }
        private void reload()
        {
            if (Program._dbl_Main.ReadData("UM_System", ref _UM_System, new List<object[]>()) != 0)
            {
                //错误
            }
            this.dataGridView1.DataBindings.Clear();
            this.dataGridView1.DataBindings.Add(new Binding("DataSource",_UM_System.SQLDataSet , _UM_System.SQLList[0].Data.TableName));
            set_dataColumn(_UM_System.ColumnSetting);
        }
        private void set_dataColumn(Dictionary <string, ColumnSetting>  dcs)
        {
            foreach (DataGridViewColumn dc in this.dataGridView1.Columns)
            {
                if (dcs.ContainsKey(dc.Name))
                {
                    dc.HeaderText = dcs[dc.Name].HeadText;
                    dc.Width = dcs[dc.Name].Width;
                    dc.Visible = dcs[dc.Name].Visiable;
                }
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            _UM_System.SQLList[0].OnUpdated = true;
            Program._dbl_Main.Update(_UM_System);
            reload();
        }


    }
}
