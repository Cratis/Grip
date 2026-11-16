// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO.Compression;

namespace Cratis.Grip.Kits;

/// <summary>
/// Reads a kit a kit author sends for validation, as a zip with the manifest at its root (or inside one top folder, the way an archive of a Git repository is laid out). It is bounded before anything is unpacked: the archive's size, the number of entries and their unpacked size, and no entry may leave the kit.
/// </summary>
public static class BuildKitUpload
{
    /// <summary>
    /// The largest archive that is read, in bytes.
    /// </summary>
    public const int MaximumArchiveBytes = 5 * 1024 * 1024;

    /// <summary>
    /// The largest number of files an archive may hold.
    /// </summary>
    public const int MaximumEntries = 2000;

    /// <summary>
    /// The largest total size an archive may unpack to, in bytes.
    /// </summary>
    public const long MaximumUnpackedBytes = 20L * 1024 * 1024;

    /// <summary>
    /// Reads a zip archive into the files of a kit, keyed by their path relative to the kit root.
    /// </summary>
    /// <param name="archive">The bytes of the zip archive.</param>
    /// <returns>The files, or the reason they could not be read.</returns>
    public static BuildKitUploadContent Read(byte[] archive)
    {
        if (archive.Length == 0)
        {
            return Refused("Send the kit as a zip archive in the request body.");
        }

        if (archive.Length > MaximumArchiveBytes)
        {
            return Refused($"The archive is larger than {MaximumArchiveBytes / (1024 * 1024)} MB.");
        }

        try
        {
            using var zip = new ZipArchive(new MemoryStream(archive), ZipArchiveMode.Read);
            var entries = zip.Entries.Where(entry => !entry.FullName.EndsWith('/')).ToArray();
            if (entries.Length > MaximumEntries)
            {
                return Refused($"The archive holds more than {MaximumEntries} files.");
            }

            if (entries.Sum(entry => entry.Length) > MaximumUnpackedBytes)
            {
                return Refused($"The archive unpacks to more than {MaximumUnpackedBytes / (1024 * 1024)} MB.");
            }

            var paths = entries.Select(entry => entry.FullName.Replace('\\', '/')).ToArray();
            if (paths.FirstOrDefault(Escapes) is { } escaping)
            {
                return Refused($"The entry {escaping} leaves the kit.");
            }

            var root = SingleTopFolder(paths);
            var files = new Dictionary<string, byte[]>(StringComparer.Ordinal);
            for (var index = 0; index < entries.Length; index++)
            {
                using var stream = entries[index].Open();
                using var buffer = new MemoryStream();
                stream.CopyTo(buffer);
                files[paths[index][root.Length..]] = buffer.ToArray();
            }

            return new(files, null);
        }
        catch (InvalidDataException)
        {
            return Refused("The request body is not a zip archive.");
        }
    }

    static BuildKitUploadContent Refused(string problem) => new(new Dictionary<string, byte[]>(), problem);

    static bool Escapes(string path) =>
        path.StartsWith('/') || path.Split('/').Any(segment => segment == "..");

    /// <summary>
    /// A kit zipped from its own folder has the manifest at the root; one zipped from the folder above, or downloaded from a Git host, has everything inside one top folder. The manifest decides which.
    /// </summary>
    /// <param name="paths">The paths of every file in the archive.</param>
    /// <returns>The folder to strip from every path, or an empty string when there is none.</returns>
    static string SingleTopFolder(string[] paths)
    {
        if (paths.Any(path => BuildKitManifestText.FileNames.Contains(path)))
        {
            return string.Empty;
        }

        var tops = paths.Select(path => path.Split('/')[0]).Distinct(StringComparer.Ordinal).ToArray();
        return tops.Length == 1 && paths.All(path => path.Contains('/')) ? $"{tops[0]}/" : string.Empty;
    }
}
