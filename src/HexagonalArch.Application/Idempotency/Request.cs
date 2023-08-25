namespace HexagonalArch.Application.Idempotency;

public record Request(Guid Id, string Name, DateTime Time);
