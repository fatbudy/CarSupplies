using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms ;

namespace dbl
{
    public class CustemMessageShowArgs : EventArgs
    {
        private static bool _console_show = false;
        /// <summary>
        /// 消息显示方式，默认为form模式，如果是console模式，请修改为true
        /// </summary>
        public static bool ConsoleShow // 消息显示方式，默认为form模式，如果是console模式，请修改为true
        {
            get { return _console_show; }
            set { _console_show = value; }
        }
        public CustemMessageShowArgs(string text, string cap, int icon = 64, int button = 0)
        {
            Text = text;
            Caption = cap;
            Icon = icon;
            Button = button;
        }
        public string Caption { get; internal set; }
        public string Text { get; internal set; }
        //0,   消息框未包含符号。
        //16,     该消息框包含一个符号，该符号是由一个红色背景的圆圈及其中的白色 X 组成的。
        //32,     该消息框包含一个符号，该符号是由一个圆圈和其中的一个问号组成的。不再建议使用问号消息图标，原因是该图标无法清楚地表示特定类型的消息，并且问号形式的消息表述可应用于任何消息类型。此外，用户还可能将问号消息符号与帮助信息混淆。因此，请不要在消息框中使用此问号消息符号。系统继续支持此符号只是为了向后兼容。
        //48,     该消息框包含一个符号，该符号是由一个黄色背景的三角形及其中的一个感叹号组成的。
        //64,     该消息框包含一个符号，该符号是由一个圆圈及其中的小写字母 i 组成的
        private int _icon = 64;
        public int Icon
        {
            get { return _icon; }
            internal set
            {
                if (value.Equals(0) || value.Equals(16) || value.Equals(32) || value.Equals(48) || value.Equals(64))
                {
                    _icon = value;
                }
                else
                {
                    _icon = 0;
                }
            }
        }
        private int _button = 0;
        public int Button
        {
            get { return _button; }
            internal set
            {
                if (value >-1 && value < 6)
                {
                    _button = value;
                }
                else
                {
                    _button = 0;
                }
            }
        }
        public DialogResult  Show( )
        {
            if (_console_show)
            {
                Console.WriteLine(Text);
                return DialogResult.None;
            }
            MessageBoxIcon mbi = (MessageBoxIcon)_icon;
            MessageBoxButtons mbb = (MessageBoxButtons)_button;
            return MessageBox.Show(Text, Caption, mbb, mbi);
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
