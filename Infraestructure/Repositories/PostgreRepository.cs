using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class PostgreRepository : IRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostgreRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ApplicationDbContext DbContext => _dbContext;

    public async Task<Table> CreateTable(Table table)
    {
        var existingTableWithSameName = AlreadyExistsTableWithSameNameButDifferentType(table);
        var existingIdenticalTable = AlreadyExistsTableWithSameNameAndType(table);

        if (existingTableWithSameName is true)
            throw new Exception($"Already exists a table named {table.Name}, but with a different type");

        if (existingIdenticalTable is true)
            throw new Exception($"Already exists a table named {table.Name} with this same type");

        _dbContext.Tables.Add(table);
        await _dbContext.SaveChangesAsync();

        return table;
    }

    public Table[] ListTables()
    {
        var tables = _dbContext.Tables.AsNoTracking().ToArray();

        if (!tables.Any())
            throw new Exception("There is no created tables");

        return tables;
    }

    public async Task<Table> GetTableByExternalId(Guid externalId)
    {
        var table = await _dbContext.Tables.FirstOrDefaultAsync(t => t.ExternalId == externalId);

        if (table is null)
            throw new Exception($"Table with external id: {externalId} was not found");

        return table;
    }

    public async Task UpdateTable(Guid externalId, Table table)
    {
        var existingTable = await _dbContext.Tables.FirstOrDefaultAsync(t => t.ExternalId == externalId);

        if (existingTable is null)
            throw new Exception($"Table with external id: {externalId} was not found");
        
        table.ExternalId = externalId;
        _dbContext.Tables.Update(table);

        await _dbContext.SaveChangesAsync();
        
    }

    public async Task DeleteTable(Guid externalId)
    {
        var table = await _dbContext.Tables.FirstOrDefaultAsync(t => t.ExternalId == externalId);

        if (table is null)
            throw new Exception($"Table with external id: {externalId} was not found");

        _dbContext.Remove(table);
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
    
    private bool AlreadyExistsTableWithSameNameButDifferentType(Table table)
    {
        var existingTables = ListTables();
        foreach (var t in existingTables)
        {
            if (t.Name == table.Name && t.Type != table.Type)
                return true;
        }

        return false;
    }

    private bool AlreadyExistsTableWithSameNameAndType(Table table)
    {
        var existingTables = ListTables();

        foreach (Table t in existingTables)
        {
            if (t.Name == table.Name && t.Type == table.Type)
                return true;
        }

        return false;
    }
}