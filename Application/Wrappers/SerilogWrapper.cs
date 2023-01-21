using Serilog;
using Serilog.Core;
using ILogger = Application.Abstractions.ILogger;

namespace Application.ExternalLibraries;

public sealed class SerilogWrapper : ILogger
{
    public void Information(string message) => 
        Initialization().Information(message);

    private Logger Initialization()
    {
        using var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        return log;
    }
}