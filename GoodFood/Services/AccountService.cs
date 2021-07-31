using GoodFood.Entities;
using GoodFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        public AccountService(ApplicationDbContext db)
        {
            _db = db;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();
        }
    }
}
