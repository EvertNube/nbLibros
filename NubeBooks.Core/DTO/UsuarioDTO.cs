using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string Cuenta { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Pass { get; set; }
        public bool Active { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public int IdRol { get; set; }
        public int? IdCargo { get; set; }
        public int IdEmpresa { get; set; }
        
        public string Token { get; set; }
        //public EmpresaDTO empresa { get; set; }
        public string nombreEmpresa { get; set; }
        public string codigoEmpresa { get; set; }
         
        public string NombreRol { get; set; }
        
    }
}
