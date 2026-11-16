// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Json.Schema;

namespace Cratis.Grip.Kits;

/// <summary>
/// The published JSON Schema of the build kit manifest, embedded as build-kit.&lt;version&gt;.schema.json. A released version is never changed; a new one is a new file beside it.
/// </summary>
public static class BuildKitSchema
{
    /// <summary>
    /// Version 1 of the kit manifest format.
    /// </summary>
    public const string Version1 = "1";

    /// <summary>
    /// The name of the document the schema describes.
    /// </summary>
    public const string Document = "build-kit";

    /// <summary>
    /// The manifest format versions this library validates.
    /// </summary>
    public static readonly IReadOnlyList<string> SupportedVersions = [Version1];

    const string ResourcePrefix = "Cratis.Grip.Kits.";

    /// <summary>
    /// Lazy, because building a schema registers its $id globally and a second registration of the same id throws.
    /// </summary>
    static readonly Lazy<JsonSchema> _version1 = new(() => JsonSchema.FromText(Text(Version1) ?? throw new UnknownBuildKitSchema(Version1)));

    /// <summary>
    /// Gets the text of the published schema for a manifest format version, or null when there is none.
    /// </summary>
    /// <param name="version">The manifest format version.</param>
    /// <returns>The schema text, or null when there is none.</returns>
    public static string? Text(string version)
    {
        if (!SupportedVersions.Contains(version))
        {
            return null;
        }

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{ResourcePrefix}{Document}.{version}.schema.json");
        if (stream is null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Gets the compiled schema for a manifest format version.
    /// </summary>
    /// <param name="version">The manifest format version.</param>
    /// <returns>The compiled schema.</returns>
    /// <exception cref="UnknownBuildKitSchema">There is no schema for the version.</exception>
    public static JsonSchema Schema(string version) =>
        version == Version1 ? _version1.Value : throw new UnknownBuildKitSchema(version);
}
