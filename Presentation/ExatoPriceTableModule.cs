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
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Presentation;

public class ExatoPriceTableModule
{
    private string _connectionString;

    public ExatoPriceTableModule(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task CreateSchema(string name)
    {
        InitiateDependencyContainer(out var repositoryFactory, out var logger);
        
        if (repositoryFactory is null || logger is null)
            throw new Exception("There is a problem with the dependency injection container");
        
        try
        {

            var repository = repositoryFactory.Create();
            await repository.CreateSchema(name);
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
    
    public async Task CreateTable(string name, string description, DiscountType discountType, DateTime? expirationDate)
    {
        // try
        // {
        //     InitiateDependencyContainer();
        //     
        //     Table table = new (name, description, expirationDate, discountType);
        //     TableHandler tableHandler = new(_repositoryFactory, _logger);
        //
        //     await tableHandler.CreateTable(table);
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        //     throw;
        // }
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
