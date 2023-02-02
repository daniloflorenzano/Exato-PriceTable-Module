using Serilog;
using Serilog.Core;
using ILogger = Application.Abstractions.ILogger;

namespace Application.Wrappers;

public sealed class SerilogWrapper : ILogger
{
    public void Information(string message) => 
        Initialization().Information("{Message}", message);

    public void Warning(string message) =>
        Initialization().Warning("{Message}", message);

    public void Error(string message) =>
        Initialization().Error("{Message}", message);
    
    private Logger Initialization()
    {
        using var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        return log;
    }
}