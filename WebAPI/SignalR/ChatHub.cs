using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IUserContextService _userContextService;
        private readonly IChatService _chatService;

        public ChatHub(IChatMessageRepository chatMessageRepository, IUserContextService userContextService, IChatService chatService)
        {
            _chatMessageRepository = chatMessageRepository;
            _userContextService = userContextService;   
            _chatService = chatService;
        }

        [Authorize]
        public async Task SendMessage(Guid chatId, string message)
        {
            
                var userId = _userContextService.GetUserId();

                if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message cannot be empty.");

                if(!await _chatService.IsUserInChatAsync(chatId, userId)) throw new UnauthorizedAccessException("User is not a participant in this chat.");

            try
            {
                var chatMessage = new ChatMessage
                {
                    ChatId = chatId,
                    SenderId = userId,
                    MessageText = message,
                };

                await _chatMessageRepository.AddMessageAsync(chatMessage);

                await Clients.Caller.SendAsync("MessageSentConfirmation", chatMessage);
                await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, message);
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("Error", "Could not send message.");
            }
            
        }

        [Authorize]
        public async Task JoinChat(Guid chatId)
        {
            var userId = _userContextService.GetUserId();

            var isUserInChat = await _chatService.IsUserInChatAsync(chatId, userId);
            if (isUserInChat)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
                await Clients.Group(chatId.ToString()).SendAsync("UserJoined", userId);
            }
        }

        [Authorize]
        public async Task LeaveChat(Guid chatId)
        {
            var userId = _userContextService.GetUserId();

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Group(chatId.ToString()).SendAsync("UserLeft", userId);
        }
    }
}
