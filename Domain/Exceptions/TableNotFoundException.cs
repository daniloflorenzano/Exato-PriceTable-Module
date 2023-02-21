namespace Domain.Exceptions;

[Serializable]
public class TableNotFoundException : Exception
{
    public TableNotFoundException() : base() { }
    public TableNotFoundException(Guid tableExternalId) 
        : base(String.Format("Table with External Id: {0} not found", tableExternalId.ToString())) { }
    public TableNotFoundException(string message, Exception inner) : base(message, inner) { }
}