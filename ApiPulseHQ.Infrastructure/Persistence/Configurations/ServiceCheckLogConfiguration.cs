using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPulseHQ.Infrastructure.Persistence.Configurations
{
    public class ServiceCheckLogConfiguration : IEntityTypeConfiguration<ServiceCheckLog>
    {
        public void Configure(EntityTypeBuilder<ServiceCheckLog> builder)
        {
            builder.ToTable("ServiceCheckLogs");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ServiceEndpoint)
                   .WithMany()
                    .HasForeignKey(x => x.ServiceEndpointId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
