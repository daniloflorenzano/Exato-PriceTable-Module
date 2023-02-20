﻿global using NUnit.Framework;
using Domain.Entities;
using Domain.Entities.Enums;
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
        await _exatoPriceTableModule.CreateTable("preco fixo", "descricao", DiscountType.FixedPrice, null);
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
        await _exatoPriceTableModule.GetTableByExternalId(externalId);
    }

    [Test] public async Task DeleteTable_Should_Work()
    {
        var externalId = Guid.Parse("c3cba4ef-3300-41d7-a76a-f3e6259031d0");
        await _exatoPriceTableModule.DeleteTable(externalId);
    }
    
    [Test] public async Task UpdateTable_Should_Work()
    {
        var externalId = Guid.Parse("4e7da859-a7b2-4f03-bfc4-63dbbce297a1");
        var table = new Table("NovoNome", "atualizando a tabela", null, DiscountType.FixedPrice);

        await _exatoPriceTableModule.UpdateTable(externalId, table);
    }
    
    [Test] public async Task CreateItemWithFixedPrice_Should_Work()
    {
        var externalId = Guid.Parse("b0206c4a-e22d-475c-b892-634ef7c2e5f5");
        var newItem = new Item("Item 3", 0.99m);

        await _exatoPriceTableModule.CreateItem(newItem, externalId);
    }
    
    [Test] public async Task ListItems_Should_Work()
    {
        var externalId = Guid.Parse("b0206c4a-e22d-475c-b892-634ef7c2e5f5");
        var newItem = new Item("Item Teste", 0.19m);

        var items = await _exatoPriceTableModule.ListItems(externalId);
    }
}