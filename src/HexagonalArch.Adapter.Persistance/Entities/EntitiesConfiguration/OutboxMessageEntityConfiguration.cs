using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HexagonalArch.Adapter.Persistance.Entities.EntitiesConfiguration;

internal class OutboxMessageEntityConfiguration : IEntityTypeConfiguration<OutBoxMessage>
{
    public void Configure(EntityTypeBuilder<OutBoxMessage> builder)
    {
        builder
            .ToTable("OutBoxMessages");
        
        builder
            .HasKey(e=>e.Id);

        builder
            .Property(e=>e.Type)
            .IsRequired();
        
        builder
            .Property(e => e.EventData)
            .IsRequired();

        builder
            .Property(e => e.OccurredOnUtcTime)
            .IsRequired();
    }
}
