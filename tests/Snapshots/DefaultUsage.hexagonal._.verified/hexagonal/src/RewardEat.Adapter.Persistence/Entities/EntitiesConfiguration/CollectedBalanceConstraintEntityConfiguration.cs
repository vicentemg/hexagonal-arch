using RewardEat.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RewardEat.Adapter.Persistence.Entities.EntitiesConfiguration;

public class CollectedBalanceConstraintEntityConfiguration : IEntityTypeConfiguration<CollectedBalanceConstraint>
{
    public void Configure(EntityTypeBuilder<CollectedBalanceConstraint> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Amount).IsRequired();
        builder.Property(e => e.ChallengeId).IsRequired();
        builder.Property(e => e.BackwardDayPeriod).IsRequired();
    }
}
