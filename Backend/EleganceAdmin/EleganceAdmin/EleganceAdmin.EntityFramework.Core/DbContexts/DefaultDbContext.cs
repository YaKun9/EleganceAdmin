using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace EleganceAdmin.EntityFramework.Core
{
    [AppDbContext("EleganceAdmin", DbProvider.Sqlite)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
        }
    }
}