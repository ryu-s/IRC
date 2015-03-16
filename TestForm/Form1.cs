using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Irc4TestForm
{
    public partial class Form1 : Form
    {
        Irc4.Server server;
        public Form1()
        {
            InitializeComponent();
            server = new Irc4.Server();
            server.ConnectSuccess += server_ConnectSuccess;
            server.ReceiveEvent += server_ReceiveEvent;
        }

        void server_ConnectSuccess(object sender, EventArgs e)
        {

        }

        void server_ReceiveEvent(object sender, Irc4.IRCReceiveEventArgs e)
        {
            Action action = () =>
            {
                this.textBox1.Text += e.log.Text + Environment.NewLine;
                this.textBox1.SelectionStart = this.textBox1.TextLength - 1;
                this.textBox1.ScrollToCaret();
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var serverInfo = new Irc4.ServerInfo();
            serverInfo.DisplayName = "";
            //serverInfo.Hostname = "192.168.56.101";
            serverInfo.Hostname = "chat1.ustream.tv";
            serverInfo.Port = 6667;

            server.SetInfo(serverInfo);

            await server.Connect();


        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await server.SendCmd(this.textBox2.Text);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await server.Disconnect();
        }
    }
}
