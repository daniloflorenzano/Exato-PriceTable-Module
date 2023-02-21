﻿namespace Domain.Exceptions;

public class TableNotFoundException : Exception
{
    public TableNotFoundException() : base() { }
    public TableNotFoundException(string message) : base(message) { }
    public TableNotFoundException(string message, Exception inner) : base(message, inner) { }
}