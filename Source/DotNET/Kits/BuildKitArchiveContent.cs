// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Grip.Kits;

/// <summary>
/// A packed kit: the zip bytes and their SHA-256 checksum as lowercase hexadecimal.
/// </summary>
/// <param name="Archive">The bytes of the zip archive.</param>
/// <param name="Sha256">The SHA-256 checksum of the archive, as lowercase hexadecimal.</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "The archive is a binary payload that is written to a response or a file as it is.")]
public record BuildKitArchiveContent(byte[] Archive, string Sha256);
