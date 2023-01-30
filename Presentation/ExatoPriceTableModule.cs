using Application.Abstractions;
using Application.Handlers;
using Application.Wrappers;
using Domain.Entities;
using Domain.Entities.Enums;
using Infraestructure;
using Infraestructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Presentation;

public class ExatoPriceTableModule
{
    private static readonly ApplicationDbContext _applicationDbContext = null!;
    private readonly ILogger _logger = null!;
    private readonly RepositoryFactory _repositoryFactory = new (_applicationDbContext);

    public ExatoPriceTableModule()
    {
        
    }
    
    public async Task CreateTable(string name, string description, DiscountType discountType, DateTime? expirationDate)
    {
        try
        {
            InitiateDependencyContainer();
            
            Table table = new (name, description, expirationDate, discountType);
            TableHandler tableHandler = new(_repositoryFactory, _logger);

            await tableHandler.CreateTable(table);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<decimal> CalculateTotalPriceInDateRange(Guid tableExternalId, DateTime initialDate, DateTime limitDate)
    {
        try
        {
            ItemHandler itemHandler = new (_repositoryFactory, tableExternalId);
            TableHandler tableHandler = new(_repositoryFactory, _logger);
            var table = await tableHandler.GetTableByExternalId(tableExternalId);
            var tableType = table.Type;
            var items = await itemHandler.ListItemsInTableInDateRange(initialDate, limitDate);
            var groupsOfItems = itemHandler.SegregateItems(items);
            var totalPrice = CalculateTotalPriceByTypeOfTable(groupsOfItems, tableType);

            return totalPrice;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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

    private void InitiateDependencyContainer()
    {
        // TODO: ver como sera feita a injecao do DbContext pelo usuario
        using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) => 
                services
                    .AddDbContext<ApplicationDbContext>()
                    .AddTransient<ILogger, SerilogWrapper>())
                    .Build();
    }
}
