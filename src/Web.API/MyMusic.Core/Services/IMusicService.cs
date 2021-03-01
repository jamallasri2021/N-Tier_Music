using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IMusicService
    {
        Task<IEnumerable<Music>> GetAllWithArtist();

        Task<Music> GetMusicById(int id);

        Task<IEnumerable<Music>> GetMusicsByArtistId(int artistId);

        Task<Music> CreateMusic(Music music);

        Task UpdateMusic(Music musicToUpdate, Music music);

        Task DeleteMusic(Music music);
    }
}
