// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Cratis.Grip.Contract;

/// <summary>
/// What a builder answers an order with: exactly one of four kinds, named by "kind" on the wire. Studio reads an outcome only through BuildOutcomeReader, which validates it against build-outcome.&lt;version&gt;.schema.json first.
/// </summary>
/// <param name="ContractVersion">The contract version the document is written in.</param>
/// <param name="OrderId">The identifier of the order.</param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(BuiltOutcome), "built")]
[JsonDerivedType(typeof(BlockedOutcome), "blocked")]
[JsonDerivedType(typeof(FailedOutcome), "failed")]
[JsonDerivedType(typeof(NothingToDoOutcome), "nothingToDo")]
public abstract record BuildOutcome(string ContractVersion, Guid OrderId)
{
    /// <summary>
    /// From version 2: the tokens an agent builder used for the attempt.
    /// </summary>
    public BuildUsage? Usage { get; init; }
}
