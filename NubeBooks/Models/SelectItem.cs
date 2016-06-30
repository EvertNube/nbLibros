using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NubeBooks.Models
{
    [Serializable]
    public class SelectItem
    {
        public int valor { get; set; }
        public string nombre { get; set; }
    }
}