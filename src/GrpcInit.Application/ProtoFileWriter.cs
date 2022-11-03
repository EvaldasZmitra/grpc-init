using GrpcInit.Application.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application;

public sealed class ProtoFileWriter : IProtoFileWriter
{
    private readonly IProtoFileFilter _filter;
    private readonly IProtoWriter _writer;

    public ProtoFileWriter(
        IProtoFileFilter filter,
        IProtoWriter writer
    )
    {
        _filter = filter;
        _writer = writer;
    }

    public async Task<IEnumerable<ProtoFile>> Write(
        IEnumerable<ProtoFileTokens> protos
    )
    {
        var filteredProtoTokens = _filter.Filter(protos);
        return await Task.WhenAll(
            filteredProtoTokens.Select(
                async (x) => new ProtoFile(x.Name, await _writer.Write(x))
            )
        );
    }
}
