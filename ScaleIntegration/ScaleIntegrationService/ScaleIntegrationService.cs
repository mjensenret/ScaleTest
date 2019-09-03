﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ScaleIntegrationService
{
    public partial class ScaleIntegrationService : ServiceBase
    {
        private int eventId = 1;
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
        public ScaleIntegrationService(string [] args)
        {
            InitializeComponent();

            string eventSourceName = "ScaleSource";
            string logName = "ScaleLog";

            if (args.Length > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Length > 1)
            {
                logName = args[1];
            }

            evntScaleEvents = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("ScaleSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "ScaleSource", "ScaleLog"
                    );
            }
            evntScaleEvents.Source = eventSourceName;
            evntScaleEvents.Log = logName;
        }

        protected override void OnStart(string[] args)
        {
            evntScaleEvents.WriteEntry("Starting");
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Set up a timer that triggers every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // TODO: add starting stuff here

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            evntScaleEvents.WriteEntry("Stopping");

        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            // TODO: Look into monitoring in the background https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.backgroundworker

            evntScaleEvents.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
    }
}
