// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Kits;

/// <summary>
/// The exception thrown when a kit manifest format version has no published schema.
/// </summary>
/// <param name="version">The manifest format version.</param>
public class UnknownBuildKitSchema(string version)
    : Exception($"There is no build kit manifest schema for version '{version}'.");
