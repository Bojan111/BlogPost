using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSite.Core.Entities;

namespace WebSite.DataAccess.Configuration
{
    class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder.HasMany(c => c.Posts)
                .WithOne(a => a.AuthorEntity)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Comments)
                .WithOne(a => a.AuthorEntity)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Image)
                .HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//developer-solid-2-17-512.png"));
        }
    }
}
