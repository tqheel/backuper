using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.IO.Compression;

namespace backuper
{
    class Program
    {
        private static string source = ConfigurationSettings.AppSettings.Get("sourceFolder");
        private static string destination = ConfigurationSettings.AppSettings.Get("detinationFolder");
        private static string backupType = ConfigurationSettings.AppSettings.Get("filesOrFolder");
        
        static void Main(string[] args)
        {
            //generate new timestamped filename for zip archive
            string zipFileName = destination + @"\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                    DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                    DateTime.Now.Second.ToString() + ".zip";

            if (backupType == BackupType.folder.ToString())
            {
                Console.WriteLine("Writing " + source + " to archive file " + zipFileName + "...");
                ZipFile.CreateFromDirectory(source, zipFileName, CompressionLevel.Optimal, false);
            }
            else
            {
                string fileNamesFromConfig = ConfigurationSettings.AppSettings.Get("fileNames");
                string[] files = fileNamesFromConfig.Split(',');
                using (ZipArchive archive = ZipFile.Open(zipFileName, ZipArchiveMode.Update))
                {
                    foreach (string f in files)
                    {
                        string filePath = source + @"\" + f;
                        Console.WriteLine("Writing " + filePath + " to archive file " + zipFileName + "...");
                        archive.CreateEntryFromFile(filePath, f, CompressionLevel.Optimal);
                    }
                }
            }

        }
    }
}
