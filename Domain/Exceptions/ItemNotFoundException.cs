namespace Domain.Exceptions;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException() : base() { }
    public ItemNotFoundException(string message) : base(message) { }
    public ItemNotFoundException(string message, Exception inner) : base(message, inner) { }
}