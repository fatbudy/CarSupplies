using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OutPutData.Log
{
    public class Logs
    {

        /// <summary>
        /// 消息追加至文件结尾，按行写入
        /// </summary>
        /// <param name="filename">保存文本格式数据的文件名</param>
        /// <param name="value">追加的数据信息</param>
        public void WriteText(string filename, string value)
        {
            using (StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8))
            {
                sw.WriteLine(value);
            }
        }
    }
}
