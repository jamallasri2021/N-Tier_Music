using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyMusic.Core
{
    public class Artist
    {
        public Artist()
        {
            Musics = new Collection<Music>();
        }

        public int Id { get; set; }

        public String Name { get; set; }

        public ICollection<Music> Musics { get; set; }
    }
}
