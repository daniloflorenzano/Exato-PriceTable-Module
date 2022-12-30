using Domain;

namespace Application.Abstractions;

public abstract class DiscountPriceHandler
{
    protected readonly int TotalItems;
    protected readonly double ItemInitialPrice;
    protected readonly List<double> PriceSequence;
    protected readonly List<int> AmountLimitsToApplyDiscount;
    protected readonly int MinimalToApplyDiscount;

    public DiscountPriceHandler(List<Item> items)
    {
        TotalItems = items.Count;
        ItemInitialPrice = items[0].InitalPrice;
        PriceSequence = items[0].PriceSequence;
        AmountLimitsToApplyDiscount = items[0].AmountLimitsToApplyDiscount;
        MinimalToApplyDiscount = items[0].AmountLimitsToApplyDiscount[0];
    }

    public abstract double CalculateTotalPrice();

    /// <summary>
    /// Calcula qual sera o valor unitario do proximo item a ser adicionado em uma tabela
    /// </summary>
    /// <returns></returns>
    public double CalculateNextUnitPrice()
    {
        var price = 0.0;
        
        if (TotalItems < MinimalToApplyDiscount)
            return ItemInitialPrice;

        for (int i = 0; i < AmountLimitsToApplyDiscount.Count; i++)
        {
            var limit = AmountLimitsToApplyDiscount[i];
            var priceWithDiscount = PriceSequence[i];

            if (TotalItems + 1 > limit)
                price = priceWithDiscount;
        }

        return price;
    }
}