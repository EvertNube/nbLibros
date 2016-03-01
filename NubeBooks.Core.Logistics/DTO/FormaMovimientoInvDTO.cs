using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class FormaMovimientoInvDTO
    {
        public int IdFormaMovimientoInv { get; set; }
        public int IdTipoMovimientoInv { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        public string nTipoMovimientoInv { get; set; }
    }
}
