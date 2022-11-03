using GrpcInit.Domain;

namespace GrpcInit.Application.Abstractions;

public interface IProtoWriter
{
    Task<string> Write(ProtoFileTokens tokens);
}
