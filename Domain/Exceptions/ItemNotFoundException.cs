namespace Domain.Exceptions;

[Serializable]
public class ItemNotFoundException : Exception
{
    public ItemNotFoundException() : base() { }

    public ItemNotFoundException(Guid itemExternalId)
        : base(String.Format("Item with External Id: {0} not found", itemExternalId.ToString()))
    {
    }
    public ItemNotFoundException(string message, Exception inner) : base(message, inner) { }
}