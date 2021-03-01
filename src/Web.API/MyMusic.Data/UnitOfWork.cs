using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly MyMusicDbContext _Context;
        private IMusicRepository _MusicRepository;
        private IArtistRepository _ArtistRepository;
        private IUserRepository _UserRepository;

        public UnitOfWork(MyMusicDbContext context)
        {
            this._Context = context;
        }

        public IMusicRepository Musics => _MusicRepository ??= new MusicRepository(_Context);

        public IArtistRepository Artists => _ArtistRepository ??= new ArtistRepository(_Context);

        public IUserRepository Users => _UserRepository ??= new UserRepository(_Context);

        public async Task<int> CommitAsync()
        {
            return await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }
}
