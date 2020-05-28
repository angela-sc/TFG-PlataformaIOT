using Blazorise.Charts;
using Libreria.Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PortalWeb.Resources;
using Servicios;
using Syncfusion.Blazor.PivotView;
using Syncfusion.Blazor.TreeGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PortalWeb.ViewModel
{
    public class DataViewModel : ComponentBase
    {

        [Parameter]
        public string sensor { get; set; }
        
        [Parameter]
        public string estacionbase { get; set; }   //id de la estacion base en formato string   

        [Parameter]
        public int selectedValue { get; set; }

        private ServicioSensor servicio = new ServicioSensor("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true", null);
       
        private int idSensor, idEstacionBase; // = 2; //para obtener el id del sensor debemos tener el nombre y la estación base
        
        private List<double> datosTemperatura;
        private List<double> datosHumedad;

        public IEnumerable<EntidadDatoBase> datos; //Datos que se representan en la página web
        public LineChart<double> lineChartTemperature, lineChartHumity; //graficas        

        //Etiquetas del eje X
        string[] Labels = { }; 

        //La grafica de temperatura en naranja
        List<string> backgroundColors = new List<string> { ChartColor.FromRgba(241, 120, 95, 0.2f)};
        List<string> borderColors = new List<string> { ChartColor.FromRgba(241, 120, 95, 1f)};

        //La grafica de humedad en azul
        List<string> backgroundColorsHumedad = new List<string> { ChartColor.FromRgba(0, 180, 175, 0.2f)};
        List<string> borderColorsHumedad = new List<string> { ChartColor.FromRgba(0, 180, 175, 1f)};

        private async Task CargaDatos()
        {
            datos = new List<EntidadDatoBase>();
            datos = await servicio.ObtenerDatos(idSensor, RadioValue2);
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
            //StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            Int32.TryParse(estacionbase, out idEstacionBase);
            Int32.TryParse(sensor, out idSensor);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await HandleRedraw();
            }
        }

        protected async Task HandleRedraw()
        {
            await CargaDatos();
            
            await lineChartTemperature.Clear();
            await lineChartTemperature.Update();
            StateHasChanged();
            await lineChartTemperature.AddLabel(Labels);
            await lineChartTemperature.AddDataSet(GetLineChartTemperatureDataset());
            await lineChartTemperature.Update();
            StateHasChanged();

            await lineChartHumity.Clear();
            await lineChartHumity.Update();
            StateHasChanged();
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


        public int RadioValue2;  //cantidad de datos que se muestra en la vista

        public async void RadioSelection2(ChangeEventArgs args)
        {
            RadioValue2 = Convert.ToInt32(args.Value);
            await HandleRedraw();
            //StateHasChanged();
        }
    }
}
