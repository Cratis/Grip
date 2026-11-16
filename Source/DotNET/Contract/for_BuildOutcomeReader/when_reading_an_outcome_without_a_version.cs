// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_an_outcome_without_a_version : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read($$"""{ "orderId": "{{_orderId}}", "kind": "nothingToDo" }""");

    [Fact] void should_say_the_version_is_missing() => Reasons().Single().ShouldContain("contractVersion");
}
#endif
