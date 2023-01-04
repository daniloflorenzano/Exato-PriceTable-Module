namespace Domain.Entities.Enums
{
    public enum DiscountType : int
    {
        FixedPrice,
        CumulativeDiscount = 1,
        NonCumulativeDiscount = 2
    }
}
