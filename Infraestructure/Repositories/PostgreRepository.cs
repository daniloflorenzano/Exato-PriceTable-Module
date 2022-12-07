using Domain;
using Domain.Abstractions;

namespace Infraestructure.Repositories;

public class PostgreRepository : IRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostgreRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ApplicationDbContext DbContext => _dbContext;
    public Task CreateTable()
    {
        throw new NotImplementedException();
    }

    public Task<Table[]> ListTables()
    {
        throw new NotImplementedException();
    }

    public Task<Table> GetTableByExternalId()
    {
        throw new NotImplementedException();
    }

    public Task DeleteTable()
    {
        throw new NotImplementedException();
    }

    public Task CreateItem()
    {
        throw new NotImplementedException();
    }

    public Task<Item[]> ListItems()
    {
        throw new NotImplementedException();
    }

    public Task<Item> GetItemByExternalId()
    {
        throw new NotImplementedException();
    }

    public Task DeleteItem()
    {
        throw new NotImplementedException();
    }
}