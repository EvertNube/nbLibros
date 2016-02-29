using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class CategoriaItmDTO
    {
        public int IdCategoriaItm { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public int IdEmpresa { get; set; }
    }
}
