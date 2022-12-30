using Application.Abstractions;
using Domain;

namespace Application.Handlers;

// o preco dos itens comprados a partir da quantidade x tera um preco menor
public sealed class NonCumulativePriceDiscountHandler : DiscountPriceHandler
{
    public NonCumulativePriceDiscountHandler(List<Item> items) : base(items)
    {
    }

    public override double CalculateTotalPrice()
    {
        var previousLimit = 0;
        var price = 0.0;
        
        if (TotalItems < MinimalToApplyDiscount)
            return ItemInitialPrice;
        
        for (int i = 0; i < AmountLimitsToApplyDiscount.Count; i++)
        {
            var limit = AmountLimitsToApplyDiscount[i];
            var priceWithDiscount = PriceSequence[i];

            if (TotalItems + 1 > limit)
                price += priceWithDiscount * ( limit - previousLimit );

            previousLimit = limit;
        }

        if (previousLimit == AmountLimitsToApplyDiscount.Last())
            price += PriceSequence.Last() * ( TotalItems - previousLimit );
        
        return price;
    }
}
