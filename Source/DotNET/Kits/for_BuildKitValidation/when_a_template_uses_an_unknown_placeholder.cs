// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_a_template_uses_an_unknown_placeholder : given.a_kit
{
    void Because()
    {
        _files["build-slice.md"] = Bytes("Build {slice} with the token {apiToken}. JSON like { \"a\": 1 } and {{literal}} is left alone.");
        Replace("make test {slice}", "make test {sliceName}");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_report_two_problems() => _result.Problems.Count.ShouldEqual(2);
    [Fact] void should_point_at_the_verify_command() => _result.Problems.ShouldContain(problem => problem.StartsWith("/verify/1: {sliceName}", StringComparison.Ordinal));
    [Fact] void should_point_at_the_prompt() => _result.Problems.ShouldContain(problem => problem.StartsWith("/prompt: {apiToken}", StringComparison.Ordinal));
}
#endif
