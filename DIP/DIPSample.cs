using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DIP
{
    public partial class DIPSample : Form
    {
        #region 全域變數與表單初始化
        Bitmap NpBitmap;
        int[] f;
        int[] g;
        int w, h;

        public DIPSample()
        {
            InitializeComponent();
        }

        private void DIPSample_Load(object sender, EventArgs e)
        {
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.stStripLabel.Text = "";
        }
        #endregion

        #region 1. 開檔與基礎操作
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oFileDlg.CheckFileExists = true;
            oFileDlg.CheckPathExists = true;
            oFileDlg.Title = "Open File - DIP Sample";
            oFileDlg.ValidateNames = true;
            oFileDlg.Filter = "bmp files (*.bmp)|*.bmp";
            oFileDlg.FileName = "";

            if (oFileDlg.ShowDialog() == DialogResult.OK)
            {
                MSForm childForm = new MSForm();
                childForm.MdiParent = this;
                childForm.pf1 = stStripLabel;

                NpBitmap = new Bitmap(oFileDlg.FileName);
                w = NpBitmap.Width;
                h = NpBitmap.Height;

                childForm.pBitmap = NpBitmap;
                childForm.Show();
            }
        }
        #endregion

        #region 核心底層：Bitmap ↔ 一維陣列 轉換
        private int[] dyn_bmp2array(Bitmap myBitmap, ref int ByteDepth, ref PixelFormat pixelFormat, ref ColorPalette palette)
        {
            BitmapData byteArray = myBitmap.LockBits(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), ImageLockMode.ReadWrite, myBitmap.PixelFormat);
            pixelFormat = myBitmap.PixelFormat;
            palette = myBitmap.Palette;
            ByteDepth = byteArray.Stride / myBitmap.Width;

            int[] ImgData = new int[myBitmap.Width * myBitmap.Height * ByteDepth];
            int ByteOfSkip = byteArray.Stride - byteArray.Width * ByteDepth;

            unsafe
            {
                byte* imgPtr = (byte*)byteArray.Scan0;
                for (int y = 0; y < byteArray.Height; y++)
                {
                    for (int x = 0; x < byteArray.Width; x++)
                    {
                        for (int c = 0; c < ByteDepth; c++)
                        {
                            ImgData[(x + byteArray.Width * y) * ByteDepth + c] = *imgPtr;
                            imgPtr++;
                        }
                    }
                    imgPtr += ByteOfSkip;
                }
            }
            myBitmap.UnlockBits(byteArray);
            return ImgData;
        }

        private static Bitmap dyn_array2bmp(int[] ImgData, int w, int h, int ByteDepth, PixelFormat pixelFormat, ColorPalette palette)
        {
            Bitmap myBitmap = new Bitmap(w, h, pixelFormat);
            BitmapData byteArray = myBitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, pixelFormat);
            try { myBitmap.Palette = palette; } catch { }

            int ByteOfSkip = byteArray.Stride - w * ByteDepth;
            unsafe
            {
                byte* imgPtr = (byte*)byteArray.Scan0;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        for (int c = 0; c < ByteDepth; c++)
                        {
                            *imgPtr = (byte)ImgData[(x + w * y) * ByteDepth + c];
                            imgPtr++;
                        }
                    }
                    imgPtr += ByteOfSkip;
                }
            }
            myBitmap.UnlockBits(byteArray);
            return myBitmap;
        }
        #endregion

        #region 2. 影像轉換功能 (RGB to Gray, 負片, 位元切面)
        private void RGBtoGrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (MSForm cF in MdiChildren)
            {
                if (!cF.Focused) continue;
                int ByteDepth = 0;
                PixelFormat pixelFormat = new PixelFormat();
                ColorPalette palette = null;
                int imgW = cF.pBitmap.Width;
                int imgH = cF.pBitmap.Height;

                f = dyn_bmp2array(cF.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
                g = new int[imgW * imgH * ByteDepth];

                unsafe
                {
                    fixed (int* f0 = f) fixed (int* g0 = g)
                    {
                        for (int i = 0; i < imgH; i++)
                        {
                            for (int j = 0; j < imgW; j++)
                            {
                                int idx = (i * imgW + j) * ByteDepth;
                                int b = f0[idx + 0];
                                int gVal = f0[idx + 1];
                                int r = f0[idx + 2];
                                int gray = (int)(r * 0.299 + gVal * 0.587 + b * 0.114);

                                g0[idx + 0] = gray;
                                g0[idx + 1] = gray;
                                g0[idx + 2] = gray;
                                if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                            }
                        }
                    }
                }
                NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);

                MSForm childForm = new MSForm();
                childForm.MdiParent = this;
                childForm.pBitmap = NpBitmap;
                childForm.Show();
                break;
            }
        }

        private void 負片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (MSForm cF in MdiChildren)
            {
                if (!cF.Focused) continue;
                int ByteDepth = 0;
                PixelFormat pixelFormat = new PixelFormat();
                ColorPalette palette = null;
                int imgW = cF.pBitmap.Width;
                int imgH = cF.pBitmap.Height;

                f = dyn_bmp2array(cF.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
                g = new int[imgW * imgH * ByteDepth];

                unsafe
                {
                    fixed (int* f0 = f) fixed (int* g0 = g)
                    {
                        for (int i = 0; i < imgH; i++)
                        {
                            for (int j = 0; j < imgW; j++)
                            {
                                int idx = (i * imgW + j) * ByteDepth;
                                if (ByteDepth >= 3)
                                {
                                    g0[idx + 0] = 255 - f0[idx + 0];
                                    g0[idx + 1] = 255 - f0[idx + 1];
                                    g0[idx + 2] = 255 - f0[idx + 2];
                                    if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                                }
                                else if (ByteDepth == 1)
                                {
                                    g0[idx] = 255 - f0[idx];
                                }
                            }
                        }
                    }
                }
                NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);

                MSForm childForm = new MSForm();
                childForm.MdiParent = this;
                childForm.pBitmap = NpBitmap;
                childForm.Show();
                break;
            }
        }

        private void BitSection(int targetBit)
        {
            foreach (MSForm cF in MdiChildren)
            {
                if (!cF.Focused) continue;
                int ByteDepth = 0;
                PixelFormat pixelFormat = new PixelFormat();
                ColorPalette palette = null;
                int imgW = cF.pBitmap.Width;
                int imgH = cF.pBitmap.Height;

                f = dyn_bmp2array(cF.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
                g = new int[imgW * imgH * ByteDepth];

                unsafe
                {
                    fixed (int* f0 = f) fixed (int* g0 = g)
                    {
                        for (int i = 0; i < imgH; i++)
                        {
                            for (int j = 0; j < imgW; j++)
                            {
                                int idx = (i * imgW + j) * ByteDepth;
                                if (ByteDepth >= 3)
                                {
                                    g0[idx + 0] = ((f0[idx + 0] >> targetBit) & 1) == 1 ? 255 : 0;
                                    g0[idx + 1] = ((f0[idx + 1] >> targetBit) & 1) == 1 ? 255 : 0;
                                    g0[idx + 2] = ((f0[idx + 2] >> targetBit) & 1) == 1 ? 255 : 0;
                                    if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                                }
                                else if (ByteDepth == 1)
                                {
                                    g0[idx] = ((f0[idx] >> targetBit) & 1) == 1 ? 255 : 0;
                                }
                            }
                        }
                    }
                }
                NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);

                MSForm childForm = new MSForm();
                childForm.MdiParent = this;
                childForm.pBitmap = NpBitmap;
                childForm.Show();
                break;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) { BitSection(0); }
        private void toolStripMenuItem3_Click(object sender, EventArgs e) { BitSection(1); }
        private void toolStripMenuItem4_Click(object sender, EventArgs e) { BitSection(2); }
        private void toolStripMenuItem5_Click(object sender, EventArgs e) { BitSection(3); }
        private void toolStripMenuItem6_Click(object sender, EventArgs e) { BitSection(4); }
        private void toolStripMenuItem7_Click(object sender, EventArgs e) { BitSection(5); }
        private void toolStripMenuItem8_Click(object sender, EventArgs e) { BitSection(6); }
        private void toolStripMenuItem9_Click(object sender, EventArgs e) { BitSection(7); }
        #endregion

        #region 3. 亮度與對比 (呼叫子視窗 Form2, Form3)
        private void 亮度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null) return;

            Form2 f2 = new Form2();
            f2.OriginalImage = currentForm.pBitmap;
            f2.ShowDialog();
        }


        // ==========================================
        // 1. 對比功能 (呼叫 Form3)
        // ==========================================
        private void 對比ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null)
            {
                MessageBox.Show("請先開啟一張圖片！");
                return;
            }

            Form3 f3 = new Form3();
            f3.OriginalImage = currentForm.pBitmap;
            f3.ShowDialog();
        }

        // ==========================================
        // 2. 顯示直方圖功能 (呼叫 Form5)
        // ==========================================
        private void 顯示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null)
            {
                MessageBox.Show("請先開啟一張圖片！");
                return;
            }

            Form5 f5 = new Form5();
            f5.TargetImage = currentForm.pBitmap;
            f5.Text = "直方圖";
            f5.Show();
        }

        private void 平均ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int imgW = currentForm.pBitmap.Width; int imgH = currentForm.pBitmap.Height;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[f.Length];
            Array.Copy(f, g, f.Length); // 先複製原圖，保留邊界

            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    // 避開最外圍的 1 像素邊界
                    for (int i = 1; i < imgH - 1; i++)
                    {
                        for (int j = 1; j < imgW - 1; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;

                            if (ByteDepth >= 3)
                            {
                                int sumB = 0, sumG = 0, sumR = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int nIdx = ((i + dy) * imgW + (j + dx)) * ByteDepth;
                                        sumB += f0[nIdx + 0];
                                        sumG += f0[nIdx + 1];
                                        sumR += f0[nIdx + 2];
                                    }
                                }
                                g0[idx + 0] = sumB / 9;
                                g0[idx + 1] = sumG / 9;
                                g0[idx + 2] = sumR / 9;
                                if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                            }
                            else if (ByteDepth == 1)
                            {
                                int sum = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        sum += f0[((i + dy) * imgW + (j + dx)) * ByteDepth];
                                    }
                                }
                                g0[idx] = sum / 9;
                            }
                        }
                    }
                }
            }

            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Text = "平均濾波 (Mean)";
            childForm.Show();
        }

        private void 中值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int imgW = currentForm.pBitmap.Width; int imgH = currentForm.pBitmap.Height;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[f.Length];
            Array.Copy(f, g, f.Length);

            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    // 準備 9 個元素的陣列來做排序
                    int[] winB = new int[9]; int[] winG = new int[9]; int[] winR = new int[9]; int[] winGray = new int[9];

                    for (int i = 1; i < imgH - 1; i++)
                    {
                        for (int j = 1; j < imgW - 1; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;

                            if (ByteDepth >= 3)
                            {
                                int k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int nIdx = ((i + dy) * imgW + (j + dx)) * ByteDepth;
                                        winB[k] = f0[nIdx + 0];
                                        winG[k] = f0[nIdx + 1];
                                        winR[k] = f0[nIdx + 2];
                                        k++;
                                    }
                                }
                                Array.Sort(winB); Array.Sort(winG); Array.Sort(winR);
                                g0[idx + 0] = winB[4]; // 取第 5 個數字 (index 4) 就是中位數
                                g0[idx + 1] = winG[4];
                                g0[idx + 2] = winR[4];
                                if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                            }
                            else if (ByteDepth == 1)
                            {
                                int k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        winGray[k++] = f0[((i + dy) * imgW + (j + dx)) * ByteDepth];
                                    }
                                }
                                Array.Sort(winGray);
                                g0[idx] = winGray[4];
                            }
                        }
                    }
                }
            }

            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Text = "中值濾波 (Median)";
            childForm.Show();
        }

        private void 高斯ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int imgW = currentForm.pBitmap.Width; int imgH = currentForm.pBitmap.Height;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[f.Length];
            Array.Copy(f, g, f.Length);

            // 經典的 3x3 高斯遮罩權重
            int[] kernel = { 1, 2, 1,
                     2, 4, 2,
                     1, 2, 1 };

            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    for (int i = 1; i < imgH - 1; i++)
                    {
                        for (int j = 1; j < imgW - 1; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;

                            if (ByteDepth >= 3)
                            {
                                int sumB = 0, sumG = 0, sumR = 0, k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int nIdx = ((i + dy) * imgW + (j + dx)) * ByteDepth;
                                        sumB += f0[nIdx + 0] * kernel[k];
                                        sumG += f0[nIdx + 1] * kernel[k];
                                        sumR += f0[nIdx + 2] * kernel[k];
                                        k++;
                                    }
                                }
                                // 總權重是 16
                                g0[idx + 0] = sumB / 16;
                                g0[idx + 1] = sumG / 16;
                                g0[idx + 2] = sumR / 16;
                                if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                            }
                            else if (ByteDepth == 1)
                            {
                                int sum = 0, k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        sum += f0[((i + dy) * imgW + (j + dx)) * ByteDepth] * kernel[k++];
                                    }
                                }
                                g0[idx] = sum / 16;
                            }
                        }
                    }
                }
            }

            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Text = "高斯濾波 (Gaussian)";
            childForm.Show();
        }

        private void 銳化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int imgW = currentForm.pBitmap.Width; int imgH = currentForm.pBitmap.Height;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[f.Length];
            Array.Copy(f, g, f.Length);

            // 基本銳化遮罩 (中心為5，四周扣除)
            int[] kernel = {  0, -1,  0,
                     -1,  5, -1,
                      0, -1,  0 };

            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    for (int i = 1; i < imgH - 1; i++)
                    {
                        for (int j = 1; j < imgW - 1; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;

                            if (ByteDepth >= 3)
                            {
                                int sumB = 0, sumG = 0, sumR = 0, k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int nIdx = ((i + dy) * imgW + (j + dx)) * ByteDepth;
                                        sumB += f0[nIdx + 0] * kernel[k];
                                        sumG += f0[nIdx + 1] * kernel[k];
                                        sumR += f0[nIdx + 2] * kernel[k++];
                                    }
                                }
                                // 限制數值在 0~255 之間
                                g0[idx + 0] = Math.Max(0, Math.Min(255, sumB));
                                g0[idx + 1] = Math.Max(0, Math.Min(255, sumG));
                                g0[idx + 2] = Math.Max(0, Math.Min(255, sumR));
                                if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                            }
                            else if (ByteDepth == 1)
                            {
                                int sum = 0, k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        sum += f0[((i + dy) * imgW + (j + dx)) * ByteDepth] * kernel[k++];
                                    }
                                }
                                g0[idx] = Math.Max(0, Math.Min(255, sum));
                            }
                        }
                    }
                }
            }

            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm(); childForm.MdiParent = this; childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap; childForm.Text = "銳化 (Sharpen)"; childForm.Show();
        }

        private void 拉普拉斯銳化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int imgW = currentForm.pBitmap.Width; int imgH = currentForm.pBitmap.Height;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[f.Length];
            Array.Copy(f, g, f.Length);

            // 強烈拉普拉斯銳化遮罩 (中心為9，八個方向全扣除)
            int[] kernel = { -1, -1, -1,
                     -1,  9, -1,
                     -1, -1, -1 };

            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    for (int i = 1; i < imgH - 1; i++)
                    {
                        for (int j = 1; j < imgW - 1; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;

                            if (ByteDepth >= 3)
                            {
                                int sumB = 0, sumG = 0, sumR = 0, k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int nIdx = ((i + dy) * imgW + (j + dx)) * ByteDepth;
                                        sumB += f0[nIdx + 0] * kernel[k];
                                        sumG += f0[nIdx + 1] * kernel[k];
                                        sumR += f0[nIdx + 2] * kernel[k++];
                                    }
                                }
                                g0[idx + 0] = Math.Max(0, Math.Min(255, sumB));
                                g0[idx + 1] = Math.Max(0, Math.Min(255, sumG));
                                g0[idx + 2] = Math.Max(0, Math.Min(255, sumR));
                                if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                            }
                            else if (ByteDepth == 1)
                            {
                                int sum = 0, k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        sum += f0[((i + dy) * imgW + (j + dx)) * ByteDepth] * kernel[k++];
                                    }
                                }
                                g0[idx] = Math.Max(0, Math.Min(255, sum));
                            }
                        }
                    }
                }
            }

            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm(); childForm.MdiParent = this; childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap; childForm.Text = "拉普拉斯銳化 (Laplacian Sharpen)"; childForm.Show();
        }

        private void prewittToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int imgW = currentForm.pBitmap.Width; int imgH = currentForm.pBitmap.Height;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[f.Length];
            // 邊緣偵測預設背景為黑色，所以這裡不用 Copy 原圖

            int[] Gx = { -1, 0, 1, -1, 0, 1, -1, 0, 1 }; // 水平遮罩
            int[] Gy = { -1, -1, -1, 0, 0, 0, 1, 1, 1 }; // 垂直遮罩

            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    for (int i = 1; i < imgH - 1; i++)
                    {
                        for (int j = 1; j < imgW - 1; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;

                            if (ByteDepth >= 3)
                            {
                                double sumBx = 0, sumBy = 0, sumGx = 0, sumGy = 0, sumRx = 0, sumRy = 0;
                                int k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int nIdx = ((i + dy) * imgW + (j + dx)) * ByteDepth;
                                        sumBx += f0[nIdx + 0] * Gx[k]; sumBy += f0[nIdx + 0] * Gy[k];
                                        sumGx += f0[nIdx + 1] * Gx[k]; sumGy += f0[nIdx + 1] * Gy[k];
                                        sumRx += f0[nIdx + 2] * Gx[k]; sumRy += f0[nIdx + 2] * Gy[k];
                                        k++;
                                    }
                                }
                                // 畢氏定理：強度 = √(Gx^2 + Gy^2)
                                g0[idx + 0] = Math.Max(0, Math.Min(255, (int)Math.Sqrt(sumBx * sumBx + sumBy * sumBy)));
                                g0[idx + 1] = Math.Max(0, Math.Min(255, (int)Math.Sqrt(sumGx * sumGx + sumGy * sumGy)));
                                g0[idx + 2] = Math.Max(0, Math.Min(255, (int)Math.Sqrt(sumRx * sumRx + sumRy * sumRy)));
                                if (ByteDepth == 4) g0[idx + 3] = 255;
                            }
                            else if (ByteDepth == 1)
                            {
                                double sumX = 0, sumY = 0; int k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int val = f0[((i + dy) * imgW + (j + dx)) * ByteDepth];
                                        sumX += val * Gx[k]; sumY += val * Gy[k];
                                        k++;
                                    }
                                }
                                g0[idx] = Math.Max(0, Math.Min(255, (int)Math.Sqrt(sumX * sumX + sumY * sumY)));
                            }
                        }
                    }
                }
            }

            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm(); childForm.MdiParent = this; childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap; childForm.Text = "Prewitt 邊緣偵測"; childForm.Show();
        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int imgW = currentForm.pBitmap.Width; int imgH = currentForm.pBitmap.Height;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[f.Length];

            // Sobel 將中間權重加倍 (2)
            int[] Gx = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            int[] Gy = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };

            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    for (int i = 1; i < imgH - 1; i++)
                    {
                        for (int j = 1; j < imgW - 1; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;

                            if (ByteDepth >= 3)
                            {
                                double sumBx = 0, sumBy = 0, sumGx = 0, sumGy = 0, sumRx = 0, sumRy = 0;
                                int k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int nIdx = ((i + dy) * imgW + (j + dx)) * ByteDepth;
                                        sumBx += f0[nIdx + 0] * Gx[k]; sumBy += f0[nIdx + 0] * Gy[k];
                                        sumGx += f0[nIdx + 1] * Gx[k]; sumGy += f0[nIdx + 1] * Gy[k];
                                        sumRx += f0[nIdx + 2] * Gx[k]; sumRy += f0[nIdx + 2] * Gy[k];
                                        k++;
                                    }
                                }
                                g0[idx + 0] = Math.Max(0, Math.Min(255, (int)Math.Sqrt(sumBx * sumBx + sumBy * sumBy)));
                                g0[idx + 1] = Math.Max(0, Math.Min(255, (int)Math.Sqrt(sumGx * sumGx + sumGy * sumGy)));
                                g0[idx + 2] = Math.Max(0, Math.Min(255, (int)Math.Sqrt(sumRx * sumRx + sumRy * sumRy)));
                                if (ByteDepth == 4) g0[idx + 3] = 255;
                            }
                            else if (ByteDepth == 1)
                            {
                                double sumX = 0, sumY = 0; int k = 0;
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    for (int dx = -1; dx <= 1; dx++)
                                    {
                                        int val = f0[((i + dy) * imgW + (j + dx)) * ByteDepth];
                                        sumX += val * Gx[k]; sumY += val * Gy[k];
                                        k++;
                                    }
                                }
                                g0[idx] = Math.Max(0, Math.Min(255, (int)Math.Sqrt(sumX * sumX + sumY * sumY)));
                            }
                        }
                    }
                }
            }

            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm(); childForm.MdiParent = this; childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap; childForm.Text = "Sobel 邊緣偵測"; childForm.Show();
        }

        private void 線偵測ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int imgW = currentForm.pBitmap.Width;
            int imgH = currentForm.pBitmap.Height;
            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);

            // 1. 蒐集所有邊緣點 (配合小畫家白底黑線，找暗點)
            // ⚠️注意：如果你前面是用負片轉成黑底白線了，請把 < 128 改回 > 128
            System.Collections.Generic.List<Point> edges = new System.Collections.Generic.List<Point>();
            unsafe
            {
                fixed (int* f0 = f)
                {
                    for (int y = 0; y < imgH; y++)
                    {
                        for (int x = 0; x < imgW; x++)
                        {
                            int val = f0[(y * imgW + x) * ByteDepth];
                            if (val < 128) edges.Add(new Point(x, y)); // 找黑線
                        }
                    }
                }
            }

            // 2. 準備霍夫轉換投票箱
            int diag = (int)Math.Sqrt(imgW * imgW + imgH * imgH);
            int[,] acc = new int[180, 2 * diag];

            double[] cosT = new double[180];
            double[] sinT = new double[180];
            for (int t = 0; t < 180; t++)
            {
                cosT[t] = Math.Cos(t * Math.PI / 180.0);
                sinT[t] = Math.Sin(t * Math.PI / 180.0);
            }

            // 3. 開始投票
            foreach (Point pt in edges)
            {
                for (int t = 0; t < 180; t++)
                {
                    int r = (int)Math.Round(pt.X * cosT[t] + pt.Y * sinT[t]);
                    acc[t, r + diag]++;
                }
            }

            // 4. 找出票數最高的方程式
            int maxVote = 0;
            foreach (int v in acc) if (v > maxVote) maxVote = v;
            int threshold = (int)(maxVote * 0.5);

            System.Collections.Generic.List<Point> lines = new System.Collections.Generic.List<Point>();
            for (int t = 1; t < 179; t++)
            {
                for (int r = 1; r < 2 * diag - 1; r++)
                {
                    if (acc[t, r] > threshold &&
                        acc[t, r] > acc[t - 1, r] && acc[t, r] > acc[t + 1, r] &&
                        acc[t, r] > acc[t, r - 1] && acc[t, r] > acc[t, r + 1])
                    {
                        lines.Add(new Point(t, r));
                    }
                }
            }

            // ==========================================
            // 5. 畫出結果 (進化版：尋找線段端點)
            // ==========================================
            Bitmap resultBmp = new Bitmap(imgW, imgH, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(resultBmp))
            {
                g.DrawImage(currentForm.pBitmap, 0, 0);
                Pen redPen = new Pen(Color.Red, 3); // 把線調粗一點比較好看

                foreach (Point p in lines)
                {
                    int t = p.X;
                    int r_actual = p.Y - diag;
                    double theta = t * Math.PI / 180.0;
                    double ct = Math.Cos(theta), st = Math.Sin(theta);

                    // 判斷這條線是偏向「水平」還是「垂直」
                    bool isHorizontal = Math.Abs(st) > Math.Abs(ct);

                    Point pStart = new Point(0, 0);
                    Point pEnd = new Point(0, 0);
                    bool found = false;

                    // 回頭去找哪些黑點在這條線上
                    foreach (Point ep in edges)
                    {
                        // 算出這個黑點依照這個角度，算出來的 r 是多少
                        double expected_r = ep.X * ct + ep.Y * st;

                        // 如果算出來的 r 幾乎等於我們偵測到的 r (容忍度設為 3 像素)
                        if (Math.Abs(expected_r - r_actual) <= 3.0)
                        {
                            if (!found)
                            {
                                pStart = ep; pEnd = ep;
                                found = true;
                            }
                            else
                            {
                                // 如果偏水平，就找最左邊跟最右邊的 X 座標
                                if (isHorizontal)
                                {
                                    if (ep.X < pStart.X) pStart = ep;
                                    if (ep.X > pEnd.X) pEnd = ep;
                                }
                                // 如果偏垂直，就找最上面跟最下面的 Y 座標
                                else
                                {
                                    if (ep.Y < pStart.Y) pStart = ep;
                                    if (ep.Y > pEnd.Y) pEnd = ep;
                                }
                            }
                        }
                    }

                    // 如果有找到端點，就只畫這兩個端點之間的線段！
                    if (found)
                    {
                        g.DrawLine(redPen, pStart.X, pStart.Y, pEnd.X, pEnd.Y);
                    }
                }
            }

            MSForm childForm = new MSForm(); childForm.MdiParent = this; childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = resultBmp; childForm.Text = "Hough 線段偵測"; childForm.Show();
        }

        private void ostu分割ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int imgW = currentForm.pBitmap.Width;
            int imgH = currentForm.pBitmap.Height;
            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[f.Length];

            // ==========================================
            // 1. 統計直方圖 (PDF)
            // ==========================================
            int[] histogram = new int[256];
            unsafe
            {
                fixed (int* f0 = f)
                {
                    for (int i = 0; i < imgH; i++)
                    {
                        for (int j = 0; j < imgW; j++)
                        {
                            // 假設已經轉成灰階，我們取第一個通道的值當作亮度
                            int val = f0[(i * imgW + j) * ByteDepth];
                            histogram[val]++;
                        }
                    }
                }
            }

            // ==========================================
            // 2. Otsu 演算法：尋找最佳閾值 (Threshold)
            // ==========================================
            int totalPixels = imgW * imgH;
            float sum = 0;
            for (int i = 0; i < 256; i++) sum += i * histogram[i];

            float sumB = 0; // 背景的亮度總和
            int wB = 0;     // 背景的像素總數
            int wF = 0;     // 前景的像素總數

            float varMax = 0;   // 記錄最大的類間變異數
            int threshold = 0;  // 記錄最佳的閾值

            for (int i = 0; i < 256; i++)
            {
                wB += histogram[i];               // 背景像素數累加
                if (wB == 0) continue;            // 避免除以零

                wF = totalPixels - wB;            // 前景像素數 = 總數 - 背景數
                if (wF == 0) break;               // 已經算到最後了，提早結束

                sumB += (float)(i * histogram[i]); // 背景亮度累加

                float mB = sumB / wB;             // 背景平均亮度
                float mF = (sum - sumB) / wF;     // 前景平均亮度

                // 計算類間變異數 (Between Class Variance)
                float varBetween = (float)wB * (float)wF * (mB - mF) * (mB - mF);

                // 如果找到更大的變異數，就更新最佳閾值
                if (varBetween > varMax)
                {
                    varMax = varBetween;
                    threshold = i;
                }
            }

            // ==========================================
            // 3. 根據算出來的最佳閾值，將圖片二值化 (0 或 255)
            // ==========================================
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    for (int i = 0; i < imgH; i++)
                    {
                        for (int j = 0; j < imgW; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;

                            // 取出亮度
                            int val = f0[idx];

                            // 如果大於等於閾值，設為純白(255)，否則設為純黑(0)
                            int binaryVal = (val >= threshold) ? 255 : 0;

                            if (ByteDepth >= 3)
                            {
                                g0[idx + 0] = binaryVal;
                                g0[idx + 1] = binaryVal;
                                g0[idx + 2] = binaryVal;
                                if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3]; // 保留透明度
                            }
                            else if (ByteDepth == 1)
                            {
                                g0[idx] = binaryVal;
                            }
                        }
                    }
                }
            }

            // 4. 顯示結果 (將算出的閾值顯示在視窗標題上，跟你照片一模一樣！)
            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Text = $"Otsu 分割 (Otsu 閾值 : {threshold})"; // 動態顯示算出來的閾值
            childForm.Show();
        }

        // 1. 動態生成「滑桿」輸入視窗
        private int ShowSliderAngleDialog()
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "滑桿選擇旋轉角度",
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };

            Label textLabel = new Label() { Left = 20, Top = 20, Width = 350, Text = "請拖曳滑桿選擇角度: 0 度" };

            // 建立滑桿元件 (設定範圍為 -180度 到 180度)
            TrackBar trackBar = new TrackBar()
            {
                Left = 20,
                Top = 50,
                Width = 340,
                Minimum = -180,
                Maximum = 180,
                Value = 0,
                TickFrequency = 15 // 每 15 度顯示一個刻度
            };

            Button confirmation = new Button() { Text = "確定", Left = 140, Width = 100, Top = 110, DialogResult = DialogResult.OK };

            // 當滑桿被拖動時，即時更新 Label 上面的文字
            trackBar.ValueChanged += (sender, e) => {
                textLabel.Text = $"請拖曳滑桿選擇角度: {trackBar.Value} 度";
            };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(trackBar);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            // 如果按下確定，就回傳滑桿的數字；如果是按叉叉取消，就回傳 0
            return prompt.ShowDialog() == DialogResult.OK ? trackBar.Value : 0;
        }

        // 2. 旋轉核心功能 (稍微修改以接收滑桿的整數值)
        private void 旋轉ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null) { MessageBox.Show("請先開啟一張圖片！"); return; }

            int angleDegrees = 0;

            // --- 1. 呼叫 Form6 開啟即時拖曳預覽視窗 ---
            using (Form6 f6 = new Form6(currentForm.pBitmap))
            {
                if (f6.ShowDialog() == DialogResult.OK)
                {
                    angleDegrees = f6.SelectedAngle; // 抓取使用者最後決定的角度
                }
                else
                {
                    return; // 按了叉叉或取消，就不處理
                }
            }

            if (angleDegrees == 0) return; // 沒轉就不浪費時間算

            // --- 2. 確定後，執行精準的反向映射演算法 (不裁切、無破洞) ---
            int imgW = currentForm.pBitmap.Width;
            int imgH = currentForm.pBitmap.Height;
            int ByteDepth = 0; PixelFormat pixelFormat = new PixelFormat(); ColorPalette palette = null;
            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);

            double rad = angleDegrees * Math.PI / 180.0;
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);

            int newW = (int)Math.Round(Math.Abs(imgW * cos) + Math.Abs(imgH * sin));
            int newH = (int)Math.Round(Math.Abs(imgW * sin) + Math.Abs(imgH * cos));

            int[] g = new int[newW * newH * ByteDepth];

            double cx = imgW / 2.0;
            double cy = imgH / 2.0;
            double ncx = newW / 2.0;
            double ncy = newH / 2.0;

            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    for (int y = 0; y < newH; y++)
                    {
                        for (int x = 0; x < newW; x++)
                        {
                            double dx = x - ncx;
                            double dy = y - ncy;

                            int srcX = (int)Math.Round(cx + (dx * cos + dy * sin));
                            int srcY = (int)Math.Round(cy + (-dx * sin + dy * cos));

                            int dstIdx = (y * newW + x) * ByteDepth;

                            if (srcX >= 0 && srcX < imgW && srcY >= 0 && srcY < imgH)
                            {
                                int srcIdx = (srcY * imgW + srcX) * ByteDepth;

                                if (ByteDepth >= 3)
                                {
                                    g0[dstIdx + 0] = f0[srcIdx + 0];
                                    g0[dstIdx + 1] = f0[srcIdx + 1];
                                    g0[dstIdx + 2] = f0[srcIdx + 2];
                                    if (ByteDepth == 4) g0[dstIdx + 3] = f0[srcIdx + 3];
                                }
                                else if (ByteDepth == 1)
                                {
                                    g0[dstIdx] = f0[srcIdx];
                                }
                            }
                            else
                            {
                                // 留黑邊
                                if (ByteDepth >= 3)
                                {
                                    g0[dstIdx + 0] = 0; g0[dstIdx + 1] = 0; g0[dstIdx + 2] = 0;
                                    if (ByteDepth == 4) g0[dstIdx + 3] = 255;
                                }
                                else if (ByteDepth == 1)
                                {
                                    g0[dstIdx] = 0;
                                }
                            }
                        }
                    }
                }
            }

            Bitmap NpBitmap = dyn_array2bmp(g, newW, newH, ByteDepth, pixelFormat, palette);
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Text = $"旋轉 {angleDegrees} 度";
            childForm.Show();
        }

        // ==========================================
        // 3. 等化功能 (演算法 + 顯示新圖片與直方圖)
        // ==========================================
        private void 等化ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null || currentForm.pBitmap == null)
            {
                MessageBox.Show("請先開啟一張圖片！");
                return;
            }

            int ByteDepth = 0;
            PixelFormat pixelFormat = new PixelFormat();
            ColorPalette palette = null;
            int imgW = currentForm.pBitmap.Width;
            int imgH = currentForm.pBitmap.Height;
            int totalPixels = imgW * imgH;

            int[] f = dyn_bmp2array(currentForm.pBitmap, ref ByteDepth, ref pixelFormat, ref palette);
            int[] g = new int[imgW * imgH * ByteDepth];

            // --- 第一步：統計直方圖 (PDF) ---
            int[] histB = new int[256];
            int[] histG = new int[256];
            int[] histR = new int[256];

            unsafe
            {
                fixed (int* f0 = f)
                {
                    for (int i = 0; i < imgH; i++)
                    {
                        for (int j = 0; j < imgW; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;
                            if (ByteDepth >= 3)
                            {
                                histB[f0[idx + 0]]++;
                                histG[f0[idx + 1]]++;
                                histR[f0[idx + 2]]++;
                            }
                            else if (ByteDepth == 1)
                            {
                                histB[f0[idx]]++;
                            }
                        }
                    }
                }
            }

            // --- 第二步：計算累計分佈函數 (CDF) ---
            int[] cdfB = new int[256];
            int[] cdfG = new int[256];
            int[] cdfR = new int[256];

            cdfB[0] = histB[0];
            cdfG[0] = histG[0];
            cdfR[0] = histR[0];

            for (int i = 1; i < 256; i++)
            {
                cdfB[i] = cdfB[i - 1] + histB[i];
                cdfG[i] = cdfG[i - 1] + histG[i];
                cdfR[i] = cdfR[i - 1] + histR[i];
            }

            // --- 第三步：像素重映射 (Mapping) ---
            unsafe
            {
                fixed (int* f0 = f) fixed (int* g0 = g)
                {
                    for (int i = 0; i < imgH; i++)
                    {
                        for (int j = 0; j < imgW; j++)
                        {
                            int idx = (i * imgW + j) * ByteDepth;
                            if (ByteDepth >= 3)
                            {
                                g0[idx + 0] = (int)Math.Round((cdfB[f0[idx + 0]] * 255.0) / totalPixels);
                                g0[idx + 1] = (int)Math.Round((cdfG[f0[idx + 1]] * 255.0) / totalPixels);
                                g0[idx + 2] = (int)Math.Round((cdfR[f0[idx + 2]] * 255.0) / totalPixels);
                                if (ByteDepth == 4) g0[idx + 3] = f0[idx + 3];
                            }
                            else if (ByteDepth == 1)
                            {
                                g0[idx] = (int)Math.Round((cdfB[f0[idx]] * 255.0) / totalPixels);
                            }
                        }
                    }
                }
            }

            // 4. 將處理完的陣列轉回 Bitmap
            Bitmap NpBitmap = dyn_array2bmp(g, imgW, imgH, ByteDepth, pixelFormat, palette);

            // 顯示等化後的圖片
            MSForm childForm = new MSForm();
            childForm.MdiParent = this;
            childForm.pf1 = this.stStripLabel;
            childForm.pBitmap = NpBitmap;
            childForm.Text = "等化後圖片";
            childForm.Show();

            // 顯示等化後的直方圖
            Form5 eqHistogramForm = new Form5();
            eqHistogramForm.Text = "等化後直方圖";
            eqHistogramForm.TargetImage = NpBitmap;
            eqHistogramForm.Show();
        }

        #endregion
    }
}