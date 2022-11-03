namespace GrpcInit.Domain.Message;

public sealed record ProtoMessageToken(
    string Name,
    IEnumerable<ProtoMessageField> Fields
);
