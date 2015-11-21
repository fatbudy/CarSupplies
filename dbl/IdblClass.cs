using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using dbll3;

namespace dbl
{
    /// <summary>
    /// 数据库连接管理类
    /// </summary>
    public interface  IdblClass
    {
        event ShowMessageEventHandler ShowMessage;

        void get_Data(string sql, ref DataTable dt);
        void RefreshColumnSettingLinkData(ref SqlCmd sqlc, string colname);
        void set_dataColumn(Dictionary<string, ColumnSetting> dcs, DataGridView dgv);
        bool get_ColumnSetting(int sqid, ref Dictionary<string, ColumnSetting> dcs, out SqlListItem sqli);
        void LoadSqlCmdItems();
        /// <summary>
        /// 根据字典读取数据
        /// </summary>
        /// <remarks>其他值代表具体的错误类型</remarks>
        /// <param name="key">操作字典key</param>
        /// <param name="ds">返回的数据</param>
        /// <param name="param">带入的参数</param>
        /// <returns>0成功，-1失败</returns>
        int ReadData(string key, ref SqlCmd sqlc,  List<object[]> param, int id);

        /// <summary>
        /// 更新或插入数据
        /// </summary>
        int Update(SqlCmd sqlc);

        /// <summary>
        /// 从远程数据库载入操作数据字典
        /// </summary>
        int LoadKey();

        /// <summary>
        /// 从远程数据库载入消息字典
        /// </summary>
        int LoadMessageList();

        /// <summary>
        /// 检查数据连接
        /// </summary>
        bool CheckDBLinkState();

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        int LinkSetting(string filename = "");

        int InitSetting();
        int InitSetting(string filename);
        string getLinkDataSerialNumber();
    }
}
