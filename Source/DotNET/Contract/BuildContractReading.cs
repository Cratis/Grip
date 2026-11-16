// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Cratis.Grip.Contract;

/// <summary>
/// How Studio reads any document a builder sends: the text must be JSON, name a contract version Studio speaks and that has the document, answer the expected order and match that version's schema. Anything else is refused with the reasons.
/// </summary>
public static class BuildContractReading
{
    /// <summary>
    /// Reads a document of the given kind, or returns the reasons it was refused.
    /// </summary>
    /// <typeparam name="T">The type of the document read.</typeparam>
    /// <param name="document">The name of the document kind, such as build-outcome.</param>
    /// <param name="noun">What the document is called in the reasons given when it is refused.</param>
    /// <param name="text">The text to read.</param>
    /// <param name="orderOf">Gets the order a document answers.</param>
    /// <param name="expectedOrderId">The order the document has to answer, or null to accept any.</param>
    /// <returns>The document, or the reasons it was refused.</returns>
    public static Result<T, IReadOnlyList<string>> Read<T>(string document, string noun, string text, Func<T, Guid> orderOf, Guid? expectedOrderId)
    {
        JsonDocument parsed;
        try
        {
            parsed = JsonDocument.Parse(text);
        }
        catch (JsonException exception)
        {
            return Refuse<T>($"The {noun} is not JSON: {exception.Message}");
        }

        using (parsed)
        {
            var root = parsed.RootElement;
            if (root.ValueKind != JsonValueKind.Object) return Refuse<T>($"The {noun} must be a JSON object.");

            if (!root.TryGetProperty("contractVersion", out var version) || version.ValueKind != JsonValueKind.String)
            {
                return Refuse<T>($"The {noun} does not say which contract version it is written in ('contractVersion').");
            }

            var contractVersion = version.GetString()!;
            if (!BuildContract.SupportedVersions.Contains(contractVersion))
            {
                return Refuse<T>($"Contract version '{contractVersion}' is not one Studio speaks ({string.Join(", ", BuildContract.SupportedVersions)}).");
            }

            if (BuildContractSchemas.Text(document, contractVersion) is null)
            {
                return Refuse<T>($"Contract version '{contractVersion}' has no {noun}.");
            }

            var problems = BuildContractValidation.Problems(document, contractVersion, root);
            if (problems.Count > 0) return Result<T, IReadOnlyList<string>>.Failed(problems);

            var read = root.Deserialize<T>(BuildContract.SerializerOptions)!;
            if (expectedOrderId is { } orderId && orderOf(read) != orderId)
            {
                return Refuse<T>($"The {noun} answers order '{orderOf(read)}', not '{orderId}'.");
            }

            return read;
        }
    }

    static Result<T, IReadOnlyList<string>> Refuse<T>(string reason) => Result<T, IReadOnlyList<string>>.Failed([reason]);
}
