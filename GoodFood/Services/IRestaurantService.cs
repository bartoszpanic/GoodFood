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
        Task<RestaurantPagedResult<RestaurantDto>> GetAllAsync(RestaurantQuery query);
        Task<int> CreateAsync(CreateRestaurantDto dto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, UpdateRestaurantDto dto);
    }
}
