using GrpcInit.Application.Writers.Abstractions;

namespace GrpcInit.Application.Writers;

public class ElementWriter : IElementWriter
{
    public IEnumerable<string> WriteBlock(
        string name,
        IEnumerable<string> contents
    )
    {
        var result = new List<string>()
        {
            $"{name} {{"
        };

        foreach (var line in contents)
        {
            result.Add($"\t{line}");
        }

        result.Add("}");
        result.Add("");

        return result;
    }
}
