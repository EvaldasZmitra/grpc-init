using GrpcInit.Domain;
using GrpcInit.Output.Abstractions;

namespace GrpcInit.Output.File;

public sealed class FileWriter : IOutput
{
    private readonly DirectoryInfo _directory;

    public FileWriter(DirectoryInfo directory) => _directory = directory;

    public async Task Write(IEnumerable<ProtoFile> files) => await Task.WhenAll(
            files.Select(
                async (x) => await Task.Run(() => Write(x))
            )
        );

    public void Write(ProtoFile protoFile)
    {
        var fullPath = Path.Combine(_directory.FullName, protoFile.FullName);
        var file = new FileInfo(fullPath);
        if (file.Directory?.Exists == false)
        {
            file.Directory.Create();
        }
        System.IO.File.WriteAllText(file.FullName, protoFile.Text);
    }
}
