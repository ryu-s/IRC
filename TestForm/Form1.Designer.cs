﻿namespace Irc4TestForm
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
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ircDataGridView1 = new Irc4Control.IrcDataGridView();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ircDataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ircDataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
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
            this.textBox2.Location = new System.Drawing.Point(12, 674);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(778, 20);
            this.textBox2.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(796, 674);
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
            this.txtHost.TabIndex = 16;
            // 
            // txtNickname
            // 
            this.txtNickname.Location = new System.Drawing.Point(89, 64);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(211, 20);
            this.txtNickname.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 99;
            this.label1.Text = "Nickname";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 99;
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
            this.label3.TabIndex = 99;
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
            this.btnCreateNewChannel.Click += new System.EventHandler(this.btnCreateNewChannel_Click);
            // 
            // btnAddChannel
            // 
            this.btnAddChannel.Location = new System.Drawing.Point(294, 282);
            this.btnAddChannel.Name = "btnAddChannel";
            this.btnAddChannel.Size = new System.Drawing.Size(75, 23);
            this.btnAddChannel.TabIndex = 25;
            this.btnAddChannel.Text = "AddChannel";
            this.btnAddChannel.UseVisualStyleBackColor = true;
            this.btnAddChannel.Click += new System.EventHandler(this.btnAddChannel_Click);
            // 
            // btnCancelCreateNewChannel
            // 
            this.btnCancelCreateNewChannel.Location = new System.Drawing.Point(375, 282);
            this.btnCancelCreateNewChannel.Name = "btnCancelCreateNewChannel";
            this.btnCancelCreateNewChannel.Size = new System.Drawing.Size(75, 23);
            this.btnCancelCreateNewChannel.TabIndex = 26;
            this.btnCancelCreateNewChannel.Text = "Cancel";
            this.btnCancelCreateNewChannel.UseVisualStyleBackColor = true;
            this.btnCancelCreateNewChannel.Click += new System.EventHandler(this.btnCancelCreateNewChannel_Click);
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
            this.ircDataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.ircDataGridView2.Location = new System.Drawing.Point(12, 308);
            this.ircDataGridView2.Name = "ircDataGridView2";
            this.ircDataGridView2.RowHeadersVisible = false;
            this.ircDataGridView2.Size = new System.Drawing.Size(859, 290);
            this.ircDataGridView2.TabIndex = 19;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "time";
            this.dataGridViewTextBoxColumn9.HeaderText = "time";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "sender";
            this.dataGridViewTextBoxColumn10.HeaderText = "sender";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "command";
            this.dataGridViewTextBoxColumn11.HeaderText = "command";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "text";
            this.dataGridViewTextBoxColumn12.HeaderText = "text";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Width = 400;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "time";
            this.dataGridViewTextBoxColumn1.HeaderText = "time";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "sender";
            this.dataGridViewTextBoxColumn2.HeaderText = "sender";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "command";
            this.dataGridViewTextBoxColumn3.HeaderText = "command";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "text";
            this.dataGridViewTextBoxColumn4.HeaderText = "text";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 400;
            // 
            // ircDataGridView1
            // 
            this.ircDataGridView1.AllowUserToAddRows = false;
            this.ircDataGridView1.AllowUserToDeleteRows = false;
            this.ircDataGridView1.AllowUserToResizeRows = false;
            this.ircDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ircDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ircDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.ircDataGridView1.Location = new System.Drawing.Point(12, 99);
            this.ircDataGridView1.Name = "ircDataGridView1";
            this.ircDataGridView1.RowHeadersVisible = false;
            this.ircDataGridView1.Size = new System.Drawing.Size(859, 177);
            this.ircDataGridView1.TabIndex = 11;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "time";
            this.dataGridViewTextBoxColumn13.HeaderText = "time";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "sender";
            this.dataGridViewTextBoxColumn14.HeaderText = "sender";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "command";
            this.dataGridViewTextBoxColumn15.HeaderText = "command";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "text";
            this.dataGridViewTextBoxColumn16.HeaderText = "text";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.Width = 400;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "time";
            this.dataGridViewTextBoxColumn5.HeaderText = "time";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "sender";
            this.dataGridViewTextBoxColumn6.HeaderText = "sender";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "command";
            this.dataGridViewTextBoxColumn7.HeaderText = "command";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "text";
            this.dataGridViewTextBoxColumn8.HeaderText = "text";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 400;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 710);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(883, 22);
            this.statusStrip1.TabIndex = 100;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 732);
            this.Controls.Add(this.statusStrip1);
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
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    }
}

