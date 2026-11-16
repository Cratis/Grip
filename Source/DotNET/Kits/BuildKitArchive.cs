// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO.Compression;
using System.Security.Cryptography;

namespace Cratis.Grip.Kits;

/// <summary>
/// Packs a kit as a zip a host unpacks as is. The same files always give the same bytes (sorted entries, one fixed timestamp), so the checksum a host verifies against only changes when the kit does.
/// </summary>
public static class BuildKitArchive
{
    static readonly DateTimeOffset _timestamp = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

    /// <summary>
    /// Packs the files of a kit, keyed by their path relative to the kit root, into a reproducible zip.
    /// </summary>
    /// <param name="files">The files of the kit, keyed by their path relative to the kit root, using / as the separator.</param>
    /// <returns>The packed kit.</returns>
    public static BuildKitArchiveContent Pack(IReadOnlyDictionary<string, byte[]> files)
    {
        using var buffer = new MemoryStream();
        using (var zip = new ZipArchive(buffer, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var (path, content) in files.OrderBy(file => file.Key, StringComparer.Ordinal))
            {
                var entry = zip.CreateEntry(path, CompressionLevel.Optimal);
                entry.LastWriteTime = _timestamp;
                using var stream = entry.Open();
                stream.Write(content);
            }
        }

        var archive = buffer.ToArray();
        return new(archive, Convert.ToHexStringLower(SHA256.HashData(archive)));
    }
}
