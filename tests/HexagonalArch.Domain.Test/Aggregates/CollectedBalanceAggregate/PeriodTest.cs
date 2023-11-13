using HexagonalArch.Domain.Primitives;

namespace HexagonalArch.Domain.Test.Aggregates.CollectedBalanceAggregate;

public class PeriodTest
{
    [Fact]
    public void Given_TwoDates_When_CreatingAPeriodUsingDatesInOtherWayAround_Then_ShouldReturnNonSuccess()
    {
        //Arrange
        var dateOne = DateTime.Now;
        var dateTwo = DateTime.Now.AddDays(1);

        //Act
        var period = Period.Create(dateTwo, dateOne);

        //Assert
        Assert.False(period.IsSuccess);
        Assert.NotEmpty(period.Errors);
    }
}