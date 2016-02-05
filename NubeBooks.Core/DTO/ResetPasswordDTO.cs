using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class ResetPasswordDTO
    {
        public string rt { get; set; }
        public string emp { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } 
    }
}
