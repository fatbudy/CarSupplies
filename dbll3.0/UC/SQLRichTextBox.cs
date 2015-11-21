using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using dbll3.UC.rtfFormat;

namespace dbll3.UC
{
    [DesignerCategory("UserControl")]
    public class SQLRichTextBox : RichTextBox
    {
        // Fields
        private static List<WeakReference> __ENCList = new List<WeakReference>();
        private const int CFE_AUTOBACKCOLOR = 0x4000000;
        private const int CFM_BACKCOLOR = 0x4000000;
        private const int EM_GETCHARFORMAT = 0x43a;
        private const int EM_SETBKGNDCOLOR = 0x443;
        private const int EM_SETCHARFORMAT = 0x444;
        private const int LF_FACESIZE = 0x20;
        private const long SCF_SELECTION = 1L;
        private const int WM_SETTEXT = 12;
        private const int WM_USER = 0x400;

        private System.Windows.Forms.Timer timer1;
        private IContainer components;

        private List<dbll3.UC.rtfFormat.ItemCollections> _iclist = new List<ItemCollections>();

        public List<dbll3.UC.rtfFormat.ItemCollections> KeyList
        {
            get { return _iclist; }
        }
        public SQLRichTextBox()
        {
            InitializeComponent();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ClearBackColor(true);
            RichTextBoxFinds Finds = RichTextBoxFinds.WholeWord;
            LockWindowUpdate(this.Handle);

            this.SuspendLayout();
            int ScrollPosVert = this.GetScrollBarPos(this.Handle, ScrollBarTypes.SB_VERT);
            int ScrollPosHoriz = this.GetScrollBarPos(this.Handle, ScrollBarTypes.SB_HORZ);
            int SelStart = this.SelectionStart;
            int SelLength = this.SelectionLength;
            int StartFrom = 0;
            int Length = 0;
            foreach (ItemCollections ic in _iclist)
            {
                foreach (var item in ic.Keys())
                {
                    Length = item.KeyName.Length;
                    while (this.Find(item.KeyName, StartFrom, Finds) > -1)
                    {
                        this.SelectionBackColor = item.Item.BackColor;
                        this.SelectionColor = item.Item.Color;
                        StartFrom = this.SelectionStart + this.SelectionLength;
                    }
                }
            }
            this.SelectionStart = SelStart;
            this.SelectionLength = SelLength;
            SendMessage(this.Handle, 0x4de, 0, new POINT(ScrollPosHoriz, ScrollPosVert));
            this.ResumeLayout();
            LockWindowUpdate(IntPtr.Zero);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            timer1.Enabled = false;
            base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            timer1.Enabled = true;
            base.OnKeyUp(e);
        }
        public void ClearBackColor([Optional, DefaultParameterValue(true)] bool ClearAll)
        {
            CharFormat2 Format;
            IntPtr HWND = this.Handle;
            LockWindowUpdate(this.Handle);
            this.SuspendLayout();
            int ScrollPosVert = this.GetScrollBarPos(this.Handle, ScrollBarTypes.SB_VERT);
            int ScrollPosHoriz = this.GetScrollBarPos(this.Handle, ScrollBarTypes.SB_HORZ);
            int SelStart = this.SelectionStart;
            int SelLength = this.SelectionLength;
            if (ClearAll)
            {
                this.SelectAll();
            }
            Format = new CharFormat2();
            Format.crBackColor = -1;
            Format.crTextColor = -1;
            Format.dwMask = 0x4000000;
            Format.dwEffects = 0x4000000;
            Format.cbSize = Marshal.SizeOf(Format);
            SendMessage(this.Handle, 0x444, 1, ref Format);
            this.SelectionStart = SelStart;
            this.SelectionLength = SelLength;
            SendMessage(this.Handle, 0x4de, 0, new POINT(ScrollPosHoriz, ScrollPosVert));
            this.ResumeLayout();
            LockWindowUpdate(IntPtr.Zero);
        }

        private int GetScrollBarPos(IntPtr hWnd, ScrollBarTypes BarType)
        {
            SCROLLINFO INFO = new SCROLLINFO();
            INFO.fMask = ScrollBarInfoFlags.SIF_POS;
            INFO.cbSize = Marshal.SizeOf(INFO);
            GetScrollInfo(hWnd, BarType, ref INFO);
            return INFO.nPos;
        }

