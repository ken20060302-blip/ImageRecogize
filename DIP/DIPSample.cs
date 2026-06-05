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


        private void 對比ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 把這段邏輯剪下貼過來：
            MSForm currentForm = ActiveMdiChild as MSForm;
            if (currentForm == null)
            {
                MessageBox.Show("請先開啟一張圖片！"); // 順便加個防呆提示，讓你知道有沒有抓到圖
                return;
            }

            Form3 f3 = new Form3();
            f3.OriginalImage = currentForm.pBitmap;
            f3.ShowDialog();
        }

        #endregion
    }
}