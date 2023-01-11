using Application.Abstractions;
using Application.ExternalLibraries;
using Application.Handlers;
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

    public ExatoPriceTableModule() {}
    
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
