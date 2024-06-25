<script lang="ts">
    import { createEventDispatcher, onMount } from 'svelte';
    import "$lib/app.css"
    // Replace these with your actual imports
    import { createMessageStore } from '../stores/messageStore';
    import type { Conversation, Message, SecondColumnData, User } from '$lib/types/interfaces';
    export let senderUserId;
    export let selectedReceiver: User | null;
    export let selectedConversation: Conversation | null;
    export let handleSendMessage;
    export let messagesForConversation: Message[];
    export let handleDeleteMessage;
    const messagesStore = createMessageStore();

    // // Get all messages by selected conversation id
    // let messagesForConversation: Message[] = [];
    // $: messagesStore.messagesForConversation.subscribe((maps) => {
    //     messagesForConversation = maps;
    // });
    // $: selectedConversation && messagesStore.getMessagesForConversationId(selectedConversation.id);
    // $: {
    //     console.log('Check messagesForConversation', messagesForConversation);
    // }
    // $: {
    //     console.log('Check SOS', messagesStore.messagesForConversation);
    // }

    let newMessage = '';
  </script>

<div class="w-3/4">
    {#if selectedConversation && selectedReceiver}
      <div class="relative w-full box-border h-screen p-10 flex flex-col ">
        <div class="flex gap-10">
          <h2 class="text-2xl font-semibold">{selectedReceiver?.username}</h2>
          <button on:click={() => selectedReceiver = null} on:click={() => selectedConversation = null} class="text-blue-500">Close</button>
        </div>
        <div class="flex flex-col-reverse overflow-auto h-[800px]">
          <div class="mt-4 mb-7 flex flex-col flex-1 gap-4 " >
            {#each messagesForConversation as message}
              <div class={message.sendUserId === senderUserId ? "flex justify-end" : "block"}>
                <button class={message.sendUserId === senderUserId ? "block" : "hidden"} on:click={()=>handleDeleteMessage(message)}>X</button>
              <div>
                <div
                  class="inline-block p-2 rounded-lg mb-2 max-w-3/4 w-fit"
                  class:bg-blue-500={message.sendUserId === senderUserId}
                  class:text-white={message.sendUserId === senderUserId}
                  class:text-right={message.sendUserId === senderUserId}
                  class:bg-gray-200={message.sendUserId !== senderUserId}
                  class:text-black={message.sendUserId !== senderUserId}
                >
                  {message.content}
                </div>
              </div>
              </div>
            {/each}
          </div>
        </div>
        <div class="sticky bottom-0 flex items-center w-full">
          <input
            bind:value={newMessage}
            placeholder="Type your message..."
            class="px-4 py-2 flex flex-1 border border-gray-400 rounded"
          />
          <button
            on:click={() => handleSendMessage(senderUserId, selectedReceiver?.id, newMessage, selectedConversation?.id)}
            on:click={() => newMessage = ''}
            disabled={newMessage.trim() === ''}
            class="bg-blue-500 text-white px-4 py-2 rounded ml-4"
          >
            Send
          </button>
        </div>
      </div>
    {:else}
      <p class="text-center text-gray-500 p-10">Select a receiver to start chatting</p>
    {/if}
  </div>