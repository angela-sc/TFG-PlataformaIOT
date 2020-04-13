using Blazorise.Charts;
using Microsoft.AspNetCore.Components;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class DataViewModel : ComponentBase
    {
        public LineChart<double> lineChartTemperature, lineChartHumity;
        private ServicioSensor servicio = new ServicioSensor("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = plataformadb; Integrated Security = true", null);
        string[] Labels = { };
        List<string> backgroundColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
        List<string> borderColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };

        private int id = 2;
        private List<double> datosTemperatura = new List<double>();
        private List<double> datosHumedad = new List<double>();

        protected override async Task OnInitializedAsync()
        {
            var datos = await servicio.ObtenerDatos(id);
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
                Label = "Temperatura",
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
                Label = "Humedad",
                Data = datosHumedad,
                BackgroundColor = backgroundColors,
                BorderColor = borderColors,
                Fill = true,
                PointRadius = 2,
                BorderDash = new List<int> { }
            };
        }
    }
}
