function chatbot() {
    return {
        showWelcome: false,
        showChat: false,
        userMessage: '',
        chatMessages: [],
        showImage: false,
        isLoading: false,

        // Initialize the chat UI
        init() {
            setTimeout(() => this.showWelcome = true, 500);
            setTimeout(() => { this.showWelcome = false; this.showChat = true }, 4000);
            setTimeout(() => this.showImage = true, 3500);
        },

        // Handle sending a message
        async sendMessage() {
            console.log('here')
            if (this.userMessage.trim() !== '') {
                // Add user message
                this.chatMessages.push({
                    id: Date.now(),
                    sender: 'user',
                    content: this.userMessage
                });

                // Wait for DOM update, then scroll to bottom
                this.$nextTick(() => {
                    this.$refs.chatContainer.scrollTop = this.$refs.chatContainer.scrollHeight;
                });

                // Show loading bar and add loading message
                this.isLoading = true;
                this.chatMessages.push({
                    id: Date.now(),
                    sender: 'bot',
                    content: '...' // Loading message
                });

                console.log(this.userMessage);
                // Send user message to API to get bot response
                await this.getBotResponse(this.userMessage);

                // Clear input field
                this.userMessage = '';
            }
        },

        // Fetch bot's response from the API
        async getBotResponse(userMessage) {
            console.log(userMessage + " getBotResponse start");
            try {
                const response = await fetch('/api/chat/ask', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(userMessage)
                });

                if (response.ok) {
                    const data = await response.json();
                    if (data.response) {
                        // Remove the loading message and add the bot's actual response
                        const loadingMessageIndex = this.chatMessages.findIndex(message => message.content === '...');
                        if (loadingMessageIndex !== -1) {
                            this.chatMessages[loadingMessageIndex].content = data.response;
                        } else {
                            this.chatMessages.push({
                                id: Date.now(),
                                sender: 'bot',
                                content: data.response
                            });
                        }

                        // Scroll to bottom after bot reply
                        this.$nextTick(() => {
                            this.$refs.chatContainer.scrollTop = this.$refs.chatContainer.scrollHeight;
                        });
                    } else {
                        console.error('No response in the data');
                    }
                } else {
                    console.error('Failed to get bot response');
                }
            } catch (error) {
                console.error('Error:', error);
            } finally {
                // Hide loading bar after response is received
                this.isLoading = false;
            }
        }
    };
}
