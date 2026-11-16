// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_the_kit_version_is_unknown : given.a_kit
{
    void Because()
    {
        Replace("kitVersion: \"1\"", "kitVersion: \"2\"");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_name_the_versions_studio_reads() => _result.Problems.ShouldContainOnly("/kitVersion: must be one of \"1\".");
}
#endif
