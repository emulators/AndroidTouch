namespace m8916touch
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pbox_screen = new System.Windows.Forms.PictureBox();
            this.text_debugmessage = new System.Windows.Forms.TextBox();
            this.btn_back = new System.Windows.Forms.Button();
            this.btn_home = new System.Windows.Forms.Button();
            this.timer_updateScreen = new System.Windows.Forms.Timer(this.components);
            this.btn_task = new System.Windows.Forms.Button();
            this.chkbox_touch = new System.Windows.Forms.CheckBox();
            this.btn_appswitch = new System.Windows.Forms.Button();
            this.chkbox_autorefresh = new System.Windows.Forms.CheckBox();
            this.text_input = new System.Windows.Forms.TextBox();
            this.btn_sendText = new System.Windows.Forms.Button();
            this.btn_power = new System.Windows.Forms.Button();
            this.btn_swipe = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_screen)).BeginInit();
            this.SuspendLayout();
            // 
            // pbox_screen
            // 
            this.pbox_screen.Location = new System.Drawing.Point(11, 119);
            this.pbox_screen.Name = "pbox_screen";
            this.pbox_screen.Size = new System.Drawing.Size(450, 800);
            this.pbox_screen.TabIndex = 0;
            this.pbox_screen.TabStop = false;
            // 
            // text_debugmessage
            // 
            this.text_debugmessage.Location = new System.Drawing.Point(11, 9);
            this.text_debugmessage.Name = "text_debugmessage";
            this.text_debugmessage.Size = new System.Drawing.Size(450, 25);
            this.text_debugmessage.TabIndex = 1;
            // 
            // btn_back
            // 
            this.btn_back.Location = new System.Drawing.Point(11, 71);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(58, 41);
            this.btn_back.TabIndex = 2;
            this.btn_back.Text = "←Back";
            this.btn_back.UseVisualStyleBackColor = true;
            this.btn_back.Click += new System.EventHandler(this.btn_back_Click);
            // 
            // btn_home
            // 
            this.btn_home.Location = new System.Drawing.Point(75, 71);
            this.btn_home.Name = "btn_home";
            this.btn_home.Size = new System.Drawing.Size(57, 41);
            this.btn_home.TabIndex = 3;
            this.btn_home.Text = "HOME";
            this.btn_home.UseVisualStyleBackColor = true;
            this.btn_home.Click += new System.EventHandler(this.btn_home_Click);
            // 
            // timer_updateScreen
            // 
            this.timer_updateScreen.Tick += new System.EventHandler(this.timer_updateScreen_Tick);
            // 
            // btn_task
            // 
            this.btn_task.AutoEllipsis = true;
            this.btn_task.Location = new System.Drawing.Point(198, 71);
            this.btn_task.Name = "btn_task";
            this.btn_task.Size = new System.Drawing.Size(59, 42);
            this.btn_task.TabIndex = 4;
            this.btn_task.Text = "Refresh Screen";
            this.btn_task.UseVisualStyleBackColor = true;
            this.btn_task.Click += new System.EventHandler(this.btn_task_Click);
            // 
            // chkbox_touch
            // 
            this.chkbox_touch.AutoSize = true;
            this.chkbox_touch.Location = new System.Drawing.Point(371, 93);
            this.chkbox_touch.Name = "chkbox_touch";
            this.chkbox_touch.Size = new System.Drawing.Size(65, 19);
            this.chkbox_touch.TabIndex = 5;
            this.chkbox_touch.Text = "Touch";
            this.chkbox_touch.UseVisualStyleBackColor = true;
            // 
            // btn_appswitch
            // 
            this.btn_appswitch.Location = new System.Drawing.Point(138, 71);
            this.btn_appswitch.Name = "btn_appswitch";
            this.btn_appswitch.Size = new System.Drawing.Size(54, 41);
            this.btn_appswitch.TabIndex = 6;
            this.btn_appswitch.Text = "App Switch";
            this.btn_appswitch.UseVisualStyleBackColor = true;
            this.btn_appswitch.Click += new System.EventHandler(this.btn_appswitch_Click);
            // 
            // chkbox_autorefresh
            // 
            this.chkbox_autorefresh.AutoSize = true;
            this.chkbox_autorefresh.Location = new System.Drawing.Point(371, 71);
            this.chkbox_autorefresh.Name = "chkbox_autorefresh";
            this.chkbox_autorefresh.Size = new System.Drawing.Size(104, 19);
            this.chkbox_autorefresh.TabIndex = 7;
            this.chkbox_autorefresh.Text = "Auto Refresh";
            this.chkbox_autorefresh.UseVisualStyleBackColor = true;
            this.chkbox_autorefresh.CheckedChanged += new System.EventHandler(this.chkbox_autorefresh_CheckedChanged);
            // 
            // text_input
            // 
            this.text_input.Location = new System.Drawing.Point(11, 40);
            this.text_input.Name = "text_input";
            this.text_input.Size = new System.Drawing.Size(386, 25);
            this.text_input.TabIndex = 8;
            // 
            // btn_sendText
            // 
            this.btn_sendText.Location = new System.Drawing.Point(403, 40);
            this.btn_sendText.Name = "btn_sendText";
            this.btn_sendText.Size = new System.Drawing.Size(58, 25);
            this.btn_sendText.TabIndex = 9;
            this.btn_sendText.Text = "Send";
            this.btn_sendText.UseVisualStyleBackColor = true;
            this.btn_sendText.Click += new System.EventHandler(this.btn_sendText_Click);
            // 
            // btn_power
            // 
            this.btn_power.Location = new System.Drawing.Point(263, 71);
            this.btn_power.Name = "btn_power";
            this.btn_power.Size = new System.Drawing.Size(43, 42);
            this.btn_power.TabIndex = 10;
            this.btn_power.Text = "pow";
            this.btn_power.UseVisualStyleBackColor = true;
            this.btn_power.Click += new System.EventHandler(this.btn_power_Click);
            // 
            // btn_swipe
            // 
            this.btn_swipe.Location = new System.Drawing.Point(312, 70);
            this.btn_swipe.Name = "btn_swipe";
            this.btn_swipe.Size = new System.Drawing.Size(53, 42);
            this.btn_swipe.TabIndex = 11;
            this.btn_swipe.Text = "swipe";
            this.btn_swipe.UseVisualStyleBackColor = true;
            this.btn_swipe.Click += new System.EventHandler(this.btn_swipe_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(478, 948);
            this.Controls.Add(this.btn_swipe);
            this.Controls.Add(this.btn_power);
            this.Controls.Add(this.btn_sendText);
            this.Controls.Add(this.text_input);
            this.Controls.Add(this.chkbox_autorefresh);
            this.Controls.Add(this.btn_appswitch);
            this.Controls.Add(this.chkbox_touch);
            this.Controls.Add(this.btn_task);
            this.Controls.Add(this.btn_home);
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.text_debugmessage);
            this.Controls.Add(this.pbox_screen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "AndroidTouch";
            ((System.ComponentModel.ISupportInitialize)(this.pbox_screen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox_screen;
        private System.Windows.Forms.TextBox text_debugmessage;
        private System.Windows.Forms.Button btn_back;
        private System.Windows.Forms.Button btn_home;
        private System.Windows.Forms.Timer timer_updateScreen;
        private System.Windows.Forms.Button btn_task;
        private System.Windows.Forms.CheckBox chkbox_touch;
        private System.Windows.Forms.Button btn_appswitch;
        private System.Windows.Forms.CheckBox chkbox_autorefresh;
        private System.Windows.Forms.TextBox text_input;
        private System.Windows.Forms.Button btn_sendText;
        private System.Windows.Forms.Button btn_power;
        private System.Windows.Forms.Button btn_swipe;
    }
}

