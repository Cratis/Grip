// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildProgressReader;

public class when_reading_a_version_1_progress_report : Specification
{
    BuildProgressRefused _refused = null!;

    void Because() => BuildProgressReader.Read($$"""{ "contractVersion": "1", "orderId": "{{Guid.NewGuid()}}", "step": "Compiling" }""")
        .TryGetError(out _refused!).ShouldBeTrue();

    [Fact] void should_say_version_1_has_no_progress_report() => _refused.Reasons.Single().ShouldEqual("Contract version '1' has no progress report.");
}
#endif
