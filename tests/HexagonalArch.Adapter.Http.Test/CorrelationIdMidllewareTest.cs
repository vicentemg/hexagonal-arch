using HexagonalArch.Adapter.Http.Middlewares;
using HexagonalArch.Application.Idempotency;
using HexagonalArch.Application.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace HexagonalArch.Adapter.Http.Test;

public class CorrelationIdMidllewareTest
{
    private readonly Mock<IIdempotencyService> _idempontcyManagerMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    public CorrelationIdMidllewareTest()
    {
        _idempontcyManagerMock = new Mock<IIdempotencyService>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-guid")]
    public async Task Given_HttpMiddlewareThatThreatsCorrelationIdHeader_When_CorrelationIdHeaderIsNotSentOrItIsInvalid_Then_ShouldThrowAnException(string correlationId)
    {
        //Arrange
        using var host = await CreateWebHost(
             services =>
             {
                 services.AddTransient<CorrelationIdMidlleware>();
                 services.AddTransient(typeof(IIdempotencyService), sp => _idempontcyManagerMock.Object);
                 services.AddTransient(typeof(IDateTimeProvider), sp => _dateTimeProviderMock.Object);
             },
             app =>
             {
                 app.UseWhen(
                    context => context.Request.Method == "POST",
                    appBuilder => appBuilder.UseMiddleware<CorrelationIdMidlleware>()
                );
             }
         );
        var server = host.GetTestServer();

        //Act
        Func<Task> testCode = () => server.SendAsync(context =>
                {
                    context.Request.Method = "POST";
                    context.Request.Path = "/some-endpoint";
                    context.Request.Headers.Add(CorrelationIdMidlleware.HeaderCorrelationId, correlationId);
                });

        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        _idempontcyManagerMock.Verify(m => m.HasBeenProcessed(It.IsAny<Guid>()), Times.Never);
        _idempontcyManagerMock.Verify(m => m.AddRequestAsync(It.IsAny<Request>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Given_HttpMiddlewareThatThreatsCorrelationIdHeader_When_CorrelationIdHeaderHasBeenAlreadyProcessed_Then_ShouldThrowAnException()
    {
        //Arrange
        _idempontcyManagerMock
            .Setup(m => m.HasBeenProcessed(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        using var host = await CreateWebHost(
             services =>
             {
                 services.AddTransient<CorrelationIdMidlleware>();
                 services.AddTransient(typeof(IIdempotencyService), sp => _idempontcyManagerMock.Object);
                 services.AddTransient(typeof(IDateTimeProvider), sp => _dateTimeProviderMock.Object);
             },
             app =>
             {
                 app.UseWhen(
                    context => context.Request.Method == "POST",
                    appBuilder => appBuilder.UseMiddleware<CorrelationIdMidlleware>()
                );
             }
         );
        var correlationId = Guid.NewGuid();
        var endPoint = "/some-endpoint";

        var server = host.GetTestServer();
        //Act
        Func<Task> testCode = () => server.SendAsync(context =>
                {
                    context.Request.Method = "POST";
                    context.Request.Path = endPoint;
                    context.Request.Headers.Add(CorrelationIdMidlleware.HeaderCorrelationId, correlationId.ToString());
                });

        //Assert
         await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        _idempontcyManagerMock.Verify(m => m.HasBeenProcessed(It.IsAny<Guid>()), Times.Once());
        _idempontcyManagerMock.Verify(m =>
            m.AddRequestAsync(
                It.IsAny<Request>(),
                It.IsAny<CancellationToken>()),
            Times.Never());
    }

    [Fact]
    public async Task Given_HttpMiddlewareThatThreatsCorrelationIdHeader_When_CorrelationIdHeaderIsSentAndItDoesNotExistOnPersitanceDB_Then_ShouldSuccess()
    {
        //Arrange
        _idempontcyManagerMock
            .Setup(m => m.HasBeenProcessed(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        _dateTimeProviderMock
            .Setup(m => m.UtcNow)
            .Returns(DateTime.UtcNow);

        using var host = await CreateWebHost(
             services =>
             {
                 services.AddTransient<CorrelationIdMidlleware>();
                 services.AddTransient(typeof(IIdempotencyService), sp => _idempontcyManagerMock.Object);
                 services.AddTransient(typeof(IDateTimeProvider), sp => _dateTimeProviderMock.Object);
             },
             app =>
             {
                 app.UseWhen(
                    context => context.Request.Method == "POST",
                    appBuilder => appBuilder.UseMiddleware<CorrelationIdMidlleware>()
                );
             }
         );
        var correlationId = Guid.NewGuid();
        var endPoint = "/some-endpoint";

        var server = host.GetTestServer();
        //Act
        var context = await server.SendAsync(context =>
                {
                    context.Request.Method = "POST";
                    context.Request.Path = endPoint;
                    context.Request.Headers.Add(CorrelationIdMidlleware.HeaderCorrelationId, correlationId.ToString());
                });

        //Assert
        _idempontcyManagerMock.Verify(m => m.HasBeenProcessed(It.IsAny<Guid>()), Times.Once());
        _idempontcyManagerMock.Verify(m =>
            m.AddRequestAsync(
                It.Is<Request>(r =>
                    r.Id.Equals(correlationId)
                    && r.Name == endPoint
                    ),
                It.IsAny<CancellationToken>()),
            Times.Once());
    }

    private static async Task<IHost> CreateWebHost(
        Action<IServiceCollection> configureServices,
        Action<IApplicationBuilder> configureApp)
    {
        var builder = new HostBuilder().ConfigureWebHost(config =>
        {
            config
                .UseTestServer()
                .ConfigureServices(configureServices)
                .Configure(configureApp);
        });

        return await builder.StartAsync();
    }
}
