//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NubeBooks.Data
{
    using System;
    
    public partial class SP_Rep_FacturacionPorHonorarios_Result
    {
        public int IdHonorario { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public int IdEmpresa { get; set; }
        public Nullable<decimal> Monto { get; set; }
        public Nullable<decimal> Monto_SinIGV { get; set; }
    }
}
