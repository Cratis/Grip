// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using System.Text.Json.Nodes;

namespace Cratis.Grip.Kits.for_BuildKitManifestText;

public class when_reading_yaml_scalars : Specification
{
    JsonObject _document = null!;

    void Because() => _document = BuildKitManifestText.Parse("build-kit.yaml", """
        quoted: "1"
        number: 1
        version: 1.0.0
        flag: true
        empty: null
        list: [a, "b"]
        """).AsObject();

    [Fact] void should_keep_a_quoted_value_as_text() => _document["quoted"]!.GetValue<string>().ShouldEqual("1");
    [Fact] void should_read_an_unquoted_number_as_a_number() => _document["number"]!.GetValue<long>().ShouldEqual(1L);
    [Fact] void should_keep_a_version_as_text() => _document["version"]!.GetValue<string>().ShouldEqual("1.0.0");
    [Fact] void should_read_true_as_a_boolean() => _document["flag"]!.GetValue<bool>().ShouldBeTrue();
    [Fact] void should_read_null_as_null() => _document["empty"].ShouldBeNull();
    [Fact] void should_read_a_list() => _document["list"]!.AsArray().Select(item => item!.GetValue<string>()).ShouldContainOnly("a", "b");
}
#endif
