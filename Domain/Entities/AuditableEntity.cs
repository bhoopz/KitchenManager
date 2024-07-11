using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public User CreatedBy { get; set; }  
        public DateTime? UpdatedAt { get; set; } 
        public User? UpdatedBy { get; set; }   

    }
}
