# Cratis builder

The Cratis builder image: the Cratis CLI with Git, Node.js, a JDK and Maven, so the builder Studio runs in the cloud runs anywhere Docker does. It installs the latest released Cratis CLI, and the Axon and Eventmodelers builders beside it are built on top of it.

```bash
cratis build run --docker
```

The image is published to Docker Hub as `cratis/builder` by the Publish workflow with every Studio release, tagged with the Studio version and `latest`. `cratis build run --docker` uses the newest version, tells you when a newer one is out, and `cratis update` pulls it.

The user documentation is `Documentation/developers/builders/local/cratis.md`.
