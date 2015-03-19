namespace Irc4TestForm
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtNickname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpdateHost = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnAddServer = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.btnCreateNewServer = new System.Windows.Forms.Button();
            this.btnCancelCreateNewServer = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtChannelDisplayName = new System.Windows.Forms.TextBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.comboBoxCurrentChannel = new System.Windows.Forms.ComboBox();
            this.btnPart = new System.Windows.Forms.Button();
            this.btnCreateNewChannel = new System.Windows.Forms.Button();
            this.btnAddChannel = new System.Windows.Forms.Button();
            this.btnCancelCreateNewChannel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnUpdateChannelSetting = new System.Windows.Forms.Button();
            this.ircDataGridView2 = new Irc4Control.IrcDataGridView();
            this.ircDataGridView1 = new Irc4Control.IrcDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ircDataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ircDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(511, 41);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 604);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(859, 64);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(12, 700);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(778, 20);
            this.textBox2.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(796, 700);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(592, 41);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(89, 38);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(211, 20);
            this.txtHost.TabIndex = 5;
            // 
            // txtNickname
            // 
            this.txtNickname.Location = new System.Drawing.Point(89, 64);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(211, 20);
            this.txtNickname.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Nickname";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Host";
            // 
            // btnUpdateHost
            // 
            this.btnUpdateHost.Location = new System.Drawing.Point(673, 41);
            this.btnUpdateHost.Name = "btnUpdateHost";
            this.btnUpdateHost.Size = new System.Drawing.Size(88, 23);
            this.btnUpdateHost.TabIndex = 10;
            this.btnUpdateHost.Text = "UpdateSetting";
            this.btnUpdateHost.UseVisualStyleBackColor = true;
            this.btnUpdateHost.Click += new System.EventHandler(this.btnUpdateHost_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(592, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(169, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // btnAddServer
            // 
            this.btnAddServer.Location = new System.Drawing.Point(343, 41);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(75, 23);
            this.btnAddServer.TabIndex = 13;
            this.btnAddServer.Text = "AddServer";
            this.btnAddServer.UseVisualStyleBackColor = true;
            this.btnAddServer.Click += new System.EventHandler(this.btnAddServer_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "DisplayName";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(89, 12);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(211, 20);
            this.txtDisplayName.TabIndex = 15;
            // 
            // btnCreateNewServer
            // 
            this.btnCreateNewServer.Location = new System.Drawing.Point(343, 12);
            this.btnCreateNewServer.Name = "btnCreateNewServer";
            this.btnCreateNewServer.Size = new System.Drawing.Size(104, 23);
            this.btnCreateNewServer.TabIndex = 16;
            this.btnCreateNewServer.Text = "CreateNewServer";
            this.btnCreateNewServer.UseVisualStyleBackColor = true;
            this.btnCreateNewServer.Click += new System.EventHandler(this.btnCreateNewServer_Click);
            // 
            // btnCancelCreateNewServer
            // 
            this.btnCancelCreateNewServer.Location = new System.Drawing.Point(343, 70);
            this.btnCancelCreateNewServer.Name = "btnCancelCreateNewServer";
            this.btnCancelCreateNewServer.Size = new System.Drawing.Size(75, 23);
            this.btnCancelCreateNewServer.TabIndex = 17;
            this.btnCancelCreateNewServer.Text = "Cancel";
            this.btnCancelCreateNewServer.UseVisualStyleBackColor = true;
            this.btnCancelCreateNewServer.Click += new System.EventHandler(this.btnCancelCreateNewServer_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(508, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Current Server";
            // 
            // txtChannelDisplayName
            // 
            this.txtChannelDisplayName.Location = new System.Drawing.Point(64, 284);
            this.txtChannelDisplayName.Name = "txtChannelDisplayName";
            this.txtChannelDisplayName.Size = new System.Drawing.Size(130, 20);
            this.txtChannelDisplayName.TabIndex = 20;
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(654, 281);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(57, 23);
            this.btnJoin.TabIndex = 21;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // comboBoxCurrentChannel
            // 
            this.comboBoxCurrentChannel.FormattingEnabled = true;
            this.comboBoxCurrentChannel.Location = new System.Drawing.Point(467, 283);
            this.comboBoxCurrentChannel.Name = "comboBoxCurrentChannel";
            this.comboBoxCurrentChannel.Size = new System.Drawing.Size(181, 21);
            this.comboBoxCurrentChannel.TabIndex = 22;
            this.comboBoxCurrentChannel.SelectedIndexChanged += new System.EventHandler(this.comboBoxCurrentChannel_SelectedIndexChanged);
            // 
            // btnPart
            // 
            this.btnPart.Location = new System.Drawing.Point(717, 281);
            this.btnPart.Name = "btnPart";
            this.btnPart.Size = new System.Drawing.Size(52, 23);
            this.btnPart.TabIndex = 23;
            this.btnPart.Text = "Part";
            this.btnPart.UseVisualStyleBackColor = true;
            this.btnPart.Click += new System.EventHandler(this.btnPart_Click);
            // 
            // btnCreateNewChannel
            // 
            this.btnCreateNewChannel.Location = new System.Drawing.Point(200, 282);
            this.btnCreateNewChannel.Name = "btnCreateNewChannel";
            this.btnCreateNewChannel.Size = new System.Drawing.Size(88, 23);
            this.btnCreateNewChannel.TabIndex = 24;
            this.btnCreateNewChannel.Text = "CreateChannel";
            this.btnCreateNewChannel.UseVisualStyleBackColor = true;
            // 
            // btnAddChannel
            // 
            this.btnAddChannel.Location = new System.Drawing.Point(294, 282);
            this.btnAddChannel.Name = "btnAddChannel";
            this.btnAddChannel.Size = new System.Drawing.Size(75, 23);
            this.btnAddChannel.TabIndex = 25;
            this.btnAddChannel.Text = "AddChannel";
            this.btnAddChannel.UseVisualStyleBackColor = true;
            // 
            // btnCancelCreateNewChannel
            // 
            this.btnCancelCreateNewChannel.Location = new System.Drawing.Point(375, 282);
            this.btnCancelCreateNewChannel.Name = "btnCancelCreateNewChannel";
            this.btnCancelCreateNewChannel.Size = new System.Drawing.Size(75, 23);
            this.btnCancelCreateNewChannel.TabIndex = 26;
            this.btnCancelCreateNewChannel.Text = "Cancel";
            this.btnCancelCreateNewChannel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 287);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Channel";
            // 
            // btnUpdateChannelSetting
            // 
            this.btnUpdateChannelSetting.Location = new System.Drawing.Point(775, 281);
            this.btnUpdateChannelSetting.Name = "btnUpdateChannelSetting";
            this.btnUpdateChannelSetting.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateChannelSetting.TabIndex = 28;
            this.btnUpdateChannelSetting.Text = "Update";
            this.btnUpdateChannelSetting.UseVisualStyleBackColor = true;
            // 
            // ircDataGridView2
            // 
            this.ircDataGridView2.AllowUserToAddRows = false;
            this.ircDataGridView2.AllowUserToDeleteRows = false;
            this.ircDataGridView2.AllowUserToResizeRows = false;
            this.ircDataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ircDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ircDataGridView2.Location = new System.Drawing.Point(12, 308);
            this.ircDataGridView2.Name = "ircDataGridView2";
            this.ircDataGridView2.RowHeadersVisible = false;
            this.ircDataGridView2.Size = new System.Drawing.Size(859, 290);
            this.ircDataGridView2.TabIndex = 19;


            // 
            // ircDataGridView1
            // 
            this.ircDataGridView1.AllowUserToAddRows = false;
            this.ircDataGridView1.AllowUserToDeleteRows = false;
            this.ircDataGridView1.AllowUserToResizeRows = false;
            this.ircDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ircDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ircDataGridView1.Location = new System.Drawing.Point(12, 99);
            this.ircDataGridView1.Name = "ircDataGridView1";
            this.ircDataGridView1.RowHeadersVisible = false;
            this.ircDataGridView1.Size = new System.Drawing.Size(859, 177);
            this.ircDataGridView1.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 732);
            this.Controls.Add(this.btnUpdateChannelSetting);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancelCreateNewChannel);
            this.Controls.Add(this.btnAddChannel);
            this.Controls.Add(this.btnCreateNewChannel);
            this.Controls.Add(this.btnPart);
            this.Controls.Add(this.comboBoxCurrentChannel);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.txtChannelDisplayName);
            this.Controls.Add(this.ircDataGridView2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancelCreateNewServer);
            this.Controls.Add(this.btnCreateNewServer);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAddServer);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ircDataGridView1);
            this.Controls.Add(this.btnUpdateHost);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNickname);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ircDataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ircDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtNickname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdateHost;
        private Irc4Control.IrcDataGridView ircDataGridView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnAddServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Button btnCreateNewServer;
        private System.Windows.Forms.Button btnCancelCreateNewServer;
        private System.Windows.Forms.Label label4;
        private Irc4Control.IrcDataGridView ircDataGridView2;
        private System.Windows.Forms.TextBox txtChannelDisplayName;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.ComboBox comboBoxCurrentChannel;
        private System.Windows.Forms.Button btnPart;
        private System.Windows.Forms.Button btnCreateNewChannel;
        private System.Windows.Forms.Button btnAddChannel;
        private System.Windows.Forms.Button btnCancelCreateNewChannel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnUpdateChannelSetting;
    }
}

