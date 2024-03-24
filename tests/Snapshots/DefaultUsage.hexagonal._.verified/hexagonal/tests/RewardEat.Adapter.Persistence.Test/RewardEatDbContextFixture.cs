using RewardEat.Application.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace RewardEat.Adapter.Persistence.Test;

public class RewardEatDbContextFixture
{
    public RewardEatDbContextFixture()
    {
        DomainEventDispatcherMock = new Mock<IEventDispatcher>();

        var dbContextOptions = new DbContextOptionsBuilder<RewardEatDbContext>()
            .UseInMemoryDatabase("BloggingControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var loggerMock = new Mock<ILogger<RewardEatDbContext>>();

        DbContext = new RewardEatDbContext(dbContextOptions,
            DomainEventDispatcherMock.Object,
            loggerMock.Object);

        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    public RewardEatDbContext DbContext { get; }
    public Mock<IEventDispatcher> DomainEventDispatcherMock { get; }

}
