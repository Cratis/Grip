// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_a_blocked_outcome : given.an_outcome
{
    BlockedOutcome _outcome = null!;

    void Because()
    {
        _result = BuildOutcomeReader.Read(Outcome("""
            "kind": "blocked", "reason": "The payment provider is not modeled", "questions": ["Which provider?"]
            """));
        _outcome = (BlockedOutcome)Read();
    }

    [Fact] void should_have_the_reason() => _outcome.Reason.ShouldEqual("The payment provider is not modeled");
    [Fact] void should_have_the_questions() => _outcome.Questions.ShouldContainOnly(["Which provider?"]);
}
#endif
