// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitUpload;

public class when_the_body_is_not_a_zip : given.an_archive
{
    void Because() => _result = BuildKitUpload.Read([1, 2, 3, 4]);

    [Fact] void should_refuse_it() => _result.Problem.ShouldEqual("The request body is not a zip archive.");
}
#endif
