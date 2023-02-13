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
        await _exatoPriceTableModule.CreateTable("Outra tabela do dale", "descricao", DiscountType.FixedPrice, null);
    }
    
    [Test]
    public async Task ListTables_Should_Work()
    {
        var tables = await _exatoPriceTableModule.ListTables();
    }
    
    [Test]
    public async Task GetTableById_Should_Work()
    {
        var externalId = Guid.Parse("4bdb1f65-f2b6-4437-bbcd-ef743f9eca5b");
        var table = await _exatoPriceTableModule.GetTableByExternalId(externalId);
    }

    [Test] public async Task DeleteTable_Should_Work()
    {
        var externalId = Guid.Parse("4bdb1f65-f2b6-4437-bbcd-ef743f9eca5b");
        await _exatoPriceTableModule.DeleteTable(externalId);
    }
    
    [Test] public async Task UpdateTable_Should_Work()
    {
        var externalId = Guid.Parse("4e7da859-a7b2-4f03-bfc4-63dbbce297a1");
        var table = new Table("NovoNome", "atualizando a tabela", null, DiscountType.FixedPrice);

        await _exatoPriceTableModule.UpdateTable(externalId, table);
    }
}