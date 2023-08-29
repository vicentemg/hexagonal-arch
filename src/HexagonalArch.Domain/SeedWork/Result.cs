namespace HexagonalArch.Domain.SeedWork;
public record Result<TValue>
{
    private readonly string[] _errors;
    private readonly TValue? _value;
    private Result(TValue value)
    {
        _value = value;
    }

    private Result(string[] errors)
    {
        _errors = errors;
    }

    public TValue Value => _value;

    public IReadOnlyCollection<string> Errors => _errors;

    public bool IsSuccess
        => (_errors is null || _errors?.Length == 0) && _value is not null;

    public static Result<TValue> Success(TValue value)
        => new(value);

    public static Result<TValue> Failure(string[] errors)
        => new(errors);

    public static implicit operator Result<TValue>(TValue value)
        => new(value);

}