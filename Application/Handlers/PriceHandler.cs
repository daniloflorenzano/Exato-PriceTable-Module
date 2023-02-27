using Domain.Entities;
using Domain.Entities.Enums;
using Domain.UseCases;

namespace Application.Handlers;

public class PriceHandler
{
    private List<Item> _items;
    private readonly DiscountType _discountType;

    public PriceHandler(List<Item> items, DiscountType discountType)
    {
        _items = items;
        _discountType = discountType;
    }

    public decimal TotalPrice()
    {
        try
        {
            switch (_discountType)
            {
                case DiscountType.NonCumulativeDiscount:
                    return TotalPriceForNonCumulativePriceTable();
                
                case DiscountType.CumulativeDiscount:
                    return TotalPriceForCumulativePriceTable();
                
                default:
                    return TotalPriceForFixedPriceTable();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private decimal TotalPriceForFixedPriceTable()
    {
        var totalPrice = 0.0m;
        foreach (var item in _items)
        {
            totalPrice += item.Price.InitialValue;
        }

        return totalPrice;
    }
    
    private decimal TotalPriceForCumulativePriceTable()
    {
        var totalPrice = 0.0m;
        
        var segregatedItems = SegregateItems();
        foreach (var groupOfItems in segregatedItems)
        {
            CumulativePriceDiscount discountHandler = new(groupOfItems);
            totalPrice += discountHandler.CalculateTotalPrice();
        }

        return totalPrice;
    }

    private decimal TotalPriceForNonCumulativePriceTable()
    {
        var totalPrice = 0.0m;
        
        var segregatedItems = SegregateItems();
        foreach (var groupOfItems in segregatedItems)
        {
            NonCumulativePriceDiscount discountHandler = new(groupOfItems);
            totalPrice += discountHandler.CalculateTotalPrice();
        }

        return totalPrice;
    }
    
    private List<List<Item>> SegregateItems()
    {
        var descriptions = _items.Select(i => i.Description).Distinct().ToList();
        var list = new List<List<Item>>();

        for (int i = 0; i < descriptions.Count; i++)
        {
            var groupOfItems = _items.Where(item => item.Description == descriptions[i]).ToList();
            list.Add(groupOfItems);
        }

        return list;
    }

    public decimal PriceOfNextItem()
    {
        if (_discountType is DiscountType.FixedPrice)
            return _items[0].Price.InitialValue;

        DiscountPrice discountHandler = new CumulativePriceDiscount(_items);
        return discountHandler.CalculateNextUnitPrice();
    }
}