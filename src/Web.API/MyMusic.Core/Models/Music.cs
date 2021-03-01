using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Core
{
    public class Music
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }
    }
}
