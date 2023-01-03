namespace Domain.Entities.Enums
{
    public enum TableType : int
    {
        FixedPrice,
        CumulativePriceDiscount = 1,
        NonCumulativePriceDiscount = 2
    }
}
