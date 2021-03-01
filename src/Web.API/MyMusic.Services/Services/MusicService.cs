using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services
{
    public class MusicService : IMusicService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public MusicService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public async Task<Music> CreateMusic(Music music)
        {
            await _UnitOfWork.Musics.AddAsync(music);
            await _UnitOfWork.CommitAsync();

            return music;
        }

        public async Task DeleteMusic(Music music)
        {
            _UnitOfWork.Musics.Remove(music);

            await _UnitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Music>> GetAllWithArtist()
        {
            return await _UnitOfWork.Musics.GetAllWithArtistAsync();
        }

        public async Task<Music> GetMusicById(int id)
        {
            return await _UnitOfWork.Musics.GetMusicWithArtistByIdAsync(id);
        }

        public async Task<IEnumerable<Music>> GetMusicsByArtistId(int artistId)
        {
            return await _UnitOfWork.Musics.GetAllWithArtistByArtistIdAsync(artistId);
        }

        public async Task UpdateMusic(Music musicToUpdate, Music music)
        {
            musicToUpdate.Name = music.Name;
            musicToUpdate.ArtistId = music.ArtistId;

            await _UnitOfWork.CommitAsync();
        }
    }
}
