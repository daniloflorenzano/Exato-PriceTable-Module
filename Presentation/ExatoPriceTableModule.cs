using Application.Abstractions;
using Application.Handlers;
using Application.Wrappers;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.Enums;
using Infraestructure;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Presentation;

public class ExatoPriceTableModule
{
    private string _connectionString;
    private string _schema;

    public ExatoPriceTableModule(string connectionString, string schema)
    {
        _connectionString = connectionString;
        _schema = schema;
    }

    public async Task CreateTable(string name, string description, DiscountType discountType, DateTime? expirationDate)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");

        try
        {
            await CreateSchema();
            await CreateTablesTable();

            var table = new Table(name, description, expirationDate, discountType);
            var tableHandler = new TableHandler(repositoryFactory, logger, _schema);
            await tableHandler.CreateTable(table);
            logger.Information($"Table '{table.Name}' created.");
        }
        catch (PostgresException e) when (e.Message.Contains("violates unique constraint"))
        {
            throw new Exception($"Table '{name}' already exists");
        }
        catch (PostgresException e) when(e.Message.Contains("already exists"))
        {
            logger.Warning($"{e.Message}, continuing...");
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
            throw;
        }
    }

    private async Task CreateSchema()
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");
        
        try
        {
            var repository = repositoryFactory.Create();
            await repository.CreateSchema(_schema);
        }
        catch (PostgresException e) when(e.Message.Contains("already exists"))
        {
            logger.Warning($"{e.Message}, continuing...");
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
            throw;
        }
    }
    
    private async Task CreateTablesTable()
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");
        
        try
        {
            await CreateSchema();
            
            var repository = repositoryFactory.Create();
            await repository.CreateTablesTable(_schema);
        }
        catch (PostgresException e) when(e.Message.Contains("already exists"))
        {
            logger.Warning($"{e.Message}, continuing...");
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
            throw;
        }
    }

    public async Task<List<Table>> ListTables()
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");

        try
        {
            var tableHandler = new TableHandler(repositoryFactory, logger, _schema);
            var tables = await tableHandler.ListTables();

            return tables!;
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
            throw;
        }
    }

    public async Task<Table?> GetTableByExternalId(Guid externalId)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");

        try
        {
            var repository = repositoryFactory.Create();
            var table = await repository.GetTableByExternalId(externalId);
            
            if (table is null)
                throw new Exception("Table not found");
            
            return table;
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
            throw;
        }
    }
    
    public async Task<decimal> CalculateTotalPriceInDateRange(Guid tableExternalId, DateTime initialDate, DateTime limitDate)
    {
        // try
        // {
        //     ItemHandler itemHandler = new (_repositoryFactory, tableExternalId);
        //     TableHandler tableHandler = new(_repositoryFactory, _logger);
        //     var table = await tableHandler.GetTableByExternalId(tableExternalId);
        //     var tableType = table.Type;
        //     var items = await itemHandler.ListItemsInTableInDateRange(initialDate, limitDate);
        //     var groupsOfItems = itemHandler.SegregateItems(items);
        //     var totalPrice = CalculateTotalPriceByTypeOfTable(groupsOfItems, tableType);
        //
        //     return totalPrice;
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        //     throw;
        // }
        throw new NotImplementedException();
    }

    private decimal CalculateTotalPriceByTypeOfTable(List<List<Item>> groupsOfItems, DiscountType tableType)
    {
        var totalPrice = 0.0m;
        foreach (var group in groupsOfItems)
        {
            if (tableType == DiscountType.FixedPrice)
                totalPrice += group.Count * group[0].Price.InitialValue;
                
            if (tableType == DiscountType.CumulativeDiscount)
            {
                CumulativePriceDiscountHandler discountHandler = new(group);
                totalPrice += discountHandler.CalculateTotalPrice();
            }
            else
            {
                NonCumulativePriceDiscountHandler discountHandler = new(group);
                totalPrice += discountHandler.CalculateTotalPrice();
            }
        }

        return totalPrice;
    }

    private void InitiateDependencyContainer(out IRepositoryFactory? repositoryFactory, out ILogger? logger)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_connectionString))
            .AddSingleton<IRepositoryFactory, RepositoryFactory>()
            .AddSingleton<ILogger, SerilogWrapper>()
            .BuildServiceProvider();

        repositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        logger = serviceProvider.GetService<ILogger>();
    }
}
