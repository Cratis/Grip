// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_a_version_2_outcome_with_usage_and_api : given.an_outcome
{
    void Because() => _result = BuildOutcomeReader.Read($$"""
        { "contractVersion": "2", "orderId": "{{_orderId}}", "kind": "built", "branch": "b", "files": [], "specifications": [], "assumptions": [],
          "api": [{ "kind": "query", "name": "Open Invoices" }], "usage": { "inputTokens": 10, "outputTokens": 4 } }
        """);

    [Fact] void should_read_the_tokens_used() => Read().Usage.ShouldEqual(new BuildUsage(10, 4));
    [Fact] void should_read_the_api() => ((BuiltOutcome)Read()).Api!.Single().ShouldEqual(new BuildApiOperation(BuildApiOperationKind.Query, "Open Invoices"));
}
#endif
