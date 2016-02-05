using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class CategoriaPorPeriodoDTO
    {
        public int IdCategoria { get; set; }
        public int IdPeriodo { get; set; }
        public decimal Monto { get; set; }
    }
}
