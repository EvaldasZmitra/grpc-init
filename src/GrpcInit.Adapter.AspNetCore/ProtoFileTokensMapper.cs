using System.Globalization;
using Google.Protobuf.Reflection;
using GrpcInit.Domain;
using GrpcInit.Domain.Enum;
using GrpcInit.Domain.Message;
using GrpcInit.Domain.Service;

namespace GrpcInit.Adapter.AspNetCore;

public static class ProtoFileTokensMapper
{
    public static ProtoFileTokens Map(FileDescriptorProto model) => new(
        model.Name,
        model.Syntax,
        model.Dependency,
        model.EnumType.Select(x => Map(x)),
        model.MessageType.Select(x => Map(x)),
        model.Service.Select(x => Map(x))
    );

    private static ProtoEnumToken Map(EnumDescriptorProto model) => new(
        model.Name,
        model.Value.Select(x => Map(x))
    );

    private static ProtoEnumValue Map(EnumValueDescriptorProto model) => new(
        model.Name,
        model.Number.ToString(CultureInfo.InvariantCulture)
    );

    private static ProtoMessageToken Map(DescriptorProto model) => new(
            model.Name,
            model.Field.Select(x => Map(x))
        );

    private static ProtoMessageField Map(FieldDescriptorProto x) => new(
        x.HasLabel ? $"{x.Label.ToString().ToLower(CultureInfo.InvariantCulture)} " : "",
        x.Name,
        x.HasTypeName ? x.TypeName[1..] : x.Type.ToString().ToLower(CultureInfo.InvariantCulture),
        x.Number
    );

    private static ProtoServiceToken Map(ServiceDescriptorProto model) => new(
        model.Name,
        model.Method.Select(x => Map(x))
    );

    private static ProtoServiceMethod Map(MethodDescriptorProto x) => new(
        x.Name,
        x.InputType[1..],
        x.OutputType[1..]
    );
}
