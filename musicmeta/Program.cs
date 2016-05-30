using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicmeta
{
    class Program
    {
        public static string OUTFORMAT = "csv";
        public static string OUTFILE = "";
        public static string SCANDIR = "";
        public static string LOGDIR = "logs";
        public static string LOGFILE = "last.log";
        public static string DETAILEDLOG = "detailed.log";
        public static string OLDLOGFILE = "previous.log";
        public static readonly List<string> FORMATLIST = new List<string>{ "flac", "ogg", "mp3", "wav" }; 

        static void Main(string[] args)
        {
            //arg 0 is root to scan
            //arg 1 is outfile
            //arg 2 if set is format (default csvish)

            if (args.Length == 3 && args[2].Equals("--xml"))
            {
                OUTFORMAT = "xml";
            }

            if (args.Length >= 2)
            {
                OUTFILE = args[1];
                SCANDIR = args[0];
            }

            // Checking if scandir exists
            if (!Directory.Exists(SCANDIR))
            {
                Console.WriteLine("Scandirectory does not exist: " + SCANDIR);
            }

            // Checking if logdir exists
            if (!Directory.Exists(LOGDIR))
            {
                try
                {
                    Directory.CreateDirectory(LOGDIR);
                }
                catch (Exception e)
                {
                    Console.WriteLine("There were some problems creating the logdir:");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }

            // Remove an existing oldlog and move lastlog to oldlog
            if (File.Exists(LOGDIR + Path.DirectorySeparatorChar + LOGFILE))
            {
                if (File.Exists(LOGDIR + Path.DirectorySeparatorChar + OLDLOGFILE))
                {
                    File.Delete(LOGDIR + Path.DirectorySeparatorChar + OLDLOGFILE);
                }

                File.Move(LOGDIR + Path.DirectorySeparatorChar + LOGFILE, LOGDIR + Path.DirectorySeparatorChar + OLDLOGFILE);
            }

            // If detailed log (stacktraces) exist, delete it
            if (File.Exists(LOGDIR + Path.DirectorySeparatorChar + DETAILEDLOG))
            {
                File.Delete(LOGDIR + Path.DirectorySeparatorChar + DETAILEDLOG);
            }

            if (File.Exists(OUTFILE))
            {
                File.Delete(OUTFILE);
            }


            Program p = new Program();
        }

        public Program()
        {
            // Start scanning....
            MediaDir rootDir = new MediaDir(SCANDIR);
            List<string> foundFiles = new List<string>();
            foundFiles.Add(MediaDir.GetHeader());
            foundFiles.AddRange(rootDir.GetFiles());

            File.AppendAllLines(OUTFILE, foundFiles);
        }
    }
}
