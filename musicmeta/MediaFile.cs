using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace musicmeta
{
    //Todo: Implement
    class MediaFile
    {
        public string Filename;
        public string Title;
        public string Trackno;
        public string Year;
        public string Artist;
        public string Album;
        
        public bool FileFailed { get; protected set; }

        public MediaFile(string file)
        {
            FileFailed = false;

            if (!System.IO.File.Exists(file))
            {
                System.IO.File.AppendAllText(Program.LOGFILE, "File not found: " + file);
            }

            try
            {
                TagLib.File f = TagLib.File.Create(file);

                Filename = file;
                Title = f.Tag.Title;
                Trackno = f.Tag.Track.ToString();
                Year = f.Tag.Year.ToString();
                Artist = string.Join(", ", f.Tag.Performers);
                Album = f.Tag.Album;
            }
            catch (Exception e)
            {
                // Todo: Crappy way to handle this, but I need to log all exceptions, and taglib docs doesn't 
                // list the applicable ones....

                System.IO.File.AppendAllText(Program.LOGFILE, "Exception when opening file: " + file + ":" + e.Message + Environment.NewLine);
                
                System.IO.File.AppendAllText(Program.DETAILEDLOG, "Error " + file + ":" + Environment.NewLine);
                System.IO.File.AppendAllText(Program.DETAILEDLOG,
                    e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
            }
        }

        public new string ToString()
        {
            return Filename + '\t' + Artist + '\t' + Album + '\t' + Year + '\t' + Trackno + '\t' + Title;
        }
    }
}
