namespace ExatoDigital.OpenSource.PriceTableModule.Domain.Table
{
    public enum TableType : int
    {
        FixedPrice,
        CumulativePrice = 1,
        NonCumulativePrice = 2
    }
}
