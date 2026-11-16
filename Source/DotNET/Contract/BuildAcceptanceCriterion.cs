// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// A specification of a slice a build has to satisfy, in given, when and then form.
/// </summary>
/// <param name="SliceId">The identifier of the slice.</param>
/// <param name="Id">The identifier of the specification.</param>
/// <param name="Name">The name of the specification.</param>
/// <param name="Given">What is given before the behavior.</param>
/// <param name="When">The behavior under specification, or null when there is none.</param>
/// <param name="Then">What is expected to hold afterwards.</param>
public record BuildAcceptanceCriterion(
    Guid SliceId,
    Guid Id,
    string Name,
    IReadOnlyList<string> Given,
    string? When,
    IReadOnlyList<string> Then);
