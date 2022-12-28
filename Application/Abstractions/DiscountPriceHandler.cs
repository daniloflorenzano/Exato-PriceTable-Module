using Domain;

namespace Application.Abstractions;

public abstract class DiscountPriceHandler
{
    private static List<Item> _items;

    public DiscountPriceHandler(List<Item> items)
    {
        _items = items;
    }
    
    private double _itemInitialPrice = _items[0].InitalPrice;
    private List<double> _priceSequence = _items[0].PriceSequence;
    private List<int> _amountLimitsToApplyDiscount = _items[0].AmountLimitsToApplyDiscount;
    private int _minimalToApplyDiscount = _items[0].AmountLimitsToApplyDiscount[0];
    
    public abstract double CalculateTotalPrice();

    public double CalculateUnitPrice()
    {
        var totalItems = _items.Count;
        var price = 0.0;
        
        if (totalItems < _minimalToApplyDiscount)
            return _itemInitialPrice;

        for (int i = 0; i < _amountLimitsToApplyDiscount.Count; i++)
        {
            var limit = _amountLimitsToApplyDiscount[i];
            var priceWithDiscount = _priceSequence[i];

            if (totalItems > limit)
                price = priceWithDiscount;
        }

        return price;
    }
}