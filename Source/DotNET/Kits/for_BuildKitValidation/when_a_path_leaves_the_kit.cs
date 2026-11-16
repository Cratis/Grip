// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_a_path_leaves_the_kit : given.a_kit
{
    void Because()
    {
        Replace("instructions: ./AGENTS.md", "instructions: ./../AGENTS.md");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
    [Fact] void should_point_at_the_instructions() => _result.Problems.ShouldContain(problem => problem.StartsWith("/instructions", StringComparison.Ordinal));
}
#endif
