using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application.Writers;

public sealed class MessageWriter : IWriter
{
    private readonly IElementWriter _elementWriter;

    public MessageWriter(IElementWriter elementWriter) =>
        _elementWriter = elementWriter;

    public IEnumerable<string> Write(ProtoFileTokens proto)
    {
        var result = new List<string>();
        foreach (var type in proto.Messages)
        {
            var content = type.Fields.Select(x =>
                $"{x.Label}{x.Type} {x.Name} = {x.Number};"
            );
            result.AddRange(
                _elementWriter.WriteBlock($"message {type.Name}", content)
            );
        }
        return result;
    }
}
