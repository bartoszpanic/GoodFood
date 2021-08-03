using GoodFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoodFood.Services
{
    public interface IRestaurantService
    {
        Task<RestaurantDto> GetByIdAsync(int id);
        Task<IEnumerable<RestaurantDto>> GetAllAsync();
        Task<int> CreateAsync(CreateRestaurantDto dto, int userId);
        Task DeleteAsync(int id, ClaimsPrincipal user);
        Task UpdateAsync(int id, UpdateRestaurantDto dto, ClaimsPrincipal user);
    }
}
