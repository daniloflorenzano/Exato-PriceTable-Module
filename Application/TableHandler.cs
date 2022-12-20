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
    
    public async Task<ItemHandler> CreateTable(Table table)
    {
        try
        {
            var repository = _repositoryFactory.Create();
            await repository.CreateTable(table);
            
            // permite fazer as operacoes com itens na nova tabela criada
            return new ItemHandler(_repositoryFactory, table.ExternalId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateTable(Guid tableExternalId, Table table)
    {
        try
        {
            var repository = _repositoryFactory.Create();
            await repository.UpdateTable(tableExternalId, table);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteTable(Guid externalId)
    {
        try
        {
            var repository = _repositoryFactory.Create();
            await repository.DeleteTable(externalId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}