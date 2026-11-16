// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitUpload;

public class when_an_entry_leaves_the_kit : given.an_archive
{
    void Because() => _result = BuildKitUpload.Read(Zip("build-kit.yaml", "../outside.md"));

    [Fact] void should_refuse_it() => _result.Problem.ShouldEqual("The entry ../outside.md leaves the kit.");
    [Fact] void should_read_no_files() => _result.Files.ShouldBeEmpty();
}
#endif
