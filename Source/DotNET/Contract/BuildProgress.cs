// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// What a builder reports while it builds the order it holds (version 2 and later): the step it is on, the log lines written since its previous report, and the tokens used so far. The wire shape is build-progress.&lt;version&gt;.schema.json.
/// </summary>
/// <param name="ContractVersion">The contract version the document is written in.</param>
/// <param name="OrderId">The identifier of the order.</param>
/// <param name="Step">The step the builder is on, or null.</param>
/// <param name="Log">The log lines written since the previous report, or null.</param>
/// <param name="Usage">The tokens used so far, or null.</param>
public record BuildProgress(
    string ContractVersion,
    Guid OrderId,
    string? Step = null,
    IReadOnlyList<BuildLogLine>? Log = null,
    BuildUsage? Usage = null);
