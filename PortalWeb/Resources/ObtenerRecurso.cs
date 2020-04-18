using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalWeb.Resources
{
    public static class ObtenerRecurso
    {
        public static byte[] Obtener()
        {
            return Properties.Resources.dinosaurio;
        }

        public static byte[] ObtenerSensor()
        {
            return Properties.Resources.SE01;
        }
    }
}
