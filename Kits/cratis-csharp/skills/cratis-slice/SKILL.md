---
name: cratis-slice
description: Use when building a State Change, State View, Automation or Translation slice of a Screenplay model in a Cratis Arc and Chronicle application. Not for editing the model.
---

# Building a Cratis slice

Pick the shape by the slice type in the build order.

| Slice type | Backend in `<Slice>.cs` | Specifications |
| --- | --- | --- |
| `stateChange` | `[Command]` record with `Handle()` returning the event; `CommandValidator<T>` for each rule; `[EventType]` records; concepts | `CommandScenario<T>`: the happy path asserts each appended event; one spec per rule asserting `ShouldNotBeSuccessful()` and `ShouldHaveValidationErrors()` |
| `stateView` | `[ReadModel]` record with model-bound projection attributes and static query methods | `ReadModelScenario<T>`: given the events, the read model holds the expected state |
| `automation` | `IReactor` handling the trigger event and executing the follow-up command through `ICommandPipeline`; `[OnceOnly]` on non-idempotent effects | `ReactorScenario<T>` for the invocation, plus the invoked command's specs |
| `translation` | A reactor or an ingestion adapter that records outside data as our own events | As automation |

## Order of work

1. Backend file, then `dotnet build` (this also generates the TypeScript proxies).
2. Specifications, one per model specification, then `dotnet test --filter "FullyQualifiedName~<Slice>"`.
3. For the ui part: the React component from the generated proxies, registered in `App.tsx`.

## Traps

- A `[Command]` without a public instance `Handle()` is not discovered.
- A query on a read model must be `public static`; returning `ISubject<T>` makes it observable.
- An event property that may be missing means a second event, not a nullable property.
- Never call `.AutoMap()`; matching names map by default.
