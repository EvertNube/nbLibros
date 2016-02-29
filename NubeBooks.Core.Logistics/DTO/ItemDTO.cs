using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class ItemDTO
    {
        public int IdItem { get; set; }
        public int IdCategoriaItm { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UnidadMedida { get; set; }
        public bool Estado { get; set; }
        public int IdEmpresa { get; set; }

        public string nCategoriaItem { get; set; }
    }
}
