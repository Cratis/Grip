// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
namespace Cratis.Grip.Kits.for_BuildKitValidation;

public class when_the_manifest_is_written_in_json : given.a_kit
{
    void Because()
    {
        _files.Remove("build-kit.yaml");
        _files["build-kit.json"] = Bytes("""
            { "kitVersion": "1", "name": "sample", "version": "2.1.0", "description": "A sample stack.", "sliceTypes": ["automation"],
              "ui": true, "instructions": "./AGENTS.md", "prompt": "./build-slice.md", "verify": ["make"], "contract": { "versions": ["1"] } }
            """);
        _result = BuildKitValidation.Validate(_files);
    }

    [Fact] void should_be_valid() => _result.IsValid.ShouldBeTrue();
    [Fact] void should_read_the_ui() => _result.Manifest!.Ui.ShouldBeTrue();
    [Fact] void should_remember_the_manifest_file() => _result.Manifest!.ManifestFile.ShouldEqual("build-kit.json");
}
#endif
