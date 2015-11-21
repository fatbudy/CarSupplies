using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dbl;
namespace CasuppliesSetting
{
    static class Program
    {

        public static DBLClass _dbl_Main = new DBLClass();
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!_dbl_Main.InitSetting().Equals(0))
            {
                return;
            }
            if (!_dbl_Main.CheckDBLinkState())
            {
                //数据库无法访问
                return;
            }
            _dbl_Main.LoadKey();
            _dbl_Main.LoadMessageList();

            Application.Run(new FormSetting());
        }
    }
}
