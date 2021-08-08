using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Models
{
    public class RestaurantPagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemsTo { get; set; }
        public int ItemsCount { get; set; }

        public RestaurantPagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            ItemsCount = items.Count;
            ItemFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount / (double) pageSize);
        }
    }
}
