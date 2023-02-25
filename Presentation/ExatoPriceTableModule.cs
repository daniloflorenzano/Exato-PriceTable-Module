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
    private readonly string _connectionString;
    private readonly string _schema;

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

        await CreateSchema();
        await CreateTablesTable();

        var table = new Table(name, description, expirationDate, discountType);
        var tableHandler = new TableHandler(repositoryFactory, logger, _schema);
        await tableHandler.CreateTable(table);
        logger.Information($"Created: [{table}].");
    }

    private async Task CreateSchema()
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");

        var tableHandler = new TableHandler(repositoryFactory, logger, _schema);
        await tableHandler.CreateSchema(_schema);
    }
    
    private async Task CreateTablesTable()
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");

        await CreateSchema();

        var tableHandler = new TableHandler(repositoryFactory, logger, _schema);
        await tableHandler.CreateRegisterTable();
    }

    public async Task<List<Table>> ListTables()
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");

        var tableHandler = new TableHandler(repositoryFactory, logger, _schema);
        var tables = await tableHandler.ListTables();
        logger.Information($"Found {tables.Count} tables");

        return tables;
    }

    public async Task<Table> GetTableByExternalId(Guid externalId)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);

        var tableHhandler = new TableHandler(repositoryFactory, logger, _schema);
        var table = await tableHhandler.GetTableByExternalId(externalId);

        logger.Information($"Table founded: [{table}].");
        return table;
    }

    public async Task UpdateTable(Guid externalId, Table table)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        var tablheHandler = new TableHandler(repositoryFactory, logger, _schema);
        await tablheHandler.UpdateTable(externalId, table);
        logger.Information($"Table updated: {table}");
    }
    
    public async Task DeleteTable(Guid externalId)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        var tableHanlder = new TableHandler(repositoryFactory, logger, _schema);
        await tableHanlder.DeleteTable(externalId);
        logger.Information("Table deleted");
    }
    
    public async Task CreateItem(Item item, Guid tableExternalId)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);

        var itemHandler = new ItemHandler(repositoryFactory, logger, tableExternalId, _schema);
        await itemHandler.CreateItemInTable(item);
    }
    
    public async Task<List<Item>> ListItems(Guid tableExternalId)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);

        var itemHandler = new ItemHandler(repositoryFactory, logger, tableExternalId, _schema);
        return await itemHandler.ListAllItemsInTable();
    }
    
    public async Task<List<Item>> ListItemsSinceDate(Guid tableExternalId, DateTime date)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);

        var itemHandler = new ItemHandler(repositoryFactory, logger, tableExternalId, _schema);
        return await itemHandler.ListItemsInTableSinceDate(date);
    }
    
    public async Task<Item> GetItemByExternalId(Guid tableExternalId, Guid itemExternalId)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        var itemHandler = new ItemHandler(repositoryFactory, logger, tableExternalId, _schema);
        return await itemHandler.GetItemByExternalIdInTable(itemExternalId);
    }
    
    public async Task UpdateItem(Guid tableExternalId, Guid itemExternalId, Item item)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);

        var itemHandler = new ItemHandler(repositoryFactory, logger, tableExternalId, _schema);
        await itemHandler.UpdateItemInTable(itemExternalId, item);
    }
    
    public async Task DeleteItem(Guid tableExternalId, Guid itemExternalId)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);

        var itemHandler = new ItemHandler(repositoryFactory, logger, tableExternalId, _schema);
        await itemHandler.Delete(itemExternalId);
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
    

    private void InitiateDependencyContainer(out IRepositoryFactory repositoryFactory, out ILogger logger)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_connectionString))
            .AddSingleton<IRepositoryFactory, RepositoryFactory>()
            .AddSingleton<ILogger, SerilogWrapper>()
            .BuildServiceProvider();

        var exceptionMessage = "Dependecy container could not initiate";
        
        repositoryFactory = serviceProvider.GetService<IRepositoryFactory>() ?? 
                            throw new InvalidOperationException(exceptionMessage);
        
        logger = serviceProvider.GetService<ILogger>() ?? 
                 throw new InvalidOperationException(exceptionMessage);
    }
}
