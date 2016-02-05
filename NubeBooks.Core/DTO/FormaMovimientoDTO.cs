using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class FormaMovimientoDTO
    {
        public int IdFormaMovimiento { get; set; }
        public int IdTipoMovimiento { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        public string NombreTipo { get; set; }
    }
}
