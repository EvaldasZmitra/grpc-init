namespace GrpcInit.Domain.Enum;

public sealed record ProtoEnumToken(
    string Name,
    IEnumerable<ProtoEnumValue> Values
);
