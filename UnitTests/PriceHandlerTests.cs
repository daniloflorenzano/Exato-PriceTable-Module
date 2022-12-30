using Application.Handlers;
using Domain;
using FluentAssertions;

namespace UnitTests;

public class PriceHandlerTests
{
    [Test]
    public void CalculateNextUnitPrice_Should_Return_2()
    {
        var priceSequence = new List<double>() { 5.0, 4.0, 3.0, 2.0 };
        var amountLimitsToApplyDicount = new List<int>() { 3, 5, 7, 9 };
        
        // 11 itens
        var itemsList = new List<Item>()
        {
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
        };

        var priceHandler = new CumulativePriceDiscountHandler(itemsList);
        var res = priceHandler.CalculateNextUnitPrice();

        res.Should().Be(2.0);
    }
    
    [Test]
    public void CalculateTotalPriceCumulative_Should_Return_22()
    {
        var priceSequence = new List<double>() { 5.0, 4.0, 3.0, 2.0 };
        var amountLimitsToApplyDicount = new List<int>() { 3, 5, 7, 9 };
        
        // 11 itens
        var itemsList = new List<Item>()
        {
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
        };

        var priceHandler = new CumulativePriceDiscountHandler(itemsList);
        var res = priceHandler.CalculateTotalPrice();

        res.Should().Be(22.0);
    }
    
    [Test]
    public void CalculateTotalPriceNonCumulative_Should_Return_37()
    {
        var priceSequence = new List<double>() { 5.0, 4.0, 3.0, 2.0 };
        var amountLimitsToApplyDicount = new List<int>() { 3, 5, 7, 9 };
        
        // 11 itens
        var itemsList = new List<Item>()
        {
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
            new Item("A", 6.0, priceSequence, amountLimitsToApplyDicount),
        };

        var priceHandler = new NonCumulativePriceDiscountHandler(itemsList);
        var res = priceHandler.CalculateTotalPrice();

        res.Should().Be(37.0);
    }
}