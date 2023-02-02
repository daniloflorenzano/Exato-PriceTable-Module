namespace Application.Abstractions;

public interface ILogger
{
    public void Information(string message);
    public void Warning(string message);
    public void Error(string message);

}