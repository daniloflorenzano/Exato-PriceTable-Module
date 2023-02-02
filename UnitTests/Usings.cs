global using NUnit.Framework;
using Presentation;

public class Usings
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=Testes;Username=postgres;Password=mysecretpassword";
    [Test]
    public async Task CreateSchema_Showl_Work()
    {
        var priceTableModule = new ExatoPriceTableModule(ConnectionString);
        await priceTableModule.CreateSchema("Descontos");
    }
}