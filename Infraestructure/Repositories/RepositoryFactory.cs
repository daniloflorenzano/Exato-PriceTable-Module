﻿using Domain.Abstractions;

namespace Infraestructure.Repositories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly ApplicationDbContext _dbContext;

    public RepositoryFactory(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IRepository Create(string schema)
    {
        return new PostgreRepository(_dbContext, schema);
    }
}