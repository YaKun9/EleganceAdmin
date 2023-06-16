using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace EleganceAdmin.EntityFramework.Core.DbContexts
{
    [AppDbContext("EleganceAdmin", DbProvider.MySql)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
        }
    }
}