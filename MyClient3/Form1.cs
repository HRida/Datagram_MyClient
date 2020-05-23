using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace MyClient3
{
    public partial class Form1 : Form
    {
        private UdpClient client;
        private IPEndPoint receivePoint;
        public Form1()
        {
            InitializeComponent();
            receivePoint = new IPEndPoint(new IPAddress(0), 0);
            client = new UdpClient(5002);
            Thread thread =
               new Thread(new ThreadStart(WaitForPackets));
            thread.Start();
        }
        // shut down the client
        protected void Client_Closing(
           object sender, CancelEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }
        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // create packet (datagram) as string
                string packet = inputTextBox.Text;
                Action updateLabel = () => displayTextBox.Text += "\r\nSending packet containing: " + packet;
                displayTextBox.Invoke(updateLabel);

                // convert packet to byte array
                byte[] data =
                   System.Text.Encoding.ASCII.GetBytes(packet);

                // send packet to server on port 5000
                client.Send(data, data.Length, "localhost", 5000);
                updateLabel = () => displayTextBox.Text += "\r\nPacket sent\r\n";
                displayTextBox.Invoke(updateLabel);
                inputTextBox.Clear();
            }
        }
        // wait for packets to arrive
        public void WaitForPackets()
        {
            while (true)
            {
                // receive byte array from server 
                byte[] data = client.Receive(ref receivePoint);
                Action updateLabel = () => displayTextBox.Text +="\r\nPacket received:" +
                   "\r\nLength: " + data.Length + "\r\nContaining: " +
                   System.Text.Encoding.ASCII.GetString(data) +
                   "\r\n";
                displayTextBox.Invoke(updateLabel);
            }

        }
    }
}
