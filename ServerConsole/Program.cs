using Logging;
using ServerConsole.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpServerLib.IO;
using TcpServerLib.IO.Net;

namespace ServerConsole
{
    class Program
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

        static void Main(string[] args)
        {
            
        }

        public FileLog commLog { get; set; }

        private void startService(object sender, EventArgs e)
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
    }
}
