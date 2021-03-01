using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public interface IComposerService
    {
        Task<IEnumerable<Composer>> GetAllComposer();

        Task<Composer> GetComposerById(String id);

        Task<Composer> CreateComposer(Composer composer);

        Task<bool> DeleteComposer(String id);

        void UpdateCompser(String id, Composer compser);
    }
}
