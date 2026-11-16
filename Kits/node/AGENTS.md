# Node — stack instructions

You build slices of an event model in a Node.js service written in TypeScript on the Cratis Chronicle client (`@cratis/chronicle`), specified with Vitest. The build order names the slices; its Screenplay model is the source of truth. The code follows the model, never the other way round.

## Layout

- One vertical slice per folder: `src/<module>/<feature>/<Slice>/`, module and feature folders in kebab case, the slice folder named exactly as the model names the slice (`src/orders/ordering/PlaceOrder/`).
- A slice holds `<Slice>.ts` with its events, decision or projection and handler, and `<Slice>.spec.ts` with its specifications, both named exactly as the model names the slice: the host verifies with `npx vitest run <Slice>`, which only finds files whose path contains that name.
- Slices are built on separate branches, so an event another slice also needs may not be in the project yet. An event type that more than one slice of the model uses goes in a file of its own, `src/<module>/<feature>/shared/<EventName>.ts`, holding only that declaration exactly as the model states it; every slice that needs it writes the same file at the same path, so the branches merge instead of declaring it twice.
- `src/index.ts` is the entry point; register each slice's handler or read model there.

## Names

- Before declaring an event or read model, look for it in the project: a slice that uses what an earlier slice declared imports it and never declares it again.
- Every name comes from the model: commands, events, read models and their properties keep exactly the names the model gives them, and a model `validate` rule refuses with the model's message.

## State change slices

- Each event is a class decorated with `@eventType()` from `@cratis/chronicle`, named in the past tense, with every property declared with `@field(Type)` from `@cratis/fundamentals`. Events never carry their own event source id and have no optional properties: a missing value means a second event.
- The decision is a pure function, `decide(command, state)`, that returns the events to append or a refusal `{ refused: '<reason>' }`. It never throws for a business rule.
- The handler validates the command, reads the state it needs, calls `decide` and appends with `store.eventLog.append(eventSourceId, event)`. It checks `result.isSuccess` and reports constraint violations as refusals.

## State view slices

- The read model is a plain type. Its state is computed by a pure function, `evolve(state, event)`, folded over the events.
- Register it with Chronicle as a reducer or a projection when the service serves it.

## Specifications

- Write one Vitest specification for every specification in the model, named after it: `describe('<slice>')` with one `it` per specification.
- A state change specification gives prior events, folds them into state, calls `decide` with the command and asserts the exact events or the refusal.
- A state view specification folds the given events with `evolve` and asserts the resulting state.
- Specifications never need a running Chronicle. They are never weakened, skipped or deleted to make code pass.

## Working

- Build exactly the slices in the order. Never change a slice's status and never edit the model, and never copy the model or its `.screenplay` state into the project; read it where the order names it.
- Run `npx tsc --noEmit` and `npx vitest run <Slice>` until both pass.
- Do not commit, push or switch branches. The builder host runs the verification commands and handles Git after you are done.
- Never write credentials, tokens or `.env` values into files, prompts or logs.
- Report what you could not build, and why, in the outcome file instead of guessing.
