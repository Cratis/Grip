// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// A builder says which contract versions it reads; Studio offers an order only in one of them, the newest both speak, and refuses to offer one when they share none.
/// </summary>
public static class BuildContractNegotiation
{
    /// <summary>
    /// Chooses the newest contract version both sides speak, or says that they share none.
    /// </summary>
    /// <param name="accepted">The contract versions the builder reads.</param>
    /// <returns>The newest shared version, or why there is none.</returns>
    public static Result<string, BuildContractNotShared> Choose(IEnumerable<string> accepted)
    {
        var acceptedVersions = accepted.Distinct().ToList();
        if (BuildContract.SupportedVersions.LastOrDefault(acceptedVersions.Contains) is { } shared)
        {
            return shared;
        }

        return Result<string, BuildContractNotShared>.Failed(new BuildContractNotShared(acceptedVersions, BuildContract.SupportedVersions));
    }
}
