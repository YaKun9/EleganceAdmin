using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace EleganceAdmin.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.AddDbPool<DefaultDbContext>();
            }, "EleganceAdmin.Database.Migrations");
        }
    }
}