// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_validating_a_valid_kit : given.a_kit
{
    void Because() => _result = BuildKitValidation.Validate(_files);

    [Fact] void should_be_valid() => _result.IsValid.ShouldBeTrue();
    [Fact] void should_have_no_problems() => _result.Problems.ShouldBeEmpty();
    [Fact] void should_read_the_name() => _result.Manifest!.Name.ShouldEqual("sample");
    [Fact] void should_read_the_slice_types() => _result.Manifest!.SliceTypes.ShouldContainOnly("stateChange", "stateView");
    [Fact] void should_default_to_no_ui() => _result.Manifest!.Ui.ShouldBeFalse();
    [Fact] void should_default_to_contract_version_1() => _result.Manifest!.ContractVersions.ShouldContainOnly("1");
}
#endif
