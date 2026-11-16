// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Turns a Studio build order into the slice files the Eventmodelers BuildKit works from, and reads what the kit did to them
// back as a build outcome. Pure functions: nothing here touches the disk or a process.

const sliceTypes = {
    stateChange: 'STATE_CHANGE',
    stateView: 'STATE_VIEW',
    automation: 'AUTOMATION',
    translation: 'TRANSLATION'
};

// The kit's own names for where a slice is in its work.
export const Status = Object.freeze({
    planned: 'Planned',
    inProgress: 'InProgress',
    done: 'Done',
    review: 'Review',
    waiting: 'Waiting',
    blocked: 'Blocked'
});

export function slug(name) {
    return String(name).toLowerCase().replace(/[^a-z0-9]+/g, '');
}

export function eventmodelersSliceType(type) {
    return sliceTypes[type] ?? 'STATE_CHANGE';
}

// One slice.json for each slice of the order, under the context the kit groups slices by: the feature.
export function toSliceFiles(order) {
    const context = order.scope.feature.name;
    const specifications = order.specifications ?? [];
    const files = order.scope.slices.map(slice => ({
        path: `${slug(context)}/${slug(slice.name)}/slice.json`,
        content: {
            id: slice.id,
            status: Status.planned,
            title: slice.name,
            context,
            sliceType: eventmodelersSliceType(slice.type),
            specifications: specifications
                .filter(specification => specification.sliceId === slice.id)
                .map(specification => ({ id: specification.id, title: specification.name })),
            studio: { orderId: order.orderId, application: order.scope.application.name, featurePath: order.scope.feature.path ?? [], part: order.part },
            commands: [],
            events: [],
            readmodels: []
        }
    }));
    files.push({ path: 'current_context.json', content: { name: context } });
    files.push({ path: `${slug(context)}/context.json`, content: { name: context } });
    return files;
}

// What the kit left the slices in decides the outcome. A slice the kit has not finished is no outcome at all.
export function outcomeOf(order, slices) {
    const head = { contractVersion: order.contractVersion ?? '1', orderId: order.orderId };
    const blocked = slices.filter(slice => slice.status === Status.blocked);
    if (blocked.length > 0) {
        return { ...head, kind: 'blocked', reason: blocked.map(slice => `${slice.title}: ${slice.statusReason ?? 'the kit could not build it'}`).join(' '), questions: [] };
    }

    const finished = [Status.done, Status.review, Status.waiting];
    if (slices.length > 0 && slices.every(slice => finished.includes(slice.status))) {
        return { ...head, kind: 'built', files: [], specifications: [], assumptions: [] };
    }

    return null;
}

export function isSettled(slices) {
    return slices.length > 0 && slices.every(slice => [Status.done, Status.review, Status.waiting, Status.blocked].includes(slice.status));
}

export function failure(order, error, retryable = true) {
    return { contractVersion: order.contractVersion ?? '1', orderId: order.orderId, kind: 'failed', error, retryable };
}
