using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data
{
    public class AppInfo
    {
        public AppInfo(string appId, string appName, string appVersion, string deviceName, string appToken = null)
        {
            AppId = appId ?? throw new ArgumentNullException(nameof(appId));
            AppName = appName ?? throw new ArgumentNullException(nameof(appName));
            AppVersion = appVersion ?? throw new ArgumentNullException(nameof(appVersion));
            DeviceName = deviceName ?? throw new ArgumentNullException(nameof(deviceName));
            AppToken = appToken;
        }

        [NotNull]
        public string AppId { get; }

        [NotNull]
        public string AppName { get; }

        [NotNull]
        public string AppVersion { get; }

        [NotNull]
        public string DeviceName { get; }

        public string AppToken { get; internal set; }
    }
}
