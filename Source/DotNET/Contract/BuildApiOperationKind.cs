// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// Whether an operation of the API a backend build exposes changes state or only reads it.
/// </summary>
public enum BuildApiOperationKind
{
    /// <summary>
    /// Changes state.
    /// </summary>
    Command = 0,

    /// <summary>
    /// Only reads state.
    /// </summary>
    Query = 1,
}
