// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// What the backend builder built for a slice: where it is and the API it exposes.
/// </summary>
/// <param name="SliceId">The identifier of the slice.</param>
/// <param name="Commit">The commit the work is in, or null.</param>
/// <param name="Branch">The branch the work is on, or null.</param>
/// <param name="PullRequest">The pull request the work is in, or null.</param>
/// <param name="Files">The files of the kit, keyed by their path relative to the kit root.</param>
/// <param name="Api">The commands and queries the build exposes.</param>
public record BuildBackend(Guid SliceId, string? Commit, string? Branch, string? PullRequest, IReadOnlyList<string> Files, IReadOnlyList<BuildApiOperation> Api);
