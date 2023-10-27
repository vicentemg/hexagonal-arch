using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HexagonalArch.Adapter.Persistance.Entities.EntitiesConfiguration;

internal class IdempotencyRequestEntityConfiguration : IEntityTypeConfiguration<IdempotencyRequest>
{
    public void Configure(EntityTypeBuilder<IdempotencyRequest> builder)
    {
        builder
            .ToTable("IdempotencyRequests");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(e => e.OccurredOnUtcTime)
            .IsRequired();

    }
}