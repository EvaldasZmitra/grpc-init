using GrpcInit.Adapter.AspNetCore;
using GrpcInit.Application;
using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Console.Commands.Abstractions;
using GrpcInit.Output.File;
using static Grpc.Reflection.V1Alpha.ServerReflection;

namespace GrpcInit.Console.Commands;

public sealed class InitCommand : ICommand
{
    private readonly string _address;
    private readonly string _outputDirectory;

    public InitCommand(string address, string outputDirectory)
    {
        _address = address;
        _outputDirectory = outputDirectory;
    }

    public async Task Execute()
    {
        using var channel = Grpc.Net.Client.GrpcChannel.ForAddress(_address);
        var client = new ServerReflectionClient(channel);
        var grpcInit = new GrpcInitApp(
            new FileWriter(new DirectoryInfo(_outputDirectory)),
            new ServiceReader(client),
            new ProtoReader(client),
            new ProtoFileWriter(
                new ProtoFileFilter(),
                new ProtoWriter(
                    new List<IWriter>()
                    {
                        new HeaderWriter(),
                        new ImportWriter(),
                        new ServiceWriter(new ElementWriter()),
                        new MessageWriter(new ElementWriter()),
                        new EnumWriter(new ElementWriter())
                    }
                )
            )
        );
        await grpcInit.Run();
    }
}
