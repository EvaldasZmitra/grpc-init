using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;
using Moq;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class MessageWriterTests
{
    private readonly IWriter _writer;
    private readonly Mock<IElementWriter> _elementWriter = new();

    public MessageWriterTests() => _writer = new MessageWriter(_elementWriter.Object);

    [Theory]
    [MemberData(nameof(WritesCorrectOutputData))]
    public void WritesCorrectOutput(
        ProtoFileTokens tokens,
        string expectedName,
        IEnumerable<string> expectedContent
    )
    {
        var lines = _writer.Write(tokens);

        _elementWriter.Verify(x => x.WriteBlock(
            expectedName,
            expectedContent
        ));
    }

    public static IEnumerable<object[]> WritesCorrectOutputData =>
        new List<object[]>
        {
            new object[] {
                new ProtoFileTokens(
                    "",
                    "",
                    Array.Empty<string>(),
                    Array.Empty<ProtoEnumToken>(),
                    new [] {
                        new ProtoMessageToken(
                            "MyMessage",
                            new [] {
                                new ProtoMessageField("Label ", "Name", "Type", 1)
                            }
                        )
                    },
                    Array.Empty<ProtoServiceToken>()
                ),
                "message MyMessage",
                new [] {
                    "Label Type Name = 1;"
                }
            }
        };
}
