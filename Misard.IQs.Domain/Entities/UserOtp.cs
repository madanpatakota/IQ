using Misard.IQs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misard.IQs.Domain.Entities
{
    public class UserOtp : BaseEntity
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public DateTime Expiry { get; set; }
    }
}
