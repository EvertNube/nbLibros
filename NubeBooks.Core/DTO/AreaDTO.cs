using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class AreaDTO
    {
        public int IdArea { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int IdEmpresa { get; set; }

        public List<EntidadResponsableR_DTO> lstClientes { get; set; }
        public Decimal SumaClientes { get; set; }
        public Decimal SumaClientes_SinIGV { get; set; }

        public Decimal Ingresos { get; set; }
        public Decimal Egresos { get; set; }

        public Decimal Ingresos_SinIGV { get; set; }
        public Decimal Egresos_SinIGV { get; set; }

        public Decimal Porcentaje { get; set; }
    }
}
