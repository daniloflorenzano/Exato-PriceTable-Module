using ExatoDigital.OpenSource.PriceTableModule.Domain;
using ExatoDigital.OpenSource.AccountModule.Repository.Repositories;

namespace ExatoDigital.OpenSource.TablePriceModule.Core;

public sealed class PriceTableModuleFacade : IPriceTableModuleFacade
{
    private readonly IPriceTableModuleRepository _priceTableModuleRepository;

    public PriceTableModuleFacade(IPriceTableModuleRepository priceTableModuleRepository)
    {
        _priceTableModuleRepository = priceTableModuleRepository;
    }
}
