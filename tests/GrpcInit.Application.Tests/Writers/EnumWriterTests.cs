using FluentAssertions;
using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class EnumWriterTests
{
    private readonly IWriter _writer;

    public EnumWriterTests() => _writer = new EnumWriter();

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
                new [] {
                    "enum MyEnum {",
                    "\tA = 1;",
                    "\tB = 2;",
                    "}",
                    "",
                }
            }
        };
}
