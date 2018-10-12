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
using Domain.Repositories;
using System.Globalization;

namespace ScaleIntegration_Server
{
    public partial class frmServerForm : Form
    {
        private Socket _serverSocket, _clientSocket;
        private byte[] _buffer;
        private static TransferOrderRepository repo = new TransferOrderRepository();
        
        public frmServerForm()
        {
            
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

            AppendToInboundTextBox(text);

            byte[] response;
            int auditNumber;
            int sequenceNumber;

            if (text.ToLower().Contains("query"))
            {
                Console.WriteLine("You are looking for an audit number");
                int start = text.IndexOf("|", 0) + 1;
                int end = text.IndexOf("|", (start));
                int transferOrderId = Convert.ToInt32(text.Substring(start, end - start));

                if (repo.TransferOrderValid(transferOrderId))
                {
                    response = Encoding.ASCII.GetBytes("F#1=QUERY|Y|" + DateTime.Now.ToLongTimeString());
                }
                else
                {
                    response = Encoding.ASCII.GetBytes("F#1=QUERY|N|" + DateTime.Now.ToLongTimeString());
                }

            }
            else if (text.ToLower().Contains("inbound"))
            {
                Console.WriteLine("You just sent me an inbound transaction");
                int auditNumberStart = text.IndexOf("|", 0) + 1;
                int auditNumberEnd = text.IndexOf("|", auditNumberStart);
                int sequenceNumberStart = text.IndexOf("|", auditNumberEnd) + 1;
                int sequenceNumberEnd = text.IndexOf("|", sequenceNumberStart);
                int driverIdStart = text.IndexOf("|", sequenceNumberEnd) + 1;
                int driverIdEnd = text.IndexOf("|", driverIdStart);
                int truckNumberStart = text.IndexOf("|", driverIdEnd) + 1;
                int truckNumberEnd = text.IndexOf("|", truckNumberStart);
                int trailerNumberStart = text.IndexOf("|", truckNumberEnd) + 1;
                int trailerNumberEnd = text.IndexOf("|", trailerNumberStart);
                int weightStart = text.IndexOf("|", trailerNumberEnd) + 1;
                int weightEnd = text.IndexOf("|", weightStart);
                int dateStart = text.IndexOf("|", weightEnd) + 1;
                int dateEnd = text.IndexOf("|", dateStart);

                auditNumber = Convert.ToInt32(text.Substring(auditNumberStart, auditNumberEnd - auditNumberStart));
                sequenceNumber = Convert.ToInt32(text.Substring(sequenceNumberStart, sequenceNumberEnd - sequenceNumberStart));
                int driverId = Convert.ToInt32(text.Substring(driverIdStart, driverIdEnd - driverIdStart));
                int truckNumber = Convert.ToInt32(text.Substring(truckNumberStart, truckNumberEnd - truckNumberStart));
                int trailerNumber = Convert.ToInt32(text.Substring(trailerNumberStart, trailerNumberEnd - trailerNumberStart));
                decimal weight = Convert.ToDecimal(text.Substring(weightStart, weightEnd - weightStart));
                string scaleDateTxt = text.Substring(dateStart, dateEnd - dateStart);
                DateTime scaleDate = DateTime.ParseExact(scaleDateTxt, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);

                if (repo.UpdateInboundScaleData(auditNumber, sequenceNumber, driverId, truckNumber, trailerNumber, weight, scaleDate))
                {
                    response = Encoding.ASCII.GetBytes("F#1=INBOUND|Y|" + DateTime.Now.ToLongTimeString());
                }
                else
                {
                    response = Encoding.ASCII.GetBytes("F#1=INBOUND|N|" + DateTime.Now.ToLongTimeString());
                };
                
            }
            else if (text.ToLower().Contains("outbound"))
            {
                Console.WriteLine("We have an outbound transaction");
                int auditNumberStart = text.IndexOf("|", 0) + 1;
                int auditNumberEnd = text.IndexOf("|", auditNumberStart);
                int sequenceNumberStart = text.IndexOf("|", auditNumberEnd) + 1;
                int sequenceNumberEnd = text.IndexOf("|", sequenceNumberStart);
                int loaderStart = text.IndexOf("|", sequenceNumberEnd) + 1;
                int loaderEnd = text.IndexOf("|", loaderStart);
                int weightStart = text.IndexOf("|", loaderEnd) + 1;
                int weightEnd = text.IndexOf("|", weightStart);
                int dateStart = text.IndexOf("|", weightEnd) + 1;
                int dateEnd = text.IndexOf("|", dateStart);

                auditNumber = Convert.ToInt32(text.Substring(auditNumberStart, auditNumberEnd - auditNumberStart));
                sequenceNumber = Convert.ToInt32(text.Substring(sequenceNumberStart, sequenceNumberEnd - sequenceNumberStart));
                int loaderId = Convert.ToInt32(text.Substring(loaderStart, loaderEnd - loaderStart));
                decimal weight = Convert.ToDecimal(text.Substring(weightStart, weightEnd - weightStart));
                string scaleDateTxt = text.Substring(dateStart, dateEnd - dateStart);
                DateTime scaleDate = DateTime.ParseExact(scaleDateTxt, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);

                if (repo.UpdateOutboundScaleData(auditNumber, sequenceNumber, loaderId, weight, scaleDate))
                {
                    response = Encoding.ASCII.GetBytes("F#1=OUTBOUND|Y|" + DateTime.Now.ToLongTimeString());
                }
                else
                {
                    response = Encoding.ASCII.GetBytes("F#1=OUTBOUND|N|" + DateTime.Now.ToLongTimeString());
                }

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
            AppendToResponseTextBox(response);

            _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _clientSocket);

        }

        private void AppendToInboundTextBox(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                lstClientCommand.Items.Add(text);
            });
        }

        private void AppendToResponseTextBox(byte[] response)
        {
            string responseText = Encoding.ASCII.GetString(response);
            Invoke((MethodInvoker)delegate
            {
                lstServerResponse.Items.Add(responseText);
            });
        }
    }
}
