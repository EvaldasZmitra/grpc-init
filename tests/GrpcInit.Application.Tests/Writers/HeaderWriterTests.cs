using FluentAssertions;
using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class HeaderWriterTests
{
    private readonly IWriter _writer;

    public HeaderWriterTests() => _writer = new HeaderWriter();

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
                    "proto3",
                    Array.Empty<string>(),
                    Array.Empty<ProtoEnumToken>(),
                    Array.Empty<ProtoMessageToken>(),
                    Array.Empty<ProtoServiceToken>()
                ),
                new [] {
                    "syntax = \"proto3\";\n"
                }
            },
            new object[] {
                new ProtoFileTokens(
                    "",
                    "proto2",
                    Array.Empty<string>(),
                    Array.Empty<ProtoEnumToken>(),
                    Array.Empty<ProtoMessageToken>(),
                    Array.Empty<ProtoServiceToken>()
                ),
                new [] {
                    "syntax = \"proto2\";\n"
                }
            }
        };
}
