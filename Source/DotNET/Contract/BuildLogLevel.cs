// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// How serious a log line in a progress report is.
/// </summary>
public enum BuildLogLevel
{
    /// <summary>
    /// Something that happened as expected.
    /// </summary>
    Information = 0,

    /// <summary>
    /// Something unexpected that did not stop the build.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Something that went wrong.
    /// </summary>
    Error = 2,
}
