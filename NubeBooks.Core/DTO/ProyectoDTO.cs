using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class ProyectoDTO
    {
        public int IdProyecto { get; set; }
        public int IdEntidadResponsable { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public int? IdResponsable { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
