namespace Domain.Exceptions;

[Serializable]
public class CannotUpdateItemException : Exception
{
    public CannotUpdateItemException() : base() { }

    public CannotUpdateItemException(string message) : base(message) { }
    public CannotUpdateItemException(string message, Exception inner) : base(message, inner) { }
}