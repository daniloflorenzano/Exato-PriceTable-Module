using Domain.Entities;
using Domain.UseCases;
using FluentAssertions;

namespace UnitTests;

public class PriceHandlerTests
{
    [Test]
    public void CalculateNextUnitPrice_Should_Return_2()
    {
        var priceSequence = new List<decimal>() { 5.0m, 4.0m, 3.0m, 2.0m };
        var amountLimitsToApplyDicount = new List<int>() { 3, 5, 7, 9 };
        
        // 11 itens
        var itemsList = new List<Item>()
        {
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
        };

        var priceHandler = new CumulativePriceDiscount(itemsList);
        var res = priceHandler.CalculateNextUnitPrice();

        res.Should().Be(2.0m);
    }
    
    [Test]
    public void CalculateTotalPriceCumulative_Should_Return_22()
    {
        var priceSequence = new List<decimal>() { 5.0m, 4.0m, 3.0m, 2.0m };
        var amountLimitsToApplyDicount = new List<int>() { 3, 5, 7, 9 };
        
        // 11 itens
        var itemsList = new List<Item>()
        {
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0m, priceSequence, amountLimitsToApplyDicount),
        };

        var priceHandler = new CumulativePriceDiscount(itemsList);
        var res = priceHandler.CalculateTotalPrice();

        res.Should().Be(22.0m);
    }
    
    [Test]
    public void CalculateTotalPriceNonCumulative_Should_Return_24()
    {
        var priceSequence = new List<decimal> { 0.30m, 0.20m, 0.10m };
        var limitSequence = new List<int> { 2, 4, 6 };

        // 10 itens
        var itemsList = new List<Item>()
        {
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
            new Item("Item 2", 0.50m, priceSequence, limitSequence),
        };

        var priceHandler = new NonCumulativePriceDiscount(itemsList);
        var res = priceHandler.CalculateTotalPrice();

        res.Should().Be(2.40m);
    }
}