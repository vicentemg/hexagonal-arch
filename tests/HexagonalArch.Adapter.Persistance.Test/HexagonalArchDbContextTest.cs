using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

namespace HexagonalArch.Adapter.Persistance.Test;

public class HexagonalArchDbContextTest : IClassFixture<HexagonalArchDbContextFixture>
{
    private readonly HexagonalArchDbContext _sut;

    public HexagonalArchDbContextTest(HexagonalArchDbContextFixture fixture)
    {
        _sut = fixture.DbContext;
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
}