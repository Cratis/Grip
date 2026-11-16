// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Contract.for_BuildContractSchemas;

public class when_asking_for_a_schema_that_is_not_published : Specification
{
    [Fact] void should_have_no_order_schema_for_an_unknown_version() => BuildContractSchemas.Text(BuildContractSchemas.Order, "99").ShouldBeNull();
    [Fact] void should_have_no_schema_for_an_unknown_document() => BuildContractSchemas.Text("build-secrets", BuildContract.Version1).ShouldBeNull();
    [Fact] void should_publish_the_order_schema() => BuildContractSchemas.Text(BuildContractSchemas.Order, BuildContract.Version1).ShouldNotBeNull();
    [Fact] void should_publish_the_outcome_schema() => BuildContractSchemas.Text(BuildContractSchemas.Outcome, BuildContract.Version1).ShouldNotBeNull();
    [Fact] void should_publish_version_2_of_the_order_schema() => BuildContractSchemas.Text(BuildContractSchemas.Order, BuildContract.Version2).ShouldNotBeNull();
    [Fact] void should_publish_version_2_of_the_outcome_schema() => BuildContractSchemas.Text(BuildContractSchemas.Outcome, BuildContract.Version2).ShouldNotBeNull();
    [Fact] void should_publish_the_progress_schema_from_version_2() => BuildContractSchemas.Text(BuildContractSchemas.Progress, BuildContract.Version2).ShouldNotBeNull();
    [Fact] void should_have_no_progress_schema_in_version_1() => BuildContractSchemas.Text(BuildContractSchemas.Progress, BuildContract.Version1).ShouldBeNull();
}
#endif
