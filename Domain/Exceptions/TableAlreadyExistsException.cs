namespace Domain.Exceptions;

[Serializable]
public class TableAlreadyExistsException : Exception
{
    public TableAlreadyExistsException() : base() { }

    public TableAlreadyExistsException(string tableName)
        : base(String.Format("Table {0} already exists", tableName))
    {
    }
    public TableAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
}