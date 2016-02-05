using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class AreaPorComprobanteDTO
    {
        public int IdArea { get; set; }
        public int IdComprobante { get; set; }
        public Decimal Monto { get; set; }
        public string NombreArea { get; set; }
    }
}
