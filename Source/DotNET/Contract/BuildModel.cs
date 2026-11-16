// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Cratis.Grip.Contract;

/// <summary>
/// Canonical is the executable semantic model JSON as Screenplay serializes it, described by Screenplay's own schema for its SchemaVersion; it is absent when the model could not be compiled into it.
/// </summary>
/// <param name="Language">The language the model is written in.</param>
/// <param name="SchemaVersion">The schema version of the canonical model.</param>
/// <param name="Revision">The revision of the model.</param>
/// <param name="Documents">The documents of the model.</param>
/// <param name="Canonical">The executable semantic model as JSON, or null when the model could not be compiled into it.</param>
public record BuildModel(
    string Language,
    string SchemaVersion,
    string Revision,
    IReadOnlyList<BuildModelDocument> Documents,
    JsonElement? Canonical);
