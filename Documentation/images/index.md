---
title: Builder images
description: The Cratis builder images on Docker Hub - the Cratis CLI with the tools a build needs, and the Axon and Eventmodelers builders on top of it - and how to run and build them.
---

A builder that runs in Docker needs the same tools wherever it runs. Grip publishes three images to Docker Hub with every
release, each for `linux/amd64` and `linux/arm64` so they run on Apple Silicon too.

| Image | What it is | Source |
| --- | --- | --- |
| [`cratis/builder`](https://hub.docker.com/r/cratis/builder) | The Cratis CLI with Git, Node.js, a JDK and Maven, on the .NET SDK | `Docker/cratis` |
| [`cratis/studio-axon-builder`](https://hub.docker.com/r/cratis/studio-axon-builder) | `cratis/builder` with the `axon` kit, for Java applications on Axon Framework | `Docker/axon` |
| [`cratis/studio-eventmodelers-builder`](https://hub.docker.com/r/cratis/studio-eventmodelers-builder) | `cratis/builder` with an adapter for projects built with the Eventmodelers BuildKit | `Docker/eventmodelers` |

The image names are unchanged from when they were published with Studio, so existing `--docker` runs keep working.

## Run a builder in Docker

The Cratis CLI starts the image for you. It mounts the project at `/workspace` and passes the builder's connection
through the environment:

```bash
cratis build run --docker
cratis build run --level code --target axon --kit axon --docker
cratis build run --docker --image cratis/studio-eventmodelers-builder
```

With `--target axon`, `--docker` picks `cratis/studio-axon-builder`. Without `--image` it picks `cratis/builder`. The CLI
uses the newest version, tells you when a newer one is out, and `cratis update` pulls it.

## Versions

Each release is tagged with its [semantic version](https://semver.org), for example `1.4.2`, and with `latest` unless
it is a prerelease. The Axon and Eventmodelers images are built `FROM cratis/builder` of the same release, through the
`BUILDER_VERSION` build argument, so one release is one consistent set.

`cratis/builder` installs the latest released Cratis CLI at build time. Pass `CLI_VERSION` to pin another one.

## Build an image yourself

Build from the repository root, because the Axon image copies the reference kit from `Kits/axon`:

```bash
docker build -f Docker/cratis/Dockerfile -t cratis/builder:latest .
docker build -f Docker/axon/Dockerfile -t cratis/studio-axon-builder:latest .
docker build -f Docker/eventmodelers/Dockerfile -t cratis/studio-eventmodelers-builder:latest .
```

To pin the CLI, add `--build-arg CLI_VERSION=<version>` to the first command.

## The Eventmodelers adapter

`Docker/eventmodelers/eventmodelers-builder.mjs` is a generator for `cratis build run --exec`. It writes the build order's
slices where the [Eventmodelers BuildKit](https://github.com/Nebulit-GmbH/Eventmodelers-Build-Kits) looks for work, runs the
kit's own agent loop, and reports what the kit did as the build outcome. The adapter does not contain or copy the kit,
which is licensed separately: install it in the project with `npx @eventmodelers/cli init`. Run the adapter's tests with
`npm test` in `Docker/eventmodelers`.
