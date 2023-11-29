using HexagonalArch.Application.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace HexagonalArch.Adapter.Persistence.Test;

public class HexagonalArchDbContextFixture
{
    public HexagonalArchDbContextFixture()
    {
        DomainEventDispatcherMock = new Mock<IDomainEventDispatcher>();

        var dbContextOptions = new DbContextOptionsBuilder<HexagonalArchDbContext>()
            .UseInMemoryDatabase("BloggingControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var loggerMock = new Mock<ILogger<HexagonalArchDbContext>>();

        DbContext = new HexagonalArchDbContext(dbContextOptions,
            DomainEventDispatcherMock.Object,
            loggerMock.Object);

        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    public HexagonalArchDbContext DbContext { get; }
    public Mock<IDomainEventDispatcher> DomainEventDispatcherMock { get; }

}
