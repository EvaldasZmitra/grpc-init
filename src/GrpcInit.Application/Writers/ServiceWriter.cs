using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application.Writers;

public sealed class ServiceWriter : IWriter
{
    private readonly IElementWriter _elementWriter;

    public ServiceWriter(IElementWriter elementWriter) =>
        _elementWriter = elementWriter;

    public IEnumerable<string> Write(ProtoFileTokens proto)
    {
        var result = new List<string>();
        foreach (var service in proto.Services)
        {
            var contents = service.Methods.Select(x =>
                $"rpc {x.Name}({x.InputType}) returns ({x.OutputType});"
            );
            result.AddRange(
                _elementWriter.WriteBlock($"service {service.Name}", contents)
            );
        }
        return result;
    }
}
