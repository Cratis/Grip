// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildProgressReader;

public class when_reading_a_progress_report : Specification
{
    readonly Guid _orderId = Guid.NewGuid();
    BuildProgress _progress = null!;

    void Because()
    {
        var json = $$"""
            { "contractVersion": "2", "orderId": "{{_orderId}}", "step": "Compiling",
              "log": [{ "occurred": "2026-11-06T09:00:00+00:00", "level": "warning", "text": "slow" }] }
            """;
        BuildProgressReader.Read(json, _orderId).TryGetResult(out _progress!).ShouldBeTrue();
    }

    [Fact] void should_read_the_step() => _progress.Step.ShouldEqual("Compiling");
    [Fact] void should_read_the_log() => _progress.Log!.Single().Level.ShouldEqual(BuildLogLevel.Warning);
}
#endif
