﻿namespace RewardEat.Domain.SeedWork;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}
