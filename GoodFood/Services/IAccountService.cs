using GoodFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Services
{
    public interface IAccountService
    {
        public void RegisterUser(RegisterUserDto dto);

    }
}
