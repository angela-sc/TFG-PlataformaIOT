using Blazorise.Charts;
using Libreria.Entidades;
using Libreria.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalWebLogin.Data;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWebLogin.ViewModel
{
    public class DetalleEstacionBaseViewModel : UsuarioAutenticadoViewModel
    {
        // > -- PARAMETROS
        #region PARAMETROS
        [Parameter]
        public List<Tuple<string, List<double>>> listaDatosTemp { get; set; }

        [Parameter]
        public List<Tuple<string, List<double>>> listaDatosHum { get; set; }
        #endregion

        // > -- ATRIBUTOS PRIVADOS            
        private IServicioEstacionBase servicioEstacionBase = FactoriaServicios.GetServicioEstacionBase();
        private IServicioSensor servicioSE = FactoriaServicios.GetServicioSensor();
               
        // > -- GRAFICAS
        protected LineChart<double> graficaTemperatura, graficaHumedad;        
        List<string> coloresGraficas = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };
        string[] Labels;
        // > -- FILTRO GRAFICAS
        protected DateTime? fechaInicio = null;
        protected DateTime? fechaFin=null;

        protected string nombreEstacionBase { get; set; }

        //> -- FUNCIONES      
        private async Task CargarDatos(DateTime? fechaInicio=null, DateTime? fechaFin=null)
        {
            Int32.TryParse(idEstacionBase, out int id);
            nombreEstacionBase = await servicioEstacionBase.Nombre(id);
            var sensores = await servicioEstacionBase.ObtenerSensores(id);
            listaDatosTemp = new List<Tuple<string, List<double>>>();
            listaDatosHum = new List<Tuple<string, List<double>>>();
            List<DateTime> stamps = new List<DateTime>();

            foreach (var sensor in sensores)
            {
                IEnumerable<EntidadDatoBase> datos;
                
                if(fechaInicio == null || fechaFin == null)
                {
                    datos = await servicioSE.ObtenerDatos(sensor.IdSensor, 10);
                }
                else
                {
                    datos = await servicioSE.ObtenerDatos(sensor.IdSensor, fechaInicio, fechaFin.Value.AddDays(1)); // FILTRAMOS POR FECHA
                }                
                listaDatosTemp.Add(new Tuple<string, List<double>>(sensor.NombreSensor, datos.Select(_ => (double)_.Temperatura).ToList()));
                listaDatosHum.Add(new Tuple<string, List<double>>(sensor.NombreSensor, datos.Select(_ => (double)_.Humedad).ToList()));

                stamps.AddRange(datos.Select(_ => _.Stamp).ToList());
            }

            Labels = stamps.OrderBy(_ => _.Ticks).Distinct().Select(_ => _.ToString()).ToArray();
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await HandleRedraw();
            }
        }

        protected async Task HandleRedraw(DateTime? inicio=null, DateTime? fin=null) //inicio tiene el valor por defecto null si no se especifica
        {
            await CargarDatos(inicio, fin); // -- cargamos los datos para las dos gráficas          
            await CargarGrafica();           
        }

        private async Task CargarGrafica()
        {
            await graficaTemperatura.Clear();
            int indiceColor = 0; // -- indice utilizado en el array de colores para que cada sensor (dataset) tenga un color
            List<LineChartDataset<double>> dataset = new List<LineChartDataset<double>>();
            foreach (var datosSensor in listaDatosTemp.OrderBy(_ => _.Item1)) // -- datasets ordenados por nombre de sensor
            {
                dataset.Add(ObtenerDataSet(datosSensor.Item1, datosSensor.Item2, indiceColor));
                indiceColor = (indiceColor + 1) % coloresGraficas.Count();
            }
            await graficaTemperatura.AddLabelsDatasetsAndUpdate(Labels, dataset.ToArray());

            await graficaHumedad.Clear();
            indiceColor = 0;
            dataset = new List<LineChartDataset<double>>();
            foreach (var datosSensor in listaDatosHum.OrderBy(_ => _.Item1)) // -- datasets ordenados por nombre de sensor
            {
                dataset.Add(ObtenerDataSet(datosSensor.Item1, datosSensor.Item2, indiceColor));
                indiceColor = (indiceColor + 1) % coloresGraficas.Count();
            }
            await graficaHumedad.AddLabelsDatasetsAndUpdate(Labels, dataset.ToArray());
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
            if(fechaInicio != null && fechaFin != null)
            {
                await HandleRedraw(fechaInicio, fechaFin);
            }
            else
            {               
                await HandleRedraw(); //Si las fechas son incorrectas no se mostrara nada diferente                
                this.StateHasChanged();
            }
        }
    }
 }

