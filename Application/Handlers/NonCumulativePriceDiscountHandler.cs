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
        throw new NotImplementedException();
    }
}
