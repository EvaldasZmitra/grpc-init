using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application.Writers;

public sealed class ServiceWriter : IWriter
{
    public IEnumerable<string> Write(ProtoFileTokens proto)
    {
        var result = new List<string>();
        foreach (var service in proto.Services)
        {
            result.Add($"service {service.Name} {{");
            foreach (var method in service.Methods)
            {
                result.Add($"\trpc {method.Name}({method.InputType}) returns ({method.OutputType});");
            }
            result.Add("}");
            result.Add("");
        }
        return result;
    }
}