        [DllImport("User32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool GetScrollInfo(IntPtr hWnd, ScrollBarTypes fnBar, ref SCROLLINFO lpsi);
        [DllImport("User32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool GetScrollRange(IntPtr hWnd, int nBar, ref int lpMinPos, ref int lpMaxPos);
        public void Highlight(string FindWhat, Color Highlight, bool MatchCase, bool MatchWholeWord)
        {
            RichTextBoxFinds Finds = RichTextBoxFinds.None;
            LockWindowUpdate(this.Handle);
            this.SuspendLayout();
            int ScrollPosVert = this.GetScrollBarPos(this.Handle, ScrollBarTypes.SB_VERT);
            int ScrollPosHoriz = this.GetScrollBarPos(this.Handle, ScrollBarTypes.SB_HORZ);
            int SelStart = this.SelectionStart;
            int SelLength = this.SelectionLength;
            int StartFrom = 0;
            int Length = FindWhat.Length;
            if (MatchCase)
            {
                Finds = RichTextBoxFinds.MatchCase;
            }
            if (MatchWholeWord)
            {
                Finds = RichTextBoxFinds.WholeWord;
            }

            while (this.Find(FindWhat, StartFrom, Finds) > -1)
            {
                this.SelectionBackColor = Highlight;
                StartFrom = this.SelectionStart + this.SelectionLength;
            }
            this.SelectionStart = SelStart;
            this.SelectionLength = SelLength;
            SendMessage(this.Handle, 0x4de, 0, new POINT(ScrollPosHoriz, ScrollPosVert));
            this.ResumeLayout();
            LockWindowUpdate(IntPtr.Zero);
        }

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool LockWindowUpdate(IntPtr hWndLock);
        public void ScrollToBottom()
        {
            int Max=0;
            int Min=0;
            GetScrollRange(this.Handle, 1, ref Min, ref Max);
            SendMessage(this.Handle, 0x4de, 0, new POINT(0, Max - this.Height));
        }

        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, POINT lParam);
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, ref CharFormat2 lParam);

        // Properties
        public new  Color SelectionBackColor
        {
            get
            {
                CharFormat2 Format;
                IntPtr HWND = this.Handle;
                Format = new CharFormat2();
                Format.dwMask = 0x4000000;
                Format.cbSize = Marshal.SizeOf(Format);
                SendMessage(this.Handle, 0x43a, 1, ref Format);
                return ColorTranslator.FromOle(Format.crBackColor);
            }
            set
            {
                CharFormat2 Format;
                IntPtr HWND = this.Handle;
                Format = new CharFormat2();
                Format.crBackColor = ColorTranslator.ToOle(value);
                Format.dwMask = 0x4000000;
                Format.cbSize = Marshal.SizeOf(Format);

                SendMessage(this.Handle, 0x444, 1, ref Format);
            }
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct CharFormat2
        {
            public int cbSize;
            public int dwMask;
            public int dwEffects;
            public int yHeight;
            public int yOffset;
            public int crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string szFaceName;
            public short wWeight;
            public short sSpacing;
            public int crBackColor;
            public int lcid;
            public int dwReserved;
            public short sStyle;
            public short wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        private enum EMFlags
        {
            EM_SETSCROLLPOS = 0x4de
        }

        [StructLayout(LayoutKind.Sequential)]
        private class POINT
        {
            public int x;
            public int y;
            public POINT()
            {
            }

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private enum ScrollBarFlags
        {
            SBS_BOTTOMALIGN = 4,
            SBS_HORZ = 0,
            SBS_LEFTALIGN = 2,
            SBS_RIGHTALIGN = 4,
            SBS_SIZEBOX = 8,
            SBS_SIZEBOXBOTTOMRIGHTALIGN = 4,
            SBS_SIZEBOXTOPLEFTALIGN = 2,
            SBS_SIZEGRIP = 0x10,
            SBS_TOPALIGN = 2,
            SBS_VERT = 1
        }

        private enum ScrollBarInfoFlags
        {
            SIF_ALL = 0x17,
            SIF_DISABLENOSCROLL = 8,
            SIF_PAGE = 2,
            SIF_POS = 4,
            SIF_RANGE = 1,
            SIF_TRACKPOS = 0x10
        }

        private enum ScrollBarTypes
        {
            SB_HORZ,
            SB_VERT,
            SB_CTL,
            SB_BOTH
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SCROLLINFO
        {
            public int cbSize;
            public ScrollBarInfoFlags fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            this.ResumeLayout(false);

        }


    }
    
}
