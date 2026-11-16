// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader.given;

public class an_outcome : Specification
{
    protected readonly Guid _orderId = Guid.NewGuid();
    protected Result<BuildOutcome, BuildOutcomeRefused> _result = null!;

    protected string Outcome(string kindAndMembers) => $$"""{ "contractVersion": "1", "orderId": "{{_orderId}}", {{kindAndMembers}} }""";

    protected BuildOutcome Read()
    {
        _result.TryGetResult(out var outcome).ShouldBeTrue();
        return outcome!;
    }

    protected IReadOnlyList<string> Reasons()
    {
        _result.TryGetError(out var refused).ShouldBeTrue();
        return refused!.Reasons;
    }
}
#endif
