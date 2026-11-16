// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Contract;

/// <summary>
/// The exception thrown when a document has no published schema for a contract version.
/// </summary>
/// <param name="document">The name of the document kind.</param>
/// <param name="version">The contract version.</param>
public class UnknownBuildContractSchema(string document, string version)
    : Exception($"There is no '{document}' schema for build contract version '{version}'.");
