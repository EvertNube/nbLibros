using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class Select2DTO_B
    {
        public int? id { get; set; }
        public string text { get; set; }
        public bool ejecutado { get; set; }
    }
}
