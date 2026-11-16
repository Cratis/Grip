---
title: The build contract
description: Cratis.Grip.Contract reads and writes the build orders, outcomes and progress reports Cratis Studio and a builder exchange, and validates each against its published JSON Schema.
---

`Cratis.Grip.Contract` is the code behind the [build order and outcome documents](orders-and-outcomes.md). Add it to a
.NET host or builder that talks to Studio:

```bash
dotnet add package Cratis.Grip.Contract
```

Everything the contract exchanges is a JSON document that names its `contractVersion`. The library validates each
document against that version's schema before it reads anything from it, and refuses it with the reasons otherwise.
The schemas are embedded in the package, and a released schema is never changed: a new version is a new file beside it.

## Choose a contract version

A builder says which versions it reads; the newest one both sides speak wins:

```csharp
using Cratis.Grip.Contract;

var chosen = BuildContractNegotiation.Choose(["1", "2"]);
```

`Choose` returns a `Result`: the version, or a `BuildContractNotShared` that lists what the builder accepts and what
the library speaks.

## Write an order

```csharp
var written = BuildOrderWriter.Write(order);
```

The order is checked against the schema of its `ContractVersion` first, so a host never hands a builder a document the
builder could refuse.

## Read an outcome or a progress report

```csharp
var outcome = BuildOutcomeReader.Read(text, expectedOrderId: orderId);
var progress = BuildProgressReader.Read(text, expectedOrderId: orderId);
```

Both check that the text is JSON, names a version the library speaks, answers the expected order and matches the
schema. An outcome is one of `BuiltOutcome`, `BlockedOutcome`, `FailedOutcome` or `NothingToDoOutcome`.

## Read a schema

```csharp
string? text = BuildContractSchemas.Text(BuildContractSchemas.Order, BuildContract.Version3);
```

## Versions

| Version | Adds |
| --- | --- |
| `1` | The build order and the build outcome |
| `2` | Build progress, token usage on outcomes, the API a backend build exposes, and the backend handed to a UI order |
| `3` | The `removal` part: an order to remove what was built for slices deleted from the model |

The full description of every document, with examples, is in [Build orders and outcomes](orders-and-outcomes.md).
