using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dbl;

namespace Carsupplies
{
    public partial class SelectItemValue : Form
    {
        private static SelectItemValue _default = null;
        public static SelectItemValue Default
        {
            get
            {
                if (_default == null || _default.IsDisposed)
                {
                    _default = new SelectItemValue();
                }
                return _default;
            }
        }
        private DataTable _selectData = new DataTable();
        private ColumnSetting _columnSetting = null;
        public SelectItemValue()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 选择的值
        /// </summary>
        public string SelectValue { get; private set; }
        public void SetColumnSetting(DBLClass dblc, ColumnSetting cs,params object[] param)
        {
            if (dblc != null)
            {
                Dictionary<string, ColumnSetting> dcs = new Dictionary<string, ColumnSetting>();
                SqlListItem sqli = null;
                _columnSetting = cs;

                if (dblc.get_ColumnSetting(cs.LinkDataSQLString, ref dcs, out sqli))
                {
                    if (_selectData != null)
                    {
                        _selectData.Clear();
                        _selectData.Dispose();
                        _selectData = new DataTable();
                    }
                    dblc.get_Data(string.Format(sqli.SQLString, param), ref _selectData);
                    this.SelectItemdataGridView.DataSource = _selectData;
                    dblc.set_dataColumn(dcs, this.SelectItemdataGridView);
                }

            }
        }

        private void Selectbutton_Click(object sender, EventArgs e)
        {
            if (this.SelectItemdataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("没有选择任何数据", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.SelectValue = Convert.ToString(this.SelectItemdataGridView.SelectedRows[0].Cells[_columnSetting.LinkColumnName].Value);
            if (_columnSetting.LinkData)
            {
                foreach (DataGridViewColumn dc in this.SelectItemdataGridView.Columns)
                {
                    if (_columnSetting.LinkDataColumnEx.ContainsKey(dc.Name))
                    {
                        _columnSetting.LinkDataColumnEx[dc.Name] =
                            this.SelectItemdataGridView.SelectedRows[0].Cells[dc.Name].Value;
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SelectItemdataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (this.SelectItemdataGridView.SelectedRows.Count > 0)
            {
               this.SelectValue=Convert.ToString ( this.SelectItemdataGridView.SelectedRows[0].Cells[_columnSetting.LinkColumnName].Value);
               if (_columnSetting.LinkData)
               {
                   foreach (DataGridViewColumn dc in this.SelectItemdataGridView.Columns)
                   {
                       if (_columnSetting.LinkDataColumnEx.ContainsKey(dc.Name))
                       {
                           _columnSetting.LinkDataColumnEx[dc.Name] =
                               this.SelectItemdataGridView.SelectedRows[0].Cells[dc.Name].Value;
                       }
                   }
               }
               this.DialogResult = DialogResult.OK;
               this.Close();
            }
        }
    }
}
