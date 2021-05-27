using System;
using System.Collections.Generic;
using System.Text;


namespace Скрипт_создания_папки_ежедневки
{
    class Configurator
    {
        public string Path { get; set; }
        public bool DeleteEmpty { get; set; }
        public string FolderMask { get; set; }
        public bool DeleteOlder { get; set; }
        public int SavePeriod { get; set; }
    }
}
