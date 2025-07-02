    const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    const chatWindow = document.getElementById("chat-window");
    const chatToggleButton = document.getElementById("chat-toggle-btn");
    const chatBody = document.getElementById("chat-body");
    const chatInput = document.getElementById("chat-input");
const sendButton = document.getElementById("send-btn");

    connection.on("ReceiveMessage", function (sender, message) {
        if (sender !== 'User') {
        hideTypingIndicator();
    enableInput();
    addMessageToChat(sender, message);
        }
    });

    connection.start().catch(err => console.error(err.toString()));

    function sendMessage() {
        const message = chatInput.value.trim();
    if (message) {
        disableInput();

    addMessageToChat("User", message);

    connection.invoke("SendMessage", "User", message).catch(err => {
    console.error(err.toString());
    addMessageToChat("Bot", "Có lỗi xảy ra, vui lòng thử lại.");
    hideTypingIndicator();
    enableInput();
            });

    chatInput.value = "";

    showTypingIndicator();
        }
    }
    function addMessageToChat(sender, message) {
        const isUser = sender === "User";
    const messageClass = isUser ? "user-message" : "bot-message";

    const userAvatarSrc = "/imgs/theme/user-avatar.jpg";
    const botAvatarSrc = "/imgs/theme/bot-avatar.jpg";
    const avatarSrc = isUser ? userAvatarSrc : botAvatarSrc;
    const avatarAlt = isUser ? "User Avatar" : "Bot Avatar";

    const messageElement = document.createElement('div');
    messageElement.className = `message-container ${messageClass}`;
    messageElement.innerHTML = `
    <img src="${avatarSrc}" alt="${avatarAlt}" class="avatar">
        <div class="message-bubble">${message}</div>
        `;

        chatBody.appendChild(messageElement);
        chatBody.scrollTop = chatBody.scrollHeight;
    }
        function showTypingIndicator() {
        if (document.getElementById('typing-indicator-container')) return;

        const indicatorElement = document.createElement('div');
        indicatorElement.id = 'typing-indicator-container';
        indicatorElement.className = 'message-container bot-message';

        indicatorElement.innerHTML = `
        <img src="/imgs/theme/bot-avatar.jpg" alt="Bot Avatar" class="avatar">
            <div class="message-bubble">
                <div class="typing-indicator">
                    <span></span>
                    <span></span>
                    <span></span>
                </div>
            </div>
            `;
            chatBody.appendChild(indicatorElement);
            chatBody.scrollTop = chatBody.scrollHeight;
    }

            function hideTypingIndicator() {
        const indicatorElement = document.getElementById('typing-indicator-container');
            if (indicatorElement) {
                indicatorElement.remove();
        }
    }

            function disableInput() {
                chatInput.disabled = true;
            sendButton.disabled = true;
    }

            function enableInput() {
                chatInput.disabled = false;
            sendButton.disabled = false;
            chatInput.focus();
    }

            // 5. CÁC HÀM TIỆN ÍCH KHÁC (Không đổi)
            function toggleChat() {
        const isHidden = chatWindow.style.display === "none" || chatWindow.style.display === "";
            if (isHidden) {
                chatWindow.style.display = "flex";
            chatToggleButton.style.display = "none";
            chatInput.focus();
        } else {
                chatWindow.style.display = "none";
            chatToggleButton.style.display = "flex";
        }
    }

            function handleEnter(event) {
        if (event.key === "Enter" && !chatInput.disabled) {
                sendMessage();
        }
    }