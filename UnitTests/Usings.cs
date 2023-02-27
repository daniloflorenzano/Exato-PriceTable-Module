global using NUnit.Framework;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.ValueObjects;
using Presentation;

public class Usings
{
    private ExatoPriceTableModule _exatoPriceTableModule;
    private const string ConnectionString = "Host=localhost;Port=5432;Database=Testes;Username=postgres;Password=mysecretpassword";
    private const string Schema = "descontos";

    public Usings()
    {
        _exatoPriceTableModule = new ExatoPriceTableModule(ConnectionString, Schema);
    }
    
    [Test]
    public async Task CreateTable_Should_Work()
    {
        await _exatoPriceTableModule.CreateTable("desconto nao cumulativo", "descricao", DiscountType.NonCumulativeDiscount, null);
    }
    
    [Test]
    public async Task ListTables_Should_Work()
    {
        var tables = await _exatoPriceTableModule.ListTables();
    }
    
    [Test]
    public async Task GetTableById_Should_Work()
    {
        var externalId = Guid.Parse("1184a0f7-00cb-4675-b559-1d36e85a82d6");
        var table = await _exatoPriceTableModule.GetTableByExternalId(externalId);
        Console.WriteLine(table);
    }

    [Test] public async Task DeleteTable_Should_Work()
    {
        var externalId = Guid.Parse("c3cba4ef-3300-41d7-a76a-f3e6259031d0");
        await _exatoPriceTableModule.DeleteTable(externalId);
    }
    
    [Test] public async Task UpdateTable_Should_Work()
    {
        var externalId = Guid.Parse("d58415ce-456a-4fde-a3a1-a91394fde461");
        var table = new Table("NovoNome", "atualizando a tabela", null, DiscountType.FixedPrice);

        await _exatoPriceTableModule.UpdateTable(externalId, table);
    }
    
    [Test] public async Task CreateItemWithFixedPrice_Should_Work()
    {
        var externalId = Guid.Parse("b0206c4a-e22d-475c-b892-634ef7c2e5f5");
        var newItem = new Item("Item 3", 0.99m);

        await _exatoPriceTableModule.CreateItem(newItem, externalId);
    }
    
    [Test] public async Task CreateItemWithCumulativeDiscount_Should_Work()
    {
        var externalId = Guid.Parse("b22d4496-0e30-4a08-ab32-289cbc2e9258");
        var priceSequence = new List<decimal> { 0.30m, 0.20m, 0.10m };
        var limitSequence = new List<int> { 2, 4, 6 };

        var newItem = new Item("Item 2", 0.50m, priceSequence, limitSequence);

        await _exatoPriceTableModule.CreateItem(newItem, externalId);
    }
    
    [Test] public async Task ListItems_Should_Work()
    {
        var externalId = Guid.Parse("d58415ce-456a-4fde-a3a1-a91394fde461");

        var items = await _exatoPriceTableModule.ListItems(externalId);
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }
    
    [Test] public async Task ListItemsSinceDate_Should_Work()
    {
        var externalId = Guid.Parse("b0206c4a-e22d-475c-b892-634ef7c2e5f5");

        var items = await _exatoPriceTableModule.ListItems(externalId, DateTime.Parse("2023-03-03"), DateTime.Today);
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }
    
    [Test] public async Task GetItemByExternalId_Should_Work()
    {
        var tableExternalId = Guid.Parse("d58415ce-456a-4fde-a3a1-a91394fde461");
        var itemExternalId = Guid.Parse("4e3f185a-c9c6-40b9-85e9-9f1284d685ec");
        
        var item = await _exatoPriceTableModule.GetItemByExternalId(tableExternalId, itemExternalId);
        Console.WriteLine(item);
    }
    
    [Test] public async Task UpdateItem_Should_Work()
    {
        var tableExternalId = Guid.Parse("b0206c4a-e22d-475c-b892-634ef7c2e5f5");
        var itemExternalId = Guid.Parse("b02dafbc-2ffa-4a5c-8265-3bd338add696");
        var price = new Price()
        {
            InitialValue = 0.21m,
            PriceSequence = new List<decimal>()
        };
        Item item = new() { Description = "testando update", Price = price};
        
        await _exatoPriceTableModule.UpdateItem(tableExternalId, itemExternalId, item);
    }
    
    [Test] public async Task DeleteItem_Should_Work()
    {
        var tableExternalId = Guid.Parse("d58415ce-456a-4fde-a3a1-a91394fde461");
        var itemExternalId = Guid.Parse("4e3f185a-c9c6-40b9-85e9-9f1284d685ec");
        
        await _exatoPriceTableModule.DeleteItem(tableExternalId, itemExternalId);
    }
    
    [Test] public async Task CalculatePrice_Should_Work()
    {
        var tableExternalId = Guid.Parse("b22d4496-0e30-4a08-ab32-289cbc2e9258");
        
        var totalPrice = await _exatoPriceTableModule.CalculatePrice(tableExternalId);
        Console.WriteLine(totalPrice);
    }
    
    [Test] public async Task CalculatePriceInDateRange_Should_Work()
    {
        var tableExternalId = Guid.Parse("b22d4496-0e30-4a08-ab32-289cbc2e9258");
        
        var totalPriceInAMonth = await _exatoPriceTableModule.CalculatePrice(tableExternalId, new DateTime(2022,02,01), DateTime.Today);
        Console.WriteLine(totalPriceInAMonth);
    }
    
    [Test] public async Task CalculatePriceOfTheNextItem_Should_Work()
    {
        var tableExternalId = Guid.Parse("b22d4496-0e30-4a08-ab32-289cbc2e9258");
        
        var priceOfTheNextItem = await _exatoPriceTableModule.CalculatePriceOfTheNextItem(tableExternalId);
        Console.WriteLine(priceOfTheNextItem);
    }
    
    [Test] public async Task CalculatePriceOfTheNextItemInDateRange_Should_Work()
    {
        var tableExternalId = Guid.Parse("b22d4496-0e30-4a08-ab32-289cbc2e9258");
        
        var priceOfTheNextItem = await _exatoPriceTableModule.CalculatePriceOfTheNextItem(tableExternalId, new DateTime(2022,02,01), DateTime.Today);
        Console.WriteLine(priceOfTheNextItem);
    }
}