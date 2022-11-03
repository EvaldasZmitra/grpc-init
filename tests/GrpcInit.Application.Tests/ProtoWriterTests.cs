using AutoFixture.Xunit2;
using FluentAssertions;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using Moq;
using Xunit;

namespace GrpcInit.Application.Tests;

public class ProtoWriterTests
{
    private readonly List<IWriter> _writers = new();
    private readonly ProtoWriter _sut;

    public ProtoWriterTests() => _sut = new ProtoWriter(_writers);

    [Theory]
    [AutoData]
    public async Task Should(ProtoFileTokens tokens)
    {
        var exptected = "Line1\n\n\nLine2\n\n\n";
        var writer1 = new Mock<IWriter>();
        var writer2 = new Mock<IWriter>();
        writer1.Setup(x => x.Write(tokens)).Returns(new[] { "Line1" });
        writer2.Setup(x => x.Write(tokens)).Returns(new[] { "Line2" });
        _writers.Add(writer1.Object);
        _writers.Add(writer2.Object);

        var result = await _sut.Write(tokens);

        result.Should().Be(exptected);
    }
}
