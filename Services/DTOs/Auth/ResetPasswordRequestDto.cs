using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misard.IQs.Application.DTOs.Auth
{
    public class ResetPasswordRequestDto
    {
        public string SessionId { get; set; }
        public string NewPassword { get; set; }

        public string Phone { get; set; }
    }

}
