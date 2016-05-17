using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class ProformaDTO
    {
        public Int32 IdProforma { get; set; }
        public string CodigoProforma { get; set; }
        public Int32 IdEmpresa { get; set; }
        public Int32 IdResponsable { get; set; }
        public Int32 IdEntidadResponsable { get; set; }
        public Int32 IdUbicacion { get; set; }
        public string FormaPago { get; set; }
        public string LugarEntrega { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public List<DetalleProformaDTO> DetalleProforma { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public ResponsableDTO Responsable { get; set; }
        public EntidadResponsableDTO EntidadResponsable { get; set; }
        //public UbicacionDTO Ubicacion { get; set; }
        public List<CuentaBancariaDTO> CuentaBancaria { get; set; }
    }
}
