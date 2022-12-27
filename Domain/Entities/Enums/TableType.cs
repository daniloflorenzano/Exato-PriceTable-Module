namespace Domain.Enums
{
    public enum TableType : int
    {
        FixedPrice,
        CumulativePriceDiscount = 1,
        NonCumulativePriceDiscount = 2
    }
}
