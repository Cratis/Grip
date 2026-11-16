// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import assert from 'node:assert/strict';
import { describe, it } from 'node:test';
import { Status, eventmodelersSliceType, isSettled, outcomeOf, slug, toSliceFiles } from './slices.mjs';

const order = {
    contractVersion: '1',
    orderId: 'order-1',
    part: 'backend',
    scope: {
        application: { name: 'Shop' },
        feature: { name: 'Ordering', path: ['Sales'] },
        slices: [
            { id: 's1', name: 'Place Order', type: 'stateChange' },
            { id: 's2', name: 'Open Orders', type: 'stateView' }
        ]
    },
    specifications: [{ id: 'p1', sliceId: 's1', name: 'Placing an order' }]
};

describe('toSliceFiles', () => {
    const files = toSliceFiles(order);
    const placeOrder = files.find(file => file.path === 'ordering/placeorder/slice.json').content;

    it('writes a planned slice for each slice of the order', () => {
        assert.equal(files.filter(file => file.path.endsWith('slice.json')).length, 2);
        assert.equal(placeOrder.status, Status.planned);
    });

    it('maps the slice type to the kit\'s', () => {
        assert.equal(placeOrder.sliceType, 'STATE_CHANGE');
        assert.equal(eventmodelersSliceType('translation'), 'TRANSLATION');
        assert.equal(eventmodelersSliceType('unknown'), 'STATE_CHANGE');
    });

    it('carries only the specifications of that slice', () => {
        assert.deepEqual(placeOrder.specifications, [{ id: 'p1', title: 'Placing an order' }]);
        assert.deepEqual(files.find(file => file.path === 'ordering/openorders/slice.json').content.specifications, []);
    });

    it('names the context the kit groups slices by', () => {
        assert.deepEqual(files.find(file => file.path === 'current_context.json').content, { name: 'Ordering' });
    });

    it('slugs names the way the kit names folders', () => assert.equal(slug('Submit Cart'), 'submitcart'));
});

describe('outcomeOf', () => {
    it('is built when every slice is done', () => {
        const outcome = outcomeOf(order, [{ title: 'A', status: Status.done }, { title: 'B', status: Status.review }]);
        assert.equal(outcome.kind, 'built');
        assert.equal(outcome.orderId, 'order-1');
    });

    it('is blocked with the reason when a slice is blocked', () => {
        const outcome = outcomeOf(order, [{ title: 'A', status: Status.blocked, statusReason: 'needs a price' }]);
        assert.equal(outcome.kind, 'blocked');
        assert.match(outcome.reason, /needs a price/);
    });

    it('is nothing while a slice is still planned', () => {
        assert.equal(outcomeOf(order, [{ title: 'A', status: Status.planned }]), null);
        assert.equal(isSettled([{ status: Status.inProgress }]), false);
    });
});
