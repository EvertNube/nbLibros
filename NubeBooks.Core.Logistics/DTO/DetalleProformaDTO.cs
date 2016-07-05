using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class DetalleProformaDTO
    {
        public Int32 IdDetalleProforma { get; set; }
        public Int32 IdProforma { get; set; }
        public Int32 IdItem { get; set; }
        public string NombreItem { get; set; }
        public Int32 Cantidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> MontoTotal { get; set; }
        public Nullable<decimal> TipoCambio { get; set; }
        public Nullable<decimal> PorcentajeIgv { get; set; }
        public Nullable<decimal> Igv { get; set; }
        public string UnidadMedida { get; set; }
    }
}
