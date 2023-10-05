using HexagonalArch.Application.Events;
using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace HexagonalArch.Adapter.Persistance.Test;

public class HexagonalArchDbContextTest : IDisposable
{
    private readonly HexagonalArchDbContext _sut;

    private readonly Mock<ILogger<HexagonalArchDbContext>> _loggerMock;
    private readonly Mock<IDomainEventDispatcher> _domainEventDispatcher;
    public HexagonalArchDbContextTest()
    {
        var dbContextOptions = new DbContextOptionsBuilder<HexagonalArchDbContext>()
            .UseInMemoryDatabase("BloggingControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _loggerMock = new Mock<ILogger<HexagonalArchDbContext>>();
        _domainEventDispatcher = new Mock<IDomainEventDispatcher>();

        _sut = new HexagonalArchDbContext(dbContextOptions,
                                          _domainEventDispatcher.Object,
                                          _loggerMock.Object);
        _sut.Database.EnsureDeleted();
        _sut.Database.EnsureCreated();

    }

    [Fact]
    public async Task SaveChangesAsync_WhenSomeEntitiesWithDomainEventsWereModifiedOrAdded_ShouldDispatchDomainEvents()
    {
        //Arrange
        var entity = CollectedBalanceConstraint.Create(1, 100);
        _sut.CollectedBalanceConstraints.Add(entity);
        
        //Act
        var numberOfEntities = await _sut.SaveChangesAsync(default);

        //Assert
        Assert.Equal(1, numberOfEntities);
    }

    public void Dispose()
    {
        _sut.Dispose();
    }


}