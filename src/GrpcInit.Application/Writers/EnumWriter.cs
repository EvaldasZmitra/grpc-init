using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application.Writers;

public sealed class EnumWriter : IWriter
{
    private readonly IElementWriter _elementWriter;

    public EnumWriter(IElementWriter elementWriter) =>
        _elementWriter = elementWriter;

    public IEnumerable<string> Write(ProtoFileTokens proto)
    {
        var result = new List<string>();
        foreach (var e in proto.Enums)
        {
            var contents = e.Values.Select(
                x => $"{x.Name} = {x.Value};"
            );
            result.AddRange(
                _elementWriter.WriteBlock($"enum {e.Name}", contents)
            );
        }
        return result;
    }
}
