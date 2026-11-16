// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// One line of a builder's log, with when it was written and how serious it is.
/// </summary>
/// <param name="Occurred">When the line was written.</param>
/// <param name="Level">How serious the line is.</param>
/// <param name="Text">The line, without line breaks.</param>
public record BuildLogLine(DateTimeOffset Occurred, BuildLogLevel Level, string Text);
