using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class PostgreRepository : IRepository
{
    private readonly ApplicationDbContext _dbContext;
    private string _schema;

    public PostgreRepository(ApplicationDbContext dbContext, string schema)
    {
        _dbContext = dbContext;
        _schema = schema;
    }

    public async Task CreateSchema()
    {
        await _dbContext.Database.ExecuteSqlRawAsync("CREATE SCHEMA " + _schema);
    }

    public async Task CreateTablesTable()
    {
        var query = $"create table {_schema}.Tables (" +
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
                SELECT * FROM {_schema}.tables");
    }
    
    public async Task CreateTable(Table table)
    {
        var tableExpirationDate = table.ExpirationDate.HasValue ? $"'{table.ExpirationDate}'" : "null"; 
        
        var insertQuery = $"insert into {_schema}.tables(" +
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
        
        var createQuery = $"create table {_schema}.{table.Name}(" +
            "Id serial primary key," +
            $"Table_Id integer references {_schema}.Tables(id)," +
            "External_Id uuid," +
            "Description text," +
            "Price json," +
            "Purchase_Date timestamp" +
            ")";
        
        await _dbContext.Database.ExecuteSqlRawAsync(createQuery);
    }

    public async Task<List<Table?>> ListTables()
    {
        return await _dbContext.Tables.ToListAsync();
    }

    public async Task<Table?> GetTableByExternalId(Guid externalId)
    {
        return await _dbContext.Tables.FirstOrDefaultAsync(table => table.ExternalId == externalId);
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
        var table = await _dbContext.Tables.FirstOrDefaultAsync(table => table.ExternalId == externalId);

        if (table is null)
            throw new Exception("Table not found");
        
        var tableName = table.Name;
        
        var dropQuery = $"drop table {_schema}.{tableName}";
        var deleteQuery = $"delete from {_schema}.Tables where external_id = '{externalId}'";

        await _dbContext.Database.ExecuteSqlRawAsync(dropQuery);
        await _dbContext.Database.ExecuteSqlRawAsync(deleteQuery);
    }

    public async Task CreateItem(Item item, Guid tableExternalId)
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