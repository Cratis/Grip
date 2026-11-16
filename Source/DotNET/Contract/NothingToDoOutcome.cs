// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The outcome of an order that needed no change.
/// </summary>
/// <param name="ContractVersion">The contract version the document is written in.</param>
/// <param name="OrderId">The identifier of the order.</param>
public record NothingToDoOutcome(string ContractVersion, Guid OrderId) : BuildOutcome(ContractVersion, OrderId);
