using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace FolderMakerUtility
{
    /// <summary>
    /// Read json-config file
    /// </summary>
    /// <remarks>
    /// Requires 'config.json in the root app directory.
    /// PASTE IT THERE MANUALLY
    /// </remarks>
    internal class Configurator
    {
        public string Path { get; set; }

        public bool DeleteEmpty { get; set; }
        public string FolderMask { get; set; }
        public bool DeleteOlder { get; set; }
        public double SavePeriod { get; set; }
    }
}