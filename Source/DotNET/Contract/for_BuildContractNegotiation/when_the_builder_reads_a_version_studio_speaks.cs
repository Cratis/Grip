// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildContractNegotiation;

public class when_the_builder_reads_a_version_studio_speaks : Specification
{
    Result<string, BuildContractNotShared> _result = null!;

    void Because() => _result = BuildContractNegotiation.Choose(["0", BuildContract.Version1, "7"]);

    [Fact] void should_offer_that_version() => _result.TryGetResult(out _).ShouldBeTrue();
    [Fact]
    void should_choose_version_1()
    {
        _result.TryGetResult(out var version);
        version.ShouldEqual(BuildContract.Version1);
    }
}
#endif
