namespace HexagonalArch.Domain;

public class Transaction
{
    public Transaction(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; }
}
