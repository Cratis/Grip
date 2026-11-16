// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_a_failed_outcome : given.an_outcome
{
    FailedOutcome _outcome = null!;

    void Because()
    {
        _result = BuildOutcomeReader.Read(Outcome("""
            "retryable": true, "error": "The build timed out", "kind": "failed"
            """));
        _outcome = (FailedOutcome)Read();
    }

    [Fact] void should_have_the_error() => _outcome.Error.ShouldEqual("The build timed out");
    [Fact] void should_say_a_retry_makes_sense() => _outcome.Retryable.ShouldBeTrue();
}
#endif
