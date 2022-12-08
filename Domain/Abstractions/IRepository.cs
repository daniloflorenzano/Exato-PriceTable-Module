namespace Domain.Abstractions;

public interface IRepository
{
    public Task<Table> CreateTable(Table table);
    public Task<Table[]> ListTables();
    public Task<Table> GetTableByExternalId();
    public Task DeleteTable();

    public Task CreateItem();
    public Task<Item[]> ListItems();
    public Task<Item> GetItemByExternalId();
    public Task DeleteItem();
}