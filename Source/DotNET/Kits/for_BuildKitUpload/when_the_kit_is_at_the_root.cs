// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitUpload;

public class when_the_kit_is_at_the_root : given.an_archive
{
    void Because() => _result = BuildKitUpload.Read(Zip("build-kit.yaml", "scaffold/Program.cs"));

    [Fact] void should_keep_the_paths() => _result.Files.Keys.ShouldContainOnly("build-kit.yaml", "scaffold/Program.cs");
}
#endif
