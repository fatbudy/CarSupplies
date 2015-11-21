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
    public partial class UserLogin : fbParent
    {
        public UserLogin()
        {
            InitializeComponent();
            CloseButtonShow = false;
        }

        private void Enterbutton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace ( UserNametextBox.Text ))
            {
                // error
                return;
            }
            if (_dblClass == null)
            {
                //error
                return;
            }
            if (!_dblClass.ActionList.ContainsKey("UM_Login"))
            {
                //错误
                return;
            }
            _sqlcommand = _dblClass.ActionList["UM_Login"];
            if (_sqlcommand.Window == null)
            {
                _sqlcommand.Window = this;
            }

            List<object[]> param = new List<object[]>();
            param.Add(new object[] { UserNametextBox.Text, PasswordtextBox.Text });
            if (_dblClass.ReadData("UM_Login", ref _sqlcommand, param).Equals(-1))
            {
                return ;
            }
            if (_sqlcommand.SQLList[0].Data .Rows  .Count<1)
            {
                //错误的用户名和密码
                return;
            }
            string perd = Convert.ToString(_sqlcommand.SQLList[0].Data.Rows[0][4]);
            base.RaiseEvent(new ClosedArgs() { DialogResult = DialogResult.OK, Closing = true, Data =perd });
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            base.RaiseEvent(new ClosedArgs() { DialogResult = DialogResult.Cancel , Closing = true });
        }

        public override void AcceptButton()
        {
            Enterbutton_Click(this, null);
        }

        public override void CancelButton()
        {
            Cancelbutton_Click(this, null);
        }

    }
}
