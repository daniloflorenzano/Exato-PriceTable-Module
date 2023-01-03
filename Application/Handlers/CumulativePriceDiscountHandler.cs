using Application.Abstractions;
using Domain.Entities;

namespace Application.Handlers;


// preco de todas os itens diminui a cada x quantidades compradas
public sealed class CumulativePriceDiscountHandler : DiscountPriceHandler
{
    // a lista de itens recebida ja deve vir filtrada com apenas itens iguais, e dado um periodo de tempo
    // exemplo: de tudo que o cliente comprou, a lista vira apenas com os itens A comprados no mes de dezembro
    public CumulativePriceDiscountHandler(List<Item> items) : base(items)
    {
    }

    public override decimal CalculateTotalPrice()
    {
        var price = 0.0m;

        if (TotalItems < MinimalToApplyDiscount)
            return TotalItems * ItemInitialPrice;

        for (int i = 0; i < AmountLimitsToApplyDiscount.Count; i++)
        {
            var limit = AmountLimitsToApplyDiscount[i];
            var priceWithDiscount = PriceSequence[i];

            if (TotalItems + 1 > limit)
                price = priceWithDiscount;
        }

        return TotalItems * price;
    }
}