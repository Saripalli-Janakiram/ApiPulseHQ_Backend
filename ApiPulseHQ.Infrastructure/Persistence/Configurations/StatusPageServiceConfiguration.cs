using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StatusPageServiceConfiguration : IEntityTypeConfiguration<StatusPageService>
{
    public void Configure(EntityTypeBuilder<StatusPageService> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.StatusPage)
            .WithMany(x => x.StatusPageServices)
            .HasForeignKey(x => x.StatusPageId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ServiceEndpoint)
            .WithMany(x => x.StatusPageServices)
            .HasForeignKey(x => x.ServiceEndpointId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
