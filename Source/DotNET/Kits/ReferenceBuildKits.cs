// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Cratis.Grip.Kits;

/// <summary>
/// The reference build kits, registered by name: the folders under Kits/ at the repository root, embedded in the assembly as Cratis.Grip.Kits.Reference/&lt;kit&gt;/&lt;path&gt;. A host installs one with `cratis build init --kit &lt;name&gt;`. A kit that fails validation is never offered; a specification proves every shipped kit validates.
/// </summary>
public static class ReferenceBuildKits
{
    /// <summary>
    /// The prefix of the embedded resource names of the reference kits.
    /// </summary>
    public const string ResourcePrefix = "Cratis.Grip.Kits.Reference/";

    static readonly Lazy<IReadOnlyDictionary<string, IReadOnlyDictionary<string, byte[]>>> _files = new(Read);
    static readonly Lazy<IReadOnlyList<BuildKitManifest>> _valid = new(() =>
        [.. _files.Value.Select(kit => BuildKitValidation.Validate(kit.Value).Manifest)
            .OfType<BuildKitManifest>()
            .Where(manifest => _files.Value.ContainsKey(manifest.Name))
            .OrderBy(manifest => manifest.Name, StringComparer.Ordinal)]);

    /// <summary>
    /// Gets the files of every reference kit, keyed by kit name and then by path relative to the kit root.
    /// </summary>
    public static IReadOnlyDictionary<string, IReadOnlyDictionary<string, byte[]>> Files => _files.Value;

    /// <summary>
    /// Gets the manifest summary of every reference kit that validates, by name.
    /// </summary>
    public static IReadOnlyList<BuildKitManifest> All => _valid.Value;

    /// <summary>
    /// Gets the manifest summary of a reference kit.
    /// </summary>
    /// <param name="name">The name of the kit.</param>
    /// <returns>The summary, or null when there is no valid kit of that name.</returns>
    public static BuildKitManifest? Named(string name) =>
        All.FirstOrDefault(manifest => string.Equals(manifest.Name, name, StringComparison.Ordinal));

    /// <summary>
    /// Gets the files of a reference kit.
    /// </summary>
    /// <param name="name">The name of the kit.</param>
    /// <returns>The files, keyed by path relative to the kit root, or null when there is no valid kit of that name.</returns>
    public static IReadOnlyDictionary<string, byte[]>? FilesOf(string name) =>
        Named(name) is not null && Files.TryGetValue(name, out var files) ? files : null;

    static Dictionary<string, IReadOnlyDictionary<string, byte[]>> Read()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var kits = new Dictionary<string, Dictionary<string, byte[]>>(StringComparer.Ordinal);
        foreach (var resource in assembly.GetManifestResourceNames().Where(name => name.StartsWith(ResourcePrefix, StringComparison.Ordinal)))
        {
            var path = resource[ResourcePrefix.Length..].Replace('\\', '/');
            var separator = path.IndexOf('/', StringComparison.Ordinal);
            if (separator <= 0)
            {
                continue;
            }

            using var stream = assembly.GetManifestResourceStream(resource)!;
            using var buffer = new MemoryStream();
            stream.CopyTo(buffer);

            var kit = path[..separator];
            if (!kits.TryGetValue(kit, out var files))
            {
                kits[kit] = files = [with(StringComparer.Ordinal)];
            }

            files[path[(separator + 1)..]] = buffer.ToArray();
        }

        return kits.ToDictionary(kit => kit.Key, kit => (IReadOnlyDictionary<string, byte[]>)kit.Value, StringComparer.Ordinal);
    }
}
