using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application.Writers;

public sealed class MessageWriter : IWriter
{
    public IEnumerable<string> Write(ProtoFileTokens proto)
    {
        var result = new List<string>();
        foreach (var type in proto.Messages)
        {
            result.Add($"message {type.Name} {{");
            foreach (var field in type.Fields)
            {
                result.Add($"\t{field.Label}{field.Type} {field.Name} = {field.Number};");
            }
            result.Add("}");
            result.Add("");
        }
        return result;
    }
}
