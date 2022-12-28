using Application.Abstractions;
using Domain;

namespace Application.Handlers;


// preco de todas os itens diminui a cada x quantidades compradas
public sealed class CumulativePriceDiscountHandler : DiscountPriceHandler
{
    // a lista de itens recebida ja deve vir filtrada com apenas itens iguais, e dado um periodo de tempo
    // exemplo: de tudo que o cliente comprou, a lista vira apenas com os itens A comprados no mes de dezembro
    public CumulativePriceDiscountHandler(List<Item> items) : base(items)
    {
    }

    public override double CalculateTotalPrice()
    {
        throw new NotImplementedException();
    }
}