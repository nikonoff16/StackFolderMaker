using System;
using System.IO;

namespace FolderMakerUtility
{
    internal class Program {
        private static void Main(string[] args)
        {
            /**
             * Версия 2.0.1
             */
            var runner = new FolderMaker(@"config.json");
            runner.Start();

        }
    }
}

