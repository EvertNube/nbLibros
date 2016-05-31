using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class UbicacionDTO
    {
        public int IdUbicacion { get; set; }
        
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public int IdEmpresa { get; set; }
    }
}
