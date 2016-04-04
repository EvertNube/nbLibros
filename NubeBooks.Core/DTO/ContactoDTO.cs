using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class ContactoDTO
    {
        public int IdContacto { get; set; }
        public int IdEntidadResponsable { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Cargo { get; set; }
        public bool Estado { get; set; }
    }
}
