// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_a_skill_has_no_skill_file : given.a_kit
{
    void Because()
    {
        _files["skills/other/notes.md"] = Bytes("notes");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_name_the_skill() => _result.Problems.ShouldContainOnly("/skills: the skill other has no SKILL.md.");
}
#endif
