using Domain.Entities;

namespace Domain.Abstractions;

public interface IRepository
{
    public Task CreateSchema(string name);
    public Task CreateTablesTable(string schema);
    public Task CreateTable(Table table, string schema);
    public Task<List<Table?>> ListTables(string schema);
    public Task<Table?> GetTableByExternalId(Guid externalId);
    public Task UpdateTable(Guid externalId, Table table);
    public Task DeleteTable(Guid externalId);

    public Task CreateItem(Item item, Guid tableExternalId);
    public Task<Item[]> ListItems(Guid tableExternalId);
    public Task<Item> GetItemByExternalId(Guid itemExternalId, Guid tableExternalId);
    public Task UpdateItem(Guid itemExternalId, Item item, Guid tableExternalId);
    public Task DeleteItem(Guid itemExternalId, Guid tableExternalId);
}