using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPulseHQ.Infrastructure.Persistence.Configurations
{
    public class AlertRuleConfiguration : IEntityTypeConfiguration<AlertRule>
    {
        public void Configure(EntityTypeBuilder<AlertRule> builder)
        {
            builder.ToTable("AlertRules");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ServiceEndpoint)
                    .WithMany()
                    .HasForeignKey(x => x.ServiceEndpointId)
                    .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
