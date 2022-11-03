using Google.Protobuf.Reflection;
using Grpc.Reflection.V1Alpha;
using GrpcInit.Adapter.Abstractions;
using GrpcInit.Domain;
using static Grpc.Reflection.V1Alpha.ServerReflection;

namespace GrpcInit.Adapter.AspNetCore;

public class ProtoReader : IProtoReader
{
    private readonly ServerReflectionClient _client;

    public ProtoReader(ServerReflectionClient client) => _client = client;

    public async Task<IEnumerable<ProtoFileTokens>> Read(IEnumerable<string> services)
    {
        var allProtos = new List<ProtoFileTokens>();
        foreach (var service in services)
        {
            allProtos.AddRange(await ReadService(service));
        }
        return allProtos;
    }

    private async Task<IEnumerable<ProtoFileTokens>> ReadService(string service)
    {
        using var stream = _client.ServerReflectionInfo();
        await stream.RequestStream.WriteAsync(new ServerReflectionRequest
        {
            FileContainingSymbol = service
        });
        await stream.ResponseStream.MoveNext(CancellationToken.None);
        await stream.RequestStream.CompleteAsync();
        var parser = FileDescriptorProto.Parser;
        return stream.ResponseStream.Current.FileDescriptorResponse.FileDescriptorProto.Select(
            x => ProtoFileTokensMapper.Map(parser.ParseFrom(x))
        );
    }
}
