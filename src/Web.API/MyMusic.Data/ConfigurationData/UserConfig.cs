using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusic.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .Property(u => u.Id)
                .UseIdentityColumn();

            builder
                .Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .ToTable("User");
        }
    }
}
