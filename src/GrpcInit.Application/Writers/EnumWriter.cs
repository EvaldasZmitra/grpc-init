using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application.Writers;

public sealed class EnumWriter : IWriter
{
    public IEnumerable<string> Write(ProtoFileTokens proto)
    {
        var result = new List<string>();
        foreach (var e in proto.Enums)
        {
            result.Add($"enum {e.Name} {{");
            foreach (var v in e.Values)
            {
                result.Add($"\t{v.Name} = {v.Value};");
            }
            result.Add("}");
            result.Add("");
        }
        return result;
    }
}
