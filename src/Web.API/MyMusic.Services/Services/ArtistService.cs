using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ArtistService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public async Task<Artist> CreateArtist(Artist artist)
        {
            await _UnitOfWork.Artists.AddAsync(artist);
            await _UnitOfWork.CommitAsync();

            return artist;
        }

        public async Task DeleteArtist(Artist artist)
        {
            _UnitOfWork.Artists.Remove(artist);
            await _UnitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Artist>> GetAllArtists()
        {
            return await _UnitOfWork.Artists.GetAllAsync();
        }

        public async Task<Artist> GetArtistById(int id)
        {
            return await _UnitOfWork.Artists.GetByIdAsync(id);
        }

        public async Task<Artist> GetArtistWithMusicsById(int id)
        {
            return await _UnitOfWork.Artists.GetArtistWithMusicByIdAsync(id);
        }

        public async Task<IEnumerable<Artist>> GetAllWithMusics()
        {
            return await _UnitOfWork.Artists.GetAllWithMusicAsync();
        }

        public async Task UpdateArtist(Artist artistToUpdate, Artist artist)
        {
            artistToUpdate.Name = artist.Name;

            await _UnitOfWork.CommitAsync();
        }
    }
}
