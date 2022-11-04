using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;
using Moq;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class ServiceWriterTests
{
    private readonly IWriter _writer;
    private readonly Mock<IElementWriter> _elementWriter = new();

    public ServiceWriterTests() => _writer = new ServiceWriter(_elementWriter.Object);

    [Theory]
    [MemberData(nameof(WritesCorrectOutputData))]
    public void WritesCorrectOutput(
        ProtoFileTokens tokens,
        string expectedName,
        IEnumerable<string> expectedContent
    )
    {
        var lines = _writer.Write(tokens);

        _elementWriter.Verify(x => x.WriteBlock(expectedName, expectedContent));
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
                "service MyService",
                new [] {
                    "rpc MyMethod(MyInput) returns (MyReturn);"
                }
            }
        };
}
