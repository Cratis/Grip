#!/usr/bin/env node
// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// A Studio builder for projects built with the Eventmodelers BuildKit. `cratis build run --exec` hands it a build order; it
// writes the order's slices where the kit looks for work, lets the kit's own agent loop build them, and reports what the kit
// did as the build outcome. The kit itself is not part of this repository: install it in the project with
// `npx @eventmodelers/cli init` and this builder runs it as it is.

import { spawn } from 'node:child_process';
import { existsSync, mkdirSync, readFileSync, readdirSync, rmSync, writeFileSync, appendFileSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { failure, isSettled, outcomeOf, toSliceFiles } from './slices.mjs';

const orderFile = process.env.STUDIO_BUILD_ORDER;
const outcomeFile = process.env.STUDIO_BUILD_OUTCOME;
const project = process.cwd();
const kitDirectory = process.env.EVENTMODELERS_KIT_DIR ?? join(project, '.build-kit');
const agentCommand = process.env.EVENTMODELERS_AGENT_COMMAND;
const runner = process.env.EVENTMODELERS_RUNNER ?? (agentCommand ? 'ralph-exec.js' : 'ralph-claude.js');
const timeoutMs = Number(process.env.EVENTMODELERS_TIMEOUT_MINUTES ?? 30) * 60_000;
const slicesDirectory = join(kitDirectory, '..', '.slices');

function write(outcome) {
    writeFileSync(outcomeFile, JSON.stringify(outcome));
}

function readSlices() {
    const slices = [];
    const visit = directory => {
        if (!existsSync(directory)) return;
        for (const entry of readdirSync(directory, { withFileTypes: true })) {
            const path = join(directory, entry.name);
            if (entry.isDirectory()) visit(path);
            else if (entry.name === 'slice.json') slices.push(JSON.parse(readFileSync(path, 'utf8')));
        }
    };
    visit(slicesDirectory);
    return slices;
}

function keepOutOfGit() {
    const exclude = join(project, '.git', 'info', 'exclude');
    if (!existsSync(dirname(exclude))) return;
    const current = existsSync(exclude) ? readFileSync(exclude, 'utf8') : '';
    for (const entry of ['.slices/', '.build-kit/']) {
        if (!current.includes(entry)) appendFileSync(exclude, `${current.endsWith('\n') || current === '' ? '' : '\n'}${entry}\n`);
    }
}

if (!orderFile || !outcomeFile) {
    console.error('Run this through `cratis build run --exec`: it names the order and the outcome files.');
    process.exit(2);
}

const order = JSON.parse(readFileSync(orderFile, 'utf8'));

if (!existsSync(join(kitDirectory, runner))) {
    write(failure(order, `The Eventmodelers BuildKit is not installed in the project: ${join(kitDirectory, runner)} is missing. Install it with "npx @eventmodelers/cli init".`, false));
    process.exit(0);
}

rmSync(slicesDirectory, { recursive: true, force: true });
for (const file of toSliceFiles(order)) {
    const target = join(slicesDirectory, file.path);
    mkdirSync(dirname(target), { recursive: true });
    writeFileSync(target, JSON.stringify(file.content, null, 2));
}
keepOutOfGit();

// The kit works on the board when it is online. Here it works on the files only.
const environment = { ...process.env, RALPH_LOCAL: '1', ...(agentCommand ? { RALPH_EXEC_CMD: agentCommand } : {}) };
const kit = spawn('node', [join(kitDirectory, runner), project], { cwd: kitDirectory, env: environment, stdio: ['ignore', 'inherit', 'inherit'] });

let stopped = false;
const stop = () => {
    if (stopped) return;
    stopped = true;
    kit.kill('SIGTERM');
    setTimeout(() => kit.kill('SIGKILL'), 5000).unref();
};

const deadline = Date.now() + timeoutMs;
const watch = setInterval(() => {
    const slices = readSlices();
    if (isSettled(slices)) {
        clearInterval(watch);
        stop();
        write(outcomeOf(order, slices) ?? failure(order, 'The kit settled the slices without finishing them.'));
        process.exit(0);
    }
    if (Date.now() > deadline) {
        clearInterval(watch);
        stop();
        write(failure(order, `The Eventmodelers BuildKit did not finish within ${timeoutMs / 60_000} minutes.`));
        process.exit(0);
    }
}, 3000);

kit.on('exit', code => {
    if (stopped) return;
    clearInterval(watch);
    const slices = readSlices();
    write(outcomeOf(order, slices) ?? failure(order, `The Eventmodelers BuildKit stopped with exit code ${code} before finishing the slices.`));
    process.exit(0);
});
