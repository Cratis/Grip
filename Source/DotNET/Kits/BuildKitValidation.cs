// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Schema;

namespace Cratis.Grip.Kits;

/// <summary>
/// Validates a whole kit, not only its manifest: the manifest against the published schema, then that every file and folder it names is in the kit, every skill folder holds a SKILL.md, and every placeholder is one a host knows how to fill. Every problem is reported with where it is, so a kit author fixes them all in one pass. Paths in files use / and are relative to the kit's root.
/// </summary>
public static class BuildKitValidation
{
    static readonly EvaluationOptions _options = new() { OutputFormat = OutputFormat.List };
    static readonly string[] _gitTemplates = ["branch", "commit"];

    /// <summary>
    /// Validates the files of a kit, keyed by their path relative to the kit root.
    /// </summary>
    /// <param name="files">The files of the kit, keyed by their path relative to the kit root, using / as the separator.</param>
    /// <returns>The result of validating the kit.</returns>
    public static BuildKitValidationResult Validate(IReadOnlyDictionary<string, byte[]> files)
    {
        var manifests = BuildKitManifestText.FileNames.Where(files.ContainsKey).ToArray();
        if (manifests.Length != 1)
        {
            return Refused(manifests.Length == 0
                ? "The kit has no build-kit.yaml or build-kit.json at its root."
                : "The kit has both build-kit.yaml and build-kit.json; keep one.");
        }

        JsonNode document;
        try
        {
            document = BuildKitManifestText.Parse(manifests[0], Encoding.UTF8.GetString(files[manifests[0]]));
        }
        catch (BuildKitManifestUnreadable exception)
        {
            return Refused(exception.Message);
        }

        if (document is not JsonObject manifest)
        {
            return Refused("(document): the manifest must be an object.");
        }

        var version = manifest["kitVersion"] is JsonValue value && value.TryGetValue<string>(out var text) ? text : null;
        if (version is null || !BuildKitSchema.SupportedVersions.Contains(version))
        {
            return Refused($"/kitVersion: must be one of {string.Join(", ", BuildKitSchema.SupportedVersions.Select(v => $"\"{v}\""))}.");
        }

        var problems = SchemaProblems(version, manifest).ToList();
        if (problems.Count > 0)
        {
            return new(problems, null);
        }

        problems.AddRange(FileProblems(manifest, files));
        problems.AddRange(PlaceholderProblems(manifest, files));
        return problems.Count > 0 ? new(problems, null) : new([], BuildKitManifest.From(manifests[0], manifest));
    }

    static BuildKitValidationResult Refused(string problem) => new([problem], null);

    static List<string> SchemaProblems(string version, JsonObject manifest)
    {
        using var json = JsonDocument.Parse(manifest.ToJsonString());
        var results = BuildKitSchema.Schema(version).Evaluate(json.RootElement, _options);
        if (results.IsValid)
        {
            return [];
        }

        var problems = (results.Details ?? [])
            .Where(detail => detail.Errors is { Count: > 0 })
            .SelectMany(detail => detail.Errors!.Values.Select(error => $"{Location(detail.InstanceLocation.ToString())}: {error}"))
            .Distinct()
            .ToList();
        return problems.Count > 0 ? problems : ["(document): the manifest does not match the schema."];
    }

    static IEnumerable<string> FileProblems(JsonObject manifest, IReadOnlyDictionary<string, byte[]> files)
    {
        foreach (var (location, path) in KitFiles(manifest))
        {
            if (!files.ContainsKey(Relative(path)))
            {
                yield return $"{location}: the kit has no file {path}.";
            }
        }

        foreach (var (location, path) in KitFolders(manifest))
        {
            var folder = Relative(path).TrimEnd('/') + "/";
            if (!files.Keys.Any(file => file.StartsWith(folder, StringComparison.Ordinal)))
            {
                yield return $"{location}: the kit has no folder {path}, or it is empty.";
            }
        }

        if (manifest["skills"] is JsonValue skills)
        {
            var folder = Relative((string)skills!).TrimEnd('/') + "/";
            var skillFolders = files.Keys
                .Where(file => file.StartsWith(folder, StringComparison.Ordinal))
                .Select(file => file[folder.Length..].Split('/')[0])
                .Distinct(StringComparer.Ordinal);
            foreach (var skill in skillFolders.Where(skill => !files.ContainsKey($"{folder}{skill}/SKILL.md")))
            {
                yield return $"/skills: the skill {skill} has no SKILL.md.";
            }
        }
    }

    static IEnumerable<string> PlaceholderProblems(JsonObject manifest, IReadOnlyDictionary<string, byte[]> files)
    {
        var texts = new List<(string Location, string Text)>();
        texts.AddRange((manifest["verify"]?.AsArray() ?? []).Select((command, index) => ($"/verify/{index}", (string)command!)));
        texts.AddRange(_gitTemplates
            .Where(name => manifest["git"]?[name] is JsonValue)
            .Select(name => ($"/git/{name}", (string)manifest["git"]![name]!)));

        foreach (var (location, path) in new[] { ("/prompt", manifest["prompt"]), ("/existingCodebase/learn", manifest["existingCodebase"]?["learn"]) })
        {
            if (path is JsonValue && files.TryGetValue(Relative((string)path!), out var content))
            {
                texts.Add((location, Encoding.UTF8.GetString(content)));
            }
        }

        return texts.SelectMany(entry => BuildKitPlaceholders.Unknown(entry.Text)
            .Select(name => $"{entry.Location}: {{{name}}} is not a placeholder a host fills; use one of {string.Join(", ", BuildKitPlaceholders.Known.Keys.Select(k => $"{{{k}}}"))}."));
    }

    static IEnumerable<(string Location, string Path)> KitFiles(JsonObject manifest) =>
        new[] { ("/instructions", manifest["instructions"]), ("/prompt", manifest["prompt"]), ("/existingCodebase/learn", manifest["existingCodebase"]?["learn"]) }
            .Where(entry => entry.Item2 is JsonValue)
            .Select(entry => (entry.Item1, (string)entry.Item2!));

    static IEnumerable<(string Location, string Path)> KitFolders(JsonObject manifest)
    {
        if (manifest["skills"] is JsonValue skills)
        {
            yield return ("/skills", (string)skills!);
        }

        var scaffold = manifest["scaffold"] is JsonValue ? manifest["scaffold"] : manifest["scaffold"]?["path"];
        if (scaffold is JsonValue)
        {
            yield return (manifest["scaffold"] is JsonValue ? "/scaffold" : "/scaffold/path", (string)scaffold!);
        }
    }

    static string Relative(string path) => path.StartsWith("./", StringComparison.Ordinal) ? path[2..] : path;

    static string Location(string pointer) => string.IsNullOrEmpty(pointer) ? "(document)" : pointer;
}
