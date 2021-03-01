using Microsoft.EntityFrameworkCore;
using MyMusic.Core;
using System;

namespace MyMusic.Data
{
    public class MyMusicDbContext: DbContext
    {
        public DbSet<Artist> Artists { get; set; }

        public DbSet<Music> Musics { get; set; }

        public DbSet<User> Users { get; set; }

        public MyMusicDbContext(DbContextOptions<MyMusicDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MusicConfig());
            builder.ApplyConfiguration(new ArtistConfig());
            builder.ApplyConfiguration(new UserConfig());
        }
    }
}
