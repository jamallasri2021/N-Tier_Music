using Microsoft.EntityFrameworkCore;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data
{
    public class ArtistRepository: Repository<Artist>, IArtistRepository
    {
        private MyMusicDbContext MyMusicDbContext
        {
            get { return Context as MyMusicDbContext; }
        }

        public ArtistRepository(MyMusicDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Artist>> GetAllWithMusicAsync()
        {
            return await MyMusicDbContext
                            .Artists
                            .Include(a => a.Musics)
                            .ToListAsync();
        }

        public async Task<Artist> GetArtistWithMusicByIdAsync(int id)
        {
            return await MyMusicDbContext
                            .Artists
                            .Include(a => a.Musics)
                            .SingleOrDefaultAsync(a => a.Id == id);

        }
    }
}
