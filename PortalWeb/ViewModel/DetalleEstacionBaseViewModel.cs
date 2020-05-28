using Blazorise.Charts;
using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Servicios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class DetalleEstacionBaseViewModel : ComponentBase
    {
        // > -- PARAMETROS
        [Parameter]
        public string nombreEstacionBase { get; set; }


        // > -- ATRIBUTOS PRIVADOS        
        private IServicioEstacionBase servicioEB = new ServicioEstacionBase("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true", null);
        private IServicioSensor servicioSE = new ServicioSensor("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true", null);
        
        private IEnumerable<EntidadSensorResultado> sensores; //sensores de una estacion base


        // > -- GRAFICAS
        protected LineChart<double> graficaTemperatura, graficaHumedad;

        private List<double> datosTemperatura;

        List<string> backgroundColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
        List<string> borderColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };
        string[] labels = { "Red", "Blue", "Yellow", "Green", "Purple", "Orange" };

        // > -- FUNCIONES
        protected override async Task OnInitializedAsync()
        {
            sensores = await servicioEB.ObtenerSensores(nombreEstacionBase);

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
            graficaTemperatura.Clear();
            graficaTemperatura.AddLabel(labels);
            
            foreach(var sensor in sensores)
            {
                ObtenerListaDatosTemperatura(sensor.IdSensor); //obtenemos la lista con los datos para el dataset
                graficaTemperatura.AddDataSet(ObtenerDataSetTemperatura(sensor));
            }

            await graficaTemperatura.Update();
        }

        LineChartDataset<double> ObtenerDataSetTemperatura(EntidadSensorResultado sensor) // obtiene cada uno de los datasets de temperatura: 1 dataset x sensor
        {
          
            return new LineChartDataset<double>
            {
                Data = datosTemperatura,
                Label = sensor.NombreSensor,
                BackgroundColor = backgroundColors,
                BorderColor = borderColors,
                Fill = false,
                PointRadius = 2,
                BorderDash = new List<int> { }
            };
            //Data = ,
            //Label = "",
            //BackgroundColor =,
            //BorderColor =,
            //Fill = false,
            //PointRadius = 2,
            //BorderDash = new List<int> { }
        }

        LineChartDataset<double> ObtenerDataSetHumedad()
        {
            return new LineChartDataset<double>();
            //Data = ,
            //Label = "",
            //BackgroundColor =,
            //BorderColor =,
            //Fill = false,
            //PointRadius = 2,
            //BorderDash = new List<int> { }
        }

        private async void ObtenerListaDatosTemperatura(int idSensor)
        {
            IEnumerable<EntidadDatoBase> datos = new List<EntidadDatoBase>();
            datos =  await servicioSE.ObtenerDatos(idSensor, 10);

            datosTemperatura = new List<double>();

            foreach(var dato in datos)
            {
                datosTemperatura.Add(dato.Temperatura);
            }            
        }
    }
 }

