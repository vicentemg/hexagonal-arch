﻿using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Domain.Test.SeedWork;

public class ResultTest
{
    [Fact]
    public void Given_ResultByValidClass_When_FailureIsCalled_Then_ErrorsMustNotBeEmptyAndValueShouldBeNull()
    {
        //Arrange
        var errors = new[] { "Error 1", "Error 2" };

        //Act
        var result = Result<Valid>.Failure(errors);

        //Assert
        Assert.Null(result.Value);
        Assert.Equal(errors, result.Errors);
        Assert.NotEmpty(result.Errors);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void
        Given_ResultByValidClass_When_FailureIsCalledUsingSingleArg_Then_ErrorsMustNotBeEmptyAndValueShouldBeNull()
    {
        //Arrange
        var error = "Error 1";

        //Act
        var result = Result<Valid>.Failure(error);

        //Assert
        Assert.Null(result.Value);
        Assert.Contains(error, result.Errors);
        Assert.NotEmpty(result.Errors);
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
        Assert.Empty(result.Errors);
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
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
    }
}

internal class Valid
{
    public string Message { get; } = default!;
}
