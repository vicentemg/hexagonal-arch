using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HexagonalArch.Adapter.Persistance.Entities.EntitiesConfiguration;

public class CollectedBalanceChallengeEntityConfiguration : IEntityTypeConfiguration<CollectedBalanceChallenge>
{
    public void Configure(EntityTypeBuilder<CollectedBalanceChallenge> builder)
    {
        builder
            .ToTable("CollectedBalanceChallenges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Active);
        builder.Property(x => x.Name);
        builder.Property(x => x.CreatedDateTime);
        builder
            .HasOne(x => x.Constraint);
    }
}