// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildProgressReader;

public class when_reading_a_progress_report_with_nothing_in_it : Specification
{
    bool _read;

    void Because() => _read = BuildProgressReader.Read($$"""{ "contractVersion": "2", "orderId": "{{Guid.NewGuid()}}" }""").TryGetResult(out _);

    [Fact] void should_refuse_it() => _read.ShouldBeFalse();
}
#endif
