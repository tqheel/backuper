using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace backuper
{
    class Program
    {
        private static string source = ConfigurationSettings.AppSettings.Get("sourceFolder");
        private static string destination = ConfigurationSettings.AppSettings.Get("detinationFolder");
        static void Main(string[] args)
        {
            //create all the directories
            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                string newDir = dirPath.Replace(source, destination);
                if (!Directory.Exists(newDir))
                {
                    Console.WriteLine("Creating directory " + newDir + "...");
                    Directory.CreateDirectory(newDir);
                }
            }
            //copy files
            foreach (string newPath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                Console.WriteLine("Copying " + newPath + "...");
                File.Copy(newPath, newPath.Replace(source, destination), true);
            }
        }
    }
}
