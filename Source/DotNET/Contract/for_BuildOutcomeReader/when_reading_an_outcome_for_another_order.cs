// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_an_outcome_for_another_order : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read(Outcome("\"kind\": \"nothingToDo\""), Guid.NewGuid());

    [Fact] void should_say_it_answers_another_order() => Reasons().Single().ShouldContain(_orderId.ToString());
}
#endif
