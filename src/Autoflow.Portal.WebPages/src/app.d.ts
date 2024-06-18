// See https://kit.svelte.dev/docs/types#app
// for information about these interfaces
declare global {
	namespace App {}

	declare namespace svelteHTML {
		interface HTMLAttributes<T> {
			'on:clickoutside'?: CompositionEventHandler<T>;
		}
	}
}

export {};
