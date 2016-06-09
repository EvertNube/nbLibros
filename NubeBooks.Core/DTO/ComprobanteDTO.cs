using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class ComprobanteDTO
    {
        public int IdComprobante { get; set; }
        public int IdTipoComprobante { get; set; }
        public int IdTipoDocumento { get; set; }
        public int IdEntidadResponsable { get; set; }
        public int? IdEntidadResponsable2 { get; set; }
        public int IdMoneda { get; set; }
        public int IdEmpresa { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string NroDocumento { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public Decimal Monto { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public Decimal MontoSinIGV { get; set; }
        public int? IdArea { get; set; }
        public int? IdResponsable { get; set; }
        public int? IdCategoria { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaConclusion { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
        public bool Ejecutado { get; set; }
        public int? IdHonorario { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public Decimal TipoCambio { get; set; }
        public int UsuarioCreacion { get; set; }
        public DateTime? FechaPago { get; set; }

        public int? IdProyecto { get; set; }

        //Nombres
        public string NombreTipoComprobante { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string NombreEntidad { get; set; }
        public string NombreMoneda { get; set; }
        public string SimboloMoneda { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreProyecto { get; set; }
        public string NombreAreas { get; set; }
        public string nHonorario { get; set; }

        //Montos Auxs
        public Decimal MontoIncompleto { get; set; }
        public int? diasVencidos { get; set; }

        //Listas
        public List<AreaPorComprobanteDTO> lstMontos { get; set; }
    }
}
