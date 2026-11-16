// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_an_outcome_in_a_version_studio_does_not_speak : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read($$"""{ "contractVersion": "99", "orderId": "{{_orderId}}", "kind": "nothingToDo" }""");

    [Fact] void should_name_the_version() => Reasons().Single().ShouldContain("'99'");
}
#endif
