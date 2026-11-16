// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// What the builder is told beside the model: conventions, hints, feedback and what went wrong in the previous attempt.
/// </summary>
/// <param name="Conventions">The conventions the builder follows.</param>
/// <param name="Hints">Hints for the builder.</param>
/// <param name="Feedback">Feedback on slices.</param>
/// <param name="Attempt">The attempt number, starting at 1.</param>
/// <param name="PreviousFailure">What went wrong in the previous attempt, or null.</param>
public record BuildContext(
    IReadOnlyList<string> Conventions,
    IReadOnlyList<string> Hints,
    IReadOnlyList<BuildFeedback> Feedback,
    int Attempt,
    string? PreviousFailure);
