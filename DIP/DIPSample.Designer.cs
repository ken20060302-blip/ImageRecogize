namespace DIP
{
    partial class DIPSample
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBtoGrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.負片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.亮度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.對比ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ostu分割ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gggToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.顯示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.等化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.直圖圖轉換與直圖等化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平均ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高斯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.銳化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.拉普拉斯銳化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prewittToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.線偵測ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.線偵測ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.oFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.旋轉ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stStripLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 449);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(876, 25);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stStripLabel
            // 
            this.stStripLabel.Name = "stStripLabel";
            this.stStripLabel.Size = new System.Drawing.Size(158, 19);
            this.stStripLabel.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.iPToolStripMenuItem,
            this.gggToolStripMenuItem,
            this.直圖圖轉換與直圖等化ToolStripMenuItem,
            this.線偵測ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(876, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.fileToolStripMenuItem.Text = "檔案";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(130, 26);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // iPToolStripMenuItem
            // 
            this.iPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rGBtoGrayToolStripMenuItem,
            this.fffToolStripMenuItem,
            this.負片ToolStripMenuItem,
            this.亮度ToolStripMenuItem,
            this.對比ToolStripMenuItem,
            this.旋轉ToolStripMenuItem,
            this.ostu分割ToolStripMenuItem});
            this.iPToolStripMenuItem.Name = "iPToolStripMenuItem";
            this.iPToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.iPToolStripMenuItem.Text = "基本";
            // 
            // rGBtoGrayToolStripMenuItem
            // 
            this.rGBtoGrayToolStripMenuItem.Name = "rGBtoGrayToolStripMenuItem";
            this.rGBtoGrayToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.rGBtoGrayToolStripMenuItem.Text = "RGBtoGray";
            this.rGBtoGrayToolStripMenuItem.Click += new System.EventHandler(this.RGBtoGrayToolStripMenuItem_Click);
            // 
            // fffToolStripMenuItem
            // 
            this.fffToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9});
            this.fffToolStripMenuItem.Name = "fffToolStripMenuItem";
            this.fffToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.fffToolStripMenuItem.Text = "Bit Section";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(101, 26);
            this.toolStripMenuItem2.Text = "0";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(101, 26);
            this.toolStripMenuItem3.Text = "1";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(101, 26);
            this.toolStripMenuItem4.Text = "2";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(101, 26);
            this.toolStripMenuItem5.Text = "3";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(101, 26);
            this.toolStripMenuItem6.Text = "4";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(101, 26);
            this.toolStripMenuItem7.Text = "5";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(101, 26);
            this.toolStripMenuItem8.Text = "6";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(101, 26);
            this.toolStripMenuItem9.Text = "7";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // 負片ToolStripMenuItem
            // 
            this.負片ToolStripMenuItem.Name = "負片ToolStripMenuItem";
            this.負片ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.負片ToolStripMenuItem.Text = "負片";
            this.負片ToolStripMenuItem.Click += new System.EventHandler(this.負片ToolStripMenuItem_Click);
            // 
            // 亮度ToolStripMenuItem
            // 
            this.亮度ToolStripMenuItem.Name = "亮度ToolStripMenuItem";
            this.亮度ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.亮度ToolStripMenuItem.Text = "亮度";
            this.亮度ToolStripMenuItem.Click += new System.EventHandler(this.亮度ToolStripMenuItem_Click);
            // 
            // 對比ToolStripMenuItem
            // 
            this.對比ToolStripMenuItem.Name = "對比ToolStripMenuItem";
            this.對比ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.對比ToolStripMenuItem.Text = "對比";
            this.對比ToolStripMenuItem.Click += new System.EventHandler(this.對比ToolStripMenuItem_Click_1);
            // 
            // ostu分割ToolStripMenuItem
            // 
            this.ostu分割ToolStripMenuItem.Name = "ostu分割ToolStripMenuItem";
            this.ostu分割ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ostu分割ToolStripMenuItem.Text = "ostu分割";
            this.ostu分割ToolStripMenuItem.Click += new System.EventHandler(this.ostu分割ToolStripMenuItem_Click);
            // 
            // gggToolStripMenuItem
            // 
            this.gggToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.顯示ToolStripMenuItem,
            this.等化ToolStripMenuItem});
            this.gggToolStripMenuItem.Name = "gggToolStripMenuItem";
            this.gggToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.gggToolStripMenuItem.Text = "直方圖";
            // 
            // 顯示ToolStripMenuItem
            // 
            this.顯示ToolStripMenuItem.Name = "顯示ToolStripMenuItem";
            this.顯示ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.顯示ToolStripMenuItem.Text = "顯示";
            this.顯示ToolStripMenuItem.Click += new System.EventHandler(this.顯示ToolStripMenuItem_Click);
            // 
            // 等化ToolStripMenuItem
            // 
            this.等化ToolStripMenuItem.Name = "等化ToolStripMenuItem";
            this.等化ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.等化ToolStripMenuItem.Text = "等化";
            this.等化ToolStripMenuItem.Click += new System.EventHandler(this.等化ToolStripMenuItem_Click_1);
            // 
            // 直圖圖轉換與直圖等化ToolStripMenuItem
            // 
            this.直圖圖轉換與直圖等化ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.平均ToolStripMenuItem,
            this.中值ToolStripMenuItem,
            this.高斯ToolStripMenuItem,
            this.銳化ToolStripMenuItem,
            this.拉普拉斯銳化ToolStripMenuItem,
            this.prewittToolStripMenuItem,
            this.sobelToolStripMenuItem});
            this.直圖圖轉換與直圖等化ToolStripMenuItem.Name = "直圖圖轉換與直圖等化ToolStripMenuItem";
            this.直圖圖轉換與直圖等化ToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.直圖圖轉換與直圖等化ToolStripMenuItem.Text = "濾波器";
            // 
            // 平均ToolStripMenuItem
            // 
            this.平均ToolStripMenuItem.Name = "平均ToolStripMenuItem";
            this.平均ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.平均ToolStripMenuItem.Text = "平均";
            this.平均ToolStripMenuItem.Click += new System.EventHandler(this.平均ToolStripMenuItem_Click);
            // 
            // 中值ToolStripMenuItem
            // 
            this.中值ToolStripMenuItem.Name = "中值ToolStripMenuItem";
            this.中值ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.中值ToolStripMenuItem.Text = "中值";
            this.中值ToolStripMenuItem.Click += new System.EventHandler(this.中值ToolStripMenuItem_Click);
            // 
            // 高斯ToolStripMenuItem
            // 
            this.高斯ToolStripMenuItem.Name = "高斯ToolStripMenuItem";
            this.高斯ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.高斯ToolStripMenuItem.Text = "高斯";
            this.高斯ToolStripMenuItem.Click += new System.EventHandler(this.高斯ToolStripMenuItem_Click);
            // 
            // 銳化ToolStripMenuItem
            // 
            this.銳化ToolStripMenuItem.Name = "銳化ToolStripMenuItem";
            this.銳化ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.銳化ToolStripMenuItem.Text = "銳化";
            this.銳化ToolStripMenuItem.Click += new System.EventHandler(this.銳化ToolStripMenuItem_Click);
            // 
            // 拉普拉斯銳化ToolStripMenuItem
            // 
            this.拉普拉斯銳化ToolStripMenuItem.Name = "拉普拉斯銳化ToolStripMenuItem";
            this.拉普拉斯銳化ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.拉普拉斯銳化ToolStripMenuItem.Text = "拉普拉斯銳化";
            this.拉普拉斯銳化ToolStripMenuItem.Click += new System.EventHandler(this.拉普拉斯銳化ToolStripMenuItem_Click);
            // 
            // prewittToolStripMenuItem
            // 
            this.prewittToolStripMenuItem.Name = "prewittToolStripMenuItem";
            this.prewittToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.prewittToolStripMenuItem.Text = "Prewitt";
            this.prewittToolStripMenuItem.Click += new System.EventHandler(this.prewittToolStripMenuItem_Click);
            // 
            // sobelToolStripMenuItem
            // 
            this.sobelToolStripMenuItem.Name = "sobelToolStripMenuItem";
            this.sobelToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.sobelToolStripMenuItem.Text = "Sobel";
            this.sobelToolStripMenuItem.Click += new System.EventHandler(this.sobelToolStripMenuItem_Click);
            // 
            // 線偵測ToolStripMenuItem
            // 
            this.線偵測ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.線偵測ToolStripMenuItem1});
            this.線偵測ToolStripMenuItem.Name = "線偵測ToolStripMenuItem";
            this.線偵測ToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.線偵測ToolStripMenuItem.Text = "線偵測";
            // 
            // 線偵測ToolStripMenuItem1
            // 
            this.線偵測ToolStripMenuItem1.Name = "線偵測ToolStripMenuItem1";
            this.線偵測ToolStripMenuItem1.Size = new System.Drawing.Size(137, 26);
            this.線偵測ToolStripMenuItem1.Text = "線偵測";
            this.線偵測ToolStripMenuItem1.Click += new System.EventHandler(this.線偵測ToolStripMenuItem1_Click);
            // 
            // oFileDlg
            // 
            this.oFileDlg.FileName = "openFileDialog1";
            // 
            // 旋轉ToolStripMenuItem
            // 
            this.旋轉ToolStripMenuItem.Name = "旋轉ToolStripMenuItem";
            this.旋轉ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.旋轉ToolStripMenuItem.Text = "旋轉";
            this.旋轉ToolStripMenuItem.Click += new System.EventHandler(this.旋轉ToolStripMenuItem_Click);
            // 
            // DIPSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 474);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "DIPSample";
            this.Load += new System.EventHandler(this.DIPSample_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iPToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog oFileDlg;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem rGBtoGrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gggToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem 負片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 亮度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 對比ToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel stStripLabel;

        // 修正後的直方圖選單變數
        private System.Windows.Forms.ToolStripMenuItem 顯示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 等化ToolStripMenuItem;

        // 刪除重複的分身，只保留一個濾波器選單變數
        private System.Windows.Forms.ToolStripMenuItem 直圖圖轉換與直圖等化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平均ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 中值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 高斯ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 線偵測ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 銳化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 拉普拉斯銳化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prewittToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 線偵測ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ostu分割ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 旋轉ToolStripMenuItem;
    }
}