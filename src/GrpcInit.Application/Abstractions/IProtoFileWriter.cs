using GrpcInit.Domain;

namespace GrpcInit.Application.Abstractions;

public interface IProtoFileWriter
{
    Task<IEnumerable<ProtoFile>> Write(IEnumerable<ProtoFileTokens> protos);
}
