//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NubeBooks.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class CategoriaPorPeriodo
    {
        public int IdCategoria { get; set; }
        public int IdPeriodo { get; set; }
        public decimal Monto { get; set; }
    
        public virtual Categoria Categoria { get; set; }
        public virtual Periodo Periodo { get; set; }
    }
}
