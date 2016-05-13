using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class ComprobanteR_DTO
    {
        public int? IdCategoria { get; set; }
        public int? IdCategoriaPadre { get; set; }
        public string NombreCategoria { get; set; }
        public int IdComprobante { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreEntidad { get; set; }
        public string NombreDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Moneda { get; set; }
        public Decimal Monto { get; set; }
        public Decimal MontoSinIGV { get; set; }
        public string Areas { get; set; }
        public string Comentario { get; set; }
    }
}
