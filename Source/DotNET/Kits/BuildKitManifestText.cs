// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Cratis.Grip.Kits;

/// <summary>
/// Reads a kit manifest written as build-kit.yaml or build-kit.json into the JSON document the schema is evaluated against. YAML is read as YAML 1.2's JSON-compatible core: an unquoted true, false, null or number is that value, anything quoted or otherwise is a string. A manifest that cannot be read is refused with where and why, never read in part.
/// </summary>
public static class BuildKitManifestText
{
    /// <summary>
    /// The file name of a manifest written in YAML.
    /// </summary>
    public const string YamlFile = "build-kit.yaml";

    /// <summary>
    /// The file name of a manifest written in JSON.
    /// </summary>
    public const string JsonFile = "build-kit.json";

    /// <summary>
    /// The file names a kit manifest may have, at the kit root.
    /// </summary>
    public static readonly IReadOnlyList<string> FileNames = [YamlFile, JsonFile];

    /// <summary>
    /// Parses manifest text, YAML or JSON as the file name says, into the one JSON document the schema is evaluated against.
    /// </summary>
    /// <param name="fileName">The manifest file name, which says whether the text is YAML or JSON.</param>
    /// <param name="text">The text to read.</param>
    /// <returns>The manifest as a JSON document.</returns>
    /// <exception cref="BuildKitManifestUnreadable">The text is not valid YAML or JSON, is empty, or holds more than one YAML document.</exception>
    public static JsonNode Parse(string fileName, string text)
    {
        if (fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                return JsonNode.Parse(text) ?? throw new BuildKitManifestUnreadable("the manifest is empty.");
            }
            catch (JsonException exception)
            {
                throw new BuildKitManifestUnreadable($"it is not valid JSON: {exception.Message}");
            }
        }

        var stream = new YamlStream();
        try
        {
            stream.Load(new StringReader(text));
        }
        catch (YamlException exception)
        {
            throw new BuildKitManifestUnreadable($"it is not valid YAML at line {exception.Start.Line}: {exception.Message}");
        }

        if (stream.Documents.Count != 1)
        {
            throw new BuildKitManifestUnreadable("it must hold exactly one YAML document.");
        }

        return Convert(stream.Documents[0].RootNode) ?? throw new BuildKitManifestUnreadable("the manifest is empty.");
    }

    static JsonNode? Convert(YamlNode node) => node switch
    {
        YamlMappingNode mapping => Mapping(mapping),
        YamlSequenceNode sequence => new JsonArray([.. sequence.Children.Select(Convert)]),
        YamlScalarNode scalar => Scalar(scalar),
        _ => throw new BuildKitManifestUnreadable($"line {node.Start.Line} holds an alias or a node kind a manifest does not use.")
    };

    static JsonObject Mapping(YamlMappingNode mapping)
    {
        var result = new JsonObject();
        foreach (var (key, value) in mapping.Children)
        {
            if (key is not YamlScalarNode { Value: { } name })
            {
                throw new BuildKitManifestUnreadable($"line {key.Start.Line} has a key that is not plain text.");
            }

            if (result.ContainsKey(name))
            {
                throw new BuildKitManifestUnreadable($"line {key.Start.Line} repeats the key '{name}'.");
            }

            result[name] = Convert(value);
        }

        return result;
    }

    static JsonValue? Scalar(YamlScalarNode scalar)
    {
        var value = scalar.Value ?? string.Empty;
        if (scalar.Style is not ScalarStyle.Plain)
        {
            return JsonValue.Create(value);
        }

        return value switch
        {
            "" or "null" or "~" => null,
            "true" => JsonValue.Create(true),
            "false" => JsonValue.Create(false),
            _ when long.TryParse(value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var whole) => JsonValue.Create(whole),
            _ when double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var number) && value.All(c => char.IsDigit(c) || c is '.' or '-' or '+' or 'e' or 'E') => JsonValue.Create(number),
            _ => JsonValue.Create(value)
        };
    }
}
