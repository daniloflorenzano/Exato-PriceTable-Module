using Domain;

namespace Application.Handlers;

public class PriceHandler
{
    private static List<Item> _items;
    private readonly double _decrement;
    private readonly int _quantityPurchased;
    private readonly double itemUnitPrice;
    private readonly int itemLimitAmount;
    
    public PriceHandler(double decrement, List<Item> items)
    {
        _decrement = decrement;
        _items = items;
        _quantityPurchased = _items.Count;
        itemUnitPrice = _items[0].Price;
        itemLimitAmount = _items[0].LimitAmount;
    }
    
    // TODO: configurar os descontos pra aplicar apenas para compras feitas durante periodo definido
    
    // retorna total da compra
    // o preco dos itens comprados a partir da quantidade x tera um preco menor
    public double CalculateTotalWithCumulativePriceDiscount()
    {
        if (_quantityPurchased <= itemLimitAmount)
            return itemUnitPrice * _quantityPurchased;
        
        // altera os precos nos itens
        for (int i = 0; i < _quantityPurchased; i++)
        {
            if (i + 1 > itemLimitAmount)
                _items[i].Price = itemUnitPrice - _decrement;
        }
        
        // recalcula com os valores novos
        var total = 0.0;
        foreach (var item in _items)
        {
            total += item.Price;
        }

        return total;
    }

    // preco de todas os itens diminui a cada x quantidades compradas
    public double CalculateTotalNonCumulativePriceDiscount()
    {
        if (_quantityPurchased <= itemLimitAmount)
            return itemUnitPrice * _quantityPurchased;
        
        // altera os precos nos itens
        for (int i = 0; i < _quantityPurchased; i++)
        {
            _items[i].Price = itemUnitPrice - _decrement;
        }
        
        // recalcula com os valores novos
        var total = 0.0;
        foreach (var item in _items)
        {
            total += item.Price;
        }

        return total;
    }
}