using Domain.Entities.Enums;

namespace Domain.Exceptions;

[Serializable]
public class CannotChangeTableTypeException : Exception
{
    public CannotChangeTableTypeException() : base() { }

    public CannotChangeTableTypeException(DiscountType typeFrom, DiscountType typeTo)
        : base(String.Format("Changing the type of the table from {0} to {1} is prohibited and may cause system issues.",
            typeFrom.ToString(), typeTo.ToString()))
    {
    }
    public CannotChangeTableTypeException(string message, Exception inner) : base(message, inner) { }
}