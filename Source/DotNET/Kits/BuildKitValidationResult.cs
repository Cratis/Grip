// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Kits;

/// <summary>
/// The outcome of validating a kit: every problem found, and the manifest summary when there were none.
/// </summary>
/// <param name="Problems">Every problem found, each with where in the kit it is.</param>
/// <param name="Manifest">The summary of the kit, or null when the kit has problems.</param>
public record BuildKitValidationResult(IReadOnlyList<string> Problems, BuildKitManifest? Manifest)
{
    /// <summary>
    /// Gets whether the kit has no problems.
    /// </summary>
    public bool IsValid => Problems.Count == 0 && Manifest is not null;
}
