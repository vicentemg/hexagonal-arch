namespace HexagonalArch.Application.Services;

public record IdempotentRequest(Guid Id, string SourceName, DateTime DateTime);
