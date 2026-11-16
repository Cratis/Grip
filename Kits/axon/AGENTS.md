# Axon — stack instructions

You build slices of an event model into a Java application on Axon Framework (Spring Boot, Maven). The application is rendered from the Screenplay model by the Axon Stage renderer (`cratis render --target axon`): the renderer writes the structure, the model stays the source of truth, and code follows the model, never the other way round.

## What the renderer owns

- Files that carry a managed header, and everything under `target/`, belong to the renderer. Do not edit them: the next render replaces your edit or refuses it. Change the model's intent through the model, which only people do.
- What the renderer cannot place is left for you: method bodies the model only describes, and anything it reports as `STAGE-AXON-UNSUPPORTED-SEMANTICS`, `STAGE-AXON-0403` or lists in `UNSUPPORTED.md`. Complete exactly those, in the slot the renderer left.

## Layout

- One vertical slice per package under `src/main/java`, the package named by module, feature and slice as the renderer lays it out. Specifications are JUnit tests under `src/test/java` beside it.
- Reuse before you write: an event, record or enum another slice already declared is imported, never declared again.

## Conventions

- Events are immutable records with past-tense names and no optional properties. A missing value means a second event.
- A command handler decides from the state it reads and appends events through the appender it is given; it does not touch another slice's store or sink.
- Inline Java bodies are LF-only, use qualified names for extra types, and must not use a Unicode escape.

## Specifications

- Every specification in the model is a JUnit test. Never weaken, skip or delete one to make code pass.
- Run `./mvnw -q -B test` until it passes. The host runs it again afterwards; only its result counts.

## Working

- Build exactly the slices in the order. Never change a slice's status, never edit the model, never commit, push or switch branches: the host does that.
- Never write credentials, tokens or environment values into files, prompts or logs.
- Report what you could not build, and why, in the outcome file instead of guessing.
