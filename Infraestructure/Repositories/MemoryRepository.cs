using Domain;
using Domain.Abstractions;

namespace Infraestructure.Repositories;

public class MemoryRepository : IRepository
{
    public Task CreateTable(Table table)
    {
        throw new NotImplementedException();
    }

    public Table[] ListTables()
    {
        throw new NotImplementedException();
    }

    public Task<Table> GetTableByExternalId(Guid externalId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTable(Guid externalId, Table table)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTable(Guid externalId)
    {
        throw new NotImplementedException();
    }

    public Task CreateItem(Item item, Guid tableExternalId)
    {
        throw new NotImplementedException();
    }

    public Task<Item[]> ListItems(Guid tableExternalId)
    {
        throw new NotImplementedException();
    }

    public Task<Item> GetItemByExternalId(Guid itemExternalId, Guid tableExternalId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateItem(Guid itemExternalId, Item item, Guid tableExternalId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteItem(Guid itemExternalId, Guid tableExternalId)
    {
        throw new NotImplementedException();
    }
}