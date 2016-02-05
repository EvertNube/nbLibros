using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class TipoDocumentoDTO
    {
        public int IdTipoDocumento { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public string NombreTipo { get; set; }
    }
}
