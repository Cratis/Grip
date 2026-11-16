# Learn this project's conventions

This project was not rendered by the kit. Before any slice is built here, record how it is written so every later build follows it.

Read `pom.xml`, the Spring configuration, two or three existing features end to end and their tests. Do not change any file except the one below.

Write `{conventions}` as short Markdown with these sections, each a list of facts you saw, with one example path per fact:

- **Build**: the Java version, the Axon Framework version, and the Maven wrapper or profile in use.
- **Slices**: where a behavior lives and how its packages and files are named.
- **Commands and events**: how intents are validated, decided and recorded.
- **Read models**: how state is derived from events and served.
- **Tests**: the framework, the folder and naming pattern, and the command that runs one feature's tests.
- **Verification**: the exact commands that must pass.

Where the project differs from the kit's stack instructions, the project wins; say so under **Differences**. Do not guess: write "not seen" for anything you did not find.
