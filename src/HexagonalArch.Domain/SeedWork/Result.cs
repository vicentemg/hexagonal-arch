namespace HexagonalArch.Domain.SeedWork;

public record Result<TValue>
{
    private readonly List<string> _errors = new();

    private Result(TValue value)
    {
        Value = value;
    }

    private Result(IEnumerable<string> errors)
    {
        _errors = errors.ToList();
    }

    public TValue? Value { get; }

    public IReadOnlyCollection<string> Errors => _errors;

    public bool IsSuccess
        => (_errors is null || !_errors.Any()) && Value is not null;

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static Result<TValue> Failure(IEnumerable<string> errors)
    {
        return new Result<TValue>(errors);
    }

    public static Result<TValue> Failure(string error)
    {
        return Failure(new[] { error });
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }

    public static implicit operator TValue(Result<TValue> result)
    {
        ArgumentNullException.ThrowIfNull(result.Value);
        return result.Value;
    }
}
