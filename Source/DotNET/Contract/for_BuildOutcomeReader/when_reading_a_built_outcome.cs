// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildOutcomeReader;

public class when_reading_a_built_outcome : given.an_outcome
{
    readonly Guid _specificationId = Guid.NewGuid();
    BuiltOutcome _outcome = null!;

    void Because()
    {
        _result = BuildOutcomeReader.Read(
            Outcome($$"""
            "kind": "built", "commit": "4f1c2e9", "pullRequest": "https://example.com/pull/7",
            "files": ["Source/Invoicing/PayInvoice.cs"],
            "specifications": [{ "id": "{{_specificationId}}", "result": "passed" }],
            "assumptions": ["Amounts are in the invoice currency"]
            """),
            _orderId);
        _outcome = (BuiltOutcome)Read();
    }

    [Fact] void should_answer_the_order() => _outcome.OrderId.ShouldEqual(_orderId);
    [Fact] void should_have_the_commit() => _outcome.Commit.ShouldEqual("4f1c2e9");
    [Fact] void should_have_no_branch() => _outcome.Branch.ShouldBeNull();
    [Fact] void should_have_the_files() => _outcome.Files.ShouldContainOnly(["Source/Invoicing/PayInvoice.cs"]);
    [Fact] void should_have_the_specification_result() => _outcome.Specifications.Single().ShouldEqual(new BuildSpecificationResult(_specificationId, BuildSpecificationVerdict.Passed, null));
    [Fact] void should_have_the_assumptions() => _outcome.Assumptions.ShouldContainOnly(["Amounts are in the invoice currency"]);
}
#endif
