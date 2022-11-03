using GrpcInit.Console.Commands;

namespace GrpcInit.Console;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var commandSelector = new CommandSelector();
        var command = commandSelector.SelectCommand(args);
        await command.Execute();
    }
}
