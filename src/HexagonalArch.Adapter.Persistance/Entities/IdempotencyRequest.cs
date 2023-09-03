using System.ComponentModel.DataAnnotations;

namespace HexagonalArch.Adapter.Persistance.Entities;

internal class IdempotencyRequest
{
    public Guid Id { get; init; }

    public string Name { get; init; }
    public DateTime OccurredOnUtcTime { get; }
}