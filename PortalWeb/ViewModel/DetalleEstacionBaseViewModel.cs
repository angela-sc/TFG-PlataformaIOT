using Blazorise.Charts;
using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.ViewModel
{
    public class DetalleEstacionBaseViewModel : ComponentBase
    {
        // > -- PARAMETROS
        #region PARAMETROS
        [Parameter]
        public string nombreEstacionBase { get; set; }

        [Parameter]
        public List<Tuple<string,List<double>>> listaDatosTemp { get; set; }

        [Parameter]
        public List<Tuple<string, List<double>>> listaDatosHum { get; set; }
        #endregion


        // > -- ATRIBUTOS PRIVADOS        
        private IServicioEstacionBase servicioEB = new ServicioEstacionBase("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true", null);
        private IServicioSensor servicioSE = new ServicioSensor("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=plataforma_iot;Integrated Security=true", null);
        
        private IEnumerable<EntidadSensorResultado> sensores; //sensores de una estacion base


        // > -- GRAFICAS
        protected LineChart<double> graficaTemperatura, graficaHumedad;

        private List<double> datosTemperatura;

        List<string> backgroundColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
        List<string> borderColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };
        string[] Labels;

         //> -- FUNCIONES
        private async Task CargaDatos()
        {
            var sensores = await servicioEB.ObtenerSensores(nombreEstacionBase);
            listaDatosTemp = new List<Tuple<string, List<double>>>();
            listaDatosHum = new List<Tuple<string, List<double>>>();
            List<DateTime> stamps = new List<DateTime>();

            foreach (var sensor in sensores)
            {
                var datos = await servicioSE.ObtenerDatos(sensor.IdSensor, 10);
                listaDatosTemp.Add(new Tuple<string, List<double>>(sensor.NombreSensor, datos.Select(_ => (double)_.Temperatura).ToList()));
                listaDatosHum.Add(new Tuple<string, List<double>>(sensor.NombreSensor, datos.Select(_ => (double)_.Humedad).ToList()));

                stamps.AddRange(datos.Select(_ => _.Stamp).ToList());
            }

            Labels = stamps.OrderBy(_ => _.Ticks).Distinct().Select(_ => _.ToString()).ToArray();
            StateHasChanged();
        }

        private int GenerarRandom()
        {
            Random rnd = new Random();
            return rnd.Next(0, backgroundColors.Count());
        }

        //protected override async Task OnInitializedAsync()
        //{

        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await HandleRedraw();
            }
        }

        protected async Task HandleRedraw()
        {
            await CargaDatos(); // -- cargamos los datos para las dos gráficas          

            await graficaTemperatura.Clear();
            await graficaTemperatura.AddLabel(Labels);

           
            foreach (var datosSensor in listaDatosTemp)
            {
                var numRandom = GenerarRandom(); // -- generamos un numero aleatorio entre 0y 7 para el color de cada dataset
                await graficaTemperatura.AddDataSet(ObtenerDataSet(datosSensor.Item1, datosSensor.Item2, numRandom));
                
            }
            await graficaTemperatura.Update();
            StateHasChanged();

            await graficaHumedad.Clear();
            await graficaHumedad.AddLabel(Labels);
            foreach (var datosSensor in listaDatosHum)
            {
                var numRandom = GenerarRandom(); // -- generamos un numero aleatorio entre 0y 7 para el color de cada dataset
                await graficaHumedad.AddDataSet(ObtenerDataSet(datosSensor.Item1, datosSensor.Item2, numRandom));
            }
            await graficaHumedad.Update();
            StateHasChanged();
        }

        LineChartDataset<double> ObtenerDataSet(string nombreSensor, List<double> datosSensor, int indice) // obtiene cada uno de los datasets de temperatura: 1 dataset x sensor
        {
            return new LineChartDataset<double>
            {
                Data = datosSensor,
                Label = nombreSensor,
                BackgroundColor = backgroundColors.ElementAt(indice),
                BorderColor = borderColors.ElementAt(indice),
                Fill = false,
                PointRadius = 2,
                BorderDash = new List<int> { }
            };
        }
    }
 }

