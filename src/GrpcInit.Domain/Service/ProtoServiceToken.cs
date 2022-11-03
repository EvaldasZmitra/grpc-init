namespace GrpcInit.Domain.Service;

public sealed record ProtoServiceToken(
    string Name,
    IEnumerable<ProtoServiceMethod> Methods
);
