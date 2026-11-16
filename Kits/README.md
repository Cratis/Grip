# Build kits

The reference build kits. Each folder is one kit: a `build-kit.yaml` manifest and the files it names. They are embedded in
the `Cratis.Grip.Kits` package, which Cratis Studio serves at `/api/builders/kits/<name>`; `cratis build init --kit <name>`
installs one. The `axon` kit is also copied into the `cratis/studio-axon-builder` image.

| Kit | Stack |
| --- | --- |
| `cratis-csharp` | .NET on Cratis Arc and Chronicle with a React frontend; every slice type and user interfaces |
| `axon` | Java on Axon Framework, rendered by the Axon Stage renderer; every slice type, backend only |
| `node` | Node.js in TypeScript on the Cratis Chronicle client; state changes and state views |

The format is documented in [the build kit format](../Documentation/kits/build-kit-format.md). A specification validates every
kit here against the published schema, so a broken kit fails the build rather than reaching a builder.
