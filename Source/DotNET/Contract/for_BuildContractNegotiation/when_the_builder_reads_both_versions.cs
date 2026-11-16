// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildContractNegotiation;

public class when_the_builder_reads_both_versions : Specification
{
    Result<string, BuildContractNotShared> _result = null!;

    void Because() => _result = BuildContractNegotiation.Choose([BuildContract.Version1, BuildContract.Version2]);

    [Fact]
    void should_choose_the_newest()
    {
        _result.TryGetResult(out var version);
        version.ShouldEqual(BuildContract.Version2);
    }
}
#endif
