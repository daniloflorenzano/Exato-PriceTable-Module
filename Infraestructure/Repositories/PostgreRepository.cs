using System.Globalization;
using System.Text.Json;
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

    public async Task CreateTablesTable(string schema)
    {
        var query = $"create table {schema}.Tables (" +
            "Id serial primary key," +
            "External_Id uuid," +
            "Name text UNIQUE," +
            "Description text," +
            "Type int," +
            "Active bool," +
            "Expiration_Date timestamp," +
            "Creation_Date timestamp" +
            ");";
        
        await _dbContext.Database.ExecuteSqlRawAsync(query);
        
        await _dbContext.Database.ExecuteSqlRawAsync($@"CREATE VIEW registered_tables AS
                SELECT * FROM {schema}.tables");
    }
    
    public async Task CreateTable(Table table, string schema)
    {
        var tableExpirationDate = table.ExpirationDate.HasValue ? $"'{table.ExpirationDate}'" : "null"; 
        
        var insertQuery = $"insert into {schema}.tables(" +
            "external_id," +
            "name," +
            "description," +
            "type," +
            "active," +
            "expiration_date," +
            "creation_date) " +
            "values (" +
            $"'{table.ExternalId}'," +
            $"'{table.Name}'," +
            $"'{table.Description}'," +
            $"{(int)table.Type}," +
            $"{table.Active}," +
            $"{tableExpirationDate}," +
            $"'{table.CreationDate}'" +
            ")";

        await _dbContext.Database.ExecuteSqlRawAsync(insertQuery);
        
        var createQuery = $"create table {schema}.{table.Name}(" +
            "Id serial primary key," +
            $"Table_Id integer references {schema}.Tables(id)," +
            "External_Id uuid," +
            "Description text," +
            "Price json," +
            "Purchase_Date timestamp" +
            ")";
        
        await _dbContext.Database.ExecuteSqlRawAsync(createQuery);
    }

    public List<Table> ListTables(string schema)
    {
        return _dbContext.Tables.ToList();
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
    
    // private bool AlreadyExistsTableWithSameNameButDifferentType(Table table)
    // {
    //     var existingTables = ListTables();
    //     foreach (var t in existingTables)
    //     {
    //         if (t.Name == table.Name && t.Type != table.Type)
    //             return true;
    //     }
    //
    //     return false;
    // }

    // private bool AlreadyExistsTableWithSameNameAndType(Table table)
    // {
    //     var existingTables = ListTables();
    //
    //     foreach (Table t in existingTables)
    //     {
    //         if (t.Name == table.Name && t.Type == table.Type)
    //             return true;
    //     }
    //
    //     return false;
    // }
}