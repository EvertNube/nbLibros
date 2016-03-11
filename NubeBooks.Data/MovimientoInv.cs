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
    
    public partial class MovimientoInv
    {
        public int IdMovimientoInv { get; set; }
        public int IdFormaMovimientoInv { get; set; }
        public int IdItem { get; set; }
        public Nullable<int> IdEntidadResponsable { get; set; }
        public Nullable<int> IdUbicacion { get; set; }
        public string NroDocumento { get; set; }
        public string GuiaRemision { get; set; }
        public string SerieLote { get; set; }
        public int Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        public System.DateTime FechaInicial { get; set; }
        public Nullable<System.DateTime> FechaFin { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
        public int UsuarioCreacion { get; set; }
        public int IdEmpresa { get; set; }
    
        public virtual Empresa Empresa { get; set; }
        public virtual EntidadResponsable EntidadResponsable { get; set; }
        public virtual FormaMovimientoInv FormaMovimientoInv { get; set; }
        public virtual Item Item { get; set; }
        public virtual Ubicacion Ubicacion { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}