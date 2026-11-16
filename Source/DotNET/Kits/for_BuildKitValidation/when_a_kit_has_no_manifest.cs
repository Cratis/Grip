// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_a_kit_has_no_manifest : given.a_kit
{
    void Because()
    {
        _files.Remove("build-kit.yaml");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
    [Fact] void should_say_the_manifest_is_missing() => _result.Problems.ShouldContainOnly("The kit has no build-kit.yaml or build-kit.json at its root.");
}
#endif
