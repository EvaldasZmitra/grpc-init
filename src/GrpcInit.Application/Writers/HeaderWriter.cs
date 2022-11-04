using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application.Writers;

public sealed class HeaderWriter : IWriter
{
    public IEnumerable<string> Write(ProtoFileTokens proto) => new List<string>
        {
            $"syntax = \"{proto.Syntax}\";\n"
        };
}
