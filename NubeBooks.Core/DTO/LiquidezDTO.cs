﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class LiquidezDTO
    {
        public Decimal Monto { get; set; }
        public int Mes { get; set; }
    }
}