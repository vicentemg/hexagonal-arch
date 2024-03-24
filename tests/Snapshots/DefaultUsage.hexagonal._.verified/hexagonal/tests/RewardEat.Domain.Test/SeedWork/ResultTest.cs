using RewardEat.Domain.SeedWork;

namespace RewardEat.Domain.Test.SeedWork;

public class ResultTest
{
    [Fact]
    public void Given_ResultByValidClass_When_FailureIsCalled_Then_ErrorMustNotBeEmptyAndValueShouldBeNull()
    {
        //Arrange
        var error = new Error(0, "one error");

        //Act
        var result = Result<Valid>.Failure(error);

        //Assert
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Given_ResultClassByValid_When_SuccessIsCalled_Then_ValueMustNotBeNullAndErrorsShouldBeNull()
    {
        //Arrange
        var valid = new Valid();

        //Act
        var result = Result<Valid>.Success(valid);

        //Assert
        Assert.NotNull(result.Value);
        Assert.Null(result.Error);
        Assert.True(result.IsSuccess);
        Assert.Equal(valid, result.Value);
    }

    [Fact]
    public void Given_ResultClassByValid_When_ImplicitCastIsMadeByValidClass_Then_ReturnAResultObject()
    {
        //Arrange
        var valid = new Valid();

        //Act
        Result<Valid> result = valid;
        //Assert
        Assert.NotNull(result);
        Assert.Null(result.Error);
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Given_ResultClassByValid_When_ImplicitCastIsMadeByAnError_Then_ReturnAResultWithError()
    {
        //Arrange
        var error = new Error(0, "one error");

        //Act
        Result<Valid> result = error;
        //Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Error);
        Assert.Null(result.Value);
        Assert.False(result.IsSuccess);
    }
}

internal class Valid
{
    public string Message { get; } = default!;
}
