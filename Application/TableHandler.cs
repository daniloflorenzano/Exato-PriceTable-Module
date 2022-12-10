using Domain;
using Domain.Abstractions;

namespace Application;

public class TableHandler : ITableHandler
{
    private readonly IRepositoryFactory _repositoryFactory;

    public TableHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public async Task<Table[]> ListTables()
    {
        var repository = _repositoryFactory.Create();
        var result = await repository.ListTables();

        return result;
    }

    public async Task<Table> GetTableByExternalId(Guid externalId)
    {
        var repository = _repositoryFactory.Create();
        var result = await repository.GetTableByExternalId(externalId);

        return result;
    }
    
    public async Task<Table> CreateTable(Table table)
    {
        var repository = _repositoryFactory.Create();

        await VerifyTableWithSameNameButDifferentType(table);
        
        var existingTableWithSameNameAndType = await ExistingTableWithSameNameAndType(table);
        
        return existingTableWithSameNameAndType ?? await repository.CreateTable(table);
    }

    public async Task<Table> UpdateTable(Guid tableExternalId, Table table)
    {
        var repository = _repositoryFactory.Create();
        var foundedTable = await repository.GetTableByExternalId(tableExternalId);

        if (foundedTable is null)
            throw new Exception($"Table with ExternalId: {tableExternalId} not founded");
        
        await repository.UpdateTable(tableExternalId, table);
        return table;
    }

    public async Task DeleteTable(Guid externalId)
    {
        var repository = _repositoryFactory.Create();
        await repository.DeleteTable(externalId);
    }

    private async Task VerifyTableWithSameNameButDifferentType(Table table)
    {
        var message = $"Already exists a table named {table.Name}, but with a different type";
        var existingTables = await ListTables();
        foreach (var t in existingTables)
        {
            if (t.Name == table.Name && t.Type != table.Type)
                throw new Exception(message);
        }
    }

    private async Task<Table?> ExistingTableWithSameNameAndType(Table table)
    {
        var existingTables = await ListTables();

        Table existingTable = null;
        
        foreach (Table t in existingTables)
        {
            if (t.Name == table.Name && t.Type == table.Type)
                existingTable = t;
        }

        return existingTable;
    }
}