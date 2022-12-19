namespace Domain.Abstractions;

public interface IRepository
{
    public Task<Table> CreateTable(Table table);
    public Table[] ListTables();
    public Task<Table> GetTableByExternalId(Guid externalId);
    public Task UpdateTable(Guid externalId, Table table);
    public Task DeleteTable(Guid externalId);

    public Task CreateItem();
    public Task<Item[]> ListItems();
    public Task<Item> GetItemByExternalId();
    public Task DeleteItem();
}