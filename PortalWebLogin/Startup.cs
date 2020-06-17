using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortalWebLogin.Areas.Identity;
using PortalWebLogin.Data;
using Serilog;
using Syncfusion.Blazor;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

namespace PortalWebLogin
{
    public class Startup
    {
        /**
         * Documentacion Syncfusion (mapas de pago)
         * https://blazor.syncfusion.com/documentation/maps/providers/openstreetmap/
         * https://blazor.syncfusion.com/documentation/maps/markers/
         * 
         * Documentacion Blazorise (graficas)
         * https://blazorise.com/docs/extensions/chart/
         * */
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            FactoriaServicios.CadenaConexion = configuration.GetValue<string>("CadenaConexion");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(configuration.GetValue<string>("DirectorioLog"))
                .CreateLogger();
            FactoriaServicios.SetLogger(Log.Logger);

            /**
             * DE MOMENTO SE HARDCODEARÁ AQUÍ HASTA QUE SE IMPLEMENTE EL LOGIN DE USUARIO
             */
            InformacionUsuario.IdUsuario = 0;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(FactoriaServicios.CadenaConexion));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddSingleton<WeatherForecastService>();

            // >- Añadimos las referencias para usar synfusion y blazorise
            services.AddSyncfusionBlazor();

            services.AddBlazorise(options => { options.ChangeTextOnKeyPress = true; })
               .AddBootstrapProviders()
               .AddFontAwesomeIcons();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
