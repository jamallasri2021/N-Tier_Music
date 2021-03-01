using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetAllArtists();

        Task<Artist> GetArtistById(int id);

        Task<Artist> CreateArtist(Artist artist);

        Task UpdateArtist(Artist artistToUpdate, Artist artist);

        Task DeleteArtist(Artist artist);

        Task<Artist> GetArtistWithMusicsById(int id);

        Task<IEnumerable<Artist>> GetAllWithMusics();
    }
}
