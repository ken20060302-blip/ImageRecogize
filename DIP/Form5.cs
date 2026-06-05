using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DIP
{
    public partial class Form5 : Form
    {
        private Bitmap srcBitmap;
        private int[] histogram = new int[256]; // 用來存放 0~255 每種亮度的數量
        private int maxFreq = 0;                // 記錄數量最多的是多少，用來縮放 Y 軸

        public Form5()
        {
            InitializeComponent();
        }

        // 接收主程式傳來的圖片，並立刻計算直方圖
        public Bitmap TargetImage
        {
            set
            {
                srcBitmap = new Bitmap(value);
                CalculateHistogram(); // 計算頻率
                pictureBox1.Invalidate(); // 通知 PictureBox 重新繪製畫面
            }
        }

        // 核心計算：統計 0~255 各出現幾次
        private void CalculateHistogram()
        {
            // 先將陣列歸零
            Array.Clear(histogram, 0, histogram.Length);
            maxFreq = 0;

            int w = srcBitmap.Width;
            int h = srcBitmap.Height;

            BitmapData srcData = srcBitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, srcBitmap.PixelFormat);
            int ByteDepth = srcData.Stride / w;
            int ByteOfSkip = srcData.Stride - w * ByteDepth;

            unsafe
            {
                byte* srcPtr = (byte*)srcData.Scan0;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        // 假設傳進來的是灰階圖，或是我們只取第一個通道(B)當作亮度代表
                        int grayValue = srcPtr[0];

                        histogram[grayValue]++; // 該亮度的計數器 +1

                        srcPtr += ByteDepth;
                    }
                    srcPtr += ByteOfSkip;
                }
            }
            srcBitmap.UnlockBits(srcData);

            // 找出出現最多次的數量，這樣畫圖的時候才知道 Y 軸最高要定多少
            for (int i = 0; i < 256; i++)
            {
                if (histogram[i] > maxFreq)
                {
                    maxFreq = histogram[i];
                }
            }
        }

        // 繪圖事件：用畫筆把直方圖畫在 PictureBox 上
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (maxFreq == 0) return; // 如果沒有資料就不畫

            Graphics g = e.Graphics;
            g.Clear(Color.White); // 背景塗白

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            // 留一點邊界 (Padding)
            int padding = 20;
            int drawWidth = width - padding * 2;
            int drawHeight = height - padding * 2;

            // 畫 X 軸和 Y 軸
            Pen axisPen = new Pen(Color.Black, 2);
            g.DrawLine(axisPen, padding, height - padding, width - padding, height - padding); // X軸
            g.DrawLine(axisPen, padding, padding, padding, height - padding); // Y軸

            // 畫 256 條直線
            Pen barPen = new Pen(Color.SteelBlue, 1); // 這裡的顏色你可以自由更換

            for (int i = 0; i < 256; i++)
            {
                // 計算這條線的 X 座標 (將 256 均勻映射到畫布寬度上)
                float x = padding + ((float)i / 255) * drawWidth;

                // 計算這條線的高度 (按比例縮放)
                float barHeight = ((float)histogram[i] / maxFreq) * drawHeight;
                float y = height - padding - barHeight;

                // 畫一條直線從底部畫到計算出的高度
                g.DrawLine(barPen, x, height - padding, x, y);
            }

            // 標示文字 (0 和 255)
            Font font = new Font("Arial", 10);
            Brush brush = Brushes.Black;
            g.DrawString("0", font, brush, padding, height - padding + 2);
            g.DrawString("255", font, brush, width - padding - 20, height - padding + 2);
            g.DrawString(maxFreq.ToString(), font, brush, 0, padding);
        }
    }
}