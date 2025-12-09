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

            var data = await _db.UserOtps.ToListAsync();

            var emailCheck = _db.UserOtps.FirstOrDefaultAsync(x=>x.Email == email);

            return await _db.UserOtps
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        //public async Task<UserOtp> GetBySessionIdAsync(string sessionId)
        //{
        //    return await _db.UserOtps.FirstOrDefaultAsync(x => x.SessionId == sessionId);
        //}

        public async Task DeleteAsync(UserOtp otp)
        {
            _db.UserOtps.Remove(otp);
            await _db.SaveChangesAsync();
        }

    }
}
