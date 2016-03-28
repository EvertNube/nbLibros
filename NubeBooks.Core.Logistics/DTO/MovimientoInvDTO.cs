using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class MovimientoInvDTO
    {
        public int IdMovimientoInv { get; set; }
        public int IdFormaMovimientoInv { get; set; }
        public int IdItem { get; set; }
        public int? IdEntidadResponsable { get; set; }
        public int? IdUbicacion { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string NroDocumento { get; set; }
        public string GuiaRemision { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string SerieLote { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public System.DateTime FechaInicial { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
        public int UsuarioCreacion { get; set; }
        public int IdEmpresa { get; set; }

        public int IdTipoMovimientoInv { get; set; }
        public string nItem { get; set; }
        public string nItemCodigo { get; set; }
        public string nCategoria { get; set; }
        public string nTipo { get; set; }
        public string nForma { get; set; }
        public string nUsuario { get; set; }
        public string nUbicacion { get; set; }
        public int StockLote { get; set; }
        public int SaldoItem { get; set; }
    }
}
