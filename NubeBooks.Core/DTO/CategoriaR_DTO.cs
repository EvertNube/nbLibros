using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class CategoriaR_DTO
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public int Orden { get; set; }
        public bool Estado { get; set; }
        public int? IdCategoriaPadre { get; set; }
        public int IdEmpresa { get; set; }
        public int Nivel { get; set; }
        public IList<CategoriaR_DTO> Hijos { get; set; }
        //public CategoriaR_DTO Padre { get; set; }

        public IList<ComprobanteR_DTO> Comprobantes { get; set; }
        public Decimal MontoTotal { get; set; }
    }
}
