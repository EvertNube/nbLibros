using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public int Orden { get; set; }
        public bool Estado { get; set; }
        public int? IdCategoriaPadre { get; set; }
        public int IdEmpresa { get; set; }
        public IList<CategoriaDTO> Hijos { get; set; }

        public Decimal? Presupuesto { get; set; }
        public int Nivel { get; set; }
    }
}
