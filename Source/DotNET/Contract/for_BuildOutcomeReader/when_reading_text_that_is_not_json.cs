// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_text_that_is_not_json : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read("Built it, all good!");

    [Fact] void should_say_it_is_not_json() => Reasons().Single().StartsWith("The outcome is not JSON").ShouldBeTrue();
}
#endif
