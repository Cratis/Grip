# Cratis C# — stack instructions

You build slices of an event model in a .NET application on Cratis Arc (commands and queries) and Cratis Chronicle (events, projections, reactors), with a React frontend on Cratis Components. The build order names the slices; its Screenplay model is the source of truth. The code follows the model, never the other way round.

## Layout

- One vertical slice per folder: `<Module>/<Feature>/<Slice>/`. Everything the slice owns lives there: one `<Slice>.cs` holding all its backend artifacts, its React components, and its generated TypeScript proxies.
- Specifications live in `Tests/<Module>/<Feature>/<Slice>/`, one class per specification in the model.
- The namespace mirrors the folders under the project's root namespace (read `<RootNamespace>` in the `.csproj`; the scaffold's is `CratisApp`). Never add a `Features` segment.
- Match an existing slice of the same type before inventing a shape.

## Backend rules

- Before declaring a concept, event or read model, look for it in the project: a slice that uses what an earlier slice declared references that declaration (`using` its namespace) and never declares it again. Two declarations of one event or concept fail at runtime once both slices are merged.
- Slices are built on separate branches, so a declaration another slice also needs may not be in the project yet. Read the model to decide where it lives: a concept or an event that more than one slice of the model uses goes in a file of its own, `<Module>/<Feature>/Shared/<Name>.cs` (namespace `<RootNamespace>.<Module>.<Feature>.Shared`), holding only that declaration exactly as the model states it. Every slice that needs it writes the same file at the same path, so the branches merge instead of declaring it twice. Only what one slice alone uses stays in `<Slice>.cs`.
- Every name comes from the model: commands, events, concepts, read models, queries and their properties keep exactly the names the model gives them. Never add a suffix such as `Event` or `Command`.
- A model `authorize` becomes `[Authorize]` (roles with `[Roles("<role>")]`) on the command or query; a model `validate` rule becomes a validator rule with the model's message.
- Commands are `[Command]` records with a public `Handle()` on the record. `Handle()` returns the event (or a tuple or list of events) to append. Never inject the event log.
- Validation goes in a `CommandValidator<T>` or a `ConceptValidator<T>`. Never throw for a business refusal; return a `ValidationResult` (or `Result<TEvent, ValidationResult>` when the rule needs a read model injected into `Handle()`).
- Events are `[EventType]` records with no attribute arguments, named in the past tense, with no nullable properties. An event never carries its own event source id.
- Every domain value is a `ConceptAs<T>`; every identity that keys a stream is an `EventSourceId`-derived concept. No raw `Guid`, `string` or `int` in the domain.
- Read models are `[ReadModel]` records built by model-bound projection attributes (`[FromEvent<T>]`, `[Key]`, `[ChildrenFrom<T>]`, `[RemovedWith<T>]`). Never call `.AutoMap()`. Queries are public static methods on the read model.
- Uniqueness the model states is an `IConstraint` (or `[Unique]`), never a query.
- Automations are `IReactor` classes. A non-idempotent side effect is `[OnceOnly]`. A reactor appends follow-up work by executing a command through `ICommandPipeline`.
- Personal data is `[PII]`; secrets are `[Encrypted]` and, on commands, `[NotAudited]`. Never weaken either.

## Specifications

- Write one executable specification for every specification in the model, named after it, plus one per validation rule asserting the command is refused with a validation error.
- `Tests/` uses xUnit `[Fact]` methods with the Cratis scenarios: `CommandScenario<T>` (`Cratis.Arc.Testing.Commands`, with `ShouldHaveAppendedEvent` from `Cratis.Arc.Chronicle.Testing.Commands`) for commands, `ReadModelScenario<T>` (`Cratis.Chronicle.Testing.ReadModels`) for projections, `EventScenario` for constraints and `ReactorScenario<T>` for reactors.
- The specification class's namespace contains the slice's name, so `dotnet test --filter "FullyQualifiedName~<Slice>"` runs exactly that slice's specifications.
- Specifications are never weakened, skipped or deleted to make code pass.

## Frontend (only for the ui part)

- `dotnet build` generates the TypeScript proxies. Build the backend first; never reference a proxy before it exists, and never edit a generated proxy.
- Pages use `DataPage` from Cratis Components; dialogs use `CommandDialog`. No other modal library.
- Register a new page in `App.tsx`.

## Working

- Build exactly the slices in the order. Never change a slice's status and never edit the model, and never copy the model or its `.screenplay` state into the project; read it where the order names it.
- Do not commit, push or switch branches. The builder host runs the verification commands and handles Git after you are done.
- Never write credentials, tokens or `.env` values into files, prompts or logs.
- Report what you could not build, and why, in the outcome file instead of guessing.
