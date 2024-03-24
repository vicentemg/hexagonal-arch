namespace HexagonalArch.Application.Services;

[Flags]
public enum OutBoxMessageStatus
{
    Pending = 1,
    Success = 1 << 1,
    InProgress = 1 << 2,
    Failed = 1 << 3
}
