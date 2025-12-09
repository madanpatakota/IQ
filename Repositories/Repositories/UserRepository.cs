using Microsoft.EntityFrameworkCore;
using Misard.IQs.Application.Interfaces.Repositories;
using Misard.IQs.Domain.Entities;
using Misard.IQs.Infrastructure.Persistence;

namespace Misard.IQs.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailorPhoneAsync(string value)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == value || u.PhoneNumber == value);
        }

        //public async Task<User> GetByEmailOrPhoneAsync(string value)
        //{
        //    return await _context.Users
        //        .FirstOrDefaultAsync(x => x.Email == value || x.PhoneNumber == value);
        //}


        //public async Task<User> getUsersList()
        //{
        //    //return await _context.Users;
        //}

        public async Task<User?> GetByPhoneAsync(string phone)
        {
            //var usersData = _context.Users;

            //foreach (var user in usersData)
            //{
            //    Console.WriteLine(user);
            //}

           var usersList =  await _context.Users.ToListAsync();

            return await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phone);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }



    }
}
