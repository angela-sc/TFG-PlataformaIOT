using Blazorise.Charts;
using Libreria.Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PortalWeb.Resources;
using Servicios;
using Syncfusion.Blazor.PivotView;
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
        public string estacionbase { get; set; }

        [Parameter]
        public int selectedValue { get; set; }


        private ServicioSensor servicio = new ServicioSensor("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = plataformadb; Integrated Security = true", null);
       
        private int id; // = 2; //para obtener el id del sensor debemos tener el nombre y la estación base
        
        private List<double> datosTemperatura = new List<double>();
        private List<double> datosHumedad = new List<double>();

        public IEnumerable<EntidadDatoBase> datos; //Datos que se representan en la página web
        public LineChart<double> lineChartTemperature, lineChartHumity; //graficaS        

        //Etiquetas del eje X
        string[] Labels = { }; 

        //La grafica de temperatura en naranja
        List<string> backgroundColors = new List<string> { ChartColor.FromRgba(241, 120, 95, 0.2f)};
        List<string> borderColors = new List<string> { ChartColor.FromRgba(241, 120, 95, 1f)};

        //La grafica de humedad en azul
        List<string> backgroundColorsHumedad = new List<string> { ChartColor.FromRgba(0, 180, 175, 0.2f)};
        List<string> borderColorsHumedad = new List<string> { ChartColor.FromRgba(0, 180, 175, 1f)};


        protected override async Task OnInitializedAsync()
        {
            id = await servicio.ObtenerIdSensor(sensor, estacionbase);

          

            //var datos = await servicio.ObtenerDatos(id);
            datos = new List<EntidadDatoBase>();
            datos = await servicio.ObtenerDatos(id, RadioValue2 );


            List<string> labels = new List<string>();

            foreach (var dato in datos)
            {
                datosTemperatura.Add(dato.temperature);
                datosHumedad.Add(dato.humity);
                labels.Add(dato.stamp.ToString());
            }

            Labels = labels.ToArray();
            await HandleRedraw();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await HandleRedraw();
            }
           
        }

        async Task HandleRedraw()
        {
            lineChartTemperature.Clear();
            lineChartTemperature.AddLabel(Labels);
            lineChartTemperature.AddDataSet(GetLineChartTemperatureDataset());

            lineChartHumity.Clear();
            lineChartHumity.AddLabel(Labels);
            lineChartHumity.AddDataSet(GetLineChartHumityDataset());

            await lineChartTemperature.Update();
            await lineChartHumity.Update();
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


        public int RadioValue2 = 20;  //cantidad de datos que se muestra en la vista

        public async void RadioSelection2(ChangeEventArgs args)
        {
            RadioValue2 = Convert.ToInt32(args.Value);

            if(RadioValue2 > datos.Count())
            {
                datos = await servicio.ObtenerDatos(id, RadioValue2);
                StateHasChanged();

            }
            else
            {
                datos = datos.Take(RadioValue2);
            }
            List<string> labels = new List<string>();

            foreach (var dato in datos)
            {
                datosTemperatura.Add(dato.temperature);
                datosHumedad.Add(dato.humity);
                labels.Add(dato.stamp.ToString());
            }

            Labels = labels.ToArray();
            await HandleRedraw();
        }
    }
}
