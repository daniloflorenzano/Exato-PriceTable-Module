using Domain;
using Domain.Abstractions;

namespace Application;

public class TableHandler
{
    private readonly IRepositoryFactory _repositoryFactory;

    public TableHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public async Task<Table> CreateTable(Table table)
    {
        var repository = _repositoryFactory.Create();

        var existingTables = await repository.ListTables();
        foreach (var t in existingTables)
        {
            if (t.Name == table.Name && t.Type != table.Type)
                throw new Exception($"Already exists a table named {table.Name}, but with a different type");
        }
        
        var existingTableWithSameNameAndType = await ExistingTableWithSameNameAndType(table);
        
        return existingTableWithSameNameAndType ?? await repository.CreateTable(table);
    }

    private async Task<Table?> ExistingTableWithSameNameAndType(Table table)
    {
        var repository = _repositoryFactory.Create();
        var existingTables = await repository.ListTables();

        Table existingTable = null;
        
        foreach (Table t in existingTables)
        {
            if (t.Name == table.Name && t.Type == table.Type)
                existingTable = t;
        }

        return existingTable;
    }
}