namespace LMS
{
    partial class LoginPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginPage));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.logparent = new System.Windows.Forms.Panel();
            this.log = new System.Windows.Forms.Panel();
            this.api = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.google = new Guna.UI2.WinForms.Guna2Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.exit = new Guna.UI2.WinForms.Guna2ImageButton();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.logparent.SuspendLayout();
            this.log.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 454F));
            this.tableLayoutPanel1.Controls.Add(this.logparent, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 600F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 600F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1300, 600);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // logparent
            // 
            this.logparent.BackColor = System.Drawing.Color.White;
            this.logparent.Controls.Add(this.log);
            this.logparent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logparent.Location = new System.Drawing.Point(849, 3);
            this.logparent.Name = "logparent";
            this.logparent.Size = new System.Drawing.Size(448, 594);
            this.logparent.TabIndex = 2;
            // 
            // log
            // 
            this.log.Controls.Add(this.api);
            this.log.Controls.Add(this.label4);
            this.log.Controls.Add(this.google);
            this.log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log.Location = new System.Drawing.Point(0, 0);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(448, 594);
            this.log.TabIndex = 0;
            // 
            // api
            // 
            this.api.Animated = true;
            this.api.AutoRoundedCorners = true;
            this.api.BorderRadius = 21;
            this.api.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.api.DefaultText = "";
            this.api.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.api.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.api.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.api.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.api.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.api.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.api.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.api.Location = new System.Drawing.Point(67, 352);
            this.api.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.api.Name = "api";
            this.api.PasswordChar = '*';
            this.api.PlaceholderText = "Input Your API Key";
            this.api.SelectedText = "";
            this.api.Size = new System.Drawing.Size(317, 44);
            this.api.TabIndex = 24;
            this.api.TextOffset = new System.Drawing.Point(5, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 315);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 23);
            this.label4.TabIndex = 23;
            this.label4.Text = "API Key : ";
            // 
            // google
            // 
            this.google.BorderRadius = 25;
            this.google.BorderThickness = 1;
            this.google.Cursor = System.Windows.Forms.Cursors.Hand;
            this.google.CustomImages.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.google.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.google.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.google.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.google.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.google.FillColor = System.Drawing.Color.White;
            this.google.Font = new System.Drawing.Font("Open Sans", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.google.ForeColor = System.Drawing.Color.Black;
            this.google.Image = global::LMS.Properties.Resources.icons8_google_logo_100;
            this.google.ImageSize = new System.Drawing.Size(40, 40);
            this.google.Location = new System.Drawing.Point(67, 430);
            this.google.Name = "google";
            this.google.Size = new System.Drawing.Size(317, 51);
            this.google.TabIndex = 13;
            this.google.Text = "Sign up with Google";
            this.google.Click += new System.EventHandler(this.google_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::LMS.Properties.Resources.steptodown_com193920__1_;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.exit);
            this.panel1.Controls.Add(this.guna2PictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(840, 594);
            this.panel1.TabIndex = 0;
            // 
            // exit
            // 
            this.exit.AnimatedGIF = true;
            this.exit.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.exit.HoverState.Image = global::LMS.Properties.Resources.icons8_exit;
            this.exit.HoverState.ImageSize = new System.Drawing.Size(50, 50);
            this.exit.Image = global::LMS.Properties.Resources.icons8_exit_50;
            this.exit.ImageOffset = new System.Drawing.Point(0, 0);
            this.exit.ImageRotate = 0F;
            this.exit.ImageSize = new System.Drawing.Size(50, 50);
            this.exit.Location = new System.Drawing.Point(3, 537);
            this.exit.Name = "exit";
            this.exit.PressedState.ImageSize = new System.Drawing.Size(50, 50);
            this.exit.Size = new System.Drawing.Size(64, 54);
            this.exit.TabIndex = 21;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = global::LMS.Properties.Resources.svgviewer_output;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(9, 9);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(244, 64);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.guna2PictureBox1.TabIndex = 4;
            this.guna2PictureBox1.TabStop = false;
            // 
            // LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1300, 600);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "LoginPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.logparent.ResumeLayout(false);
            this.log.ResumeLayout(false);
            this.log.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel logparent;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private System.Windows.Forms.Panel log;
        private Guna.UI2.WinForms.Guna2Button google;
        private System.Windows.Forms.Timer timer;
        private Guna.UI2.WinForms.Guna2ImageButton exit;
        private Guna.UI2.WinForms.Guna2TextBox api;
        private System.Windows.Forms.Label label4;
    }
}

