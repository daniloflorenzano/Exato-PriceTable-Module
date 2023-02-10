global using NUnit.Framework;
using Domain.Entities;
using Domain.Entities.Enums;
using Presentation;

public class Usings
{
    private ExatoPriceTableModule _exatoPriceTableModule;
    private const string ConnectionString = "Host=localhost;Port=5432;Database=Testes;Username=postgres;Password=mysecretpassword";
    private const string Schema = "Descontos";

    public Usings()
    {
        _exatoPriceTableModule = new ExatoPriceTableModule(ConnectionString, Schema);
    }
    
    [Test]
    public async Task CreateTable_Should_Work()
    {
        await _exatoPriceTableModule.CreateTable("TabelaDeTeste2", "descricao", DiscountType.FixedPrice, null);
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
}