// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_a_blocked_outcome_without_a_reason : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read(Outcome("\"kind\": \"blocked\", \"questions\": []"));

    [Fact] void should_say_where_it_went_wrong() => Reasons().ShouldContain(reason => reason.Contains("reason"));
}
#endif
