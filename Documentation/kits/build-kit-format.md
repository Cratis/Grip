---
title: Use the reference builder
description: Run the reference builder host from the Cratis CLI, and describe a stack once as a build kit so a builder can build slices with it without writing a loop.
---

`cratis build`, in the [Cratis CLI](https://www.cratis.io/cli/), is the reference builder. It holds the whole conversation with Studio: connecting, waiting for approval, asking for work, keeping the claim alive, running verification, committing, pushing and reporting the outcome. What turns a slice into code is yours to choose, in [three ways](https://docs.cratis.studio/developers/builders/build/#three-ways-to-build): a coding agent with `--agent` ([Build with AI](https://docs.cratis.studio/developers/builders/build/with-ai)), any program in any language with `--exec` ([Build with a generator](https://docs.cratis.studio/developers/builders/build/deterministic)), or a generator that a coding agent runs and completes ([Deterministic with AI on top](https://docs.cratis.studio/developers/builders/build/deterministic-and-ai)). A stack author describes a stack once as a [build kit](#build-kits).

```text
cratis build connect --studio --organization <name>
cratis build init --kit <name|git url|folder> [--agent <agent>]
cratis build run --agent claude|codex|gemini|copilot|opencode|cursor|acp
cratis build run --exec "<command>"
cratis build run --modeling --board <board> --agent <agent> [--unattended]
cratis build once --order order.json --exec "<command>" | --agent <agent>
cratis build status
cratis build service install|status|uninstall
```

This page describes how the CLI works with Studio. The CLI's own [build reference](https://www.cratis.io/cli/reference/build/) lists every option, and [Build a builder](https://docs.cratis.studio/developers/builders/build/) shows how to build with an agent, a generator, or both.

## What Studio exposes

Everything below is served by Studio Core. The contract documents, kits and session guidance need no sign-in; they hold no organization data. Work is only handed to a registered builder, through the builder registration and connection endpoints (see [The builder protocol](https://docs.cratis.studio/developers/builders/protocol/protocol)).

| Purpose | Address | Page |
| --- | --- | --- |
| Build order schema | `GET /api/builders/contract/{version}/build-order.schema.json` | [Build orders and outcomes](https://docs.cratis.studio/developers/builders/build/orders-and-outcomes) |
| Build outcome schema | `GET /api/builders/contract/{version}/build-outcome.schema.json` | [Build orders and outcomes](https://docs.cratis.studio/developers/builders/build/orders-and-outcomes) |
| Kit manifest schema | `GET /api/builders/kits/schema/{version}/build-kit.schema.json` | [Build kits](#build-kits) |
| Registered kits | `GET /api/builders/kits` | [Build kits](#build-kits) |
| One kit's manifest | `GET /api/builders/kits/{name}` | [Build kits](#build-kits) |
| One kit as an archive | `GET /api/builders/kits/{name}/archive` | [Build kits](#build-kits) |
| Validate a kit | `POST /api/builders/kits/validate` | [Build kits](#build-kits) |
| Agent session descriptor | `GET /api/builders/session` | [Agent sessions](https://docs.cratis.studio/developers/live-modeling#agent-sessions) |
| Screenplay guidance archive | `GET /api/builders/session/screenplay-session.zip` | [Agent sessions](https://docs.cratis.studio/developers/live-modeling#agent-sessions) |
| Live modeling on a board | MCP tools `studio_connect_local_agent`, `studio_poll_local_agent`, `studio_take_local_agent_prompt`, `studio_answer_local_agent_prompt`, `studio_disconnect_local_agent` | [Live modeling with your own agent](https://docs.cratis.studio/developers/live-modeling) |

Studio decides what gets built and settles every slice's status; see [the build loop](https://docs.cratis.studio/developers/builders/protocol/build-loop). A builder never sets a status itself.

## The commands

### `connect`

```text
cratis build connect --studio [<url>] --organization <name> [--name <name>] [--mcp-client-id <id>] [--force] [--no-wait]
```

Registers the machine as a builder of an organization with a short code a maintainer approves in Studio, and waits until they do. `--studio` alone means the hosted Studio, `https://app.cratis.studio`. The builder's credential is kept in the operating system's credential store, never in the project. `--mcp-client-id` also stores the organization's MCP credential, so an agent gets Studio's tools. See [Connect a builder and approve it](https://docs.cratis.studio/developers/builders/configure/connect-and-approve).

### `init`

Installs a kit into a project:

```text
cratis build init --kit <name|git url> [--agent <agent>] [--kit-folder <path>]
```

A value matching the kit name rule (lowercase words joined by hyphens) is a registered kit; an existing folder is a kit on disk; anything else is a Git URL (`https://…`, `ssh://…` or `git@host:owner/repo.git`), optionally followed by `#<branch, tag or commit>`. `--kit-folder` names the folder inside the repository that holds `build-kit.yaml`; without it the manifest is at the repository's root.

1. A registered kit: `GET /api/builders/kits/{name}` for the manifest, then `GET /api/builders/kits/{name}/archive`, refused unless the SHA-256 of the body equals `X-Checksum-Sha256`. A kit from Git: a shallow clone of the ref into a temporary folder outside the project, then the kit is validated; a kit with problems stops `init` with every problem shown and nothing copied. The CLI validates with the manifest schema and the checks listed under [Validating a kit](#validating-a-kit), the same checks Studio runs.
2. Every command in `requires` is checked for and its version compared; a missing or too old command stops `init`.
3. In an empty project with `scaffold`, the scaffold is copied and its `commands` run in the project, in order; any non-zero exit stops `init`. In a project that is not empty, the scaffold is not copied: the kit needs `existingCodebase`, otherwise `init` stops. With `existingCodebase`, the `learn` prompt runs once (on `init` or the first build) and the conventions are kept at the kit's `conventions` path.
4. The `instructions` and `skills` are installed where the chosen agent reads them:

   | `--agent` | Instructions | Skills |
   | --- | --- | --- |
   | `claude` | `CLAUDE.md` | `.claude/skills/<skill>/` |
   | `gemini` | `GEMINI.md` | `.agents/skills/<skill>/` |
   | `codex`, `cursor`, `copilot`, `opencode` (the default) | `AGENTS.md` | `.agents/skills/<skill>/` |

   The kit's instructions are written between the lines `<!-- cratis-build-kit:<name> -->` and `<!-- /cratis-build-kit:<name> -->`, followed by one line per skill naming its `SKILL.md` and its `description`. A file that already exists keeps everything outside that block; running `init` again replaces only the block. This is the only guidance a kit leaves in the project, and only because the kit asks for it; the Screenplay guidance never is (see [Agent sessions](https://docs.cratis.studio/developers/live-modeling#agent-sessions)).
5. The kit is recorded in `.cratis/build/kit.json`: `name`, `version`, `sha256` of the archive (or `source` with the Git URL and the resolved commit), `agent`, and `files`, every project path `init` wrote. `run`, `once` and `service install` read the kit and the agent from this file. A kit is optional: without one, name the agent with `--agent` or the generator with `--exec`.

### `run`

```text
cratis build run [--agent <agent> | --exec <command>] [--agent-command <command>] [--kit <path>] [--project <path>] [--verify <command>]... [--once] [--no-push] [--no-guidance]
```

Connects, tells Studio what it can build, asks for work, and for every slice it is given:

1. switches to the branch Studio names for a Studio-managed build, or to the kit's `git.branch` otherwise, and writes the order to a folder outside the project, with the order's Screenplay documents in a `model` folder beside it;
2. hands the order to the agent, as described in [Agent sessions](https://docs.cratis.studio/developers/live-modeling#agent-sessions), with the kit's filled prompt, or runs the `--exec` command (see [The generator contract](https://docs.cratis.studio/developers/builders/build/deterministic#the-generator-contract)); it renews the lease while the work goes on, and stops without reporting anything when Studio says the lease is lost;
3. answers the agent's permission requests from the kit's `permissions`, refusing everything else, and never starts an agent with its permission checks switched off;
4. on `built` or `nothingToDo`, runs every `verify` command of the kit and each `--verify`, in order, in the project; any non-zero exit makes the outcome `failed` and retryable, never `built`;
5. on `built`, commits the files the outcome lists (or every changed file when it lists none) that the kit permits, with Studio's commit message for a Studio-managed build or the kit's `git.commit` otherwise, pushes the branch with your own Git credentials unless `--no-push` is given, and adds the commit and branch to the outcome;
6. reports the outcome, keeping only what the build contract declares for its kind;
7. puts anything left uncommitted away in a Git stash named after the order, never discarding it, and switches back to the branch it started on.

The project must be a Git repository without uncommitted changes. `--once` stops after one slice, and `Ctrl+C` stops the loop.

What the builder tells Studio it can build comes from the kit: its name, its slice types (all of them without a kit), whether it builds user interfaces (`ui`), and the contract versions it shares with the CLI. Without a kit, the builder builds backends of every slice type. The CLI reads version 1 of [build orders and outcomes](https://docs.cratis.studio/developers/builders/build/orders-and-outcomes), so it does not send progress reports, and Studio lists a deleted slice in the next build of its feature instead of sending it a removal order.

With `--modeling`, the agent models on a board instead of building slices; see [Live modeling with your own agent](https://docs.cratis.studio/developers/live-modeling).

An agent that stops without writing its outcome, as some do when one of their actions is refused, is asked to go on up to twice before the build fails. Local tools such as Screenplay's are handed to an agent that only connects to MCP servers over HTTP, such as Copilot CLI, on a loopback address with a token of the session's own.

Credentials go to the agent session and the Git process, never into a prompt, an order, an outcome or a log.

### `once`

```text
cratis build once --order <file> [--agent <agent> | --exec <command>] [--outcome <file>] [--verify <command>]... [--push]
```

Builds one order from a file without connecting to Studio: steps 1 to 5 and 7 of `run`, without pushing unless `--push` is given. The outcome is written to the standard output, or to `--outcome`. It exits with `5` when the outcome is `failed`, so it fits in a CI job. Use it to try a generator, an agent or a kit.

### `status`

Shows the Studio and the organization the machine is registered with, whether the builder credential and Studio's MCP credential are stored, and the kit and agent of the current folder. It changes nothing.

### `service`

Runs the build loop of a project as a user service that starts at sign-in and restarts when it stops, so a machine stays a builder without a terminal open: a launchd agent on macOS and a systemd user unit on Linux. Windows has no supported service manager; run `cratis build run` there.

```bash
cratis build service install --agent claude
cratis build service install --exec "$HOME/slice-sheets/generate.sh"
cratis build service status
cratis build service uninstall
```

`install` takes the options of `run` and needs a machine registered with `connect`, and a kit, `--agent` or `--exec`. The service gets the `PATH` of the shell that installed it and no credentials; the builder token stays in the credential store. Each project gets its own service.

## Build kits

A *build kit* describes a stack to a builder host such as `cratis build`: how a new project is created, what the coding agent is told, how one slice is built and verified, and how the work lands in Git. A kit holds no loop, connection or retry code. The host does all of that the same way for every kit, and the [build loop](https://docs.cratis.studio/developers/builders/protocol/build-loop) in Studio decides what gets built.

```text
my-kit/
├── build-kit.yaml        # the manifest
├── AGENTS.md             # stack instructions
├── build-slice.md        # the prompt for one build order
├── learn-conventions.md  # optional: learning an existing codebase
├── skills/               # optional: one folder per skill, each with a SKILL.md
└── scaffold/             # optional: the files of a new project
```

### The manifest

The manifest is `build-kit.yaml` (or the same document as `build-kit.json`) at the root of the kit. It is described by a JSON Schema that Studio publishes at a stable address and never changes once released:

| Document | Address |
| --- | --- |
| Build kit manifest, version 1 | `/api/builders/kits/schema/1/build-kit.schema.json` |

```yaml
kitVersion: "1"
name: cratis-csharp
version: 1.0.0
description: Event-sourced .NET applications on Cratis Arc and Chronicle.
contract:
  versions: ["1"]
screenplay:
  schemaVersions: ">=38"
sliceTypes: [stateChange, stateView, automation, translation]
ui: true
scaffold:
  path: ./scaffold
  commands:
    - dotnet restore CratisApp.slnx
instructions: ./AGENTS.md
skills: ./skills
prompt: ./build-slice.md
verify:
  - dotnet build $(ls *.slnx *.sln 2>/dev/null | head -n 1) -warnaserror
  - dotnet test $(ls *.slnx *.sln 2>/dev/null | head -n 1) --no-build --filter "FullyQualifiedName~{slice}" -- RunConfiguration.TreatNoTestsAsError=true
git:
  branch: "studio/{feature}"
  commit: "Build {slice}"
permissions:
  paths: ["**"]
  commands: [dotnet build, dotnet test, git status]
existingCodebase:
  learn: ./learn-conventions.md
  conventions: .cratis/build/conventions.md
requires:
  - command: dotnet
    version: ">=11"
```

| Member | Required | Meaning |
| --- | --- | --- |
| `kitVersion` | yes | The manifest format version, the string `"1"`. |
| `name` | yes | Lowercase words joined by hyphens, at most 64 characters. Registered kits are installed by it. |
| `version` | yes | The kit's own semantic version. |
| `description` | yes | One sentence on the stack. |
| `contract.versions` | no | The [build order and outcome](https://docs.cratis.studio/developers/builders/build/orders-and-outcomes) versions the prompt is written for. Defaults to `["1"]`. |
| `screenplay.schemaVersions` | no | The compiled Screenplay schema versions the kit understands: `">=38"`, `"=40"` or `">=38 <50"`. |
| `sliceTypes` | yes | One or more of `stateChange`, `stateView`, `automation`, `translation`. A host takes no order for another type. |
| `ui` | no | Whether the kit also builds the `ui` part of an order. Defaults to `false`. |
| `scaffold` | no | A kit folder copied into a new project, as `./scaffold` or `{ path, commands }`. `commands` run in the new project afterwards. |
| `instructions` | yes | The stack instructions, a Markdown file, installed in the form the chosen agent reads. |
| `skills` | no | A folder holding one folder per skill, each with a `SKILL.md`. |
| `prompt` | yes | The prompt template for one build order. |
| `verify` | yes | Commands the host runs, in order, after the agent is done. A slice is reported built only when every one exits with 0. |
| `git.branch`, `git.commit` | no | Templates for the branch to build on and the commit message. |
| `permissions.paths` | no | Globs, relative to the project, the agent may write. Defaults to everything in the project. |
| `permissions.commands` | no | Command prefixes the agent may run. The `verify` commands are always allowed. Everything else is refused. |
| `existingCodebase` | no | How the kit builds into a project it did not scaffold; see below. |
| `requires` | no | Commands the host checks for, each with an optional minimum version such as `">=10"`. |

Every path inside the kit starts with `./` and never leaves the kit; every path inside the project is relative and never leaves it. A manifest member the format does not declare is refused, so a manifest never carries a token or a secret.

### Placeholders

The prompt, the learn prompt, the `verify` commands and the Git templates may use these placeholders. The host fills them from the build order. In a `verify` command, a value with spaces or shell characters is quoted. `permissions.commands` are not filled: a permitted command is a prefix, so permit `node tools/generate.mts` and have the prompt run it with `{order}` after it, as [Deterministic with AI on top](https://docs.cratis.studio/developers/builders/build/deterministic-and-ai) does. Any other name in single braces refuses the kit; double braces and braces with spaces, as in JSON or code, are left alone.

| Placeholder | Filled with |
| --- | --- |
| `{order}` | The path of the build order document the host wrote for this build |
| `{outcome}` | The path the agent writes its outcome document to |
| `{slice}` | The name of the first target slice, as written in the model |
| `{slices}` | The names of every target slice, separated by commas |
| `{sliceId}` | The identifier of the first target slice |
| `{feature}` | The name of the feature the slices belong to |
| `{featurePath}` | The modules and parent features down to the feature, joined with `/` |
| `{application}` | The name of the application |
| `{part}` | `backend` or `ui` |
| `{attempt}` | The attempt number, starting at 1 |
| `{conventions}` | The project path of the recorded conventions, or empty when the kit records none |

### What the agent does, and what the host does

The agent only writes code and an outcome. The host does everything else, so a kit cannot get it wrong:

1. The host writes the build order to `{order}` and starts the agent with the instructions, the skills, the Screenplay guidance and the tools for the session (see [Agent sessions](https://docs.cratis.studio/developers/live-modeling#agent-sessions)), and the filled prompt.
2. The agent writes code and then an outcome document to `{outcome}`: `built` with its files, specification results and assumptions; `blocked` with a reason and questions; or `nothingToDo`. It leaves `commit`, `branch` and `pullRequest` out.
3. The host runs every `verify` command. If one fails, the outcome is `failed` with the command and its exit code, whatever the agent wrote.
4. When verification passes and the outcome is `built`, the host commits the agent's files on the `git.branch`, adds the commit to the outcome and reports it.

Permission requests from the agent are answered from `permissions`; the host never runs an agent with its permission checks switched off.

### Existing codebases

A kit with `existingCodebase` can build into a project it did not scaffold. On the first build in such a project the host runs the `learn` prompt once: the agent reads the project and records its conventions at the `conventions` path, changing nothing else. Every later build passes that path as `{conventions}`, and the agent follows what was recorded. Delete the file to have the conventions learned again.

### Registered kits

Studio registers the reference kits by name. Each is one archive a host unpacks as is.

| Kit | Stack | Slice types | User interfaces |
| --- | --- | --- | --- |
| `cratis-csharp` | .NET on Cratis Arc and Chronicle, React on Cratis Components | all four | yes |
| `axon` | Java on Axon Framework, rendered by the Axon Stage renderer, JUnit | all four | no |
| `node` | Node.js in TypeScript on the Cratis Chronicle client, Vitest | state change, state view | no |

These addresses need no sign-in; kits hold no organization data and no credentials.

#### List the kits

```text
GET /api/builders/kits
```

`200 OK`, `Cache-Control: no-store`:

```json
[
  {
    "name": "cratis-csharp",
    "version": "1.0.0",
    "description": "Event-sourced .NET applications on Cratis Arc and Chronicle, with a React frontend on Cratis Components.",
    "sliceTypes": ["stateChange", "stateView", "automation", "translation"],
    "ui": true,
    "contractVersions": ["1"],
    "sha256": "9f2c…"
  }
]
```

#### Read a kit's manifest

```text
GET /api/builders/kits/{name}
```

`200 OK` with the manifest as JSON, exactly as written in the kit, or `404 Not Found` for a name Studio does not register.

#### Download a kit

```text
GET /api/builders/kits/{name}/archive
```

`200 OK` with `Content-Type: application/zip` and the header `X-Checksum-Sha256` holding the lowercase hex SHA-256 of the body, which also matches `sha256` in the listing. Entries are paths relative to the kit's root, such as `build-kit.yaml` and `scaffold/Program.cs`. The same kit always gives the same bytes. Verify the checksum before unpacking. `404 Not Found` for an unknown name.

### Installing from a Git URL

A host installs a kit that is not registered from a Git URL: it clones the repository (or the folder the URL names), finds `build-kit.yaml` or `build-kit.json` at its root, and validates the kit before anything is copied into the project.

### Validating a kit

A kit author, or a host installing a kit from Git, sends the kit as a zip:

```text
POST /api/builders/kits/validate
Content-Type: application/zip
```

The manifest may be at the root of the archive or inside one top folder, the way a Git host lays out an archive. The archive may be at most 5 MB, hold at most 2000 files and unpack to at most 20 MB, and no entry may leave the kit. The answer is always `200 OK`, `Cache-Control: no-store`:

```json
{
  "valid": false,
  "problems": [
    "/sliceTypes/1: Value should match one of the values specified by the enum",
    "/prompt: {apiToken} is not a placeholder a host fills; use one of {order}, {outcome}, …",
    "/skills: the skill other has no SKILL.md."
  ],
  "kit": null
}
```

Each problem starts with where it is: a JSON pointer into the manifest, or `(document)`. A valid kit answers `"valid": true`, no problems and `kit` in the same shape as a listing entry. Validation checks the manifest against the schema, that every file and folder it names is in the kit and not empty, that every skill folder has a `SKILL.md`, and that every placeholder is one from the table above.

## Related

- [Connect a builder and approve it](https://docs.cratis.studio/developers/builders/configure/connect-and-approve)
- [Build a builder](https://docs.cratis.studio/developers/builders/build/): three ways to build, with a coding agent, a generator of your own, or both
- [Build with AI](https://docs.cratis.studio/developers/builders/build/with-ai): write a build kit of your own, line by line
- [The `cratis build` reference](https://www.cratis.io/cli/reference/build/) in the CLI documentation
- [Talk to Studio directly](https://docs.cratis.studio/developers/builders/protocol/), when you cannot run the CLI
- [The build loop](https://docs.cratis.studio/developers/builders/protocol/build-loop)
- [The builder protocol](https://docs.cratis.studio/developers/builders/protocol/protocol)
- [Live modeling with your own agent](https://docs.cratis.studio/developers/live-modeling)
