using Application.Helpers;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserContextService _userContextService;
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatService(IUnitOfWork unitOfWork, IMembershipRepository membershipRepository, IUserContextService userContextService, IChatMessageRepository chatMessageRepository)
        {
            _unitOfWork = unitOfWork;
            _membershipRepository = membershipRepository;
            _userContextService = userContextService;
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task<bool> IsUserInChatAsync(Guid chatId, Guid userId)
        {
            var chat = await _unitOfWork.GetGenericRepository<Chat>().GetByIdAsync(chatId);

            if (chat == null) return false;

            return await _membershipRepository.IsUserInRestaurantAsync(userId, chat.RestaurantId);
        }

        public async Task<ApiResponse<IEnumerable<ChatMessage>>> GetChatHistory(Guid chatId, int page, int pageSize)
        {
            var userId = _userContextService.GetUserId();
            if (userId == Guid.Empty) return ApiResponse<IEnumerable<ChatMessage>>.Failure(ErrorHelper.Unauthorized);

            if (!await this.IsUserInChatAsync(chatId, userId)) return ApiResponse<IEnumerable<ChatMessage>>.Failure(ErrorHelper.WrongChat);

            try
            {
                var messages = await _chatMessageRepository.GetMessagesByChatIdAsync(chatId, page, pageSize);
                return ApiResponse<IEnumerable<ChatMessage>>.Success(messages);
            }
            catch (Exception)
            {
                return ApiResponse<IEnumerable<ChatMessage>>.Failure(ErrorHelper.Sww);
            }
            
        }
    }
}
