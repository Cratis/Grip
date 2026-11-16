// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The verdict of a specification the builder ran.
/// </summary>
public enum BuildSpecificationVerdict
{
    /// <summary>
    /// The specification passed.
    /// </summary>
    Passed = 0,

    /// <summary>
    /// The specification failed.
    /// </summary>
    Failed = 1,

    /// <summary>
    /// The specification was not run.
    /// </summary>
    NotRun = 2,
}
