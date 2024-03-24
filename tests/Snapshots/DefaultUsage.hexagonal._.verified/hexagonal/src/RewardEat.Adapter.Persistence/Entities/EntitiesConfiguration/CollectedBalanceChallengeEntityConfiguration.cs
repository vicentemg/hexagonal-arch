using RewardEat.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using RewardEat.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RewardEat.Adapter.Persistence.Entities.EntitiesConfiguration;

public class CollectedBalanceChallengeEntityConfiguration : IEntityTypeConfiguration<CollectedBalanceChallenge>
{
    public void Configure(EntityTypeBuilder<CollectedBalanceChallenge> builder)
    {
        builder
            .ToTable("CollectedBalanceChallenges");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Active);

        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, x => new ChallengeName(x))
            .HasMaxLength(50);

        builder.Property(x => x.CreatedDateTime);

        builder
            .HasOne(x => x.CollectedBalanceConstraint)
            .WithOne()
            .HasForeignKey<CollectedBalanceConstraint>(x => x.ChallengeId)
            .IsRequired();

        builder
            .HasMany(x => x.Participations)
            .WithOne()
            .HasForeignKey(x => x.Id);
    }
}
