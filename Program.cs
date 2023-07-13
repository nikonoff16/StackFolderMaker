namespace FolderMakerUtility
{
    internal class Program {
        private static void Main(string[] args)
        {
            var runner = new FolderMaker(@"config.json");
            runner.Start();

        }
    }
}

