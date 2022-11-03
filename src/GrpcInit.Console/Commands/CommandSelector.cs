using GrpcInit.Console.Commands.Abstractions;

namespace GrpcInit.Console.Commands;

public sealed class CommandSelector : ICommandSelector
{
    public ICommand SelectCommand(string[] args) => new InitCommand(args[0], args[1]);
}
