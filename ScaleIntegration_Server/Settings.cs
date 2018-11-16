using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleIntegration_Server.Properties
{
    internal sealed partial class Settings
    {
        public Settings()
        {

        }

        public string CommunicationLog
        {
            get
            {
                if (System.Reflection.Assembly.GetEntryAssembly() == null) return Path.Combine(LogPath, "ScaleIntegration.log");
                return Path.Combine(LogPath, System.Reflection.Assembly.GetEntryAssembly().GetName().Name + ".log");
            }
        }
    }
}
