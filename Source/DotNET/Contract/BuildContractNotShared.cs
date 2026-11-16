// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// Why no contract version could be chosen: the versions the builder accepts and the ones this library speaks.
/// </summary>
/// <param name="Accepted">The contract versions the builder said it reads.</param>
/// <param name="Supported">The contract versions this library speaks.</param>
public record BuildContractNotShared(IReadOnlyList<string> Accepted, IReadOnlyList<string> Supported);
