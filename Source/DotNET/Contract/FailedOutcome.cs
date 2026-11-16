// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The outcome of an order the builder could not carry out, and whether trying again could help.
/// </summary>
/// <param name="ContractVersion">The contract version the document is written in.</param>
/// <param name="OrderId">The identifier of the order.</param>
/// <param name="Error">What went wrong.</param>
/// <param name="Retryable">Whether trying again could help.</param>
public record FailedOutcome(string ContractVersion, Guid OrderId, string Error, bool Retryable)
    : BuildOutcome(ContractVersion, OrderId);
