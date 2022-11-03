using GrpcInit.Domain;

namespace GrpcInit.Adapter.Abstractions;

public interface IProtoReader
{
    Task<IEnumerable<ProtoFileTokens>> Read(IEnumerable<string> services);
}
