using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class TipoMovimientoInvDTO
    {
        public int IdTipoMovimientoInv { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
