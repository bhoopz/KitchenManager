using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChatMessage
    {
        public Guid ChatMessageId { get; set; } = Guid.NewGuid();
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public virtual ApplicationUser Sender { get; set; }
        [JsonIgnore]
        public virtual Chat Chat { get; set; }
    }
}
