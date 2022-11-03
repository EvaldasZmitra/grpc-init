namespace GrpcInit.Domain;

public sealed class ProtoFile
{
    public string FullName { get; }
    public string Text { get; }

    public ProtoFile(string file, string text)
    {
        FullName = file;
        Text = text;
    }
}
