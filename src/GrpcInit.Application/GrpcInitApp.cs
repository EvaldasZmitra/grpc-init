using GrpcInit.Adapter.Abstractions;
using GrpcInit.Application.Abstractions;
using GrpcInit.Output.Abstractions;

namespace GrpcInit.Application;

public sealed class GrpcInitApp
{
    private readonly IOutput _output;
    private readonly IServiceReader _serivceReader;
    private readonly IProtoReader _protoReader;
    private readonly IProtoFileWriter _writer;

    public GrpcInitApp(
        IOutput output,
        IServiceReader serviceReader,
        IProtoReader protoReader,
        IProtoFileWriter writer
    )
    {
        _protoReader = protoReader;
        _serivceReader = serviceReader;
        _output = output;
        _writer = writer;
    }

    public async Task Run()
    {
        var services = await _serivceReader.Read();
        var tokens = await _protoReader.Read(services);
        var protos = await _writer.Write(tokens);
        await _output.Write(protos);
    }
}
