using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class EntidadResponsableDTO
    {
        public int? IdEntidadResponsable { get; set; }
        public int? IdTipoIdentificacion { get; set; }
        public int? IdTipoEntidad { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public Decimal? Detraccion { get; set; }
        public string Tipo { get; set; }
        public int IdEmpresa { get; set; }
        public string NroIdentificacion { get; set; }
        public string NombreComercial { get; set; }
        public string Direccion { get; set; }
        public string Banco { get; set; }
        public string CuentaSoles { get; set; }
        public string CuentaDolares { get; set; }

        public string NombreTipoEntidad { get; set; }
        public string NombreIdentificacion { get; set; }
        public List<ProyectoDTO> lstProyectos { get; set; }
        public List<ContactoDTO> lstContactos { get; set; }
    }
}
