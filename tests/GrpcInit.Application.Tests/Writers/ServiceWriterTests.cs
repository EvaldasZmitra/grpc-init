using FluentAssertions;
using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class ServiceWriterTests
{
    private readonly IWriter _writer;

    public ServiceWriterTests() => _writer = new ServiceWriter();

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
                    Array.Empty<ProtoMessageToken>(),
                    new [] {
                        new ProtoServiceToken(
                            "MyService",
                            new [] {
                                new ProtoServiceMethod("MyMethod", "MyInput", "MyReturn")
                            }
                        )
                    }
                ),
                new [] {
                    "service MyService {",
                    "\trpc MyMethod(MyInput) returns (MyReturn);",
                    "}",
                    ""
                }
            }
        };
}
