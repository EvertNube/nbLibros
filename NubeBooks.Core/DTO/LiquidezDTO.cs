using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class LiquidezDTO
    {
        public Decimal Monto { get; set; }
        public string sMonto { get; set; }
        public int Mes { get; set; }
        public string nombreMes { get; set; }
        public Decimal Ingreso { get; set; }
        public Decimal Egreso { get; set; }
        public Decimal Ingreso_SinIGV { get; set; }
        public Decimal Egreso_SinIGV { get; set; }
    }
}
