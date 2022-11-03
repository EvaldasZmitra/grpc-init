using GrpcInit.Domain;

namespace GrpcInit.Application.Abstractions;

public interface IProtoFileFilter
{
    IEnumerable<ProtoFileTokens> Filter(IEnumerable<ProtoFileTokens> protos);
}
