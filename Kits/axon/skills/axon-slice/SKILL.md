---
name: axon-slice
description: Use when completing the parts of a Screenplay slice that the Axon Stage renderer leaves for judgment in a Java Axon Framework application. Not for editing the model or for files the renderer owns.
---

# Completing an Axon slice

The renderer writes the application; you complete what it leaves.

1. Open `UNSUPPORTED.md` and `runtime/UnsupportedImplementations.java` at the project root. Each entry names the slice and the slot.
2. Edit only the slot. A command handler returns the events it decides on, a reaction effect receives its input and the event's `occurred`, a query performer returns a `List<Model>`.
3. Keep bodies LF-only, with qualified names for extra types and no Unicode escapes.
4. Run `./mvnw -q -B test`. A failing specification means the body is wrong, not the specification.
