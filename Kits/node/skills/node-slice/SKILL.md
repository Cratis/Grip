---
name: node-slice
description: Use when building a State Change or State View slice of a Screenplay model in a Node.js TypeScript service on the Cratis Chronicle client. Not for editing the model.
---

# Building a Node slice

## State change

```typescript
import { eventType } from '@cratis/chronicle';
import { field } from '@cratis/fundamentals';

@eventType()
export class InvoicePaid {
    @field(String) invoiceId: string;
    @field(Number) amount: number;
    constructor(invoiceId: string, amount: number) {
        this.invoiceId = invoiceId;
        this.amount = amount;
    }
}

export type Refusal = { refused: string };

export const decide = (command: PayInvoice, state: InvoiceState): object[] | Refusal =>
    state.paid ? { refused: 'The invoice is already paid' } : [new InvoicePaid(command.invoiceId, command.amount)];
```

The specification gives prior events, folds them into state, decides and asserts:

```typescript
describe('pay invoice', () => {
    it('refuses an invoice that is already paid', () => {
        const state = [new InvoicePaid('42', 100)].reduce(evolve, initial);
        expect(decide({ invoiceId: '42', amount: 100 }, state)).toEqual({ refused: 'The invoice is already paid' });
    });
});
```

## State view

`evolve(state, event)` is a pure function switching on the event's class. Specifications fold the given events and compare the state with `toEqual`.

## Traps

- Two `@eventType()` classes with the same class name collide; name events after the fact, uniquely.
- Every event property needs `@field(Type)`; a property the client cannot type fails registration.
- `import 'reflect-metadata'` exactly once, in `src/index.ts`.
