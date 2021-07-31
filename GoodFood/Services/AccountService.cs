using GoodFood.Entities;
using GoodFood.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        public AccountService(ApplicationDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
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

            var hashedPassword = _hasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;

            _db.Users.Add(newUser);
            _db.SaveChanges();
        }
    }
}
