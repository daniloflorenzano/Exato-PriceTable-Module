using System.Globalization;

namespace Domain.Exceptions;

[Serializable]
public class CannotUseFutureDateException : Exception
{
    public CannotUseFutureDateException() : base() { }

    public CannotUseFutureDateException(DateTime date)
        : base(String.Format("Cannot search for Items or Tables in {0}. Expecting a DateTime before {1}",
            date.ToString(CultureInfo.InvariantCulture), DateTime.Now))
    {
    }
    public CannotUseFutureDateException(string message, Exception inner) : base(message, inner) { }
}