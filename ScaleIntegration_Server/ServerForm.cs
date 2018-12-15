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
using Logging;
using ScaleIntegration_Server.Properties;
using TcpServerLib.IO;
using TcpServerLib.IO.Net;

namespace ScaleIntegration_Server
{
    public partial class frmServerForm : Form
    {

        private readonly IServer m_tcpServer = new Server(
            new CustomTcpListenerFactory(),
            new TcpConnectionFactory(new StandardIndicatorProtocol(),
                new ScaleProtocolHandler(),
                Settings.Default.chunkSize,
                Settings.Default.chunkDelay,
                Settings.Default.closeConnectionAfterResponse),
            Settings.Default.ListeningPort,
            Settings.Default.maxConnections);

        //private Socket _serverSocket, _clientSocket;
        //private byte[] _buffer;

        private static TransferOrderRepository repo = new TransferOrderRepository();
        // Received data string.  
        public StringBuilder sb = new StringBuilder();

        public frmServerForm()
        {
            
            InitializeComponent();
            //StartServer();

        }

        public FileLog commLog { get; set; }

        private void frmServerForm_Load(object sender, EventArgs e)
        {
            try
            {
                m_tcpServer.Start();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        //private void StartServer()
        //{
        //    commLog = new FileLog(Settings.Default.CommunicationLog, Settings.Default.LogDays);

        //    writeToLog("Server starting....");
        //    Console.WriteLine("Server starting....");

        //    try
        //    {
        //        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(Settings.Default.IPAddress), Convert.ToInt32(Settings.Default.ListeningPort)));
        //        _serverSocket.Listen(0);
        //        _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //    writeToLog("Server started....");
        //    Console.WriteLine("Server started....");
        //}

        //private void AcceptCallback(IAsyncResult ar)
        //{
        //    try
        //    {

        //        _clientSocket = _serverSocket.EndAccept(ar);

        //        writeToLog("Connection from: " + _clientSocket.RemoteEndPoint.ToString());

        //        _buffer = new byte[_clientSocket.ReceiveBufferSize];
        //        _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _clientSocket);

        //        //_serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void ReceiveCallback(IAsyncResult ar)
        //{
        //    Socket current = (Socket)ar.AsyncState;
        //    int received;
        //    String content = String.Empty;

        //    try
        //    {
        //        received = _clientSocket.EndReceive(ar);
        //    }
        //    catch (SocketException)
        //    {
        //        Console.WriteLine("Client forcefully disconnected");
        //        current.Close();
        //        return;
        //    }

        //    if (received > 0)
        //    {
        //        byte[] recBuf = new byte[received];
        //        Array.Copy(_buffer, recBuf, received);
        //        sb.Append(Encoding.ASCII.GetString(recBuf));

        //        // Check for end-of-file tag. If it is not there, read   
        //        // more data.  
        //        content = sb.ToString();
        //        if (content.EndsWith("|"))
        //        {
        //            // All the data has been read from the   
        //            // client. Display it on the console.
        //            Console.WriteLine("Text received: " + content);
        //            AppendToInboundTextBox(content);
        //            sb.Clear();
        //        }
        //        else
        //        {
        //            // Not all data received. Get more.  
        //            current.BeginReceive(_buffer, 0, _buffer.Length, 0,
        //            new AsyncCallback(ReceiveCallback), current);
        //        }


        //        //string text = Encoding.ASCII.GetString(recBuf);
        //        //string text = Encoding.Default.GetString(recBuf);

        //        //writeToLog("Command received: " + text);
        //    }

        //    //byte[] response;
        //    //int auditNumber;
        //    //int sequenceNumber;

        //    //if (text.ToLower().Contains("query"))
        //    //{
        //    //    Console.WriteLine("You are looking for an audit number");
        //    //    writeToLog("Command was a query");
        //    //    try
        //    //    {
        //    //        int start = text.IndexOf("|", 0) + 1;
        //    //        int end = text.IndexOf("|", (start));
        //    //        int transferOrderId = Convert.ToInt32(text.Substring(start, end - start));

        //    //        writeToLog("TransferOrderId: " + transferOrderId.ToString());

        //    //        if (repo.TransferOrderValid(transferOrderId))
        //    //        {
        //    //            response = Encoding.ASCII.GetBytes("F#1=QUERY|Y|" + DateTime.Now.ToLongTimeString());
        //    //            writeToLog("A valid transfer order was found");
        //    //        }
        //    //        else
        //    //        {
        //    //            response = Encoding.ASCII.GetBytes("F#1=QUERY|N|" + DateTime.Now.ToLongTimeString());
        //    //            writeToLog("Invalid transfer order was entered.");
        //    //        }
        //    //    }
        //    //    catch (Exception e)
        //    //    {
        //    //        writeToLog(e.Message);
        //    //        response = Encoding.ASCII.GetBytes("F#1=QUERY|N|" + DateTime.Now.ToLongTimeString());
        //    //    }




        //    //}
        //    //else if (text.ToLower().Contains("inbound"))
        //    //{
        //    //    Console.WriteLine("You just sent me an inbound transaction");

        //    //    int auditNumberStart = text.IndexOf("|", 0) + 1;
        //    //    int auditNumberEnd = text.IndexOf("|", auditNumberStart);
        //    //    int sequenceNumberStart = text.IndexOf("|", auditNumberEnd) + 1;
        //    //    int sequenceNumberEnd = text.IndexOf("|", sequenceNumberStart);
        //    //    int driverIdStart = text.IndexOf("|", sequenceNumberEnd) + 1;
        //    //    int driverIdEnd = text.IndexOf("|", driverIdStart);
        //    //    int truckNumberStart = text.IndexOf("|", driverIdEnd) + 1;
        //    //    int truckNumberEnd = text.IndexOf("|", truckNumberStart);
        //    //    int trailerNumberStart = text.IndexOf("|", truckNumberEnd) + 1;
        //    //    int trailerNumberEnd = text.IndexOf("|", trailerNumberStart);
        //    //    int weightStart = text.IndexOf("|", trailerNumberEnd) + 1;
        //    //    int weightEnd = text.IndexOf("|", weightStart);
        //    //    int dateStart = text.IndexOf("|", weightEnd) + 1;
        //    //    int dateEnd = text.IndexOf("|", dateStart);


        //    //    auditNumber = Convert.ToInt32(text.Substring(auditNumberStart, auditNumberEnd - auditNumberStart));
        //    //    sequenceNumber = Convert.ToInt32(text.Substring(sequenceNumberStart, sequenceNumberEnd - sequenceNumberStart));
        //    //    int driverId = Convert.ToInt32(text.Substring(driverIdStart, driverIdEnd - driverIdStart));
        //    //    int truckNumber = Convert.ToInt32(text.Substring(truckNumberStart, truckNumberEnd - truckNumberStart));
        //    //    int trailerNumber = Convert.ToInt32(text.Substring(trailerNumberStart, trailerNumberEnd - trailerNumberStart));
        //    //    decimal weight = Convert.ToDecimal(text.Substring(weightStart, weightEnd - weightStart));
        //    //    string scaleDateTxt = text.Substring(dateStart, dateEnd - dateStart);
        //    //    DateTime scaleDate = DateTime.ParseExact(scaleDateTxt, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);

        //    //    writeToLog("Inbound query received for audit number: " + auditNumber + "sequence number: " + sequenceNumber);

        //    //    if (repo.UpdateInboundScaleData(auditNumber, sequenceNumber, driverId, truckNumber, trailerNumber, weight, scaleDate))
        //    //    {
        //    //        response = Encoding.ASCII.GetBytes("F#1=INBOUND|Y|" + DateTime.Now.ToLongTimeString());
        //    //        writeToLog("Inbound transaction for audit number " + auditNumber + " sequence number " + sequenceNumber + "was updated successfully");
        //    //    }
        //    //    else
        //    //    {
        //    //        response = Encoding.ASCII.GetBytes("F#1=INBOUND|N|" + DateTime.Now.ToLongTimeString());
        //    //        writeToLog("Inbound transaction for audit number " + auditNumber + " sequence number " + sequenceNumber + "was either already updated or there was an error updating the record.");
        //    //    };

        //    //}
        //    //else if (text.ToLower().Contains("outbound"))
        //    //{
        //    //    Console.WriteLine("We have an outbound transaction");
        //    //    int auditNumberStart = text.IndexOf("|", 0) + 1;
        //    //    int auditNumberEnd = text.IndexOf("|", auditNumberStart);
        //    //    int sequenceNumberStart = text.IndexOf("|", auditNumberEnd) + 1;
        //    //    int sequenceNumberEnd = text.IndexOf("|", sequenceNumberStart);
        //    //    int loaderStart = text.IndexOf("|", sequenceNumberEnd) + 1;
        //    //    int loaderEnd = text.IndexOf("|", loaderStart);
        //    //    int weightStart = text.IndexOf("|", loaderEnd) + 1;
        //    //    int weightEnd = text.IndexOf("|", weightStart);
        //    //    int dateStart = text.IndexOf("|", weightEnd) + 1;
        //    //    int dateEnd = text.IndexOf("|", dateStart);

        //    //    auditNumber = Convert.ToInt32(text.Substring(auditNumberStart, auditNumberEnd - auditNumberStart));
        //    //    sequenceNumber = Convert.ToInt32(text.Substring(sequenceNumberStart, sequenceNumberEnd - sequenceNumberStart));
        //    //    int loaderId = Convert.ToInt32(text.Substring(loaderStart, loaderEnd - loaderStart));
        //    //    decimal weight = Convert.ToDecimal(text.Substring(weightStart, weightEnd - weightStart));
        //    //    string scaleDateTxt = text.Substring(dateStart, dateEnd - dateStart);
        //    //    DateTime scaleDate = DateTime.ParseExact(scaleDateTxt, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);

        //    //    if (repo.UpdateOutboundScaleData(auditNumber, sequenceNumber, loaderId, weight, scaleDate))
        //    //    {
        //    //        response = Encoding.ASCII.GetBytes("F#1=OUTBOUND|Y|" + DateTime.Now.ToLongTimeString());
        //    //        writeToLog("Outbound transaction for audit number " + auditNumber + " sequence number " + sequenceNumber + "was updated successfully");
        //    //    }
        //    //    else
        //    //    {
        //    //        response = Encoding.ASCII.GetBytes("F#1=OUTBOUND|N|" + DateTime.Now.ToLongTimeString());
        //    //        writeToLog("Outbound transaction for audit number " + auditNumber + " sequence number " + sequenceNumber + "was either already updated or there was an error updating the record.");
        //    //    }

        //    //}
        //    //else if (text.ToLower().Contains("test"))
        //    //{
        //    //    Console.WriteLine("Test transaction was received");
        //    //    response = Encoding.ASCII.GetBytes("F#1=TEST|Y|" + DateTime.Now.ToLongTimeString());
        //    //    writeToLog("Test command was received successfully.");
        //    //}
        //    //else
        //    //{
        //    //    Console.WriteLine("We have received an invalid request");
        //    //    response = Encoding.ASCII.GetBytes("F#1=INVALID|Y|" + DateTime.Now.ToLongTimeString());
        //    //    writeToLog("An invalid command was received: " + text.ToString());
        //    //}

        //    //current.Send(response);
        //    //AppendToResponseTextBox(response);

        //    //writeToLog("Response has been sent: " + Encoding.ASCII.GetString(response));

        //    //_clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _clientSocket);

        //}

        public void AppendToInboundTextBox(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                lstClientCommand.Items.Add(text);
            });
        }

        public void AppendToResponseTextBox(byte[] response)
        {
            string responseText = Encoding.ASCII.GetString(response);
            Invoke((MethodInvoker)delegate
            {
                lstServerResponse.Items.Add(responseText);
            });
        }

        private void writeToLog(string message)
        {
            if (commLog == null)
                ServiceLog.Default.Log(message);
            else
                commLog.Log(message);
        }
    }
}
