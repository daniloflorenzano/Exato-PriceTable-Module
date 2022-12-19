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

    public Table[] ListTables()
    {
        try
        {
            var repository = _repositoryFactory.Create();
            var result = repository.ListTables();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Table> GetTableByExternalId(Guid externalId)
    {
        try
        {
            var repository = _repositoryFactory.Create();
            var result = await repository.GetTableByExternalId(externalId);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Table> CreateTable(Table table)
    {
        try
        {
            var repository = _repositoryFactory.Create();

            return await repository.CreateTable(table);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Table> UpdateTable(Guid tableExternalId, Table table)
    {
        var repository = _repositoryFactory.Create();
        var foundedTable = await repository.GetTableByExternalId(tableExternalId);

        await repository.UpdateTable(tableExternalId, table);
        return table;
    }

    public async Task DeleteTable(Guid externalId)
    {
        var repository = _repositoryFactory.Create();
        await repository.DeleteTable(externalId);
    }
}