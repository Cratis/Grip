// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_the_kit_version_is_written_as_a_number : given.a_kit
{
    void Because()
    {
        Replace("kitVersion: \"1\"", "kitVersion: 1");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
    [Fact] void should_point_at_the_kit_version() => _result.Problems.Single().StartsWith("/kitVersion", StringComparison.Ordinal).ShouldBeTrue();
}
#endif
