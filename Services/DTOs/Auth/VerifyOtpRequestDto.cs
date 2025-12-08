using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misard.IQs.Application.DTOs.Auth
{
    //public class VerifyOtpRequestDto
    //{


    //    public string Email { get; set; }
    //    public string Otp { get; set; }
    //}

    public class VerifyOtpRequestDto
    {
        public string SessionId { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
