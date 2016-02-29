﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class MovimientoInvDTO
    {
        public int IdMovimientoInv { get; set; }
        public int IdFormaMovimientoInv { get; set; }
        public int IdItem { get; set; }
        public int? IdEntidadResponsable { get; set; }
        public int? IdUbicacion { get; set; }
        public string NroDocumento { get; set; }
        public string GuiaRemision { get; set; }
        public string SerieLote { get; set; }
        public int Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        public System.DateTime FechaInicial { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
        public int UsuarioCreacion { get; set; }
        public int IdEmpresa { get; set; }

        public string nItem { get; set; }
        public string nTipo { get; set; }
        public string nForma { get; set; }
    }
}
