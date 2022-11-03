namespace GrpcInit.Adapter.Abstractions;

public interface IServiceReader
{
    Task<IEnumerable<string>> Read();
}
