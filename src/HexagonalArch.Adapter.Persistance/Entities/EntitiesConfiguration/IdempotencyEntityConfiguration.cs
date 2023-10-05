using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HexagonalArch.Adapter.Persistance.Entities.EntitiesConfiguration;

internal class IdempotencyRequestEntityConfiguration : IEntityTypeConfiguration<IdempotencyRequest>
{
    public void Configure(EntityTypeBuilder<IdempotencyRequest> builder)
    {
        builder
            .ToTable("IdempotencyRequests");
    }
}