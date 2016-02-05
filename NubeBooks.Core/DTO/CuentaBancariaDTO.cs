using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class CuentaBancariaDTO
    {
        public int IdCuentaBancaria { get; set; }
        public string NombreCuenta { get; set; }
        public DateTime FechaConciliacion { get; set; }
        public Decimal SaldoDisponible { get; set; }
        public Decimal SaldoBancario { get; set; }
        public bool Estado { get; set; }
        public int? IdMoneda { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdTipoCuenta { get; set; }
        public IList<MovimientoDTO> listaMovimiento { get; set; }
        public IPagedList<MovimientoDTO> listaMovimientoPL { get; set; }

        public string NombreMoneda { get; set; }
        public string SimboloMoneda { get; set; }
    }
}
