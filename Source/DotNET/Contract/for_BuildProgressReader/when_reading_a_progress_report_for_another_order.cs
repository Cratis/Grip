// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildProgressReader;

public class when_reading_a_progress_report_for_another_order : Specification
{
    BuildProgressRefused _refused = null!;

    void Because() => BuildProgressReader.Read($$"""{ "contractVersion": "2", "orderId": "{{Guid.NewGuid()}}", "step": "Compiling" }""", Guid.NewGuid())
        .TryGetError(out _refused!).ShouldBeTrue();

    [Fact] void should_say_it_answers_another_order() => _refused.Reasons.Single().Contains("answers order").ShouldBeTrue();
}
#endif
