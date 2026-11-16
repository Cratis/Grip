# Eventmodelers builder

Lets a Studio organization build slices with the [Eventmodelers BuildKit](https://github.com/Nebulit-GmbH/Eventmodelers-Build-Kits) already installed in a project. `eventmodelers-builder.mjs` is a generator for `cratis build run --exec`: it writes the build order's slices where the kit looks for work (`.slices/<context>/<slice>/slice.json`, status `Planned`), runs the kit's own agent loop locally, and reports what the kit did as the build outcome.

The adapter does not contain or copy any of the kit, which is licensed separately; install it in the project with `npx @eventmodelers/cli init`.

`Dockerfile` beside this file packages the adapter on `cratis/builder` (`Builders/cratis`). The Publish workflow pushes it to Docker Hub as `cratis/studio-eventmodelers-builder` with every Studio release, tagged with the Studio version and `latest`; run it with `cratis build run --docker --image cratis/studio-eventmodelers-builder`.

The documentation is `Documentation/developers/builders/eventmodelers/index.md`. It is published but not listed in the navigation. Run the tests with `npm test`.
