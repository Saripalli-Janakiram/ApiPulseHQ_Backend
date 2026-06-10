using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPulseHQ.Infrastructure.Persistence.Configurations
{
    public class StatusPageConfiguration : IEntityTypeConfiguration<StatusPage>
    {
        public void Configure(EntityTypeBuilder<StatusPage> builder)
        {
            builder.ToTable("StatusPages");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                    .WithMany()
                     .HasForeignKey(x => x.UserId)
                     .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
