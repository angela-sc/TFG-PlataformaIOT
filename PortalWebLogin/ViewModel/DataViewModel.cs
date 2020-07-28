using Blazorise.Charts;
using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
//using PortalWeb.Resources;
using PortalWebLogin.Data;
using Servicios;
using Syncfusion.Blazor.PivotView;
using Syncfusion.Blazor.TreeGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PortalWebLogin.ViewModel
{
    public class DataViewModel : UsuarioAutenticadoViewModel
    {       
        //[Inject]
        //protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public string sensor { get; set; }   
       
        private int idSensor; // = 2; //para obtener el id del sensor debemos tener el nombre y la estación base
        
        private List<double> datosTemperatura;
        private List<double> datosHumedad;

        public IEnumerable<EntidadDatoBase> datos; //Datos que se representan en la página web
        public LineChart<double> lineChartTemperature, lineChartHumity; //graficas        

        protected string nombreSensor = null;

        //Etiquetas del eje X
        string[] Labels = { }; 

        //La grafica de temperatura en naranja
        List<string> backgroundColors = new List<string> { ChartColor.FromRgba(241, 120, 95, 0.2f)};
        List<string> borderColors = new List<string> { ChartColor.FromRgba(241, 120, 95, 1f)};

        //La grafica de humedad en azul
        List<string> backgroundColorsHumedad = new List<string> { ChartColor.FromRgba(0, 180, 175, 0.2f)};
        List<string> borderColorsHumedad = new List<string> { ChartColor.FromRgba(0, 180, 175, 1f)};

        // > -- SERVICIO
        private IServicioSensor servicioSensor;
        private IServicioEstacionBase servicioEstacionBase = FactoriaServicios.GetServicioEstacionBase();

        private async Task CargaDatos()
        {
            datos = new List<EntidadDatoBase>();
            datos = await servicioSensor.ObtenerDatos(idSensor, RadioValue2);
            datosTemperatura = new List<double>();
            datosHumedad = new List<double>();

            List<string> labels = new List<string>();

            foreach (var dato in datos)
            {
                datosTemperatura.Add(dato.Temperatura);
                datosHumedad.Add(dato.Humedad);
                labels.Add(dato.Stamp.ToString());
            }

            Labels = labels.ToArray();
        }

        protected override async Task OnSecureParameterSetAsync()
        {
            Int32.TryParse(idEstacionBase, out int idEb);
            Int32.TryParse(sensor, out idSensor);

            servicioSensor = FactoriaServicios.GetServicioSensor();
            nombreSensor = await servicioSensor.ObtenerNombre(idSensor, idEb);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await HandleRedraw();
            }
        }

        protected async Task HandleRedrawHumedad()
        {
            await lineChartHumity.Clear();
            await lineChartHumity.AddLabel(Labels);
            await lineChartHumity.AddDataSet(GetLineChartHumityDataset());
            await lineChartHumity.Update();
        }

        protected async Task HandleRedrawTemperatura()
        {
            await lineChartTemperature.Clear();
            await lineChartTemperature.AddLabel(Labels);
            await lineChartTemperature.AddDataSet(GetLineChartTemperatureDataset());
            await lineChartTemperature.Update();
        }

        protected async Task HandleRedraw()
        {
            await CargaDatos();
            
            await lineChartTemperature.Clear();
            await lineChartTemperature.Update();
            await lineChartTemperature.AddLabel(Labels);
            await lineChartTemperature.AddDataSet(GetLineChartTemperatureDataset());
            await lineChartTemperature.Update();

            await lineChartHumity.Clear();
            await lineChartHumity.Update();
            await lineChartHumity.AddLabel(Labels);
            await lineChartHumity.AddDataSet(GetLineChartHumityDataset());
            await lineChartHumity.Update();

            StateHasChanged();
        }

        LineChartDataset<double> GetLineChartTemperatureDataset()
        {
            return new LineChartDataset<double>
            {
                Label = "Temperatura (ºC)",
                Data = datosTemperatura,
                BackgroundColor = backgroundColors,
                BorderColor = borderColors,
                Fill = true,
                PointRadius = 2,
                BorderDash = new List<int> { }
            };
        }

        LineChartDataset<double> GetLineChartHumityDataset()
        {
            return new LineChartDataset<double>
            {
                Label = "Humedad (%)",
                Data = datosHumedad,
                BackgroundColor = backgroundColorsHumedad,
                BorderColor = borderColorsHumedad,
                Fill = true,
                PointRadius = 2,
                BorderDash = new List<int> { }
            };
        }


        public int RadioValue2 = 10;  //cantidad de datos que se muestra en la vista

        public async void RadioSelection2(ChangeEventArgs args)
        {
            RadioValue2 = Convert.ToInt32(args.Value);

            await CargaDatos();
            await HandleRedrawHumedad();
            await HandleRedrawTemperatura();
            StateHasChanged();
        }
    }
}
