using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Handlers;

public class TableHandler 
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly ILogger _logger;

    public TableHandler(IRepositoryFactory repositoryFactory, ILogger logger)
    {
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        _logger = logger;
    }

    public Table[] ListTables()
    {
        try
        {
            _logger.Information("TableHandler.ListTables executing...");
            
            var repository = _repositoryFactory.Create();
            _logger.Information("TableHandler.ListTables repository created...");

            var result = repository.ListTables();
            _logger.Information($"TableHandler.ListTables: {result.Length} tables returned");
            
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