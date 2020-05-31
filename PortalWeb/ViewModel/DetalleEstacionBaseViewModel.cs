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
               
        // > -- GRAFICAS
        protected LineChart<double> graficaTemperatura, graficaHumedad;

        //List<string> backgroundColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
        List<string> coloresGraficas = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };
        string[] Labels;

        // > -- FILTRO GRAFICAS
        protected DateTime? fechaInicio = null;
        protected DateTime? fechaFin=null;

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

            int indiceColor = 0; // -- indice utilizado en el array de colores para que cada sensor (dataset) tenga un color
            foreach (var datosSensor in listaDatosTemp.OrderBy(_ => _.Item1)) // -- datasets ordenados por nombre de sensor
            {              
                await graficaTemperatura.AddDataSet(ObtenerDataSet(datosSensor.Item1, datosSensor.Item2, indiceColor));
                indiceColor = (indiceColor + 1) % coloresGraficas.Count();
            }
            await graficaTemperatura.Update();
            StateHasChanged();

            await graficaHumedad.Clear();
            await graficaHumedad.AddLabel(Labels);
            indiceColor = 0;
            foreach (var datosSensor in listaDatosHum.OrderBy(_ => _.Item1)) // -- datasets ordenados por nombre de sensor
            {                
                await graficaHumedad.AddDataSet(ObtenerDataSet(datosSensor.Item1, datosSensor.Item2, indiceColor));
                indiceColor = (indiceColor + 1) % coloresGraficas.Count();
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
                BackgroundColor = coloresGraficas.ElementAt(indice),
                BorderColor = coloresGraficas.ElementAt(indice),
                Fill = false,
                PointRadius = 2,
                BorderDash = new List<int> { }
            };
        }

        // > -- METODOS PARA FILTRAR LAS GRAFICAS
        protected async Task FiltrarPorFecha()
        {
            if(fechaInicio == null || fechaFin == null)
            {
                //Console.WriteLine("fechaInicio o fechaFin son null");
            }
            else
            {
                //comprobamos que la fecha de inicio es menor que la de fin

                //Console.WriteLine($"fecha inicio: {fechaInicio} / fecha fin: {fechaFin}");
            }
        }
    }
 }

