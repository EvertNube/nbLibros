﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class TipoEntidadDTO
    {
        public int IdTipoEntidad { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
