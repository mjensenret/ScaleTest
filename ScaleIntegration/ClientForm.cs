using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScaleIntegration_Client
{
    public partial class frm920Simulation : Form
    {
        private Socket _clientSocket, _serverSocket;

        public frm920Simulation()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(txtServerIpAddress.Text), Convert.ToInt32(txtPortNumber.Text)), new AsyncCallback(ConnectCallback), null);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                //byte[] buffer = Encoding.ASCII.GetBytes(txtCommand.Text);
                byte[] buffer = Encoding.Default.GetBytes(WrapDataInProtocolEnvelope(txtCommand.Text));
                _clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), _clientSocket);
            }
            catch (SocketException) { } //Server closed
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                _clientSocket.EndSend(ar);

                var recBuf = new byte[1024];
                int received = _clientSocket.Receive(recBuf, SocketFlags.None);

                if (received == 0)
                    return;

                var data = new byte[received];

                Array.Copy(recBuf, data, received);

                string text = Encoding.ASCII.GetString(data);

                Console.WriteLine("Server Response: " + text);
                AppendServerResponse(text);

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                _clientSocket.EndConnect(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendServerResponse(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                lstServerResponse.Items.Add(text);
            });
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private bool isConnected()
        {
            if(_clientSocket.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string WrapDataInProtocolEnvelope(string command)
        {
            return string.IsNullOrEmpty(command)
                ? ""
                : $"{command}\r";
        }
    }
}
