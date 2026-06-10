using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPulseHQ.Infrastructure.Persistence.Configurations
{
    public class ServiceEndpointConfiguration : IEntityTypeConfiguration<ServiceEndpoint>
    {
        public void Configure(EntityTypeBuilder<ServiceEndpoint> builder)
        {
            builder.ToTable("ServiceEndpoints");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
