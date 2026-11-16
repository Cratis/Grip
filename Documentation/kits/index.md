---
title: Build kits
description: Cratis.Grip.Kits validates build kits against the published manifest schema, packs them as reproducible archives, and carries the reference kits Studio offers by name.
---

A build kit describes a stack to a builder host: how a new project is created, what the coding agent is told, how one
slice is built and verified, and how the work lands in Git. [The build kit format](build-kit-format.md) is the
reference. `Cratis.Grip.Kits` is the code that checks one:

```bash
dotnet add package Cratis.Grip.Kits
```

## Validate a kit

`BuildKitValidation` validates a whole kit, not only its manifest: the manifest against the published schema, then that
every file and folder it names is in the kit, every skill folder holds a `SKILL.md`, and every placeholder is one a host
fills. Every problem is reported with where it is, so an author fixes them all in one pass.

```csharp
using Cratis.Grip.Kits;

IReadOnlyDictionary<string, byte[]> files = ReadTheKitFolder();
var result = BuildKitValidation.Validate(files);
if (!result.IsValid)
{
    foreach (var problem in result.Problems) Console.WriteLine(problem);
}
```

The files are keyed by their path relative to the kit root, using `/` as the separator. A manifest written as
`build-kit.yaml` or `build-kit.json` is read as YAML 1.2's JSON-compatible core.

## Pack and unpack a kit

`BuildKitArchive.Pack` writes a kit as a zip that is the same bytes for the same files, so the SHA-256 a host verifies
only changes when the kit does. `BuildKitUpload.Read` reads an uploaded zip back into files, bounded in archive size,
entry count and unpacked size, and refusing any entry that leaves the kit.

## The reference kits

The folders under `Kits/` are the reference kits, embedded in the package. `ReferenceBuildKits` lists the ones that
validate and returns their files; a specification fails the build when a shipped kit does not validate.

| Kit | Stack |
| --- | --- |
| `cratis-csharp` | .NET on Cratis Arc and Chronicle with a React frontend; every slice type and user interfaces |
| `axon` | Java on Axon Framework, rendered by the Axon Stage renderer; every slice type, backend only |
| `node` | Node.js in TypeScript on the Cratis Chronicle client; state changes and state views |

Install one with `cratis build init --kit <name>`.
