// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The only way Studio reads a progress report a builder sent: it must match the progress schema of a version that has one.
/// </summary>
public static class BuildProgressReader
{
    /// <summary>
    /// Reads a progress report, optionally checking that it is for the expected order.
    /// </summary>
    /// <param name="text">The text to read.</param>
    /// <param name="expectedOrderId">The order the document has to answer, or null to accept any.</param>
    /// <returns>The files, or the reason they could not be read.</returns>
    public static Result<BuildProgress, BuildProgressRefused> Read(string text, Guid? expectedOrderId = null)
    {
        var read = BuildContractReading.Read<BuildProgress>(BuildContractSchemas.Progress, "progress report", text, document => document.OrderId, expectedOrderId);
        if (read.TryGetResult(out var progress)) return progress;
        read.TryGetError(out var reasons);
        return Result<BuildProgress, BuildProgressRefused>.Failed(new BuildProgressRefused(reasons ?? []));
    }
}
