// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

namespace Cratis.Grip.Kits;

/// <summary>
/// The placeholders a builder host fills from the build order in a kit's prompt, learn prompt, verify commands and Git templates. An unknown placeholder is a mistake in the kit, so it refuses the kit rather than reaching an agent unfilled.
/// </summary>
public static partial class BuildKitPlaceholders
{
    /// <summary>
    /// The placeholders a host fills, each with a description of its value.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string> Known = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["order"] = "The path of the build order document the host wrote for this build.",
        ["outcome"] = "The path the agent writes its outcome document to.",
        ["slice"] = "The name of the first target slice, as written in the model.",
        ["slices"] = "The names of every target slice, separated by commas.",
        ["sliceId"] = "The identifier of the first target slice.",
        ["feature"] = "The name of the feature the slices belong to.",
        ["featurePath"] = "The modules and parent features down to the feature, joined with /.",
        ["application"] = "The name of the application.",
        ["part"] = "What to build: backend or ui.",
        ["attempt"] = "The attempt number, starting at 1.",
        ["conventions"] = "The project path of the recorded conventions, or empty when the kit records none."
    };

    // A placeholder is a name in single braces; JSON or code in a prompt uses braces too, so only an identifier counts.
    [GeneratedRegex(@"(?<!\{)\{(?<name>[A-Za-z][A-Za-z0-9]*)\}(?!\})", RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 1000)]
    private static partial Regex Placeholder { get; }

    /// <summary>
    /// Gets the names in single braces in the text that are not a known placeholder.
    /// </summary>
    /// <param name="text">The text to read.</param>
    /// <returns>The names that are not placeholders.</returns>
    public static IEnumerable<string> Unknown(string text) =>
        Placeholder.Matches(text)
            .Select(match => match.Groups["name"].Value)
            .Where(name => !Known.ContainsKey(name))
            .Distinct(StringComparer.Ordinal);
}
