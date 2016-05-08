using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NubeBooks.Core.DTO;


namespace NubeBooks.Core.BL
{
    public class ProformaBL : Base
    {
        public List<ProformaDTO> getProformaEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Proforma.Where(x => x.IdEmpresa == idEmpresa).Select(x => new ProformaDTO
                {
                    IdProforma = x.IdProforma,
                    CodigoProforma = x.CodigoProforma,
                    IdEmpresa = x.IdEmpresa,
                    IdResponsable = x.IdResponsable,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdUbicacion = x.IdUbicacion,
                    FormaPago = x.FormaPago,
                    LugarEntrega = x.LugarEntrega,
                    FechaEntrega = x.FechaEntrega,
                    FechaRegistro = x.FechaRegistro,
                    Estado = x.Estado
                }).OrderByDescending(x => x.FechaRegistro).ToList();

                foreach (var pro in result)
                {
                    pro.EntidadResponsable = new EntidadResponsableBL().getEntidadResponsableEnEmpresa_Only(pro.IdEmpresa, pro.IdResponsable);
                    pro.Empresa = new EmpresaBL().getEmpresa(pro.IdEmpresa);
                    pro.Ubicacion = new UbicacionBL().getUbicacionEnEmpresa(pro.IdEmpresa, pro.IdUbicacion);
                    pro.Responsable = new ResponsableBL().getResponsableEnEmpresa(pro.IdEmpresa, pro.IdResponsable);
                }

                return result;
            }
        }
        public ProformaDTO getProformaId(Int32 idProforma)
        {
            using (var context = getContext())
            {
                var result = context.Proforma.Where(x => x.IdProforma == idProforma).Select(x => new ProformaDTO
                {
                    IdProforma = x.IdProforma,
                    CodigoProforma = x.CodigoProforma,
                    IdEmpresa = x.IdEmpresa,
                    IdResponsable = x.IdResponsable,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdUbicacion = x.IdUbicacion,
                    FormaPago = x.FormaPago,
                    LugarEntrega = x.LugarEntrega,
                    FechaEntrega = x.FechaEntrega,
                    FechaRegistro = x.FechaRegistro,
                    Estado = x.Estado
                }).SingleOrDefault();

                result.EntidadResponsable = new EntidadResponsableBL().getEntidadResponsableEnEmpresa_Only(result.IdEmpresa, result.IdResponsable);
                result.Empresa = new EmpresaBL().getEmpresa(result.IdEmpresa);
                result.Ubicacion = new UbicacionBL().getUbicacionEnEmpresa(result.IdEmpresa, result.IdUbicacion);
                result.Responsable = new ResponsableBL().getResponsableEnEmpresa(result.IdEmpresa, result.IdResponsable);
                result.DetalleProforma = getDetalleProformaPorId(result.IdProforma);
                result.CuentaBancaria = new CuentaBancariaBL().getCuentasBancariasActivasPorTipoEnEmpresa(result.IdEmpresa,1);
                return result;
            }
        }
        public List<DetalleProformaDTO> getDetalleProformaPorId(int idProforma)
        {
            using (var context = getContext())
            {
                var result = context.DetalleProforma.Where(x => x.IdProforma == idProforma).Select(x => new DetalleProformaDTO
                {
                    IdProforma = x.IdProforma,
                    IdItem = x.IdItem,
                    Cantidad = x.Cantidad,
                    PrecioUnudad = x.PrecioUnudad,
                    MontoTotal = x.MontoTotal,
                    TipoCambio = x.TipoCambio,
                    NombreItem = context.Item.FirstOrDefault(i => i.IdItem == x.IdItem).Nombre,
                    Igv=x.IgV,
                    ProcentajeIgv=x.PorcentajeIgv
                }).ToList();
                return result;
            }
        }
    }
}
