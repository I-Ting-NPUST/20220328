using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        UdpClient U;
        Thread Th;

        //監聽副程式
        private void Listen()
        {
            //設定監聽用的通訊埠
            int Port = int.Parse(textBox_listenPort.Text);
            //監聽UDP監聽器實體
            U = new UdpClient(Port);

            //建立本機端點資訊
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

            while (true)
            {
                byte[] B = U.Receive(ref EP);
                textBox_receiveMsg.Text = Encoding.Default.GetString(B);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Th = new Thread(Listen);
            Th.Start();
            button1.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Th.Abort();//關閉監聽執行續
                U.Close();//關閉監聽器
            }
            catch
            {
                //忽略錯誤
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string IP = textBox_targetIP.Text;
            int Port = int.Parse(textBox_targetPort.Text);
            byte[] B = Encoding.Default.GetBytes(textBox_sendMsg.Text);
            UdpClient S = new UdpClient();
            S.Send(B, B.Length, IP, Port);
            S.Close();
        }
    }
}
