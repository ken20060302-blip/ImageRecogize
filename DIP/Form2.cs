using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DIP
{
    public partial class Form2 : Form
    {
        private Bitmap srcBitmap;

        public Form2()
        {
            InitializeComponent();
        }

        // 接收主視窗傳來的原圖
        public Bitmap OriginalImage
        {
            set
            {
                srcBitmap = new Bitmap(value);
                pictureBox1.Image = srcBitmap;
                pictureBox2.Image = new Bitmap(value);
            }
        }

        // 滑桿拖動事件：即時運算亮度
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (srcBitmap == null) return;

            int offset = trackBar1.Value;
            int w = srcBitmap.Width;
            int h = srcBitmap.Height;

            Bitmap dstBitmap = new Bitmap(w, h, srcBitmap.PixelFormat);
            BitmapData srcData = srcBitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, srcBitmap.PixelFormat);
            BitmapData dstData = dstBitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, dstBitmap.PixelFormat);

            int ByteDepth = srcData.Stride / w;
            int ByteOfSkip = srcData.Stride - w * ByteDepth;

            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;
                byte* dstPtr = (byte*)dstData.Scan0;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        for (int c = 0; c < ByteDepth; c++)
                        {
                            if (c < 3)
                            {
                                int pixelValue = srcPtr[c] + offset;
                                if (pixelValue > 255) pixelValue = 255;
                                else if (pixelValue < 0) pixelValue = 0;
                                dstPtr[c] = (byte)pixelValue;
                            }
                            else
                            {
                                dstPtr[c] = srcPtr[c];
                            }
                        }
                        srcPtr += ByteDepth;
                        dstPtr += ByteDepth;
                    }
                    srcPtr += ByteOfSkip;
                    dstPtr += ByteOfSkip;
                }
            }
            srcBitmap.UnlockBits(srcData);
            dstBitmap.UnlockBits(dstData);

            if (pictureBox2.Image != null) pictureBox2.Image.Dispose();
            pictureBox2.Image = dstBitmap;
        }

        // 按鈕點擊事件：將結果送回主視窗並關閉
        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                DIPSample mainForm = (DIPSample)this.Owner ?? (DIPSample)Application.OpenForms["DIPSample"];
                if (mainForm != null)
                {
                    MSForm resultForm = new MSForm();
                    resultForm.MdiParent = mainForm;
                    // 綁定狀態列，避免載入時 NullReferenceException
                    resultForm.pf1 = mainForm.stStripLabel;
                    resultForm.pBitmap = new Bitmap(pictureBox2.Image);
                    resultForm.Show();
                }
            }
            this.Close();
        }
    }
}