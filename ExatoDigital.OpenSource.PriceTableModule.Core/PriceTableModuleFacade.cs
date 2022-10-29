using ExatoDigital.OpenSource.PriceTableModule.Repository.Repositories;

namespace ExatoDigital.OpenSource.PriceTableModule.Core;
public class PriceTableModuleFacade : IPriceTableModuleFacade
{
    private readonly IPriceTableModuleRepositoryFactory _priceTableModuleRepositoryFactory;

    public PriceTableModuleFacade(IPriceTableModuleRepositoryFactory priceTableModuleRepositoryFactory)
    {
        _priceTableModuleRepositoryFactory = priceTableModuleRepositoryFactory;
    }
}
