namespace SafeSystem
{
    partial class Safesystem1
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Safesystem1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.wendu = new System.Windows.Forms.Label();
            this.yanwu = new System.Windows.Forms.PictureBox();
            this.jiujing = new System.Windows.Forms.PictureBox();
            this.qiyou = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.yanwu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jiujing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qiyou)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // wendu
            // 
            this.wendu.AutoSize = true;
            this.wendu.Font = new System.Drawing.Font("微软雅黑", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.wendu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.wendu.Location = new System.Drawing.Point(1387, 401);
            this.wendu.Name = "wendu";
            this.wendu.Size = new System.Drawing.Size(260, 124);
            this.wendu.TabIndex = 13;
            this.wendu.Text = "  0℃";
            this.wendu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // yanwu
            // 
            this.yanwu.Image = ((System.Drawing.Image)(resources.GetObject("yanwu.Image")));
            this.yanwu.Location = new System.Drawing.Point(1035, 309);
            this.yanwu.Name = "yanwu";
            this.yanwu.Size = new System.Drawing.Size(403, 69);
            this.yanwu.TabIndex = 12;
            this.yanwu.TabStop = false;
            // 
            // jiujing
            // 
            this.jiujing.Image = ((System.Drawing.Image)(resources.GetObject("jiujing.Image")));
            this.jiujing.Location = new System.Drawing.Point(670, 309);
            this.jiujing.Name = "jiujing";
            this.jiujing.Size = new System.Drawing.Size(403, 69);
            this.jiujing.TabIndex = 11;
            this.jiujing.TabStop = false;
            // 
            // qiyou
            // 
            this.qiyou.Image = ((System.Drawing.Image)(resources.GetObject("qiyou.Image")));
            this.qiyou.Location = new System.Drawing.Point(307, 309);
            this.qiyou.Name = "qiyou";
            this.qiyou.Size = new System.Drawing.Size(403, 69);
            this.qiyou.TabIndex = 10;
            this.qiyou.TabStop = false;
            this.qiyou.Click += new System.EventHandler(this.qiyou_Click);
            // 
            // Safesystem1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wendu);
            this.Controls.Add(this.yanwu);
            this.Controls.Add(this.jiujing);
            this.Controls.Add(this.qiyou);
            this.Name = "Safesystem1";
            this.Size = new System.Drawing.Size(1920, 1080);
            this.Load += new System.EventHandler(this.Safesystem1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.yanwu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jiujing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qiyou)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox qiyou;
        private System.Windows.Forms.PictureBox jiujing;
        private System.Windows.Forms.PictureBox yanwu;
        private System.Windows.Forms.Label wendu;
    }
}
