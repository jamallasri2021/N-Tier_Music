using System;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IUnitOfWork: IDisposable
    {
        IArtistRepository Artists { get; }

        IMusicRepository Musics { get; }

        IUserRepository Users { get; }

        Task<int> CommitAsync();
    }
}
