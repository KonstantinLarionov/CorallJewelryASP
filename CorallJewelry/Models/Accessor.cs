using CorallJewelry.Entitys;
using Microsoft.EntityFrameworkCore;

namespace CorallJewelry.Models
{
    public static class Accessor
    {
        /// <summary>
        /// GetDbContext
        /// </summary>
        public static BackendContext GetDbContext()
        {
            BackendContext dbContext = new BackendContext(new DbContextOptions<BackendContext>());
            return dbContext;
        }
    }
}
