using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class DetalleProformaDTO
    {
        public Int32 IdProforma { get; set; }
        public Int32 IdItem { get; set; }
        public string NombreItem { get; set; }
        public Int32 Cantidad { get; set; }
        public Nullable<decimal> PrecioUnudad { get; set; }
        public Nullable<decimal> MontoTotal { get; set; }
        public Nullable<decimal> TipoCambio { get; set; }
        public Nullable<decimal> ProcentajeIgv { get; set; }
        public Nullable<decimal> Igv { get; set; }
    }
}
