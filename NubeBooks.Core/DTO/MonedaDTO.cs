using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class MonedaDTO
    {
        public int IdMoneda { get; set; }
        public string Nombre { get; set; }
        public string Simbolo { get; set; }
    }
}
