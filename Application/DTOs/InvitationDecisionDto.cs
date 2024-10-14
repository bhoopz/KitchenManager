using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class InvitationDecisionDto
    {
        [Required]
        public Guid InvitationId { get; set; }
        [Required]
        public bool Accept { get; set; }
    }
}
