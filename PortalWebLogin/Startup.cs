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
using BlazorDownloadFile;

namespace PortalWebLogin
{
    public class Startup
    {
        /**
         * Documentacion Syncfusion (mapas)
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
            services.AddBlazorDownloadFile();

            // >- Referencias para usar synfusion y blazorise
            services.AddSyncfusionBlazor();

            services.AddBlazorise(options => { options.ChangeTextOnKeyPress = true; })
               .AddBootstrapProviders()
               .AddFontAwesomeIcons();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjk2Mjk4QDMxMzgyZTMyMmUzMEFENHB3MFdua1hWR2V3aXdpNjY1eWphbC95eE9QVmJnTTlieWV1N1Q1dmM9;Mjk2Mjk5QDMxMzgyZTMyMmUzMExvYklWY0grWFExTmpqZkFHbU9ZeUtpSFYxb3pZSlJMRjlHWStENGNJWHM9;Mjk2MzAwQDMxMzgyZTMyMmUzMGNIdktpaFdXZk8rMnU4dVNxN2ZjQm1pZ2tJVzFtUGM0aStDV0pMUE5lTkk9");

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
