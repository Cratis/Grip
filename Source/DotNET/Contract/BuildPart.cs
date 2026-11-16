// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// What an order asks a builder to do. The removal part exists from contract version 3.
/// </summary>
public enum BuildPart
{
    /// <summary>
    /// Build the backend of the slices.
    /// </summary>
    Backend = 0,

    /// <summary>
    /// Build the user interface of the slices, on top of the backend that was built.
    /// </summary>
    Ui = 1,

    /// <summary>
    /// Remove what was built for slices deleted from the model.
    /// </summary>
    Removal = 2,
}
