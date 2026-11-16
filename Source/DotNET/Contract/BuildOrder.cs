// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// What Studio hands a builder: what to build, described in Screenplay, with the context the builder needs. The wire shape is build-order.&lt;version&gt;.schema.json; these records are Studio's reading of it and use plain values on purpose, so the document does not change when Studio's own types do (decision 0023). An order never holds a credential.
/// </summary>
/// <param name="ContractVersion">The contract version the document is written in.</param>
/// <param name="OrderId">The identifier of the order.</param>
/// <param name="Issued">When the order was issued.</param>
/// <param name="Deadline">When Studio stops waiting for an outcome.</param>
/// <param name="Part">What to build.</param>
/// <param name="Scope">What the order builds.</param>
/// <param name="Model">The model to build from.</param>
/// <param name="Changes">What changed for each slice since it was last built.</param>
/// <param name="Specifications">The specifications the build has to satisfy, or the results of running them.</param>
/// <param name="Context">The context the builder needs.</param>
public record BuildOrder(
    string ContractVersion,
    Guid OrderId,
    DateTimeOffset Issued,
    DateTimeOffset Deadline,
    BuildPart Part,
    BuildScope Scope,
    BuildModel Model,
    IReadOnlyList<BuildSliceChange> Changes,
    IReadOnlyList<BuildAcceptanceCriterion> Specifications,
    BuildContext Context)
{
    /// <summary>
    /// From version 2, on a 'ui' order: what the backend builder built for each target slice.
    /// </summary>
    public IReadOnlyList<BuildBackend>? Backend { get; init; }
}
