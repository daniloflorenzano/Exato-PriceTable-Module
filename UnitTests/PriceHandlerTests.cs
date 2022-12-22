using Application.Handlers;
using Domain;
using FluentAssertions;

namespace UnitTests;

public class PriceHandlerTests
{
    [Test]
    public void CalculateTotalWithCumulativePriceDiscount_Should_Return_11()
    {
        var itemsList = new List<Item>()
        {
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
        };

        var priceHandler = new PriceHandler(1.0, itemsList);
        var res = priceHandler.CalculateTotalWithCumulativePriceDiscount();

        res.Should().Be(11.00);
    }
    
    [Test]
    public void CalculateTotalWithNonCumulativePriceDiscount_Should_Return_7()
    {
        var itemsList = new List<Item>()
        {
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
            new Item("A", 2.0, 1, 4),
        };

        var priceHandler = new PriceHandler(1.0, itemsList);
        var res = priceHandler.CalculateTotalNonCumulativePriceDiscount();

        res.Should().Be(7.00);
    }
}