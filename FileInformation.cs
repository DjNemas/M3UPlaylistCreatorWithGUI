using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3UPlaylistCreatorWithGUI
{
    public class FileInformation
    {
        public string FileName { get; set; }
        public string SongLenghtSeconds {get; set;}

        public string SongTitle { get; set; }

        public FileInformation(string fileName, string songTitle ,string songLenghtSeconds)
        {
            FileName = fileName;
            SongTitle = songTitle;
            SongLenghtSeconds = songLenghtSeconds;
        }

        public override string ToString()
        {
            return FileName.ToString();
        }
    }
}
