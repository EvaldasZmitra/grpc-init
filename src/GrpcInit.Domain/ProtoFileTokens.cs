using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;

namespace GrpcInit.Domain;

public sealed record ProtoFileTokens(
    string Name,
    string Syntax,
    IEnumerable<string> Dependencies,
    IEnumerable<ProtoEnumToken> Enums,
    IEnumerable<ProtoMessageToken> Messages,
    IEnumerable<ProtoServiceToken> Services
);
