using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Handlers;

public class TableHandler 
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly ILogger _logger;
    private string _schema;

    public TableHandler(IRepositoryFactory repositoryFactory, ILogger logger, string schema)
    {
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        _logger = logger;
        _schema = schema;
    }

    public async Task<List<Table?>> ListTables()
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var result = await repository.ListTables();
            
            return result;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
            throw;
        }
    }

    public async Task<Table?> GetTableByExternalId(Guid externalId)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var result = await repository.GetTableByExternalId(externalId);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task CreateTable(Table table)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            await repository.CreateTable(table);
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
            var repository = _repositoryFactory.Create(_schema);
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
            var repository = _repositoryFactory.Create(_schema);
            await repository.DeleteTable(externalId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}