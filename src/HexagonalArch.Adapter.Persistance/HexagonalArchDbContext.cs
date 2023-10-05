using HexagonalArch.Application.Events;
using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using HexagonalArch.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HexagonalArch.Adapter.Persistance;
public class HexagonalArchDbContext : DbContext, IUnitOfWork
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ILogger<HexagonalArchDbContext> _logger;
    public HexagonalArchDbContext(
        DbContextOptions<HexagonalArchDbContext> options,
        IDomainEventDispatcher domainEventDispatcher,
        ILogger<HexagonalArchDbContext> logger) : base(options)
    {
        _logger = logger;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public DbSet<CollectedBalanceChallenge> CollectedBalanceChallenges { get; set; }
    public DbSet<CollectedBalanceConstraint> CollectedBalanceConstraints { get; set; }
    public DbSet<CollectedBalanceChallengeParticipation> CollectedBalanceChallengeParticipations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(IPersistanceAssemblyMarker).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var entries = ChangeTracker.Entries<Entity>();
        var @events = entries.SelectMany(e => e.Entity.DomainEvents);

        foreach (var @event in @events)
        {
            await _domainEventDispatcher.DispatchAsync(@event, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}