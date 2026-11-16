// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Cratis.Grip.Contract;

/// <summary>
/// Writes an order as the JSON a builder receives, and checks it against the order schema of its version first, so Studio never hands a builder a document the builder could refuse.
/// </summary>
public static class BuildOrderWriter
{
    /// <summary>
    /// Writes the order as JSON, or says why it does not match the schema of its contract version.
    /// </summary>
    /// <param name="order">The order to write.</param>
    /// <returns>The order as JSON, or why it was not written.</returns>
    public static Result<string, BuildOrderInvalid> Write(BuildOrder order)
    {
        if (!BuildContract.SupportedVersions.Contains(order.ContractVersion))
        {
            return Result<string, BuildOrderInvalid>.Failed(new BuildOrderInvalid([$"Contract version '{order.ContractVersion}' is not one Studio speaks."]));
        }

        var element = JsonSerializer.SerializeToElement(order, BuildContract.SerializerOptions);
        var problems = BuildContractValidation.Problems(BuildContractSchemas.Order, order.ContractVersion, element);
        return problems.Count > 0
            ? Result<string, BuildOrderInvalid>.Failed(new BuildOrderInvalid(problems))
            : element.GetRawText();
    }
}
