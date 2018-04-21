namespace SafeSystem
{
    partial class FormMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.DateTimeLabel = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.TimeLabel = new System.Windows.Forms.Label();
            this.GpsTimer = new System.Windows.Forms.Timer(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.peopleNum = new System.Windows.Forms.Label();
            this.box1 = new System.Windows.Forms.PictureBox();
            this.box2 = new System.Windows.Forms.PictureBox();
            this.safesystem1 = new SafeSystem.Safesystem1();
            ((System.ComponentModel.ISupportInitialize)(this.box1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.box2)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 15000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DateTimeLabel
            // 
            this.DateTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DateTimeLabel.AutoSize = true;
            this.DateTimeLabel.Font = new System.Drawing.Font("华文细黑", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DateTimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.DateTimeLabel.Location = new System.Drawing.Point(169, 60);
            this.DateTimeLabel.Name = "DateTimeLabel";
            this.DateTimeLabel.Size = new System.Drawing.Size(275, 87);
            this.DateTimeLabel.TabIndex = 1;
            this.DateTimeLabel.Text = "label1";
            this.DateTimeLabel.Click += new System.EventHandler(this.DateTimeLabel_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // TimeLabel
            // 
            this.TimeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Font = new System.Drawing.Font("华文细黑", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.TimeLabel.Location = new System.Drawing.Point(91, 153);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(148, 46);
            this.TimeLabel.TabIndex = 2;
            this.TimeLabel.Text = "label1";
            // 
            // GpsTimer
            // 
            this.GpsTimer.Interval = 5000;
            this.GpsTimer.Tick += new System.EventHandler(this.GpsTimer_Tick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(452, 33);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(217, 30);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // peopleNum
            // 
            this.peopleNum.AutoSize = true;
            this.peopleNum.BackColor = System.Drawing.Color.Transparent;
            this.peopleNum.Font = new System.Drawing.Font("微软雅黑", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.peopleNum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.peopleNum.Location = new System.Drawing.Point(1646, 802);
            this.peopleNum.Name = "peopleNum";
            this.peopleNum.Size = new System.Drawing.Size(164, 124);
            this.peopleNum.TabIndex = 7;
            this.peopleNum.Text = "  0";
            this.peopleNum.Click += new System.EventHandler(this.peopleNum_Click);
            // 
            // box1
            // 
            this.box1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.box1.Location = new System.Drawing.Point(110, 412);
            this.box1.Name = "box1";
            this.box1.Size = new System.Drawing.Size(640, 480);
            this.box1.TabIndex = 8;
            this.box1.TabStop = false;
            this.box1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // box2
            // 
            this.box2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.box2.Location = new System.Drawing.Point(835, 412);
            this.box2.Name = "box2";
            this.box2.Size = new System.Drawing.Size(640, 480);
            this.box2.TabIndex = 9;
            this.box2.TabStop = false;
            this.box2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // safesystem1
            // 
            this.safesystem1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.safesystem1.BackColor = System.Drawing.Color.Transparent;
            this.safesystem1.Location = new System.Drawing.Point(-304, -176);
            this.safesystem1.Margin = new System.Windows.Forms.Padding(0);
            this.safesystem1.Name = "safesystem1";
            this.safesystem1.Size = new System.Drawing.Size(2192, 1080);
            this.safesystem1.TabIndex = 0;
            this.safesystem1.Load += new System.EventHandler(this.safesystem1_Load);
            // 
            // FormMain
            // 
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1916, 1053);
            this.Controls.Add(this.box2);
            this.Controls.Add(this.box1);
            this.Controls.Add(this.peopleNum);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.DateTimeLabel);
            this.Controls.Add(this.safesystem1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormMain";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.box1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.box2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Safesystem1 safesystem1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label DateTimeLabel;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Timer GpsTimer;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label peopleNum;
        private System.Windows.Forms.PictureBox box1;
        private System.Windows.Forms.PictureBox box2;
    }
}

