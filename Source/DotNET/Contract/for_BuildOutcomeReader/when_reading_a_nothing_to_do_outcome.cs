// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_a_nothing_to_do_outcome : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read(Outcome("\"kind\": \"nothingToDo\""));

    [Fact] void should_read_it() => Read().ShouldBeOfExactType<NothingToDoOutcome>();
}
#endif
