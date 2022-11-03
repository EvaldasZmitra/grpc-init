using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application.Writers;

public sealed class ImportWriter : IWriter
{
    public IEnumerable<string> Write(ProtoFileTokens proto)
    {
        var result = new List<string>();
        foreach (var import in proto.Dependencies)
        {
            result.Add($"import \"{import}\";");
        }
        return result;
    }
}
