using AutoFixture.Xunit2;
using FluentAssertions;
using GrpcInit.Application.Abstractions;
using GrpcInit.Domain;
using Moq;
using Xunit;

namespace GrpcInit.Application.Tests;

public class ProtoFileWriterTests
{
    private readonly Mock<IProtoWriter> _writer = new();
    private readonly Mock<IProtoFileFilter> _filter = new();
    private readonly ProtoFileWriter _sut;

    public ProtoFileWriterTests() => _sut = new ProtoFileWriter(
            _filter.Object,
            _writer.Object
        );

    [Theory]
    [AutoData]
    public async Task ShouldWriteCorrectResult(
        Dictionary<ProtoFileTokens, string> serializedData
    )
    {
        _filter.Setup(x => x.Filter(serializedData.Keys)).Returns(serializedData.Keys);
        var exptected = new List<ProtoFile>();
        foreach (var dataPoint in serializedData)
        {
            _writer.Setup(
                x => x.Write(dataPoint.Key)
            ).Returns(Task.FromResult(dataPoint.Value));
            exptected.Add(new ProtoFile(
                dataPoint.Key.Name,
                dataPoint.Value
            ));
        }

        var files = await _sut.Write(serializedData.Keys);

        files.Should().BeEquivalentTo(exptected);
    }
}
