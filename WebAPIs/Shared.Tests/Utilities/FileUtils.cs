using System.IO;

namespace Shared.Tests.Utilities
{
    public static class FileUtils
    {
        public static string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
