using Domain.Entities;

namespace Domain.UseCases;

public abstract class DiscountPrice
{
    protected readonly int TotalItems;
    protected readonly decimal ItemInitialPrice;
    protected readonly List<decimal> PriceSequence;
    protected readonly List<int> AmountLimitsToApplyDiscount;
    protected readonly int MinimalToApplyDiscount;

    public DiscountPrice(List<Item> items)
    {
        TotalItems = items.Count;
        ItemInitialPrice = items[0].Price.InitialValue;
        PriceSequence = items[0].Price.PriceSequence!;
        AmountLimitsToApplyDiscount = items[0].Price.AmountLimitsToApplyDiscount!;
        MinimalToApplyDiscount = items[0].Price.AmountLimitsToApplyDiscount![0];
    }

    public abstract decimal CalculateTotalPrice();

    /// <summary>
    /// Calcula qual sera o valor unitario do proximo item a ser adicionado em uma tabela
    /// </summary>
    /// <returns></returns>
    public decimal CalculateNextUnitPrice()
    {
        var price = 0.0m;
        
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