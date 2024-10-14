using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class ApiError
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public ApiError(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
