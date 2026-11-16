// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitUpload;

public class when_the_kit_is_inside_one_folder : given.an_archive
{
    void Because() => _result = BuildKitUpload.Read(Zip("my-kit-main/build-kit.yaml", "my-kit-main/AGENTS.md"));

    [Fact] void should_have_no_problem() => _result.Problem.ShouldBeNull();
    [Fact] void should_read_the_files_from_the_folder() => _result.Files.Keys.ShouldContainOnly("build-kit.yaml", "AGENTS.md");
}
#endif
