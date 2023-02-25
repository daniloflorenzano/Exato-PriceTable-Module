﻿using System.Text.Json;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public class PostgreRepository : IRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly string _schema;

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
        var tableExpirationDate = table.ExpirationDate.HasValue ? $"'{table.ExpirationDateToString()}'" : "null";
        
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
                          $"'{table.CreationDateToString()}'" +
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

    private string FormatDateForSqlQuery(DateTime date)
    {
        return date.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
    public async Task<List<Table>> ListTables()
    {
        var tables = await _dbContext.Tables.ToListAsync();
        return tables!;
    }

    public async Task<Table> GetTableByExternalId(Guid externalId)
    {
        var table = await _dbContext.Tables.FirstOrDefaultAsync(table => table.ExternalId == externalId);
        if (table is null)
            throw new TableNotFoundException();

        return table;
    }

    public async Task UpdateTable(Guid externalId, Table table)
    {
        var tableExpirationDate = table.ExpirationDate.HasValue ? $"'{table.ExpirationDateToString()}'" : "null";
        var existingTable = await GetTableByExternalId(externalId);

        var updateQuery = $"update {_schema}.tables set " +
          $"name = '{table.Name}', " +
          $"description = '{table.Description}', " +
          $"active = {table.Active}, " +
          $"expiration_date = {tableExpirationDate} " +
          $"where external_id = '{externalId}'";

        await _dbContext.Database.ExecuteSqlRawAsync(updateQuery);

        var oldName = existingTable.Name;
        var renameQuery = $"alter table {_schema}.{oldName} rename to {table.Name};";

        await _dbContext.Database.ExecuteSqlRawAsync(renameQuery);
    }

    public async Task DeleteTable(Guid externalId)
    {
        var table = await GetTableByExternalId(externalId);

        var tableName = table.Name;
        
        var dropQuery = $"drop table {_schema}.{tableName}";
        var deleteQuery = $"delete from {_schema}.Tables where external_id = '{externalId}'";

        await _dbContext.Database.ExecuteSqlRawAsync(dropQuery);
        await _dbContext.Database.ExecuteSqlRawAsync(deleteQuery);
    }

    public async Task CreateItem(Item item, Guid tableExternalId)
    {
        var table = await GetTableByExternalId(tableExternalId);
        var tableId = table.Id;
        var tableName = table.Name;
        var itemPriceAsJson = JsonSerializer.Serialize(item.Price);
        var itemPurchaseDate = FormatDateForSqlQuery(item.PurchaseDate);

        var query = $"insert into {_schema}.{tableName} " +
                    "(table_id, external_id, description, price, purchase_date) " +
                    "values (" +
                    $"{tableId}, " +
                    $"'{item.ExternalId}', " +
                    $"'{item.Description}', " +
                    $"'{{{itemPriceAsJson}}}', " +
                    $"'{itemPurchaseDate}')";
        
        _dbContext.Database.ExecuteSqlRaw(query);
    }

    public async Task<List<Item>> ListItems(Guid tableExternalId)
    {
        var table = await GetTableByExternalId(tableExternalId);
        var tableName = table.Name;

        var query = $"select * from {_schema}.{tableName}";
        var items = _dbContext.Items.FromSqlRaw(query).ToList();
        return items;
    }

    public async Task<List<Item>> ListItemsSinceDate(Guid tableExternalId, DateTime date)
    {
        var table = await GetTableByExternalId(tableExternalId);
        var tableName = table.Name;
        var dateFormated = FormatDateForSqlQuery(date);

        var query = $"select * from {_schema}.{tableName} where purchase_date > '{dateFormated}'";
        var items = _dbContext.Items.FromSqlRaw(query).ToList();
        return items;
    }

    public async Task<Item> GetItemByExternalId(Guid tableExternalId, Guid itemExternalId)
    {
        var table = await GetTableByExternalId(tableExternalId);
        var tableName = table.Name;

        var query = $"select * from {_schema}.{tableName} where external_id = '{itemExternalId}'";
        var item = _dbContext.Items.FromSqlRaw(query).FirstOrDefault();

        if (item is null)
            throw new ItemNotFoundException(itemExternalId);

        return item;
    }

    public async Task UpdateItem(Guid itemExternalId, Item item, Guid tableExternalId)
    {
        var table = await GetTableByExternalId(tableExternalId);
        var tableName = table.Name;
        await GetItemByExternalId(tableExternalId, itemExternalId);
        var itemPriceAsJson = JsonSerializer.Serialize(item.Price);
        
        var query = $"update {_schema}.{tableName} set " +
                    $"description = '{item.Description}', " +
                    $"price = '{{{itemPriceAsJson}}}' " +
                    $"where external_id = '{itemExternalId}'";

        await _dbContext.Database.ExecuteSqlRawAsync(query);
    }

    public async Task DeleteItem(Guid itemExternalId, Guid tableExternalId)
    {
        var table = await GetTableByExternalId(tableExternalId);
        var tableName = table.Name;
        await GetItemByExternalId(tableExternalId, itemExternalId);

        var query = $"delete from {_schema}.{tableName} where external_id = '{itemExternalId}'";
        await _dbContext.Database.ExecuteSqlRawAsync(query);
    }
}