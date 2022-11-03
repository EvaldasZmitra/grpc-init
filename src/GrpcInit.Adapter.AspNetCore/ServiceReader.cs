using System.Globalization;
using Grpc.Reflection.V1Alpha;
using GrpcInit.Adapter.Abstractions;
using static Grpc.Reflection.V1Alpha.ServerReflection;

namespace GrpcInit.Adapter.AspNetCore;

public class ServiceReader : IServiceReader
{
    private readonly ServerReflectionClient _client;

    public ServiceReader(ServerReflectionClient client) => _client = client;

    public async Task<IEnumerable<string>> Read()
    {
        using var stream = _client.ServerReflectionInfo();
        await stream.RequestStream.WriteAsync(new ServerReflectionRequest
        {
            ListServices = ""
        });
        await stream.ResponseStream.MoveNext(CancellationToken.None);
        await stream.RequestStream.CompleteAsync();
        return stream
            .ResponseStream
            .Current
            .ListServicesResponse
            .Service
            .Select(x => x.Name).Where(x => !x.StartsWith("grpc.", true, CultureInfo.InvariantCulture));
    }
}
