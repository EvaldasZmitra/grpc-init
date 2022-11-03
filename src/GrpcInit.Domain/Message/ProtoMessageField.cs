namespace GrpcInit.Domain.Message;

public sealed record ProtoMessageField(
    string Label,
    string Name,
    string Type,
    int Number
);
