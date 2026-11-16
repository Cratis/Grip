// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitUpload;

public class when_the_body_is_empty : given.an_archive
{
    void Because() => _result = BuildKitUpload.Read([]);

    [Fact] void should_ask_for_an_archive() => _result.Problem.ShouldEqual("Send the kit as a zip archive in the request body.");
}
#endif
