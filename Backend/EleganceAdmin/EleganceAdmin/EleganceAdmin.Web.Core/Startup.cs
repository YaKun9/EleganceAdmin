using EleganceAdmin.Core.Configuration;
using EleganceAdmin.Core.Const;
using EleganceAdmin.Web.Core.Handlers;
using Furion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EleganceAdmin.Web.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurableOptions<CacheOptions>();
            services.AddConsoleFormatter();
            services.AddJwt<JwtHandler>();

            services.AddCorsAccessor();

            services.AddControllers().AddInjectWithUnifyResult();

            #region 缓存

            var cacheOptions = App.GetConfig<CacheOptions>("Cache");
            if (cacheOptions.CacheType == CacheTypeConst.REDIS)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = cacheOptions.CacheType;
                    options.InstanceName = cacheOptions.Prefix;
                });
            }

            #endregion 缓存
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}