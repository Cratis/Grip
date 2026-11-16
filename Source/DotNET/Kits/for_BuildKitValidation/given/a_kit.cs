// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using System.Text;

namespace Cratis.Grip.Kits.for_BuildKitValidation.given;

public class a_kit : Specification
{
    protected const string ValidManifest = """
        kitVersion: "1"
        name: sample
        version: 1.0.0
        description: A sample stack.
        sliceTypes: [stateChange, stateView]
        instructions: ./AGENTS.md
        skills: ./skills
        prompt: ./build-slice.md
        verify:
          - make build
          - make test {slice}
        git:
          branch: "studio/{feature}"
          commit: "Build {slice}"
        scaffold:
          path: ./scaffold
        existingCodebase:
          learn: ./learn.md
          conventions: .cratis/build/conventions.md
        """;

    protected Dictionary<string, byte[]> _files = null!;
    protected BuildKitValidationResult _result = null!;

    void Establish() => _files = new(StringComparer.Ordinal)
    {
        ["build-kit.yaml"] = Bytes(ValidManifest),
        ["AGENTS.md"] = Bytes("# Instructions"),
        ["build-slice.md"] = Bytes("Build {slice} from {order}; write {outcome}."),
        ["learn.md"] = Bytes("Record the conventions in {conventions}."),
        ["skills/sample/SKILL.md"] = Bytes("# Skill"),
        ["scaffold/Makefile"] = Bytes("build:")
    };

    protected static byte[] Bytes(string text) => Encoding.UTF8.GetBytes(text);

    protected void Manifest(string text) => _files["build-kit.yaml"] = Bytes(text);

    protected void Replace(string from, string to) => Manifest(ValidManifest.Replace(from, to, StringComparison.Ordinal));
}
#endif
