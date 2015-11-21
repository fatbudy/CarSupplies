using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dbl;

namespace Carsupplies.Business
{
    public partial class Products : Carsupplies.fbParent
    {
        public Products()
        {
            InitializeComponent();
        }
        private static Products _default = null;
        public static Products Default
        {
            get
            {
                if (_default == null || _default.IsDisposed)
                {
                    _default = new Products();
                }
                return _default;
            }
        }

        private void Products_Load(object sender, EventArgs e)
        {
            if (!_dblClass.ActionList.ContainsKey("PM_NONE"))
            {
                //错误
            }
            _sqlcommand = _dblClass.ActionList["PM_NONE"];
            reload();
        }
        private void reload()
        {
            if (_dblClass.ReadData("PM_NONE", ref _sqlcommand, new List<object[]>()) != 0)
            {
                //错误
            }

            BindingSource bs = new BindingSource(_sqlcommand.SQLDataSet, _sqlcommand.SQLList[0].Data.TableName);
            this.CustomdataGridView.DataSource = bs;
            this.bindingNavigator1.BindingSource  = bs;
            _dblClass.set_dataColumn(_sqlcommand.ColumnSetting, this.CustomdataGridView);
        }


        private void SavetoolStripButton_Click(object sender, EventArgs e)
        {
            _sqlcommand.SQLList[0].OnUpdated = true;
            _dblClass.Update(_sqlcommand);
        }

        private void RefreshtoolStripButton_Click(object sender, EventArgs e)
        {
            _sqlcommand.ClearData();
            if (_dblClass.ReadData("PM_NONE", ref _sqlcommand, new List<object[]>()) != 0)
            {
                //错误
            }

        }
    }
}
