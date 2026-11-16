// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Kits;

/// <summary>
/// The files read from an uploaded kit, or the problem that stopped them being read.
/// </summary>
/// <param name="Files">The files of the kit, keyed by their path relative to the kit root.</param>
/// <param name="Problem">Why the archive could not be read, or null when it could.</param>
public record BuildKitUploadContent(IReadOnlyDictionary<string, byte[]> Files, string? Problem);
