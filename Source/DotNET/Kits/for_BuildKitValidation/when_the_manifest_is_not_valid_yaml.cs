// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_the_manifest_is_not_valid_yaml : given.a_kit
{
    void Because()
    {
        Manifest("name: [unclosed");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
    [Fact] void should_say_it_is_not_valid_yaml() => _result.Problems.Single().ShouldContain("not valid YAML");
}
#endif
