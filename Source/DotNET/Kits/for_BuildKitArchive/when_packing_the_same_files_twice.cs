// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using System.IO.Compression;
using System.Text;

namespace Cratis.Grip.Kits.for_BuildKitArchive;

public class when_packing_the_same_files_twice : Specification
{
    BuildKitArchiveContent _first = null!;
    BuildKitArchiveContent _second = null!;

    void Because()
    {
        _first = BuildKitArchive.Pack(new Dictionary<string, byte[]> { ["b.md"] = Encoding.UTF8.GetBytes("b"), ["a/x.md"] = Encoding.UTF8.GetBytes("x") });
        _second = BuildKitArchive.Pack(new Dictionary<string, byte[]> { ["a/x.md"] = Encoding.UTF8.GetBytes("x"), ["b.md"] = Encoding.UTF8.GetBytes("b") });
    }

    [Fact] void should_give_the_same_checksum() => _second.Sha256.ShouldEqual(_first.Sha256);
    [Fact] void should_give_the_same_bytes() => _second.Archive.SequenceEqual(_first.Archive).ShouldBeTrue();
    [Fact] void should_hold_every_file()
    {
        using var zip = new ZipArchive(new MemoryStream(_first.Archive));
        zip.Entries.Select(entry => entry.FullName).ShouldContainOnly("a/x.md", "b.md");
    }
}
#endif
