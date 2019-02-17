namespace Client1
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbox_Chat = new System.Windows.Forms.TextBox();
            this.lbox_ChatLog = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbtn_very_thick = new System.Windows.Forms.RadioButton();
            this.rbtn_thick = new System.Windows.Forms.RadioButton();
            this.rbtn_normal = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbtn_Circle = new System.Windows.Forms.RadioButton();
            this.rbtn_Rect = new System.Windows.Forms.RadioButton();
            this.rbtn_Line = new System.Windows.Forms.RadioButton();
            this.rbtn_None = new System.Windows.Forms.RadioButton();
            this.lbox_Content = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbox_BackColor = new System.Windows.Forms.TextBox();
            this.tbox_Port = new System.Windows.Forms.TextBox();
            this.tbox_ServerIP = new System.Windows.Forms.TextBox();
            this.btn_ServerStop = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbox_ClientID = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Colordialog = new System.Windows.Forms.Button();
            this.색상 = new System.Windows.Forms.Label();
            this.tbox_Shape = new System.Windows.Forms.TextBox();
            this.모양 = new System.Windows.Forms.Label();
            this.tbox_Width = new System.Windows.Forms.TextBox();
            this.선두께 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbox_Path = new System.Windows.Forms.TextBox();
            this.btn_Path = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btn_Send = new System.Windows.Forms.Button();
            this.btn_Remove = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbox_Chat
            // 
            this.tbox_Chat.Location = new System.Drawing.Point(680, 613);
            this.tbox_Chat.Name = "tbox_Chat";
            this.tbox_Chat.Size = new System.Drawing.Size(216, 21);
            this.tbox_Chat.TabIndex = 44;
            this.tbox_Chat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbox_Chat_KeyDown);
            // 
            // lbox_ChatLog
            // 
            this.lbox_ChatLog.FormattingEnabled = true;
            this.lbox_ChatLog.ItemHeight = 12;
            this.lbox_ChatLog.Location = new System.Drawing.Point(679, 229);
            this.lbox_ChatLog.Name = "lbox_ChatLog";
            this.lbox_ChatLog.Size = new System.Drawing.Size(294, 364);
            this.lbox_ChatLog.TabIndex = 43;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbtn_very_thick);
            this.groupBox3.Controls.Add(this.rbtn_thick);
            this.groupBox3.Controls.Add(this.rbtn_normal);
            this.groupBox3.Location = new System.Drawing.Point(150, 106);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(110, 106);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "굵기";
            // 
            // rbtn_very_thick
            // 
            this.rbtn_very_thick.AutoSize = true;
            this.rbtn_very_thick.Location = new System.Drawing.Point(6, 75);
            this.rbtn_very_thick.Name = "rbtn_very_thick";
            this.rbtn_very_thick.Size = new System.Drawing.Size(71, 16);
            this.rbtn_very_thick.TabIndex = 2;
            this.rbtn_very_thick.Text = "매우굵게";
            this.rbtn_very_thick.UseVisualStyleBackColor = true;
            this.rbtn_very_thick.CheckedChanged += new System.EventHandler(this.rbtn_very_thick_CheckedChanged);
            // 
            // rbtn_thick
            // 
            this.rbtn_thick.AutoSize = true;
            this.rbtn_thick.Location = new System.Drawing.Point(6, 53);
            this.rbtn_thick.Name = "rbtn_thick";
            this.rbtn_thick.Size = new System.Drawing.Size(47, 16);
            this.rbtn_thick.TabIndex = 1;
            this.rbtn_thick.Text = "굵게";
            this.rbtn_thick.UseVisualStyleBackColor = true;
            this.rbtn_thick.CheckedChanged += new System.EventHandler(this.rbtn_thick_CheckedChanged);
            // 
            // rbtn_normal
            // 
            this.rbtn_normal.AutoSize = true;
            this.rbtn_normal.Checked = true;
            this.rbtn_normal.Location = new System.Drawing.Point(6, 31);
            this.rbtn_normal.Name = "rbtn_normal";
            this.rbtn_normal.Size = new System.Drawing.Size(47, 16);
            this.rbtn_normal.TabIndex = 0;
            this.rbtn_normal.TabStop = true;
            this.rbtn_normal.Text = "보통";
            this.rbtn_normal.UseVisualStyleBackColor = true;
            this.rbtn_normal.CheckedChanged += new System.EventHandler(this.rbtn_normal_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rbtn_Circle);
            this.groupBox5.Controls.Add(this.rbtn_Rect);
            this.groupBox5.Controls.Add(this.rbtn_Line);
            this.groupBox5.Controls.Add(this.rbtn_None);
            this.groupBox5.Location = new System.Drawing.Point(13, 107);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(110, 106);
            this.groupBox5.TabIndex = 39;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "도형고르기";
            // 
            // rbtn_Circle
            // 
            this.rbtn_Circle.AutoSize = true;
            this.rbtn_Circle.Location = new System.Drawing.Point(6, 86);
            this.rbtn_Circle.Name = "rbtn_Circle";
            this.rbtn_Circle.Size = new System.Drawing.Size(67, 16);
            this.rbtn_Circle.TabIndex = 3;
            this.rbtn_Circle.Text = "CIRCLE";
            this.rbtn_Circle.UseVisualStyleBackColor = true;
            this.rbtn_Circle.CheckedChanged += new System.EventHandler(this.rbtn_Circle_CheckedChanged);
            // 
            // rbtn_Rect
            // 
            this.rbtn_Rect.AutoSize = true;
            this.rbtn_Rect.Location = new System.Drawing.Point(6, 64);
            this.rbtn_Rect.Name = "rbtn_Rect";
            this.rbtn_Rect.Size = new System.Drawing.Size(48, 16);
            this.rbtn_Rect.TabIndex = 2;
            this.rbtn_Rect.Text = "Rect";
            this.rbtn_Rect.UseVisualStyleBackColor = true;
            this.rbtn_Rect.CheckedChanged += new System.EventHandler(this.rbtn_Rect_CheckedChanged);
            // 
            // rbtn_Line
            // 
            this.rbtn_Line.AutoSize = true;
            this.rbtn_Line.Location = new System.Drawing.Point(6, 42);
            this.rbtn_Line.Name = "rbtn_Line";
            this.rbtn_Line.Size = new System.Drawing.Size(50, 16);
            this.rbtn_Line.TabIndex = 1;
            this.rbtn_Line.Text = "LINE";
            this.rbtn_Line.UseVisualStyleBackColor = true;
            this.rbtn_Line.CheckedChanged += new System.EventHandler(this.rbtn_Line_CheckedChanged);
            // 
            // rbtn_None
            // 
            this.rbtn_None.AutoSize = true;
            this.rbtn_None.Checked = true;
            this.rbtn_None.Location = new System.Drawing.Point(6, 20);
            this.rbtn_None.Name = "rbtn_None";
            this.rbtn_None.Size = new System.Drawing.Size(58, 16);
            this.rbtn_None.TabIndex = 0;
            this.rbtn_None.TabStop = true;
            this.rbtn_None.Text = "NONE";
            this.rbtn_None.UseVisualStyleBackColor = true;
            this.rbtn_None.CheckedChanged += new System.EventHandler(this.rbtn_None_CheckedChanged);
            // 
            // lbox_Content
            // 
            this.lbox_Content.FormattingEnabled = true;
            this.lbox_Content.HorizontalScrollbar = true;
            this.lbox_Content.ItemHeight = 12;
            this.lbox_Content.Location = new System.Drawing.Point(267, 137);
            this.lbox_Content.Name = "lbox_Content";
            this.lbox_Content.Size = new System.Drawing.Size(406, 76);
            this.lbox_Content.TabIndex = 40;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbox_BackColor);
            this.groupBox4.Controls.Add(this.tbox_Port);
            this.groupBox4.Controls.Add(this.tbox_ServerIP);
            this.groupBox4.Controls.Add(this.btn_ServerStop);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.btn_Connect);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.tbox_ClientID);
            this.groupBox4.Location = new System.Drawing.Point(407, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(266, 107);
            this.groupBox4.TabIndex = 38;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "서버 연결 도구";
            // 
            // tbox_BackColor
            // 
            this.tbox_BackColor.Location = new System.Drawing.Point(166, 27);
            this.tbox_BackColor.Name = "tbox_BackColor";
            this.tbox_BackColor.ReadOnly = true;
            this.tbox_BackColor.Size = new System.Drawing.Size(66, 21);
            this.tbox_BackColor.TabIndex = 30;
            this.tbox_BackColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbox_Port
            // 
            this.tbox_Port.Location = new System.Drawing.Point(73, 53);
            this.tbox_Port.Name = "tbox_Port";
            this.tbox_Port.Size = new System.Drawing.Size(87, 21);
            this.tbox_Port.TabIndex = 22;
            this.tbox_Port.Text = "3000";
            // 
            // tbox_ServerIP
            // 
            this.tbox_ServerIP.Location = new System.Drawing.Point(73, 27);
            this.tbox_ServerIP.Name = "tbox_ServerIP";
            this.tbox_ServerIP.Size = new System.Drawing.Size(87, 21);
            this.tbox_ServerIP.TabIndex = 23;
            this.tbox_ServerIP.Text = "192.168.145.1";
            // 
            // btn_ServerStop
            // 
            this.btn_ServerStop.Location = new System.Drawing.Point(166, 79);
            this.btn_ServerStop.Name = "btn_ServerStop";
            this.btn_ServerStop.Size = new System.Drawing.Size(66, 23);
            this.btn_ServerStop.TabIndex = 20;
            this.btn_ServerStop.Text = "Stop";
            this.btn_ServerStop.UseVisualStyleBackColor = true;
            this.btn_ServerStop.Click += new System.EventHandler(this.btn_ServerStop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "Client ID";
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(166, 51);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(66, 23);
            this.btn_Connect.TabIndex = 15;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "서버ip";
            // 
            // tbox_ClientID
            // 
            this.tbox_ClientID.Location = new System.Drawing.Point(73, 81);
            this.tbox_ClientID.Name = "tbox_ClientID";
            this.tbox_ClientID.Size = new System.Drawing.Size(87, 21);
            this.tbox_ClientID.TabIndex = 16;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_Colordialog);
            this.groupBox2.Controls.Add(this.색상);
            this.groupBox2.Controls.Add(this.tbox_Shape);
            this.groupBox2.Controls.Add(this.모양);
            this.groupBox2.Controls.Add(this.tbox_Width);
            this.groupBox2.Controls.Add(this.선두께);
            this.groupBox2.Location = new System.Drawing.Point(267, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(134, 106);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "도구 정보";
            // 
            // btn_Colordialog
            // 
            this.btn_Colordialog.Location = new System.Drawing.Point(51, 14);
            this.btn_Colordialog.Name = "btn_Colordialog";
            this.btn_Colordialog.Size = new System.Drawing.Size(77, 23);
            this.btn_Colordialog.TabIndex = 16;
            this.btn_Colordialog.UseVisualStyleBackColor = true;
            this.btn_Colordialog.Click += new System.EventHandler(this.btn_Colordialog_Click);
            // 
            // 색상
            // 
            this.색상.AutoSize = true;
            this.색상.Location = new System.Drawing.Point(16, 19);
            this.색상.Name = "색상";
            this.색상.Size = new System.Drawing.Size(29, 12);
            this.색상.TabIndex = 7;
            this.색상.Text = "색상";
            // 
            // tbox_Shape
            // 
            this.tbox_Shape.Location = new System.Drawing.Point(51, 43);
            this.tbox_Shape.Name = "tbox_Shape";
            this.tbox_Shape.ReadOnly = true;
            this.tbox_Shape.Size = new System.Drawing.Size(77, 21);
            this.tbox_Shape.TabIndex = 5;
            // 
            // 모양
            // 
            this.모양.AutoSize = true;
            this.모양.Location = new System.Drawing.Point(16, 46);
            this.모양.Name = "모양";
            this.모양.Size = new System.Drawing.Size(29, 12);
            this.모양.TabIndex = 8;
            this.모양.Text = "모양";
            // 
            // tbox_Width
            // 
            this.tbox_Width.Location = new System.Drawing.Point(51, 70);
            this.tbox_Width.Name = "tbox_Width";
            this.tbox_Width.ReadOnly = true;
            this.tbox_Width.Size = new System.Drawing.Size(77, 21);
            this.tbox_Width.TabIndex = 17;
            // 
            // 선두께
            // 
            this.선두께.AutoSize = true;
            this.선두께.Location = new System.Drawing.Point(16, 73);
            this.선두께.Name = "선두께";
            this.선두께.Size = new System.Drawing.Size(29, 12);
            this.선두께.TabIndex = 6;
            this.선두께.Text = "굵기";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbox_Path);
            this.groupBox1.Controls.Add(this.btn_Path);
            this.groupBox1.Controls.Add(this.btn_save);
            this.groupBox1.Location = new System.Drawing.Point(725, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 80);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "그림 파일 선택 및 저장";
            this.groupBox1.Visible = false;
            // 
            // tbox_Path
            // 
            this.tbox_Path.Location = new System.Drawing.Point(6, 20);
            this.tbox_Path.Name = "tbox_Path";
            this.tbox_Path.ReadOnly = true;
            this.tbox_Path.Size = new System.Drawing.Size(235, 21);
            this.tbox_Path.TabIndex = 1;
            // 
            // btn_Path
            // 
            this.btn_Path.Location = new System.Drawing.Point(6, 47);
            this.btn_Path.Name = "btn_Path";
            this.btn_Path.Size = new System.Drawing.Size(113, 23);
            this.btn_Path.TabIndex = 9;
            this.btn_Path.Text = "이미지 불러오기";
            this.btn_Path.UseVisualStyleBackColor = true;
            this.btn_Path.Click += new System.EventHandler(this.btn_Path_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(125, 47);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(116, 23);
            this.btn_save.TabIndex = 18;
            this.btn_save.Text = "저장하기";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(11, 229);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(662, 412);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 35;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragDrop);
            this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragEnter);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(902, 611);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(70, 23);
            this.btn_Send.TabIndex = 45;
            this.btn_Send.Text = "보내기";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // btn_Remove
            // 
            this.btn_Remove.Location = new System.Drawing.Point(67, 46);
            this.btn_Remove.Name = "btn_Remove";
            this.btn_Remove.Size = new System.Drawing.Size(113, 23);
            this.btn_Remove.TabIndex = 19;
            this.btn_Remove.Text = "도형삭제";
            this.btn_Remove.UseVisualStyleBackColor = true;
            this.btn_Remove.Click += new System.EventHandler(this.btn_Remove_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.btn_Remove);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.tbox_Chat);
            this.Controls.Add(this.lbox_ChatLog);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.lbox_Content);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Client1";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbox_Chat;
        private System.Windows.Forms.ListBox lbox_ChatLog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbtn_very_thick;
        private System.Windows.Forms.RadioButton rbtn_thick;
        private System.Windows.Forms.RadioButton rbtn_normal;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rbtn_Circle;
        private System.Windows.Forms.RadioButton rbtn_Rect;
        private System.Windows.Forms.RadioButton rbtn_Line;
        private System.Windows.Forms.RadioButton rbtn_None;
        private System.Windows.Forms.ListBox lbox_Content;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbox_BackColor;
        private System.Windows.Forms.TextBox tbox_Port;
        private System.Windows.Forms.TextBox tbox_ServerIP;
        private System.Windows.Forms.Button btn_ServerStop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbox_ClientID;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_Colordialog;
        private System.Windows.Forms.Label 색상;
        private System.Windows.Forms.TextBox tbox_Shape;
        private System.Windows.Forms.Label 모양;
        private System.Windows.Forms.TextBox tbox_Width;
        private System.Windows.Forms.Label 선두께;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbox_Path;
        private System.Windows.Forms.Button btn_Path;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Button btn_Remove;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
    }
}

