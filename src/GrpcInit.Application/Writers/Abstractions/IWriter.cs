using GrpcInit.Domain;

namespace GrpcInit.Application.Writers.Abstractions;

public interface IWriter
{
    IEnumerable<string> Write(ProtoFileTokens proto);
}
