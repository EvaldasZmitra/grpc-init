namespace GrpcInit.Domain.Service;

public sealed record ProtoServiceMethod(
    string Name,
    string InputType,
    string OutputType
);
