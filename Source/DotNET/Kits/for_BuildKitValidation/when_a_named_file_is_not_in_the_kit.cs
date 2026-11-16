// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_a_named_file_is_not_in_the_kit : given.a_kit
{
    void Because()
    {
        _files.Remove("build-slice.md");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_say_the_prompt_is_missing() => _result.Problems.ShouldContainOnly("/prompt: the kit has no file ./build-slice.md.");
}
#endif
