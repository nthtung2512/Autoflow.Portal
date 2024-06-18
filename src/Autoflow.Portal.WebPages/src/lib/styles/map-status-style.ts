import type { BotStatus } from '$lib/models/BotStatus';

const STATUS_BACKGORUND: { [id in BotStatus]: string } = {
	running: 'bg-success',
	error: 'bg-error',
	online: 'bg-primary',
	offine: 'bg-neutral',
	canceled: 'bg-warning',
} as const;

export { STATUS_BACKGORUND };
