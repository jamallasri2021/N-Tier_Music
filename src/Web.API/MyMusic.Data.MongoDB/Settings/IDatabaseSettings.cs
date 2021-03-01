using MongoDB.Driver;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data.MongoDB
{
    public interface IDatabaseSettings
    {
        IMongoCollection<Composer> Composers { get; }

    }
}
