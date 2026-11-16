// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_a_kit_has_two_manifests : given.a_kit
{
    void Because()
    {
        _files["build-kit.json"] = Bytes("{}");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_say_to_keep_one() => _result.Problems.ShouldContainOnly("The kit has both build-kit.yaml and build-kit.json; keep one.");
}
#endif
