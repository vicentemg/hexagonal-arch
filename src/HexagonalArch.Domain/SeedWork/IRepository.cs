﻿namespace HexagonalArch.Domain.SeedWork;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}
