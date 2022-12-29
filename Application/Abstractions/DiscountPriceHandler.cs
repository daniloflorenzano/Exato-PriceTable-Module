using Domain;

namespace Application.Abstractions;

public abstract class DiscountPriceHandler
{
    private static List<Item> _items;
    private double _itemInitialPrice;
    private List<double> _priceSequence;
    private List<int> _amountLimitsToApplyDiscount;
    private int _minimalToApplyDiscount;

    public DiscountPriceHandler(List<Item> items)
    {
        _items = items;
        
        _itemInitialPrice = _items[0].InitalPrice;
        _priceSequence = _items[0].PriceSequence;
        _amountLimitsToApplyDiscount = _items[0].AmountLimitsToApplyDiscount;
        _minimalToApplyDiscount = _items[0].AmountLimitsToApplyDiscount[0];
    }

    public abstract double CalculateTotalPrice();

    /// <summary>
    /// Calcula qual sera o valor unitario do proximo item a ser adicionado em uma tabela
    /// </summary>
    /// <returns></returns>
    public double CalculateNextUnitPrice()
    {
        var totalItems = _items.Count;
        var price = 0.0;
        
        if (totalItems < _minimalToApplyDiscount)
            return _itemInitialPrice;

        for (int i = 0; i < _amountLimitsToApplyDiscount.Count; i++)
        {
            var limit = _amountLimitsToApplyDiscount[i];
            var priceWithDiscount = _priceSequence[i];

            if (totalItems + 1 > limit)
                price = priceWithDiscount;
        }

        return price;
    }
}