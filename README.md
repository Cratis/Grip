# Cratis Grip

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/Cratis/Grip/actions/workflows/build.yml/badge.svg)](https://github.com/Cratis/Grip/actions/workflows/build.yml)
[![Publish](https://github.com/Cratis/Grip/actions/workflows/publish.yml/badge.svg)](https://github.com/Cratis/Grip/actions/workflows/publish.yml)
[![Cratis.Grip.Contract](https://img.shields.io/nuget/v/Cratis.Grip.Contract?logo=nuget&label=Cratis.Grip.Contract)](https://www.nuget.org/packages/Cratis.Grip.Contract)
[![Cratis.Grip.Kits](https://img.shields.io/nuget/v/Cratis.Grip.Kits?logo=nuget&label=Cratis.Grip.Kits)](https://www.nuget.org/packages/Cratis.Grip.Kits)
[![Discord](https://img.shields.io/discord/1182595891576717413?label=Discord&logo=discord&logoColor=white)](https://discord.gg/kt4AMpV8WV)

Cratis Grip is the builder side of [Cratis Studio](https://www.cratis.studio). Studio decides what gets built: it
turns an event model into build orders and settles each slice from what comes back. A *builder* is anything that turns
those slices into code, and Grip is what you need to run one, or to write one.

It gives you three things:

- **Builder images** on Docker Hub, so the Cratis builder runs the same way on your machine, in your pipeline and in
  Studio's cloud, on `linux/amd64` and `linux/arm64`.
- **`Cratis.Grip.Contract`**, the .NET library behind the build orders, outcomes and progress reports Studio and a builder
  exchange, with the JSON Schemas that describe them.
- **`Cratis.Grip.Kits`**, the .NET library that validates and packs *build kits*, the descriptions of a stack a builder
  builds with, and that carries the reference kits.

## Run the Cratis builder

The [Cratis CLI](https://www.cratis.io/cli/) starts the image for you and mounts your project into it:

```bash
cratis build run --docker
```

| Image | What it is |
| --- | --- |
| [`cratis/builder`](https://hub.docker.com/r/cratis/builder) | The Cratis CLI with Git, Node.js, a JDK and Maven, on the .NET SDK |
| [`cratis/studio-axon-builder`](https://hub.docker.com/r/cratis/studio-axon-builder) | `cratis/builder` with the `axon` kit, for Java applications on Axon Framework |
| [`cratis/studio-eventmodelers-builder`](https://hub.docker.com/r/cratis/studio-eventmodelers-builder) | `cratis/builder` with an adapter for projects built with the Eventmodelers BuildKit |

Every release is tagged with its [semantic version](https://semver.org) and, unless it is a prerelease, `latest`. Read
[the builder images documentation](Documentation/images/index.md) to choose an image, pin a version or build one yourself.

## Write a builder

Add the contract to a host or builder that talks to Studio:

```bash
dotnet add package Cratis.Grip.Contract
```

```csharp
using Cratis.Grip.Contract;

// Pick the newest contract version both sides speak.
var version = BuildContractNegotiation.Choose(["1", "2", "3"]);

// Read what the builder answered with. The text is validated against that version's JSON Schema first;
// anything that does not match is refused with the reasons, and nothing is read from it.
var outcome = BuildOutcomeReader.Read(text, expectedOrderId: orderId);
```

[The build contract](Documentation/contract/index.md) shows how to write an order and read progress, and
[Build orders and outcomes](Documentation/contract/orders-and-outcomes.md) describes every document, with examples of
each.

## Describe a stack as a build kit

A build kit tells a builder how to create a project, what to tell the coding agent, how to build and verify one slice, and
how the work lands in Git. `Cratis.Grip.Kits` checks one the way Studio does:

```bash
dotnet add package Cratis.Grip.Kits
```

```csharp
using Cratis.Grip.Kits;

var result = BuildKitValidation.Validate(files); // the kit's files, keyed by path relative to the kit root
foreach (var problem in result.Problems) Console.WriteLine(problem);
```

The package carries the reference kits for .NET on Cratis Arc and Chronicle, Java on Axon Framework and Node.js, which
live in [`Kits`](Kits). Read [Build kits](Documentation/kits/index.md) and
[the build kit format](Documentation/kits/build-kit-format.md).

## Packages

| Package | Registry | What it is |
| --- | --- | --- |
| `Cratis.Grip.Contract` | NuGet | The build order, outcome and progress documents, their JSON Schemas, version negotiation and validation |
| `Cratis.Grip.Kits` | NuGet | The build kit manifest schema, kit validation, reproducible kit archives and the reference kits |

Both target .NET 10 and .NET 11. Grip is versioned with [semantic versioning](https://semver.org): every pull request that
changes what is shipped carries one of the `major`, `minor` or `patch` labels, and merging it releases the packages and the
images at the next version.

## Repository layout

| Folder | Contents |
| --- | --- |
| [`Source/DotNET`](Source/DotNET) | The libraries, with their specifications beside the code |
| [`Docker`](Docker) | The Dockerfiles of the builder images and the Eventmodelers adapter |
| [`Kits`](Kits) | The reference build kits |
| [`Documentation`](Documentation) | The documentation |

## Contributing

To build and test:

```bash
dotnet build
dotnet test
```

You need the [.NET 11 SDK](https://dotnet.microsoft.com/download/dotnet/11.0) and, for the Eventmodelers adapter, Node.js.
Anything that changes what a consumer compiles against or runs needs a `major`, `minor` or `patch` label on its pull request;
the rest carries `no-release`. For everything else, read [contributing](https://github.com/Cratis/.github/blob/main/contributing.md),
and join the [Discord](https://discord.gg/kt4AMpV8WV) with questions.

## License

Cratis Grip is free software distributed under the [MIT License](LICENSE).
