using GrpcInit.Domain;

namespace GrpcInit.Output.Abstractions;

public interface IOutput
{
    Task Write(IEnumerable<ProtoFile> files);
}
