using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
//using System.Ef


namespace Misard.IQs.Infrastructure.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly AppDbContext _db;

        public OtpRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task SaveOtpAsync(UserOtp otp)
        {
            await _db.UserOtps.AddAsync(otp);
            await _db.SaveChangesAsync();
        }

        public async Task<UserOtp> GetLatestOtpAsync(string email)
        {
            return await _db.UserOtps
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
