// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildContractNegotiation;

public class when_the_builder_reads_no_version_studio_speaks : Specification
{
    Result<string, BuildContractNotShared> _result = null!;

    void Because() => _result = BuildContractNegotiation.Choose(["98", "99"]);

    [Fact] void should_refuse_to_offer_an_order() => _result.TryGetError(out _).ShouldBeTrue();
    [Fact]
    void should_say_what_the_builder_reads()
    {
        _result.TryGetError(out var refused);
        refused!.Accepted.ShouldContainOnly(["98", "99"]);
    }
}
#endif
