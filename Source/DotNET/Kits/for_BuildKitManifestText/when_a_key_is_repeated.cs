// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitManifestText;

public class when_a_key_is_repeated : Specification
{
    Exception _error = null!;

    void Because() => _error = Cratis.Specifications.Catch.Exception(() => BuildKitManifestText.Parse("build-kit.yaml", "name: a\nname: b\n"));

    [Fact] void should_refuse_it() => _error.ShouldBeOfExactType<BuildKitManifestUnreadable>();
}
#endif
