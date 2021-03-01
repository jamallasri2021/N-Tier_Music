using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API
{
    public class MusicResource
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public ArtistResource Artist { get; set; }
    }
}
