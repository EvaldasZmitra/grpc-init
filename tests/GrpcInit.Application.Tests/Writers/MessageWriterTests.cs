using FluentAssertions;
using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class MessageWriterTests
{
    private readonly IWriter _writer;

    public MessageWriterTests() => _writer = new MessageWriter();

    [Theory]
    [MemberData(nameof(WritesCorrectOutputData))]
    public void WritesCorrectOutput(
        ProtoFileTokens tokens,
        IEnumerable<string> expected
    )
    {
        var lines = _writer.Write(tokens);

        lines.Should().BeEquivalentTo(
            expected,
            options => options.WithStrictOrdering()
        );
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
                new [] {
                    "message MyMessage {",
                    "\tLabel Type Name = 1;",
                    "}",
                    ""
                }
            }
        };
}
