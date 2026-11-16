// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_validating_the_reference_kits : Specification
{
    Dictionary<string, BuildKitValidationResult> _results = null!;

    void Because() => _results = ReferenceBuildKits.Files.ToDictionary(kit => kit.Key, kit => BuildKitValidation.Validate(kit.Value));

    [Fact] void should_ship_the_cratis_csharp_kit() => _results.ContainsKey("cratis-csharp").ShouldBeTrue();
    [Fact] void should_ship_the_axon_kit() => _results.ContainsKey("axon").ShouldBeTrue();
    [Fact] void should_build_every_slice_type_with_the_axon_kit() => ReferenceBuildKits.Named("axon")!.SliceTypes.ShouldContainOnly("stateChange", "stateView", "automation", "translation");
    [Fact] void should_leave_user_interfaces_to_other_kits_with_the_axon_kit() => ReferenceBuildKits.Named("axon")!.Ui.ShouldBeFalse();
    [Fact] void should_ship_the_node_kit() => _results.ContainsKey("node").ShouldBeTrue();
    [Fact] void should_have_no_problems() => _results.SelectMany(kit => kit.Value.Problems.Select(problem => $"{kit.Key}: {problem}")).ShouldBeEmpty();
    [Fact] void should_offer_every_kit() => ReferenceBuildKits.All.Select(kit => kit.Name).ShouldContainOnly(_results.Keys);
    [Fact] void should_build_every_slice_type_with_the_cratis_csharp_kit() => ReferenceBuildKits.Named("cratis-csharp")!.SliceTypes.ShouldContainOnly("stateChange", "stateView", "automation", "translation");
    [Fact] void should_build_state_changes_and_views_with_the_node_kit() => ReferenceBuildKits.Named("node")!.SliceTypes.ShouldContainOnly("stateChange", "stateView");
    [Fact] void should_carry_the_scaffold_dot_files() => ReferenceBuildKits.Files["cratis-csharp"].ContainsKey("scaffold/.gitignore").ShouldBeTrue();
}
#endif
