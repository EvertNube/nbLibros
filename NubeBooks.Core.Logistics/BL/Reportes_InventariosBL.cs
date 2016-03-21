using NubeBooks.Core.Logistics.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.BL
{
    public class Reportes_InventariosBL : Base
    {
        #region Movimientos de Inventarios
        public List<MovimientoInvDTO> Get_Reporte_De_Movimientos_De_Inventarios(int IdItem, int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = getContext())
            {
                //Por mejorar SP buscar solo stock de un solo Item
                var result = context.SP_Get_Rep_De_Movimientos_De_Inventarios(IdItem, idEmpresa, fechaInicio, fechaFin).Select(x => new MovimientoInvDTO
                {
                    IdMovimientoInv = x.IdMovimientoInv,
                    IdFormaMovimientoInv = x.IdFormaMovimientoInv,
                    IdTipoMovimientoInv = x.IdTipoMovimientoInv,
                    IdItem = x.IdItem,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdUbicacion = x.IdUbicacion,
                    NroDocumento = x.NroDocumento,
                    GuiaRemision = x.GuiaRemision,
                    SerieLote = x.SerieLote,
                    Cantidad = x.Cantidad,
                    UnidadMedida = x.UnidadMedida,
                    FechaInicial = x.FechaInicial,
                    FechaFin = x.FechaFin,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    UsuarioCreacion = x.UsuarioCreacion,
                    IdEmpresa = x.IdEmpresa,
                    nForma = x.nForma,
                    nItem = x.nItemCodigo + " - " + x.nItem,
                    nTipo = x.nTipo,
                    nUsuario = x.nUsuario,
                    StockLote = x.StockLote
                }).OrderBy(x => x.FechaInicial).ToList();

                return result;
            }
        }

        public List<MovimientoInvDTO> Get_Reporte_De_Inventarios(int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_Get_Rep_De_Inventarios(idEmpresa, fechaInicio, fechaFin).Select(x => new MovimientoInvDTO
                {
                    IdItem = x.IdItem,
                    nItem = x.nItem,
                    nItemCodigo = x.nItemCodigo,
                    nCategoria = x.nCategoria,
                    SerieLote = x.SerieLote,
                    FechaFin = x.FechaFin,
                    StockLote = x.StockLote,
                    SaldoItem = x.SaldoItem,
                    nUbicacion = x.nUbicacion
                }).OrderBy(x => x.FechaInicial).ToList();

                return result;
            }
        }
        #endregion
    }
}
