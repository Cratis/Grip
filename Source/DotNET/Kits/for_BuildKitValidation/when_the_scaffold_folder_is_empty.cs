// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_the_scaffold_folder_is_empty : given.a_kit
{
    void Because()
    {
        _files.Remove("scaffold/Makefile");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_say_the_scaffold_is_missing() => _result.Problems.ShouldContainOnly("/scaffold/path: the kit has no folder ./scaffold, or it is empty.");
}
#endif
