import 'reflect-metadata';
import { ChronicleClient, ChronicleOptions } from '@cratis/chronicle';

// The service's entry point. Each built slice registers its command handlers and read models here.
const client = new ChronicleClient(ChronicleOptions.development());
const store = await client.getEventStore(process.env.EVENT_STORE ?? 'NodeService');

export { store };

process.on('SIGTERM', () => client.dispose());
