---
title: Build orders and outcomes
description: The versioned JSON contract Studio hands a builder to say what to build, and the outcome a builder answers with, with examples of every document.
---

A *builder* is anything that turns slices of an event model into code: Studio's own generation, a coding agent, or a team's own tool on any stack. Studio and a builder speak through two JSON documents, both described by JSON Schema and both versioned together:

- a **build order**, from Studio to the builder: what to build, described in Screenplay, with the context the builder needs;
- a **build outcome**, from the builder back to Studio: what came of it.

One contract serves every builder, Studio's own generation included, so the built-in path and a third-party builder cannot drift apart. Both documents are validated against their schema before Studio reads anything from them.

**Writing a generator for the Cratis CLI?** Your generator reads the order from the file named by `STUDIO_BUILD_ORDER`, and writes an outcome with only `kind` and the fields of that kind: the CLI adds `contractVersion` and `orderId`, and the `commit` and `branch` once it has committed. The CLI reads version 1 of this contract. See [Build with a generator](https://docs.cratis.studio/developers/builders/build/deterministic#the-generator-contract). A coding agent run with `--agent` is told where the order is and how to write its outcome; see [Build with AI](https://docs.cratis.studio/developers/builders/build/with-ai). The rest of this page is the full contract, for reference.

## The schemas

Studio serves every published schema at a stable address on its own host, and never changes one once it is released. No sign-in is needed, because the schemas hold no organization data:

| Document | Address |
| --- | --- |
| Build order, version 1 | `/api/builders/contract/1/build-order.schema.json` |
| Build outcome, version 1 | `/api/builders/contract/1/build-outcome.schema.json` |
| Build order, version 2 | `/api/builders/contract/2/build-order.schema.json` |
| Build outcome, version 2 | `/api/builders/contract/2/build-outcome.schema.json` |
| Build progress, version 2 | `/api/builders/contract/2/build-progress.schema.json` |
| Build order, version 3 | `/api/builders/contract/3/build-order.schema.json` |
| Build outcome, version 3 | `/api/builders/contract/3/build-outcome.schema.json` |
| Build progress, version 3 | `/api/builders/contract/3/build-progress.schema.json` |

Generate types from them in your builder's language, or validate the documents you receive and send. A new version is published beside the old one.

## Versions

A builder says which contract versions it reads. Studio offers it an order only in one of them, the newest both speak, and offers nothing when they share none. Every document names its version in `contractVersion`.

Version 2 keeps everything in version 1 and adds:

- the **build progress** document, which a builder sends while it builds (see [Reporting progress](#reporting-progress));
- `usage` on every outcome: the `inputTokens` and `outputTokens` an agent builder used for the attempt;
- `api` on a `built` outcome: the commands and queries a backend build exposes, each with its `kind` (`command` or `query`), `name`, and optionally the HTTP `method` and `route`;
- `backend` on a `ui` order: for each target slice, what the backend builder reported (`commit`, `branch`, `pullRequest`, `files`) and the `api` it exposes. Where the backend builder reported no API, Studio describes it from the slice's command and queries, by name.

A builder that reads only version 1 keeps working unchanged.

Version 3 keeps everything in version 2 and adds the **removal order** (see [Removing a deleted slice](#removing-a-deleted-slice)): an order with `part` set to `removal`, no slices to build, and the slices deleted from the feature listed as `removed` changes. Outcomes and progress reports are the same as in version 2. A builder that reads only versions 1 and 2 is never handed a removal order; the removal is listed in the next build of the feature instead.

## A build order

An order targets one or more slices of one feature. It carries:

- **`scope`**: the application, the feature with the module and parent features it sits in, and the target slices with their type and status. Ids are stable across renames.
- **`model`**: the Screenplay `.play` documents and, when available, the canonical compiled model with its `schemaVersion` and `revision`.
- **`changes`**: per target slice, whether it was `added`, `changed`, `renamed`, `moved` or `removed` since the builder's last accepted revision, and which of its declarations changed. A builder that has never had an outcome accepted gets every slice as `added`.
- **`specifications`**: each slice's Given/When/Then as acceptance criteria, by specification id. The full specification, with its values, is in the Screenplay documents.
- **`context`**: conventions and hints, people's comments on the slices, the attempt number and why the previous attempt failed. `hints` carries the description of each target slice that has one, the modeler's summary of what the slice is supposed to do, as `<slice name>: <description>`. The description is not in the `.play` documents. A builder that hands the work to an AI agent should give the agent these hints as instructions, together with the described handlers and queries in the Screenplay documents, the specifications and the feedback. A removal order carries no hints.
- **`part`**: `backend`, or `ui` once the backend is built and the slice is waiting for a builder that builds user interfaces, or, from version 3, `removal` (see below).
- **`deadline`**: when Studio stops waiting.

An order never carries a credential. A builder that pushes uses credentials of its own today; Studio handing out short-lived, repository-scoped access is proposed but not available yet. The schema refuses any member it does not declare.

```json
{
    "contractVersion": "1",
    "orderId": "6f0c3d5e-2b1a-4f7e-9a51-0d2c8e4b7a10",
    "issued": "2026-11-06T09:00:00+00:00",
    "deadline": "2026-11-06T10:00:00+00:00",
    "part": "backend",
    "scope": {
        "application": { "id": "0b9d1f0e-6a4c-4c1e-8f5a-3e2d1c0b9a87", "name": "Shop" },
        "feature": { "id": "4a7e2c91-5d3b-4e8f-a1c2-9b8d7e6f5a40", "name": "Invoicing", "path": ["Billing"] },
        "slices": [
            { "id": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f", "name": "PayInvoice", "type": "stateChange", "status": "readyForImplementation" }
        ]
    },
    "model": {
        "language": "screenplay",
        "schemaVersion": "1",
        "revision": "rev-17",
        "documents": [
            { "path": "Billing.play", "content": "module Billing\n  feature Invoicing\n    ..." }
        ],
        "canonical": null
    },
    "changes": [
        { "sliceId": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f", "kind": "changed", "declarations": ["command Pay Invoice", "specification Paying an open invoice"] }
    ],
    "specifications": [
        {
            "sliceId": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f",
            "id": "8e1f2a3b-4c5d-4e6f-8a9b-0c1d2e3f4a5b",
            "name": "Paying an open invoice",
            "given": ["event Invoice Issued"],
            "when": "command Pay Invoice",
            "then": ["event Invoice Paid"]
        }
    ],
    "context": {
        "conventions": ["Amounts are decimals in the invoice currency"],
        "hints": ["PayInvoice: Lets a customer pay an open invoice in full"],
        "feedback": [
            { "sliceId": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f", "text": "Partial payments come later", "occurred": "2026-11-05T14:12:00+00:00" }
        ],
        "attempt": 2,
        "previousFailure": "The build timed out"
    }
}
```

## Removing a deleted slice

When a slice that was built is deleted from the board, Studio does not wait for the next build of its feature. It offers a removal-only build of its own straight away, to the builder that builds the slice's backend. Like any build it is claimed under a lease, retried when it fails and blocked when its attempts run out.

A removal order has `part` set to `removal`, an empty `scope.slices`, no `specifications`, and lists in `changes` every slice deleted from the feature whose code is still there, each as `removed` with the name it had (`previousName`) and the `declarations` that were built for it. The model is the feature as it is now, without them. Remove what you built for those slices, and nothing else. See [the example](#example-a-removal-order).

Answer it like any order:

| `kind` | What happens to the removal |
| --- | --- |
| `built` | Carried out, with the commit, branch or pull request that removed the code |
| `nothingToDo` | Carried out: the code was already gone |
| `blocked` | Blocked with your reason and questions; it is not offered again, and the next build of the feature still lists it |
| `failed` | Offered again with the next attempt number, or blocked once attempts run out |

A slice that is restored before a builder claims its removal withdraws it, so nothing is removed. A removal a builder already holds finishes first. Builds of other slices in the feature keep listing the removals still waiting, so a removal is carried out by whichever build gets to it first. Studio's own generation removes the files of deleted slices the next time it generates their feature.

## A build outcome

An outcome answers one order, by its `orderId`, and is exactly one of four kinds:

| `kind` | Means | Carries |
| --- | --- | --- |
| `built` | The slices are built | at least one of `commit`, `branch`, `pullRequest`; the `files` the builder owns; a `result` per specification (`passed`, `failed`, `notRun`); the `assumptions` it made |
| `blocked` | The slices cannot be built until someone answers | the `reason` and the `questions` that need an answer |
| `failed` | The attempt went wrong | the `error`, and whether a retry makes sense (`retryable`) |
| `nothingToDo` | Nothing needed building | nothing more |

```json
{
    "contractVersion": "1",
    "orderId": "6f0c3d5e-2b1a-4f7e-9a51-0d2c8e4b7a10",
    "kind": "built",
    "pullRequest": "https://github.com/acme/shop/pull/42",
    "branch": "build/pay-invoice",
    "files": ["Source/Billing/Invoicing/PayInvoice/PayInvoice.cs"],
    "specifications": [
        { "id": "8e1f2a3b-4c5d-4e6f-8a9b-0c1d2e3f4a5b", "result": "passed" }
    ],
    "assumptions": ["An invoice that is already paid is refused"]
}
```

```json
{
    "contractVersion": "1",
    "orderId": "6f0c3d5e-2b1a-4f7e-9a51-0d2c8e4b7a10",
    "kind": "blocked",
    "reason": "The model does not say which payment provider settles the invoice",
    "questions": ["Which payment provider?", "Are partial payments allowed?"]
}
```

In version 2, any outcome may also carry `usage`, and a `built` outcome may carry `api`, as in [the example with usage and API](#example-a-built-outcome-with-usage-and-api).

Studio validates every outcome against the schema of its version before it reads anything from it. A malformed outcome is refused with the reasons, each naming where in the document it went wrong, and changes nothing. More examples follow at the end of this page.

## Blocked and waiting slices

A slice can be **Blocked**, with a reason and optionally questions, and **Waiting** for a builder that builds user interfaces. People set both from the slice's status picker, and builders through Studio. Both show on the board and in the progress board. Unblocking a slice, or changing it, moves it back to Ready for implementation. Studio's own generation does not take a blocked or waiting slice.

## Reporting progress

While it holds an order, a builder speaking version 2 or later can report what it is doing. Each report names the order and carries at least one of:

- `step`: what it is doing now, in a few words;
- `log`: the lines written since its previous report, oldest first, at most 100, each with when it was written (`occurred`), its `level` (`information`, `warning` or `error`) and its `text`;
- `usage`: the tokens used for the attempt so far, in total.

```json
{
    "contractVersion": "2",
    "orderId": "6f0c3d5e-2b1a-4f7e-9a51-0d2c8e4b7a10",
    "step": "Running the specifications",
    "log": [
        { "occurred": "2026-11-06T09:12:03+00:00", "level": "information", "text": "dotnet test Billing.Specs" }
    ],
    "usage": { "inputTokens": 31877, "outputTokens": 4410 }
}
```

A report does not renew the lease; keep sending heartbeats. Like a heartbeat, a report for an order whose lease was lost is refused, which tells the builder to stop.

**Never send a credential.** Studio keeps the step, the log, errors, block reasons and assumptions for good and shows them on the board, so before it keeps any of them it removes what looks like a credential (tokens, keys, passwords, private keys, credentials in URLs) and cuts each text to 2000 characters. That is a safety net, not permission: strip secrets from tool output before you report it.

## What the board shows

Once a builder has claimed a slice, the slice header shows what it is doing, or how its last build went, and how many assumptions are still to review. Opening it shows the step and log of the build under way, every attempt with its part, builder, outcome, reason, time and tokens, each specification's result beside the board's own run, and the builder's assumptions.

Each specification on the board also shows the builder's result beside its own run, and marks it when the two disagree: the builder says it passes and the board's run fails, or the other way round. Each assumption can be **accepted**, when the builder filled the gap the way it should be filled, or **dismissed**, for example once the model has been changed to say it.

## Separate builders for the backend and the user interface

A slice with screens can be built in two steps: one builder builds its backend, another its user interface. The backend build then ends at **Waiting** instead of Ready for review, and the slice is offered to builders of user interfaces with a `ui` order that says what the backend built and the API it exposes. That happens when:

- the builder that claimed the backend does not build user interfaces, or
- the application names a backend builder and a user interface builder. In the application's building settings, turn on **Build the user interface with a separate builder** and choose the two builders. A named application's slices are then claimed only by the named builder for each part, and any other builder's claim is refused. Turn the setting off to lift that.

A slice without screens is done when its backend is built. A builder that builds both parts, with no other builder named, builds the whole slice in one order as before.

## Examples

Every example is a complete document that validates against the schema of its version.

### Example: a backend order

The order shown [above](#a-build-order) is a version 1 order for the backend part of one slice.

### Example: a user interface order

A `ui` order, in version 2, adds `backend`: what the backend builder reported for each slice, with the API it exposes.

```json
{
    "contractVersion": "2",
    "orderId": "6f0c3d5e-2b1a-4f7e-9a51-0d2c8e4b7a10",
    "issued": "2026-11-06T09:00:00+00:00",
    "deadline": "2026-11-06T10:00:00+00:00",
    "part": "ui",
    "scope": {
        "application": { "id": "0b9d1f0e-6a4c-4c1e-8f5a-3e2d1c0b9a87", "name": "Shop" },
        "feature": { "id": "4a7e2c91-5d3b-4e8f-a1c2-9b8d7e6f5a40", "name": "Invoicing", "path": ["Billing"] },
        "slices": [
            { "id": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f", "name": "PayInvoice", "type": "stateChange", "status": "waiting" }
        ]
    },
    "model": {
        "language": "screenplay",
        "schemaVersion": "1",
        "revision": "rev-17",
        "documents": [
            { "path": "Billing.play", "content": "module Billing\n  feature Invoicing\n    ..." }
        ],
        "canonical": null
    },
    "changes": [
        { "sliceId": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f", "kind": "changed", "declarations": ["command Pay Invoice", "specification Paying an open invoice"] }
    ],
    "specifications": [
        {
            "sliceId": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f",
            "id": "8e1f2a3b-4c5d-4e6f-8a9b-0c1d2e3f4a5b",
            "name": "Paying an open invoice",
            "given": ["event Invoice Issued"],
            "when": "command Pay Invoice",
            "then": ["event Invoice Paid"]
        }
    ],
    "context": {
        "conventions": ["Amounts are decimals in the invoice currency"],
        "hints": ["PayInvoice: Lets a customer pay an open invoice in full"],
        "feedback": [
            { "sliceId": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f", "text": "Partial payments come later", "occurred": "2026-11-05T14:12:00+00:00" }
        ],
        "attempt": 2,
        "previousFailure": "The build timed out"
    },
    "backend": [
        {
            "sliceId": "c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f",
            "branch": "build/pay-invoice",
            "pullRequest": "https://github.com/acme/shop/pull/42",
            "files": ["Source/Billing/Invoicing/PayInvoice/PayInvoice.cs"],
            "api": [
                { "kind": "command", "name": "Pay Invoice", "method": "POST", "route": "/api/billing/invoicing/pay-invoice" },
                { "kind": "query", "name": "Open Invoices" }
            ]
        }
    ]
}
```

### Example: a removal order

A version 3 order with `part` set to `removal`. It has no slices to build, and lists the deleted slice under `changes`.

```json
{
    "contractVersion": "3",
    "orderId": "2b7d9e41-8c3a-4f16-b5e2-7a0c1d9f3e68",
    "issued": "2026-11-10T09:00:00+00:00",
    "deadline": "2026-11-10T10:00:00+00:00",
    "part": "removal",
    "scope": {
        "application": { "id": "0b9d1f0e-6a4c-4c1e-8f5a-3e2d1c0b9a87", "name": "Shop" },
        "feature": { "id": "4a7e2c91-5d3b-4e8f-a1c2-9b8d7e6f5a40", "name": "Invoicing", "path": ["Billing"] },
        "slices": []
    },
    "model": {
        "language": "screenplay",
        "schemaVersion": "1",
        "revision": "rev-21",
        "documents": [
            { "path": "Billing.play", "content": "module Billing\n  feature Invoicing\n    ..." }
        ],
        "canonical": null
    },
    "changes": [
        { "sliceId": "d7e8f9a0-b1c2-4d3e-8f4a-5b6c7d8e9f01", "kind": "removed", "previousName": "VoidInvoice", "declarations": ["command Void Invoice", "event Invoice Voided"] }
    ],
    "specifications": [],
    "context": {
        "conventions": [],
        "hints": [],
        "feedback": [],
        "attempt": 1
    }
}
```

### Example: a built outcome with usage and API

A version 2 outcome. `usage` is the tokens an agent builder used, and `api` is what the backend exposes.

```json
{
    "contractVersion": "2",
    "orderId": "6f0c3d5e-2b1a-4f7e-9a51-0d2c8e4b7a10",
    "kind": "built",
    "branch": "build/pay-invoice",
    "files": ["Source/Billing/Invoicing/PayInvoice/PayInvoice.cs"],
    "specifications": [
        { "id": "8e1f2a3b-4c5d-4e6f-8a9b-0c1d2e3f4a5b", "result": "passed" }
    ],
    "api": [
        { "kind": "command", "name": "Pay Invoice", "method": "POST", "route": "/api/billing/invoicing/pay-invoice" }
    ],
    "assumptions": ["An invoice that is already paid is refused"],
    "usage": { "inputTokens": 48210, "outputTokens": 6120 }
}
```

### Example: a failed outcome

```json
{
    "contractVersion": "1",
    "orderId": "6f0c3d5e-2b1a-4f7e-9a51-0d2c8e4b7a10",
    "kind": "failed",
    "error": "The build timed out after 20 minutes",
    "retryable": true
}
```

### Example: nothing to do

```json
{
    "contractVersion": "1",
    "orderId": "6f0c3d5e-2b1a-4f7e-9a51-0d2c8e4b7a10",
    "kind": "nothingToDo"
}
```

## Related

- [Builders](https://docs.cratis.studio/developers/builders/)
- [Build a builder](https://docs.cratis.studio/developers/builders/build/)
- [The build loop](https://docs.cratis.studio/developers/builders/protocol/build-loop)
- [The builder protocol](https://docs.cratis.studio/developers/builders/protocol/protocol)
- [Use the reference builder: cratis build](https://docs.cratis.studio/developers/builders/configure/cratis-build)
