// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cratis.Grip.Contract;

/// <summary>
/// The contract versions Studio speaks, newest last, and the one JSON shape both documents are written in: camelCase members, camelCase enum names, and no member written for an absent value.
/// </summary>
public static class BuildContract
{
    /// <summary>
    /// The first contract version: orders and outcomes.
    /// </summary>
    public const string Version1 = "1";

    /// <summary>
    /// Adds build progress, token usage on outcomes, the API a backend build exposes, and the backend handed to a UI order.
    /// </summary>
    public const string Version2 = "2";

    /// <summary>
    /// Adds the 'removal' part: an order to remove what was built for slices deleted from the model, with no slices to build.
    /// </summary>
    public const string Version3 = "3";

    /// <summary>
    /// The contract versions this library speaks, oldest first.
    /// </summary>
    public static readonly IReadOnlyList<string> SupportedVersions = [Version1, Version2, Version3];

    /// <summary>
    /// The options every contract document is written and read with.
    /// </summary>
    public static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        AllowOutOfOrderMetadataProperties = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) },
    };

    /// <summary>
    /// Whether a document in this version may carry what version 2 added; every later version keeps it.
    /// </summary>
    /// <param name="version">The contract version of the document.</param>
    /// <returns>True when the version is 2 or later; false otherwise.</returns>
    public static bool Speaks2(string version) => string.Equals(version, Version2, StringComparison.Ordinal) || Speaks3(version);

    /// <summary>
    /// Whether a builder reading this version can be handed a removal order.
    /// </summary>
    /// <param name="version">The contract version the builder reads.</param>
    /// <returns>True when the version is 3 or later; false otherwise.</returns>
    public static bool Speaks3(string version) => string.Equals(version, Version3, StringComparison.Ordinal);
}
