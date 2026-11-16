// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_the_manifest_has_no_verify_commands : given.a_kit
{
    void Because()
    {
        Replace("verify:\n  - make build\n  - make test {slice}\n", string.Empty);
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
    [Fact] void should_say_verify_is_required() => _result.Problems.ShouldContain(problem => problem.Contains("verify", StringComparison.Ordinal));
}
#endif
