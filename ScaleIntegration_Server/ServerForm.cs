using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ScaleIntegration_Server
{
    public partial class frmServerForm : Form
    {
        private Socket _serverSocket, _clientSocket;
        private byte[] _buffer;

        public frmServerForm()
        {
            //string path = @"C:\Users\michaelj\Documents\GitHub\ScaleTest\ScaleIntegration\bin\Debug\ScaleIntegration_Client.exe";
            //Process.Start(path);
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 9171));
                _serverSocket.Listen(0);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                _clientSocket = _serverSocket.EndAccept(ar);                
                _buffer = new byte[_clientSocket.ReceiveBufferSize];
                _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _clientSocket);

                //_serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket current = (Socket)ar.AsyncState;
            int received;

            try
            {
                received = _clientSocket.EndReceive(ar);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                current.Close();
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(_buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Text received: " + text);

            AppendToTextBox(text);

            byte[] response;

            if(text.ToLower().Contains("query"))
            {
                Console.WriteLine("You are looking for an audit number");
                response = Encoding.ASCII.GetBytes("F#1=QUERY|Y|" + DateTime.Now.ToLongTimeString());

            }
            else if (text.ToLower().Contains("inbound"))
            {
                Console.WriteLine("You just sent me an inbound transaction");
                response = Encoding.ASCII.GetBytes("F#1=INBOUND|Y|" + DateTime.Now.ToLongTimeString());
            }
            else if (text.ToLower().Contains("outbound"))
            {
                Console.WriteLine("We have an outbound transaction");
                response = Encoding.ASCII.GetBytes("F#1=OUTBOUND|Y|" + DateTime.Now.ToLongTimeString());
            }
            else if (text.ToLower().Contains("test"))
            {
                Console.WriteLine("Test transaction was received");
                response = Encoding.ASCII.GetBytes("F#1=TEST|Y|" + DateTime.Now.ToLongTimeString());
            }
            else
            {
                Console.WriteLine("We have received an invalid request");
                response = Encoding.ASCII.GetBytes("F#1=INVALID|Y|" + DateTime.Now.ToLongTimeString());
            }

            current.Send(response);

            _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _clientSocket);

        }

        private void AppendToTextBox(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                lstClientCommand.Items.Add(text);
            });
        }
    }
}
