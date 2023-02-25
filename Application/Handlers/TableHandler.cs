using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Handlers;

public class TableHandler 
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly ILogger _logger;
    private string _schema;

    public TableHandler(IRepositoryFactory repositoryFactory, ILogger logger, string schema)
    {
        _repositoryFactory = repositoryFactory;
        _logger = logger;
        _schema = schema;
    }

    public async Task<List<Table>> ListAll()
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var result = await repository.ListTables();
            
            return result;
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }

    public async Task<Table> GetByExternalId(Guid externalId)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var result = await repository.GetTableByExternalId(externalId);

            return result;
        }
        catch (TableNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }
    
    public async Task Create(Table table)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            await repository.CreateTable(table);
        }
        catch (Exception e) when (e.Message.Contains("violates unique constraint"))
        {
            throw new TableAlreadyExistsException();
        }
        catch (TableAlreadyExistsException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (Exception e) when(e.Message.Contains("already exists"))
        {
            _logger.Warning($"{e.Message}, continuing...");
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }

    public async Task Update(Guid tableExternalId, Table table)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var existingTable = await repository.GetTableByExternalId(tableExternalId);

            if (existingTable.Type != table.Type)
                throw new CannotChangeTableTypeException(existingTable.Type, table.Type);

            await repository.UpdateTable(tableExternalId, table);
        }
        catch (TableNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (CannotChangeTableTypeException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }

    public async Task Delete(Guid externalId)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            await repository.DeleteTable(externalId);
        }
        catch (TableNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task CreateSchema(string schema)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            await repository.CreateSchema();
        }
        catch (Exception e) when(e.Message.Contains("already exists"))
        {
            _logger.Warning("Schema already created, continuing...");
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
            throw;
        }
    }

    public async Task CreateRegisterTable()
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            await repository.CreateTablesTable();
        }
        catch (Exception e) when(e.Message.Contains("already exists"))
        {
            _logger.Warning("Register table already created, continuing...");
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }
}