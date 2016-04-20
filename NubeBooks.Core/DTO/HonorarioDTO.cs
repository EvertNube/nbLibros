using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class HonorarioDTO
    {
        public int IdHonorario { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public int IdEmpresa { get; set; }

        public Decimal Monto { get; set; }
        public Decimal Monto_SinIGV { get; set; }
        public Decimal Porcentaje { get; set; }
    }
}
