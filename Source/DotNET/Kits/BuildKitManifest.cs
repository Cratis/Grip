// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;

namespace Cratis.Grip.Kits;

/// <summary>
/// What Studio tells a builder host about a kit before it downloads it: enough to choose one, never the files themselves.
/// </summary>
/// <param name="Name">The name of the kit.</param>
/// <param name="Version">The version of the kit.</param>
/// <param name="Description">A short description of the kit.</param>
/// <param name="SliceTypes">The slice types the kit builds.</param>
/// <param name="Ui">Whether the kit builds user interfaces.</param>
/// <param name="ContractVersions">The build contract versions the kit works with.</param>
/// <param name="ManifestFile">The name of the manifest file, build-kit.yaml or build-kit.json.</param>
/// <param name="Document">The whole manifest document.</param>
public record BuildKitManifest(
    string Name,
    string Version,
    string Description,
    IReadOnlyList<string> SliceTypes,
    bool Ui,
    IReadOnlyList<string> ContractVersions,
    string ManifestFile,
    JsonObject Document)
{
    /// <summary>
    /// Reads the summary of a kit from its manifest document, which must already be valid.
    /// </summary>
    /// <param name="manifestFile">The name of the manifest file the kit was read from.</param>
    /// <param name="document">The manifest document.</param>
    /// <returns>The summary of the kit.</returns>
    public static BuildKitManifest From(string manifestFile, JsonObject document) => new(
        (string)document["name"]!,
        (string)document["version"]!,
        (string)document["description"]!,
        [.. document["sliceTypes"]!.AsArray().Select(type => (string)type!)],
        document["ui"]?.GetValue<bool>() ?? false,
        document["contract"]?["versions"] is JsonArray versions ? [.. versions.Select(version => (string)version!)] : [BuildKitSchema.Version1],
        manifestFile,
        document);
}
