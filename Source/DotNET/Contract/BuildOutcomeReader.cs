// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The only way Studio reads what a builder sent back: the text must be JSON, name a contract version Studio speaks, answer the expected order and match that version's outcome schema. Anything else is refused with the reasons, and nothing is read from it.
/// </summary>
public static class BuildOutcomeReader
{
    /// <summary>
    /// Reads an outcome, optionally checking that it answers the expected order.
    /// </summary>
    /// <param name="text">The text to read.</param>
    /// <param name="expectedOrderId">The order the document has to answer, or null to accept any.</param>
    /// <returns>The files, or the reason they could not be read.</returns>
    public static Result<BuildOutcome, BuildOutcomeRefused> Read(string text, Guid? expectedOrderId = null)
    {
        var read = BuildContractReading.Read<BuildOutcome>(BuildContractSchemas.Outcome, "outcome", text, document => document.OrderId, expectedOrderId);
        if (read.TryGetResult(out var outcome)) return outcome;
        read.TryGetError(out var reasons);
        return Result<BuildOutcome, BuildOutcomeRefused>.Failed(new BuildOutcomeRefused(reasons ?? []));
    }
}
