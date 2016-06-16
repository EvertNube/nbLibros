using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class ServicioDTO
    {
        public int IdServicio { get; set; }
        public int IdMoneda { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public Decimal? Precio { get; set; }
        public bool Estado { get; set; }
        public int IdEmpresa { get; set; }

        public string simboloMoneda { get; set; }
        public string nMoneda { get; set; }
    }
}
