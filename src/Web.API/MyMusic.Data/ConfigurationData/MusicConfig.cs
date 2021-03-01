using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data
{
    public class MusicConfig : IEntityTypeConfiguration<Music>
    {
        public void Configure(EntityTypeBuilder<Music> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).UseIdentityColumn();

            builder.Property(m => m.Name).IsRequired().HasMaxLength(50);

            builder.HasOne(m => m.Artist).WithMany(m => m.Musics).HasForeignKey(m => m.ArtistId);

            builder.ToTable("Music");
        }
    }
}
