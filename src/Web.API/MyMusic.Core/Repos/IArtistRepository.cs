using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IArtistRepository: IRepository<Artist>
    {
        Task<IEnumerable<Artist>> GetAllWithMusicAsync();

        Task<Artist> GetArtistWithMusicByIdAsync(int id);
    }
}
