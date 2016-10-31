using System;
using System.Collections.Generic;
using System.Text;

namespace m8916touch
{
    public static class EventLog
    {
        public static string FilePath { get; set; }

        public static void Write(string format, params object[] arg)
        {
            Write(string.Format(format, arg));
        }

        public static void Write(string message)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = System.IO.Directory.GetCurrentDirectory();
            }
            string filename = FilePath +
                string.Format("\\{0:yyyy}\\{0:MM}\\{0:yyyy-MM-dd}.txt", DateTime.Now);
            System.IO.FileInfo finfo = new System.IO.FileInfo(filename);
            if (finfo.Directory.Exists == false)
            {
                finfo.Directory.Create();
            }
            string writeString = string.Format("{0:yyyy/MM/dd HH:mm:ss} {1}",
                DateTime.Now, message) + Environment.NewLine;
            System.IO.File.AppendAllText(filename, writeString, Encoding.Unicode);
        }
    }/*public static class EventLog*/
}
