using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services
{
    public class ComposerService : IComposerService
    {
        private readonly IComposerRepository _ComposerRepository;

        public ComposerService(IComposerRepository composerRepository)
        {
            _ComposerRepository = composerRepository;
        }

        public async Task<Composer> CreateComposer(Composer composer)
        {
            return await _ComposerRepository.CreateComposer(composer);
        }

        public async Task<bool> DeleteComposer(string id)
        {
            return await _ComposerRepository.DeleteComposer(id);
        }

        public async Task<IEnumerable<Composer>> GetAllComposer()
        {
            return await _ComposerRepository.GetAllComposer();
        }

        public async Task<Composer> GetComposerById(string id)
        {
            return await _ComposerRepository.GetComposerById(id);
        }

        public void UpdateCompser(string id, Composer compser)
        {
            _ComposerRepository.UpdateCompser(id, compser);
        }
    }
}
