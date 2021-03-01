using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IMusicRepository: IRepository<Music>
    {
        Task<IEnumerable<Music>> GetAllWithArtistAsync();

        Task<Music> GetMusicWithArtistByIdAsync(int id);

        Task<IEnumerable<Music>> GetAllWithArtistByArtistIdAsync(int artistId);
    }
}
