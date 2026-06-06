using System;
using System.Drawing;
using System.Windows.Forms;

namespace DIP
{
    public partial class Form6 : Form
    {
        private Bitmap originalImage;
        private PictureBox pictureBox;
        private TrackBar trackBar;
        private Label lblAngle;

        // 用來把最後決定的角度傳回給主程式
        public int SelectedAngle { get { return trackBar.Value; } }

        public Form6(Bitmap srcImage)
        {
            originalImage = srcImage;

            // --- 設定視窗基本屬性 (提醒使用者直接關閉) ---
            this.Text = "即時旋轉預覽 (調整完請直接關閉視窗)";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;

            // --- 建立控制項 ---
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = srcImage,
                BackColor = Color.Black
            };

            // 因為移除了按鈕，底部面板的高度可以縮小
            Panel bottomPanel = new Panel { Dock = DockStyle.Bottom, Height = 80 };

            trackBar = new TrackBar
            {
                Minimum = -180,
                Maximum = 180,
                Value = 0,
                TickFrequency = 15,
                Dock = DockStyle.Top
            };
            trackBar.Scroll += TrackBar_Scroll;

            lblAngle = new Label
            {
                Text = "目前角度: 0 度",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("微軟正黑體", 12)
            };

            // --- 組裝視窗 (不再加入 btnOk) ---
            bottomPanel.Controls.Add(lblAngle);
            bottomPanel.Controls.Add(trackBar);

            this.Controls.Add(pictureBox);
            this.Controls.Add(bottomPanel);

            // --- 關鍵修改：當視窗關閉時，自動回傳 OK 訊號給主程式 ---
            this.FormClosing += (s, e) => {
                this.DialogResult = DialogResult.OK;
            };
        }

        // 當滑桿移動時，即時更新畫面
        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            lblAngle.Text = $"目前角度: {trackBar.Value} 度";

            if (pictureBox.Image != originalImage) pictureBox.Image?.Dispose();
            pictureBox.Image = GetFastPreview(originalImage, trackBar.Value);
        }

        // 快速預覽的繪圖邏輯
        private Bitmap GetFastPreview(Bitmap src, float angle)
        {
            if (angle == 0) return new Bitmap(src);

            double rad = angle * Math.PI / 180.0;
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);

            int newW = (int)Math.Round(Math.Abs(src.Width * cos) + Math.Abs(src.Height * sin));
            int newH = (int)Math.Round(Math.Abs(src.Width * sin) + Math.Abs(src.Height * cos));

            Bitmap bmp = new Bitmap(newW, newH);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.TranslateTransform(newW / 2.0f, newH / 2.0f);
                g.RotateTransform(angle);
                g.TranslateTransform(-src.Width / 2.0f, -src.Height / 2.0f);
                g.DrawImage(src, new Point(0, 0));
            }
            return bmp;
        }
    }
}