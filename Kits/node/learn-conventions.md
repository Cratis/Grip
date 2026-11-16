# Learn this project's conventions

This project was not scaffolded by the kit. Before any slice is built here, record how this project is written so every later build follows it.

Read `package.json`, the TypeScript and test configuration, two or three existing features end to end and their tests. Do not change any file except the one below.

Write `{conventions}` as short Markdown with these sections, each a list of facts you saw, with one example path per fact:

- **Packages**: the module system, the TypeScript settings that matter, and the libraries for events, persistence, HTTP and tests.
- **Slices**: where a behavior lives and how its files are split and named.
- **Commands and events**: how intents are validated, decided and recorded.
- **Read models**: how state is derived from events and served.
- **Tests**: the framework, the folder and naming pattern, and the command that runs one feature's tests.
- **Verification**: the exact type-check and test commands that must pass.

Where the project differs from the kit's stack instructions, the project wins; say so under **Differences**. Do not guess: write "not seen" for anything you did not find.
