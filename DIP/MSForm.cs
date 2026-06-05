using System;
using System.Drawing;
using System.Windows.Forms;

namespace DIP
{
    public partial class MSForm : Form
    {
        internal Bitmap pBitmap;
        internal ToolStripStatusLabel pf1;
        int w, h;

        public MSForm()
        {
            InitializeComponent();
        }

        private void MSForm_Load(object sender, EventArgs e)
        {
            // 防呆檢查：確保圖片存在才顯示
            if (pBitmap != null)
            {
                // 如果你有寫 bmp_dip 這個方法就留著，不然直接用下面那行
                // bmp_dip(pBitmap, pictureBox1); 
                pictureBox1.Image = pBitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                w = pBitmap.Width;
                h = pBitmap.Height;

                // 防呆檢查：確保有抓到主視窗的狀態列，才寫入文字
                if (pf1 != null)
                {
                    pf1.Text = "(Width,Height)=(" + w + "," + h + ")";
                }
            }
        }
    }
}