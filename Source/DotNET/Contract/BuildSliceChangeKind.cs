// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// How a slice changed since it was last built.
/// </summary>
public enum BuildSliceChangeKind
{
    /// <summary>
    /// The slice is new.
    /// </summary>
    Added = 0,

    /// <summary>
    /// The slice changed.
    /// </summary>
    Changed = 1,

    /// <summary>
    /// The slice has a new name.
    /// </summary>
    Renamed = 2,

    /// <summary>
    /// The slice moved to another feature.
    /// </summary>
    Moved = 3,

    /// <summary>
    /// The slice was deleted from the model.
    /// </summary>
    Removed = 4,
}
