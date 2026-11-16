// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Cratis.Grip.Contract.for_BuildOrder;

// An order never carries a credential (decision 0023): no member of the order types, and no property in the published order
// schema, is named for one, and every object in the schema refuses members it does not declare, so none can be added.
public partial class when_looking_for_a_member_that_could_carry_a_credential : Specification
{
    List<string> _typeMembers = [];
    List<string> _schemaProperties = [];
    List<string> _openObjects = [];

    void Because()
    {
        Collect(typeof(BuildOrder), []);
        using var schema = JsonDocument.Parse(BuildContractSchemas.Text(BuildContractSchemas.Order, BuildContract.Version1)!);
        Walk(schema.RootElement, "#");
    }

    [Fact] void should_find_the_order_members() => _typeMembers.ShouldContain(nameof(BuildOrder.Context));
    [Fact] void should_have_no_type_member_named_for_a_credential() => _typeMembers.Where(name => Credential.IsMatch(name)).ShouldBeEmpty();
    [Fact] void should_have_no_schema_property_named_for_a_credential() => _schemaProperties.Where(name => Credential.IsMatch(name)).ShouldBeEmpty();
    [Fact] void should_close_every_object_but_the_canonical_model() => _openObjects.ShouldContainOnly(["#/$defs/model/properties/canonical"]);

    [GeneratedRegex("token|secret|password|credential|key|auth|cookie|bearer|ssh|pat$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex Credential { get; }

    void Collect(Type type, HashSet<Type> seen)
    {
        if (!seen.Add(type) || type.Namespace != typeof(BuildOrder).Namespace) return;
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            _typeMembers.Add(property.Name);
            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            Collect(propertyType.IsGenericType ? propertyType.GetGenericArguments()[0] : propertyType, seen);
        }
    }

    void Walk(JsonElement element, string path)
    {
        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray()) Walk(item, path);
            return;
        }

        if (element.ValueKind != JsonValueKind.Object) return;
        if (IsObjectSchema(element) && !(element.TryGetProperty("additionalProperties", out var additional) && additional.ValueKind == JsonValueKind.False))
        {
            _openObjects.Add(path);
        }

        if (element.TryGetProperty("properties", out var properties))
        {
            _schemaProperties.AddRange(properties.EnumerateObject().Select(property => property.Name));
        }

        foreach (var member in element.EnumerateObject()) Walk(member.Value, $"{path}/{member.Name}");
    }

    static bool IsObjectSchema(JsonElement element) =>
        element.TryGetProperty("type", out var type) && type.ValueKind switch
        {
            JsonValueKind.String => type.GetString() == "object",
            JsonValueKind.Array => type.EnumerateArray().Any(each => each.GetString() == "object"),
            _ => false,
        };
}
#endif
