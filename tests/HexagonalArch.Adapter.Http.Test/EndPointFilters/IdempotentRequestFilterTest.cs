using HexagonalArch.Adapter.Http.Contexts;
using HexagonalArch.Adapter.Http.EndPointFilters;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace HexagonalArch.Adapter.Http.Test.EndPointFilters;

public class IdempotentRequestFilterTest
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IHttpHeaderContext> _httpHeaderContextMock;
    private readonly Mock<IIdempotencyService> _idempontcyServiceMock;

    private readonly IdempotentRequestFilter _sut;

    public IdempotentRequestFilterTest()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _httpHeaderContextMock = new Mock<IHttpHeaderContext>();
        _idempontcyServiceMock = new Mock<IIdempotencyService>();

        _sut = new IdempotentRequestFilter(_idempontcyServiceMock.Object,
            _dateTimeProviderMock.Object,
            _httpHeaderContextMock.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-guid")]
    public async Task InvokeAsync_WhenInvalidGuidStringIsPassedIn_ShouldReturnAPromDetail(string idempotencyKey)
    {
        //Arrange
        _httpHeaderContextMock
            .Setup(m => m.GetValue(IdempotentRequestFilter.IdempotencyKeyHeader))
            .Returns(idempotencyKey);

        var context = new Mock<EndpointFilterInvocationContext>();
        var next = new Mock<EndpointFilterDelegate>();

        //Act
        var result = await _sut.InvokeAsync(context.Object, next.Object) as ProblemHttpResult;

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        _httpHeaderContextMock.Verify(m => m.GetValue(IdempotentRequestFilter.IdempotencyKeyHeader), Times.Once);
        _idempontcyServiceMock.Verify(m => m.HasBeenProcessed(It.IsAny<Guid>()), Times.Never);
        _idempontcyServiceMock.Verify(
            m => m.AddRequestAsync(It.IsAny<IdempotentRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_WhenAnAlreadyProcessedRequestIsPassedIn_ShouldReturnAPromDetail()
    {
        //Arrange
        var alreadyProcessedkey = Guid.NewGuid();

        _httpHeaderContextMock
            .Setup(m => m.GetValue(IdempotentRequestFilter.IdempotencyKeyHeader))
            .Returns(alreadyProcessedkey.ToString());

        _idempontcyServiceMock
            .Setup(m => m.HasBeenProcessed(alreadyProcessedkey))
            .ReturnsAsync(true);

        var context = new Mock<EndpointFilterInvocationContext>();
        var next = new Mock<EndpointFilterDelegate>();

        //Act
        var result = await _sut.InvokeAsync(context.Object, next.Object) as ProblemHttpResult;

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);

        _httpHeaderContextMock.Verify(m => m.GetValue(IdempotentRequestFilter.IdempotencyKeyHeader), Times.Once);
        _idempontcyServiceMock.Verify(m => m.HasBeenProcessed(alreadyProcessedkey), Times.Once);
        _idempontcyServiceMock.Verify(
            m => m.AddRequestAsync(It.IsAny<IdempotentRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }


    [Fact]
    public async Task InvokeAsync_WhenRequestIsPassedIn_ShouldSaveItAndContinue()
    {
        //Arrange
        var now = DateTime.UtcNow;
        var idempotencyKey = Guid.NewGuid();

        _dateTimeProviderMock
            .Setup(m => m.UtcNow)
            .Returns(now);

        _httpHeaderContextMock
            .Setup(m => m.GetValue(IdempotentRequestFilter.IdempotencyKeyHeader))
            .Returns(idempotencyKey.ToString());

        _idempontcyServiceMock
            .Setup(m => m.HasBeenProcessed(idempotencyKey))
            .ReturnsAsync(false);

        var token = new CancellationTokenSource().Token;
        var httpContext = new DefaultHttpContext();

        httpContext.Request.Path = "/api/resource";
        httpContext.RequestAborted = token;

        var endPointContext = new Mock<EndpointFilterInvocationContext>();
        endPointContext
            .Setup(m => m.HttpContext)
            .Returns(httpContext);

        var nextMock = new Mock<EndpointFilterDelegate>();

        //Act
        var result = await _sut.InvokeAsync(endPointContext.Object, nextMock.Object) as ProblemHttpResult;

        //Assert
        _httpHeaderContextMock.Verify(m => m.GetValue(IdempotentRequestFilter.IdempotencyKeyHeader), Times.Once);

        _idempontcyServiceMock.Verify(m => m.HasBeenProcessed(idempotencyKey), Times.Once);
        _idempontcyServiceMock.Verify(m =>
                m.AddRequestAsync(It.Is<IdempotentRequest>(x =>
                        x.Id.Equals(idempotencyKey)
                        && x.SourceName.Equals("/api/resource", StringComparison.OrdinalIgnoreCase)
                        && x.DateTime.Equals(now)),
                    token),
            Times.Once);

        nextMock.Verify(m => m.Invoke(endPointContext.Object), Times.Once);
    }
}
