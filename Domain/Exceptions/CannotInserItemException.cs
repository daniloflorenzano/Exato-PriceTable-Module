namespace Domain.Exceptions;

[Serializable]
public class CannotInserItemException : Exception
{
    public CannotInserItemException() : base() { }

    public CannotInserItemException(string message) : base(message) { }
    public CannotInserItemException(string message, Exception inner) : base(message, inner) { }
}