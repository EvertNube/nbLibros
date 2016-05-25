using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NubeBooks.Models
{
    [Serializable]
    public class sItem
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public Decimal monto { get; set; }
        public int idElemento { get; set; }
        public string elemento { get; set; }
        public string tipo { get; set; }
    }
}