# Axon builder

The packaged builder for Java applications on Axon Framework. It is the Cratis builder at the Screenplay, Code, Code and AI or AI level with the `axon` render target, from the Axon Stage renderer, plus the `axon` kit in `Kits/axon` that tells it how to verify (`./mvnw -q -B test`) and what it may write.

```bash
cratis build run --level code --target axon --kit axon --docker
```

With `--target axon`, `--docker` picks the `cratis/studio-axon-builder` image at its newest version. The image is defined in `Dockerfile` beside this file, built on `cratis/builder` (`Builders/cratis`), and published to Docker Hub by the Publish workflow with every Studio release, tagged with the Studio version and `latest`.

The user documentation is `Documentation/developers/builders/local/axon.md`.
