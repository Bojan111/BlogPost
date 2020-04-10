using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSite.Core.Entities;

namespace WebSite.DataAccess.Configuration
{
    class BlogConfiguration : IEntityTypeConfiguration<BlogEntity>
    {
        public void Configure(EntityTypeBuilder<BlogEntity> builder)
        {
            builder.HasMany(x => x.Posts)
                .WithOne(x => x.Blog)
                .HasForeignKey(x => x.BlogId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Cover)
                .HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//blog-logo-7E0C67F0D1-seeklogo.com.png"));
        }
    }
}
