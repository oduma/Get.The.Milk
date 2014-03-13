using System.IO;

namespace GetTheMilk.Utils.IO
{
    public static class ReadWriteStrategies
    {
        public static Stream UncompressedReader(string fullPath)
        {
            return File.OpenRead(fullPath);
        }

        public static void UncompressedWriter(string content, string fullPath)
        {
            using(var fw= File.CreateText(fullPath))
            {
                fw.Write(content);
                fw.Flush();
                fw.Close();
            }
        }
    }
}
