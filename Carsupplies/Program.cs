using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dbl;
namespace Carsupplies
{
    static class Program
    {
        /// <summary>
        /// 数据连接层，执行层
        /// </summary>
        public static DBLClass _DBL_MAIN = new DBLClass();
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!_DBL_MAIN.InitSetting().Equals(0))
            {
                return;
            }
            if (!_DBL_MAIN.CheckDBLinkState())
            {
                //数据库无法访问
                return;
            }
            _DBL_MAIN.LoadKey();
            _DBL_MAIN.LoadMessageList();
            _DBL_MAIN.LoadSqlCmdItems();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

    }
}
