// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// At least one of Commit, Branch and PullRequest is given.
/// </summary>
/// <param name="ContractVersion">The contract version the document is written in.</param>
/// <param name="OrderId">The identifier of the order.</param>
/// <param name="Commit">The commit the work is in, or null.</param>
/// <param name="Branch">The branch the work is on, or null.</param>
/// <param name="PullRequest">The pull request the work is in, or null.</param>
/// <param name="Files">The files the builder created or changed.</param>
/// <param name="Specifications">The specifications the build has to satisfy, or the results of running them.</param>
/// <param name="Assumptions">The assumptions the builder made.</param>
public record BuiltOutcome(
    string ContractVersion,
    Guid OrderId,
    string? Commit,
    string? Branch,
    string? PullRequest,
    IReadOnlyList<string> Files,
    IReadOnlyList<BuildSpecificationResult> Specifications,
    IReadOnlyList<string> Assumptions) : BuildOutcome(ContractVersion, OrderId)
{
    /// <summary>
    /// From version 2: the commands and queries a backend build exposes.
    /// </summary>
    public IReadOnlyList<BuildApiOperation>? Api { get; init; }
}
