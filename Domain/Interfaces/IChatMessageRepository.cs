using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IChatMessageRepository
    {
        Task AddMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetMessagesByChatIdAsync(Guid chatId, int page, int pageSize);
    }
}
