using System.Globalization;
using GrpcInit.Application.Abstractions;
using GrpcInit.Domain;

namespace GrpcInit.Application;

public class ProtoFileFilter : IProtoFileFilter
{
    public IEnumerable<ProtoFileTokens> Filter(
        IEnumerable<ProtoFileTokens> protos
    ) => protos.Where(
        x => !x.Name.StartsWith("google", true, CultureInfo.InvariantCulture)
    );
}
