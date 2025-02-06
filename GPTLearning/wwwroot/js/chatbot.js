function chatbot() {
    return {
        showWelcome: false,
        showChat: false,
        userMessage: '',
        chatMessages: [],
        showImage: false,

        // Initialize the chat UI
        init() {
            setTimeout(() => this.showWelcome = true, 500);
            setTimeout(() => { this.showWelcome = false; this.showChat = true }, 4000);
            setTimeout(() => this.showImage = true, 3500);
        },

        // Handle sending a message
        async sendMessage() {
            if (this.userMessage.trim() !== '') {
                // Add user message
                this.chatMessages.push({
                    id: Date.now(),
                    sender: 'user',
                    content: this.userMessage
                });

                // Clear input field
                this.userMessage = '';

                // Wait for DOM update, then scroll to bottom
                this.$nextTick(() => {
                    this.$refs.chatContainer.scrollTop = this.$refs.chatContainer.scrollHeight;
                });

                // Send user message to API to get bot response
                this.getBotResponse(this.userMessage);
            }
        },

        // Fetch bot's response from the API
        async getBotResponse(userMessage) {
            try {
                const response = await fetch('/api/chat/ask', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(userMessage) // Sending the user input as plain string
                });

                if (response.Ok) {
                    console.log("test");
                    const data = await response.json();
                    console.log(data);
                    // Add bot's response to the chat
                    this.chatMessages.push({
                        id: Date.now(),
                        sender: 'bot',
                        content: data.Response
                    });

                    // Scroll to bottom after bot reply
                    this.$nextTick(() => {
                        this.$refs.chatContainer.scrollTop = this.$refs.chatContainer.scrollHeight;
                    });
                } else {
                    console.error('Failed to get bot response');
                }
            } catch (error) {
                console.error('Error:', error);
            }
        }
    }
}
