// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// Feedback on a slice from the people it was built for.
/// </summary>
/// <param name="SliceId">The identifier of the slice.</param>
/// <param name="Text">The feedback.</param>
/// <param name="Occurred">When the feedback was given.</param>
public record BuildFeedback(Guid SliceId, string Text, DateTimeOffset Occurred);
