using HexagonalArch.Application.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace HexagonalArch.Adapter.Persistance.Test;

public class HexagonalArchDbContextFixture : IDisposable
{
    private readonly Mock<ILogger<HexagonalArchDbContext>> _loggerMock;
    private readonly Mock<IDomainEventDispatcher> _domainEventDispatcher;
    public HexagonalArchDbContextFixture()
    {
        var dbContextOptions = new DbContextOptionsBuilder<HexagonalArchDbContext>()
            .UseInMemoryDatabase("BloggingControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _loggerMock = new Mock<ILogger<HexagonalArchDbContext>>();
        _domainEventDispatcher = new Mock<IDomainEventDispatcher>();

        DbContext = new HexagonalArchDbContext(dbContextOptions,
                                          _domainEventDispatcher.Object,
                                          _loggerMock.Object);
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    public HexagonalArchDbContext DbContext { get; private set; }
    
    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }

}