namespace HexagonalArch.Domain.SeedWork;
public record Result<TValue>
{
    private readonly List<string> _errors;
    private readonly TValue? _value;
    Result(TValue value)
    {
        _value = value;
    }

    Result(IEnumerable<string> errors)
    {
        _errors = errors.ToList();
    }

    public TValue Value => _value;

    public IReadOnlyCollection<string> Errors => _errors;

    public bool IsSuccess
        => (_errors is null || !_errors.Any()) && _value is not null;

    public static Result<TValue> Success(TValue value)
        => new(value);

    public static Result<TValue> Failure(IEnumerable<string> errors)
        => new(errors);

    public static implicit operator Result<TValue>(TValue value)
        => Success(value);

    public static implicit operator TValue(Result<TValue> result)
        => result.Value;
}