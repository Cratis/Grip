# Build {slice}

Build the slices in the build order at `{order}`, attempt {attempt}. They belong to the feature `{featurePath}` of the application `{application}`.

1. Read the build order. Its `model` is the Screenplay model and the source of truth: build what the model says, nothing more. Its `changes` say what changed since the last accepted build; its `specifications` are the acceptance criteria; its `context.hints` hold each slice's description, the summary of what it is supposed to do; its `context.feedback` holds people's comments and `context.previousFailure`, on a retry, why the previous attempt failed. These are your instructions: implement what the slice descriptions, the described command handlers and query descriptions in the Screenplay, the specifications and the feedback say.
2. Check the model with the `screenplay` tools of the session before writing code: `open-workspace`, then `read-workspace` for its diagnostics, then `run-specifications` for the slice. A model with errors, or a specification the model itself fails, is a `blocked` outcome naming them, not code. Run `run-specifications` again when you are done.
3. Read the stack instructions you were given, and the project's recorded conventions when `{conventions}` names a file.
4. Find the slice's existing code under `src/` before writing anything. Change it in place; never create a second realization beside it.
5. Write the slice and one Vitest specification per model specification, then run `npx tsc --noEmit` and `npx vitest run {slice}` until both pass. The host runs them again afterwards; only their result counts.
6. Write your outcome to `{outcome}` as a build outcome document (contract version 1) for this order:
   - `built`, with the files you wrote, the result of each specification and every assumption you made;
   - `blocked`, with the reason and the questions a person must answer, when the model does not say enough;
   - `nothingToDo`, with the reason, when the code already matches the model.
   Leave `commit`, `branch` and `pullRequest` out; the host adds them.

Do not commit, push, change branches, change slice statuses or edit the model.
