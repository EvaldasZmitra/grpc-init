using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;
using Moq;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class EnumWriterTests
{
    private readonly IWriter _writer;
    private readonly Mock<IElementWriter> _elementWriter = new();

    public EnumWriterTests() => _writer = new EnumWriter(_elementWriter.Object);

    [Theory]
    [MemberData(nameof(WritesCorrectOutputData))]
    public void WritesCorrectOutput(
        ProtoFileTokens tokens,
        string expectedName,
        IEnumerable<string> expectedContents
    )
    {
        var lines = _writer.Write(tokens);

        _elementWriter.Verify(x => x.WriteBlock(expectedName, expectedContents));
    }

    public static IEnumerable<object[]> WritesCorrectOutputData =>
        new List<object[]>
        {
            new object[] {
                new ProtoFileTokens(
                    "",
                    "",
                    Array.Empty<string>(),
                    new [] {
                        new ProtoEnumToken(
                            "MyEnum",
                            new [] {
                                new ProtoEnumValue("A", "1"),
                                new ProtoEnumValue("B", "2"),
                            }
                        )
                    },
                    Array.Empty<ProtoMessageToken>(),
                    Array.Empty<ProtoServiceToken>()
                ),
                "enum MyEnum",
                new List<string>() {
                    "A = 1;",
                    "B = 2;"
                }
            }
        };
}
