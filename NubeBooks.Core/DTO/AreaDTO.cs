using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class AreaDTO
    {
        public int IdArea { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int IdEmpresa { get; set; }

        public List<EntidadResponsableR_DTO> lstClientes { get; set; }
        public Decimal SumaClientes { get; set; }
        public Decimal Ingresos { get; set; }
        public Decimal Egresos { get; set; }
    }
}
