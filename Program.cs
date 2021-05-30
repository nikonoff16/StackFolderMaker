using System;
using System.IO;

namespace Скрипт_создания_папки_ежедневки
{
    class Program {
        static void Main(string[] args)
        {
            /**
             * Версия 2.0.1
             */
            FolderMaker runner = new FolderMaker(@"config.json");
            runner.start();

        }
    }
}
