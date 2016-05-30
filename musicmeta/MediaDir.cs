using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicmeta
{
    class MediaDir
    {
        string currentDir;
        List<MediaDir> subDirs;
        List<MediaFile> files;

        public MediaDir(string dir)
        {
            subDirs = new List<MediaDir>();
            files = new List<MediaFile>();

            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Directory does not exists: " + dir);
                Environment.Exit(0);
            }

            currentDir = dir;
            Scan();
        }

        public int Scan()
        {
            string[] filesInDir = Directory.GetFiles(currentDir);
            int count = 0;

            foreach (var file in filesInDir)
            {
                if (IsMediaFile(file))
                {
                    MediaFile f = new MediaFile(file);
                    files.Add(f);
                    count++;
                }
            }

            foreach (var dir in Directory.GetDirectories(currentDir))
            {
                subDirs.Add(new MediaDir(dir));
            }

            return count;
        }

        protected bool IsMediaFile(string file)
        {
            string ext = Path.GetExtension(file).Trim('.');

            if (Program.FORMATLIST.Contains(ext))
            {
                return true;
            }

            return false;
        }

        public List<string> GetFiles()
        {
            List<string> mediafiles = new List<string>();

            foreach (MediaDir md in subDirs)
            {
                List<string> f = md.GetFiles();
                mediafiles.AddRange(f);
            }

            foreach (MediaFile mf in files)
            {
                mediafiles.Add(mf.ToString());
            }

            return mediafiles;
        }

        public static string GetHeader()
        {
            return "Filename" + '\t' + "Artist" + '\t' + "Album" + '\t' + "Year" + '\t' + "Trackno" + '\t' + "Title";
        }
    }
}
