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
        private static string _source = ConfigurationSettings.AppSettings.Get("sourceFolder");
        private static string _destination = ConfigurationSettings.AppSettings.Get("detinationFolder");
        private static string _backupType = ConfigurationSettings.AppSettings.Get("filesOrFolder");
        private static string _copyOrCompress = ConfigurationSettings.AppSettings.Get("copyOrCompress");
        
        static void Main(string[] args)
        {
            if (_copyOrCompress == CopyOrCompress.copy.ToString())
            {
                CreateDirectories();
                CopyFiles();
            }
            else
            {
                CompressFiles();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            
        }

        private static void CreateDirectories()
        {
            //create all the directories
            foreach (string dirPath in Directory.GetDirectories(_source, "*", SearchOption.AllDirectories))
            {
                string newDir = dirPath.Replace(_source, _destination);
                if (!Directory.Exists(newDir))
                {
                    Console.WriteLine("Creating directory " + newDir + "...");
                    Directory.CreateDirectory(newDir);
                }
            }
        }

        private static void CopyFiles()
        {
            //copy files
            foreach (string sourcePath in Directory.GetFiles(_source, "*", SearchOption.AllDirectories))
            {
                
                try
                {
                    var destPath = sourcePath.Replace(_source, _destination);
                    if (File.Exists(destPath))
                    {
                        FileInfo sourceFi = new FileInfo(sourcePath);
                        FileInfo destFi = new FileInfo(destPath);
                        if (!sourceFi.IsReadOnly && sourceFi.LastWriteTimeUtc > destFi.LastWriteTimeUtc)
                        {
                            Console.WriteLine("Copying " + sourcePath + "...");
                            File.Copy(sourcePath, destPath, true);
                        }
                        else
                        {
                            if (destFi.IsReadOnly)
                            {
                                Console.WriteLine("Skipping " + destPath + " because it is read-only.");
                            }
                           Console.WriteLine("Skipping " + sourcePath + " because it is is unchanged.");
                        }
                    }
                    else
                    {
                        File.Copy(sourcePath, destPath, true);
                    }

                }
                catch (IOException copyError)
                {
                    throw new Exception("A copy error occured. Halting execution.");
                }
            }
        
        }

        private static void CompressFiles()
        {
            //generate new timestamped filename for zip archive
            string zipFileName = _destination + @"\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                    DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                    DateTime.Now.Second.ToString() + ".zip";

            if (_backupType == BackupType.folder.ToString())
            {
                Console.WriteLine("Writing " + _source + " to archive file " + zipFileName + "...");
                ZipFile.CreateFromDirectory(_source, zipFileName, CompressionLevel.Optimal, false);
            }
            else
            {
                string fileNamesFromConfig = ConfigurationSettings.AppSettings.Get("fileNames");
                string[] files = fileNamesFromConfig.Split(',');
                using (ZipArchive archive = ZipFile.Open(zipFileName, ZipArchiveMode.Update))
                {
                    foreach (string f in files)
                    {
                        string filePath = _source + @"\" + f;
                        Console.WriteLine("Writing " + filePath + " to archive file " + zipFileName + "...");
                        archive.CreateEntryFromFile(filePath, f, CompressionLevel.Optimal);
                    }
                }
            }
        }
        

    }


}
