using AutoFixture.Xunit2;
using GrpcInit.Adapter.Abstractions;
using GrpcInit.Application.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Output.Abstractions;
using Moq;
using Xunit;

namespace GrpcInit.Application.Tests;

public class GrpcInitAppTests
{
    private readonly Mock<IOutput> _output = new();
    private readonly Mock<IServiceReader> _serviceReader = new();
    private readonly Mock<IProtoReader> _protoReader = new();
    private readonly Mock<IProtoFileWriter> _protoWriter = new();
    private readonly GrpcInitApp _sut;

    public GrpcInitAppTests() => _sut = new GrpcInitApp(
            _output.Object,
            _serviceReader.Object,
            _protoReader.Object,
            _protoWriter.Object
        );

    [Theory]
    [AutoData]
    public async Task ShouldRunInExpectedOrder(
        IEnumerable<string> services,
        IEnumerable<ProtoFileTokens> tokens,
        IEnumerable<ProtoFile> protos
    )
    {
        _serviceReader.Setup(x => x.Read()).Returns(Task.FromResult(services));
        _protoReader.Setup(x => x.Read(services)).Returns(Task.FromResult(tokens));
        _protoWriter.Setup(x => x.Write(tokens)).Returns(Task.FromResult(protos));

        await _sut.Run();

        _output.Verify(x => x.Write(protos));
    }
}
