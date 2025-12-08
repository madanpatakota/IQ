using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misard.IQs.Application.DTOs.Auth
{
    public class ForgotPasswordRequestDto
    {
        public string Phone { get; set; } = string.Empty;
    }
}
