using NubeBooks.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.DTO
{
    [Serializable]
    public class ProformaDTO
    {
        public Int32 IdProforma { get; set; }
        public string CodigoProforma { get; set; }
        public Int32 IdEmpresa { get; set; }
        public Int32 IdResponsable { get; set; }
        public Int32 IdEntidadResponsable { get; set; }
        public Nullable<Int32> IdMoneda { get; set; }
        public Int32 ValidezOferta { get; set; }
        public string MetodoPago { get; set; }
        public Nullable<DateTime> FechaEntrega { get; set; }
        public Nullable<DateTime> FechaProforma { get; set; }
        public string LugarEntrega { get; set; }

        public Nullable<DateTime> FechaFacturacion { get; set; }
        public Nullable<DateTime> FechaCobranza { get; set; }
                
        public DateTime? FechaRegistro { get; set; }

        public string ComenterioProforma { get; set; }
        public string ComentarioAdiccional { get; set; }

        public int? Estado { get; set; }
        public List<DetalleProformaDTO> DetalleProforma { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public ResponsableDTO Responsable { get; set; }
        public EntidadResponsableDTO EntidadResponsable { get; set; }
        
        public List<CuentaBancariaDTO> CuentaBancaria { get; set; }
    }
}
