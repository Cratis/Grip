// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// What changed for a slice since it was last built.
/// </summary>
/// <param name="SliceId">The identifier of the slice.</param>
/// <param name="Kind">How the slice changed.</param>
/// <param name="PreviousName">The name the slice had before it was renamed, when it was.</param>
/// <param name="Declarations">The declarations of the slice in the model.</param>
public record BuildSliceChange(Guid SliceId, BuildSliceChangeKind Kind, string? PreviousName, IReadOnlyList<string> Declarations);
