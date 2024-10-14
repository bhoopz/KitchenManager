using Application.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IChatService
    {
        Task<bool> IsUserInChatAsync(Guid chatId, Guid userId);
        Task<ApiResponse<IEnumerable<ChatMessage>>> GetChatHistory(Guid chatId, int page, int pageSize);
    }
}
