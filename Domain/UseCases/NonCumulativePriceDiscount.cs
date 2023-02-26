using Domain.Entities;

namespace Domain.UseCases;

// o preco dos itens comprados a partir da quantidade x tera um preco menor
public sealed class NonCumulativePriceDiscount : DiscountPrice
{
    public NonCumulativePriceDiscount(List<Item> items) : base(items)
    {
    }

    public override decimal CalculateTotalPrice()
    {
        var price = 0.0m;
        
        if (TotalItems < MinimalToApplyDiscount)
            return TotalItems * ItemInitialPrice;

        var itemsToCalcule = TotalItems;
        var priceToAdd = ItemInitialPrice;
        var totalLimits = AmountLimitsToApplyDiscount.Count;
        var nItem = 1;

        for (int i = 0; i < totalLimits; i++)
        {
            for (int j = 1; j < itemsToCalcule; j++)
            {
                var priceWithDiscount = PriceSequence[i];
                var currentLimit = AmountLimitsToApplyDiscount[i];
                var nextLimit =  i + 1 == totalLimits ? currentLimit : AmountLimitsToApplyDiscount[i + 1];
                
                if (nItem <= currentLimit)
                {
                    price += priceToAdd;
                    itemsToCalcule--;
                    nItem++;
                }
                else if (nItem <= nextLimit)
                {
                    priceToAdd = priceWithDiscount;
                    price += priceToAdd;
                    itemsToCalcule--;
                    nItem++;
                }
                else
                    break;
            }
        }
        
        if (itemsToCalcule != 0)
            price += itemsToCalcule * PriceSequence.Last();

        return price;
    }
}
