using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortalWeb.Data;
using Syncfusion.Blazor;

using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Serilog;

namespace PortalWeb
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            FactoriaServicios.SetCadenaConexion(configuration.GetValue<string>("CadenaConexion"));
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(configuration.GetValue<string>("DirectorioLog"))
                .CreateLogger();
            FactoriaServicios.SetLogger(Log.Logger);

            /**
             * DE MOMENTO SE HARDCODEAR� AQU� HASTA QUE SE IMPLEMENTE EL LOGIN DE USUARIO
             */
            InformacionUsuario.IdUsuario = 0;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
