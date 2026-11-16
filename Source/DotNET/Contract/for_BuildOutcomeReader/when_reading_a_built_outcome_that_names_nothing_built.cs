// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_a_built_outcome_that_names_nothing_built : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read(Outcome("""
        "kind": "built", "files": [], "specifications": [], "assumptions": []
        """));

    [Fact] void should_refuse_it_with_reasons() => Reasons().ShouldNotBeEmpty();
}
#endif
