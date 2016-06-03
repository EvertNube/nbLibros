using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class MovimientoDTO
    {
        public int IdMovimiento { get; set; }
        public int IdCuentaBancaria { get; set; }
        public int? IdEntidadResponsable { get; set; }
        public int IdTipoMovimiento { get; set; }
        public int IdFormaMovimiento { get; set; }
        public int? IdTipoDocumento { get; set; }
        public int? IdCategoria { get; set; }
        public int IdEstadoMovimiento { get; set; }
        public string NroOperacion { get; set; }
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public Decimal Monto { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public Decimal TipoCambio { get; set; }
        public string nTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
        public int UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Decimal? MontoSinIGV { get; set; }
        public int? IdComprobante { get; set; }
        public Decimal? SaldoBancario { get; set; }

        public string NombreEntidadR { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCuenta { get; set; }
        public string SimboloMoneda { get; set; }
        //Variables del comprobante
        public Decimal? cmpMontoPendiente { get; set; }
        public bool cmpCancelado { get; set; }

        public string NumeroDocumento2 { get; set; }

    }
}
