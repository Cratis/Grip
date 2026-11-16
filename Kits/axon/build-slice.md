# Build {slice}

Build the slices in the build order at `{order}`, attempt {attempt}. They belong to the feature `{featurePath}` of the application `{application}`.

1. Read the build order. Its `model` is the Screenplay model and the source of truth. Its `specifications` are the acceptance criteria; its `context.hints` hold each slice's description, the summary of what it is supposed to do; its `context.feedback` holds people's comments and `context.previousFailure`, on a retry, why the previous attempt failed. These are your instructions: complete the bodies as the slice descriptions, the described command handlers and query descriptions in the Screenplay, the specifications and the feedback say.
2. The renderer has already generated the application. Find what it left for the slices: bodies listed in `UNSUPPORTED.md`, `runtime/UnsupportedImplementations.java` and warnings naming `STAGE-AXON-UNSUPPORTED-SEMANTICS` or `STAGE-AXON-0403`.
3. Read the stack instructions you were given, and the project's recorded conventions when `{conventions}` names a file.
4. Complete those bodies in place, in the slot the renderer left, as the specifications require. Do not edit anything else the renderer owns.
5. Run `./mvnw -q -B test` until it passes.
6. Write your outcome to `{outcome}` as a build outcome document (contract version 1) for this order: `built` with the files you wrote, each specification's result and your assumptions; `blocked` with the reason and the questions a person must answer; or `nothingToDo` when the code already matches the model. Leave `commit`, `branch` and `pullRequest` out.

Do not commit, push, change branches, change slice statuses or edit the model.
