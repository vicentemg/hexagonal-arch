﻿using RewardEat.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RewardEat.Adapter.Persistence.Entities.EntitiesConfiguration;

public class
    CollectedBalanceChallengeParticipationEntityConfiguration : IEntityTypeConfiguration<
        CollectedBalanceChallengeParticipation>
{
    public void Configure(EntityTypeBuilder<CollectedBalanceChallengeParticipation> builder)
    {
        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.IsWinner)
            .IsRequired();

        builder
            .Property(e => e.Amount)
            .IsRequired();

        builder
            .Property(e => e.OccurredOn)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .IsRequired();

        builder
            .Property(e => e.TransactionId)
            .IsRequired();

        builder
            .Property(e => e.ChallengeId)
            .IsRequired();
    }
}
