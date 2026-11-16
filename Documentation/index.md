---
title: Cratis Grip
description: Cratis Grip holds the builder side of Cratis Studio - the container images that run a builder anywhere Docker does, and the build contract and build kit format a builder and Studio share.
---

Cratis Studio decides what gets built: it turns an event model into build orders, hands them to a *builder*, and settles
each slice from what the builder reports. A builder is anything that turns slices into code. Grip is what you need to
run one, or to write one.

It has three parts:

| Part | Use it when | Start with |
| --- | --- | --- |
| Container images | You want the Cratis builder to run anywhere Docker does, locally or in your own pipeline | [Builder images](images/index.md) |
| `Cratis.Grip.Contract` | You write a builder or a host and need to read build orders and write outcomes | [The build contract](contract/index.md) |
| `Cratis.Grip.Kits` | You write or validate a build kit, or serve the reference kits | [Build kits](kits/index.md) |

The container images are the ones `cratis build run --docker` starts. The two libraries hold the code Studio itself
uses to write build orders, read outcomes and validate kits, so a builder and Studio cannot drift apart.

## How it fits together

```text
Studio ──build order──▶ builder (the Cratis CLI, in Docker or on your machine)
   ▲                         │ runs the kit's prompt and verify commands
   └─────build outcome───────┘
```

- Studio writes a **build order** with `Cratis.Grip.Contract`, validated against the published JSON Schema of its
  contract version.
- The builder reads the order, builds with the help of a **build kit**, and answers with an **outcome**.
- Studio reads the outcome only through the same library, so a document that does not match its schema is refused with
  the reasons, and nothing is read from it.

## Related

- [Builder images](images/index.md)
- [The build contract](contract/index.md)
- [Build orders and outcomes](contract/orders-and-outcomes.md)
- [Build kits](kits/index.md)
- [The build kit format](kits/build-kit-format.md)
