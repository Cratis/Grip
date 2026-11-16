---
applyTo: "**/*"
---

## Grip - project context

Grip is the builder side of Cratis Studio, and it is public. It holds the builder container images, the build contract and the build kit format.

- **The images keep their names** (`cratis/builder`, `cratis/studio-axon-builder`, `cratis/studio-eventmodelers-builder`) and their `linux/amd64` and `linux/arm64` builds. The Cratis CLI picks them by name and runs the newest version, so a release's version has to be higher than the one before it.
- **A released JSON Schema never changes.** A new contract or kit format version is a new schema file beside the old one, and `BuildContract.SupportedVersions` or `BuildKitSchema.SupportedVersions` names it.
- **The `$id` of a published schema is its identity.** Do not rename it when the repository or the namespace moves.
- **The reference kits under `Kits/` are embedded in `Cratis.Grip.Kits`**, and the Axon image copies `Kits/axon`. A specification validates every kit, so a broken kit fails the build.
- **Docker builds run from the repository root.** The `.dockerignore` keeps only `Docker/` and `Kits/` in the context.
- **Libraries target `net10.0` and `net11.0`** and double as their own specification projects in Debug; Release is the strict build that is packed.
- **Releases follow the pull request label** (`major`, `minor`, `patch` or `no-release`) through `cratis/release-action`; read [pull-requests.md](./pull-requests.md) before writing a description, which is published verbatim as the release notes.
