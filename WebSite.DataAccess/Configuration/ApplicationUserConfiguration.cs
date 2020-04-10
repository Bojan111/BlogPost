﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSite.DataAcces.Identity;

namespace WebSite.DataAccess.Configuration
{
    class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(x => x.AuthorEntity)
                .WithOne()
                .HasForeignKey<ApplicationUser>(x => x.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
