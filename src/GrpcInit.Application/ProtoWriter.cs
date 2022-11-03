using GrpcInit.Application.Abstractions;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application;

public sealed class ProtoWriter : IProtoWriter
{
    private readonly IEnumerable<IWriter> _pipeline;

    public ProtoWriter(IEnumerable<IWriter> pipeline) => _pipeline = pipeline;

    public async Task<string> Write(
        ProtoFileTokens tokens
    )
    {
        var codeChunks = await Task.WhenAll(
            _pipeline.Select(
                async (writer) => await Task.Run(() => writer.Write(tokens).Append("\n"))
            )
        );
        return string.Join("\n", codeChunks.SelectMany(x => x)) + "\n";
    }
}
