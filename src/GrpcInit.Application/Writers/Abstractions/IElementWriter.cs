namespace GrpcInit.Application.Writers.Abstractions;

public interface IElementWriter
{
    public IEnumerable<string> WriteBlock(
        string name,
        IEnumerable<string> contents
    );
}
