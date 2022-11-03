namespace GrpcInit.Console.Commands.Abstractions;

public interface ICommandSelector
{
    ICommand SelectCommand(string[] args);
}
