namespace RewardEat.Domain.SeedWork;

public record Result<TValue>
{
    private Result()
    {
    }

    private Result(TValue value)
    {
        Value = value;
    }

    public Result(Error error)
    {
        Error = error;
    }

    public TValue? Value { get; }
    public Error? Error { get; }

    public bool IsSuccess => Error is null && Value is not null;

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static Result<TValue> Failure(Error error)
    {
        return new Result<TValue>(error);
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }

    public static implicit operator Result<TValue>(Error error)
    {
        return Failure(error);
    }

    public static implicit operator TValue(Result<TValue> result)
    {
        ArgumentNullException.ThrowIfNull(result.Value);
        return result.Value;
    }

}
public record Error(int Code, string Message);
