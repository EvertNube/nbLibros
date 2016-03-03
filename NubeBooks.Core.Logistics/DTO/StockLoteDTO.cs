using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class StockLoteDTO
    {
        public string SerieLote { get; set; }
        public int StockLote { get; set; }
    }
}
