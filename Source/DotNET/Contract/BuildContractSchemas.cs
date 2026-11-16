// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Reflection;
using Json.Schema;

namespace Cratis.Grip.Contract;

/// <summary>
/// The published JSON Schemas, embedded in the assembly as &lt;document&gt;.&lt;version&gt;.schema.json. Build progress exists from version 2. A released schema is never changed; a new version is a new file beside it.
/// </summary>
public static class BuildContractSchemas
{
    /// <summary>
    /// The name of the build order document.
    /// </summary>
    public const string Order = "build-order";

    /// <summary>
    /// The name of the build outcome document.
    /// </summary>
    public const string Outcome = "build-outcome";

    /// <summary>
    /// The name of the build progress document.
    /// </summary>
    public const string Progress = "build-progress";

    const string ResourcePrefix = "Cratis.Grip.Contract.";

    /// <summary>
    /// Lazy, because building a schema registers its $id globally and a second registration of the same id throws: two callers racing to build it must share one build.
    /// </summary>
    static readonly ConcurrentDictionary<string, Lazy<JsonSchema>> _schemas = new();

    /// <summary>
    /// Gets the file name of the schema for a document in a contract version.
    /// </summary>
    /// <param name="document">The name of the document kind: build-order, build-outcome or build-progress.</param>
    /// <param name="version">The contract version.</param>
    /// <returns>The file name of the schema.</returns>
    public static string FileName(string document, string version) => $"{document}.{version}.schema.json";

    /// <summary>
    /// Gets the text of the schema for a document in a contract version, or null when that version has none.
    /// </summary>
    /// <param name="document">The name of the document kind: build-order, build-outcome or build-progress.</param>
    /// <param name="version">The contract version.</param>
    /// <returns>The schema text, or null when there is none.</returns>
    public static string? Text(string document, string version)
    {
        if (document is not (Order or Outcome or Progress) || !BuildContract.SupportedVersions.Contains(version))
        {
            return null;
        }

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourcePrefix + FileName(document, version));
        if (stream is null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Gets the compiled schema for a document in a contract version.
    /// </summary>
    /// <param name="document">The name of the document kind: build-order, build-outcome or build-progress.</param>
    /// <param name="version">The contract version.</param>
    /// <returns>The compiled schema.</returns>
    public static JsonSchema Schema(string document, string version) =>
        _schemas.GetOrAdd(FileName(document, version), static (_, key) => new(() => Build(key.document, key.version)), (document, version)).Value;

    static JsonSchema Build(string document, string version) =>
        JsonSchema.FromText(Text(document, version) ?? throw new UnknownBuildContractSchema(document, version));
}
