// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_an_outcome_with_a_member_the_contract_does_not_have : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read(Outcome("""
        "kind": "failed", "error": "Out of memory", "retryable": false, "token": "abc"
        """));

    [Fact] void should_refuse_it_with_reasons() => Reasons().ShouldNotBeEmpty();
}
#endif
