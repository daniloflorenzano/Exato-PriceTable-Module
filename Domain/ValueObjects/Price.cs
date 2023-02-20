namespace Domain.ValueObjects;

public class Price
{
    public decimal InitialValue { get; set; }
    public List<decimal>? PriceSequence { get; set; }
    public List<int>? AmountLimitsToApplyDiscount { get; set; }
}