using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class SelectDTO
    {
        public int SelectItemId { get; set; }
        public string SelectItemName { get; set; }
    }
}
