// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_the_manifest_names_an_unknown_slice_type : given.a_kit
{
    void Because()
    {
        Replace("[stateChange, stateView]", "[stateChange, saga]");
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_not_be_valid() => _result.IsValid.ShouldBeFalse();
    [Fact] void should_point_at_the_slice_type() => _result.Problems.ShouldContain(problem => problem.StartsWith("/sliceTypes/1", StringComparison.Ordinal));
}
#endif
