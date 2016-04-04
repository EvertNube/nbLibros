using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class Select2DTO
    {
        public int id { get; set; }
        public string text { get; set; }
        public string disabled { get; set; }
        public IList<Select2DTO> children { get; set; }
    }
}
