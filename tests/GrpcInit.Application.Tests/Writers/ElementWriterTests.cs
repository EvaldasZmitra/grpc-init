using FluentAssertions;
using GrpcInit.Application.Writers;
using Xunit;

namespace GrpcInit.Application.Tests.Writers;

public class ElementWriterTests
{
    private readonly ElementWriter _sut = new();

    [Theory]
    [MemberData(nameof(WritesBlockData))]
    public void WriteBlock(
        string name,
        IEnumerable<string> content,
        IEnumerable<string> expected
    )
    {
        var block = _sut.WriteBlock(name, content);

        block.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> WritesBlockData =>
        new List<object[]>
        {
            new object[] {
                "Block",
                new [] { "1", "2", "3" },
                new [] { "Block {", "\t1", "\t2", "\t3", "}", "" }
            }
        };
}
