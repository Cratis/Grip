// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The outcome of an order the builder could not carry out without answers to its questions.
/// </summary>
/// <param name="ContractVersion">The contract version the document is written in.</param>
/// <param name="OrderId">The identifier of the order.</param>
/// <param name="Reason">Why the builder could not carry the order out.</param>
/// <param name="Questions">The questions the builder needs answered.</param>
public record BlockedOutcome(string ContractVersion, Guid OrderId, string Reason, IReadOnlyList<string> Questions)
    : BuildOutcome(ContractVersion, OrderId);
