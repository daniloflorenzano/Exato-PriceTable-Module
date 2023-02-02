using Domain;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infraestructure.Repositories;

public class PostgreRepository : IRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostgreRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateSchema(string name)
    {
        await _dbContext.Database.ExecuteSqlRawAsync("CREATE SCHEMA " + name);
    }

    public async Task CreateTable(Table table)
    {
        var existingTableWithSameName = AlreadyExistsTableWithSameNameButDifferentType(table);
        var existingIdenticalTable = AlreadyExistsTableWithSameNameAndType(table);

        if (existingTableWithSameName is true)
            throw new Exception($"Already exists a table named {table.Name}, but with a different type");

        if (existingIdenticalTable is true)
            throw new Exception($"Already exists a table named {table.Name} with this same type");

        // persiste os dados da tabela em uma tabela "Tables"
        //_dbContext.Tables.Add(table);

        // realmente cria a tabela no banco de dados com o nome passado
        await _dbContext.Database.ExecuteSqlRawAsync(@"CREATE TABLE {0} (
            Id SERIAL PRIMARY KEY,
            ExternalId UNIQUEIDENTIFIER,
            Description NVARCHAR(255),
            Type INT,
            Price DOUBLE,
            InitialAmount INT,
            LimitAmount INT
        )", table.Name);
        
        await _dbContext.SaveChangesAsync();
    }

    public Table[] ListTables()
    {
        // var tables = _dbContext.Tables.AsNoTracking().ToArray();
        //
        // if (!tables.Any())
        //     throw new Exception("There is no created tables");
        //
        // return tables;
        throw new NotImplementedException();
    }

    public async Task<Table> GetTableByExternalId(Guid externalId)
    {
        // var table = await _dbContext.Tables.FirstOrDefaultAsync(t => t.ExternalId == externalId);
        //
        // if (table is null)
        //     throw new Exception($"Table with external id: {externalId} was not found");
        //
        // return table;
        throw new NotImplementedException();
    }

    public async Task UpdateTable(Guid externalId, Table table)
    {
        // var existingTable = await _dbContext.Tables.FirstOrDefaultAsync(t => t.ExternalId == externalId);
        //
        // if (existingTable is null)
        //     throw new Exception($"Table with external id: {externalId} was not found");
        //
        // table.ExternalId = externalId;
        // _dbContext.Tables.Update(table);
        //
        // await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTable(Guid externalId)
    {
        // var table = await _dbContext.Tables.FirstOrDefaultAsync(t => t.ExternalId == externalId);
        //
        // if (table is null)
        //     throw new Exception($"Table with external id: {externalId} was not found");
        //
        // _dbContext.Remove(table);
    }

    public async Task CreateItem(Item item, Guid tableExternalId)
    {
        // var table = await GetTableByExternalId(tableExternalId);
        // var tableName = table.Name;
        //
        // await _dbContext.Database
        //     .ExecuteSqlRawAsync(@"INSERT INTO {0} (ExternalId, Description, Type, Price, InitialAmount, LimitAmount)
        //     VALUES ({1}, {2}, {3}, {4}, {5}, {6})", 
        //     tableName, item.ExternalId, item.Description, item.Type, item.Price, item.InitialAmount, item.LimitAmount);

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