using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSite.Core.Entities;

namespace WebSite.DataAccess.Configuration
{
    class PostConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.HasMany(c => c.Comments)
                .WithOne(b => b.Post)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Property(x => x.Cover)
                .HasDefaultValue(System.IO.File.ReadAllBytes(".//wwwroot//images//EQUIVISION-Background-Logos-Overlay-Blue-to-Red-1500x1500.png"));

            builder.Property(x => x.Views)
                .HasDefaultValue(0);
        }
    }
}
