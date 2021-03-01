using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data
{
    public class ArtistConfig : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id).UseIdentityColumn();

            builder.Property(a => a.Name).IsRequired().HasMaxLength(50);

            builder.ToTable("Artist");
        }
    }
}
