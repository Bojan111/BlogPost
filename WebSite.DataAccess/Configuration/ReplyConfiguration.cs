using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSite.Core.Entities;

namespace WebSite.DataAccess.Configuration
{
    class ReplyConfiguration : IEntityTypeConfiguration<ReplyEntity>
    {
        public void Configure(EntityTypeBuilder<ReplyEntity> builder)
        {
        }
    }
}
