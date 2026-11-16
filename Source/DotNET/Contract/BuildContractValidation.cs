// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Json.Schema;

namespace Cratis.Grip.Contract;

/// <summary>
/// Evaluates a document against a contract schema and says, for each failure, where in the document it is and what is wrong.
/// </summary>
public static class BuildContractValidation
{
    static readonly EvaluationOptions _options = new() { OutputFormat = OutputFormat.List };

    /// <summary>
    /// Gets the problems of a document against its schema; empty when the document is valid.
    /// </summary>
    /// <param name="document">The name of the document kind, such as build-order.</param>
    /// <param name="version">The contract version the document is written in.</param>
    /// <param name="instance">The document to evaluate.</param>
    /// <returns>The problems found; empty when the document is valid.</returns>
    public static IReadOnlyList<string> Problems(string document, string version, JsonElement instance)
    {
        var results = BuildContractSchemas.Schema(document, version).Evaluate(instance, _options);
        if (results.IsValid)
        {
            return [];
        }

        var problems = (results.Details ?? [])
            .Where(detail => detail.Errors is { Count: > 0 })
            .SelectMany(detail => detail.Errors!.Values.Select(error => $"{Location(detail.InstanceLocation.ToString())}: {error}"))
            .Distinct()
            .ToList();
        return problems.Count > 0 ? problems : ["The document does not match the schema."];
    }

    static string Location(string pointer) => string.IsNullOrEmpty(pointer) ? "(document)" : pointer;
}
