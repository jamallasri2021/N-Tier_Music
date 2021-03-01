using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusicMVC
{
    public class Music
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }
    }
}
