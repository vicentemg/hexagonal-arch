using HexagonalArch.Adapter.Persistence.Entities;
using HexagonalArch.Application.Events;
using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using HexagonalArch.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HexagonalArch.Adapter.Persistence;

public class HexagonalArchDbContext : DbContext, IUnitOfWork
{
    private readonly IEventDispatcher _domainEventDispatcher;
    private readonly ILogger<HexagonalArchDbContext> _logger;

    public HexagonalArchDbContext(
        DbContextOptions<HexagonalArchDbContext> options,
        IEventDispatcher domainEventDispatcher,
        ILogger<HexagonalArchDbContext> logger) : base(options)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(domainEventDispatcher);

        _logger = logger;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public DbSet<OutBoxMessage> OutBoxMessages { get; set; }
    public DbSet<IdempotencyRequest> IdempotencyRequests { get; set; }
    public DbSet<CollectedBalanceChallenge> CollectedBalanceChallenges { get; set; }
    public DbSet<CollectedBalanceConstraint> CollectedBalanceConstraints { get; set; }

    public DbSet<CollectedBalanceChallengeParticipation> CollectedBalanceChallengeParticipations { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _logger.BeginScope("SaveChangesAsync has started: [CurrentTransaction] {CurrentTransaction}",
            Database.CurrentTransaction);

        var entries = ChangeTracker.Entries<Entity>();
        var events = entries.SelectMany(e => e.Entity.DomainEvents);

        foreach (var @event in events) await _domainEventDispatcher.DispatchAsync(@event, cancellationToken);

        var writtenEntries = await base.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("SaveChangesAsync has modified {entriesWritten} entities", writtenEntries);
        _logger.LogInformation("SaveChangesAsync has finished");

        return writtenEntries;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(IPersistenceAssemblyMarker).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}
