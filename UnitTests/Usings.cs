global using NUnit.Framework;
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
}