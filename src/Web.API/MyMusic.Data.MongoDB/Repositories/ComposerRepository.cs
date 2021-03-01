using MongoDB.Bson;
using MongoDB.Driver;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.MongoDB
{
    public class ComposerRepository : IComposerRepository
    {
        private readonly IDatabaseSettings _Context;

        public ComposerRepository(IDatabaseSettings context)
        {
            _Context = context;
        }

        public async Task<Composer> CreateComposer(Composer composer)
        {
            await _Context.Composers.InsertOneAsync(composer);

            return composer;
        }

        public async Task<bool> DeleteComposer(string id)
        {
            ObjectId mongoId = new ObjectId(id);
            FilterDefinition<Composer> filter = Builders<Composer>.Filter.Eq(m => m.Id, mongoId);

            DeleteResult deleteResult = await _Context.Composers.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Composer>> GetAllComposer()
        {
            return await _Context.Composers.Find(_ => true).ToListAsync();
        }

        public async Task<Composer> GetComposerById(string id)
        {
            ObjectId mongoId = new ObjectId(id);
            FilterDefinition<Composer> filter = Builders<Composer>.Filter.Eq(m => m.Id, mongoId);

            return await _Context.Composers.Find(filter).FirstOrDefaultAsync();
        }

        public void UpdateCompser(string id, Composer composer)
        {
            ObjectId mongoId = new ObjectId(id);
            FilterDefinition<Composer> filter = Builders<Composer>.Filter.Eq(m => m.Id, mongoId);

            var update = Builders<Composer>.Update
                                                .Set("FirstName", composer.FirstName)
                                                .Set("LastName", composer.LastName);

            _Context.Composers.FindOneAndUpdate(filter, update);
        }
    }
}
