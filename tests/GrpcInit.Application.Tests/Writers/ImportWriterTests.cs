using FluentAssertions;
using GrpcInit.Application.Writers;
using GrpcInit.Application.Writers.Abstractions;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class ImportWriterTests
{
    private readonly IWriter _writer;

    public ImportWriterTests() => _writer = new ImportWriter();

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
                    new string[] { "Protos/my_mport.proto", "google/my_mport2.proto" },
                    Array.Empty<ProtoEnumToken>(),
                    Array.Empty<ProtoMessageToken>(),
                    Array.Empty<ProtoServiceToken>()
                ),
                new [] {
                    "import \"Protos/my_mport.proto\";",
                    "import \"google/my_mport2.proto\";"
                }
            }
        };
}
