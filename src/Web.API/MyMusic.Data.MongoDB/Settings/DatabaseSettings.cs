using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data.MongoDB
{
    public class DatabaseSettings : IDatabaseSettings
    {
        private readonly IMongoDatabase _Db;

        public DatabaseSettings(IOptions<Setting> options, IMongoClient client)
        {
            _Db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<Composer> Composers => _Db.GetCollection<Composer>("Composer");
    }
}
