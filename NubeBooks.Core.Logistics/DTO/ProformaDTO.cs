using NubeBooks.Core.DTO;
using NubeBooks.Data;
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
        public int IdProforma { get; set; }
        public string CodigoProforma { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContacto { get; set; }
        public int IdEntidadResponsable { get; set; }
        public int? IdMoneda { get; set; }
        public int? IdCuentaBancaria { get; set; }
        public int ValidezOferta { get; set; }
        public string MetodoPago { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaProforma { get; set; }
        public string LugarEntrega { get; set; }

        public DateTime? FechaFacturacion { get; set; }
        public DateTime? FechaCobranza { get; set; }
                
        public DateTime? FechaRegistro { get; set; }

        public string ComentarioProforma { get; set; }
        public string ComentarioAdiccional { get; set; }

        public string OrdenCompra { get; set; }

        public int? Estado { get; set; }
        public List<DetalleProformaDTO> DetalleProforma { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public ContactoDTO Contacto { get; set; }
        public EntidadResponsableDTO EntidadResponsable { get; set; }
        
        public List<CuentaBancariaDTO> CuentaBancaria { get; set; }

        public string NombreCuentaBancaria { get; set; }
    }
}
