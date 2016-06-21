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
        public int? IdResponsable { get; set; }
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
        public int? Credito { get; set; }
        public int? TipoPersona { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Email { get; set; }
        public string Comentario { get; set; }


        public string NombreTipoEntidad { get; set; }
        public string NombreIdentificacion { get; set; }
        public string nResponsable { get; set; }
        public string TipoIdentificacion { get; set; }
        public List<ProyectoDTO> lstProyectos { get; set; }
        public List<ContactoDTO> lstContactos { get; set; }
    }
}
