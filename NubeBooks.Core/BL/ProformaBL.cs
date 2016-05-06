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
                    Estado = x.Estado,
                    /////EntidadResponsable = new EntidadResponsableBL().getEntidadResponsableEnEmpresa_Only(x.IdEmpresa, x.IdResponsable),
                    //Empresa = new EmpresaBL().getEmpresa(x.IdEmpresa),
                    //Ubicacion = new UbicacionBL().getUbicacionEnEmpresa(x.IdEmpresa, x.IdUbicacion),
                    //Responsable = new ResponsableBL().getResponsableEnEmpresa(x.IdEmpresa, x.IdResponsable)
                }).OrderByDescending(x => x.FechaRegistro).ToList();

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
                    Estado = x.Estado,
                    EntidadResponsable = new EntidadResponsableBL().getEntidadResponsableEnEmpresa_Only(x.IdEmpresa, x.IdResponsable),
                    Empresa = new EmpresaBL().getEmpresa(x.IdEmpresa),
                    Ubicacion = new UbicacionBL().getUbicacionEnEmpresa(x.IdEmpresa, x.IdUbicacion),
                    Responsable = new ResponsableBL().getResponsableEnEmpresa(x.IdEmpresa, x.IdResponsable),
                    DetalleProforma = getDetalleProformaPorId(x.IdProforma)
                }).SingleOrDefault();
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
                    NombreItem = context.Item.FirstOrDefault(i => i.IdItem == x.IdItem).Nombre

                }).ToList();
                return result;
            }
        }
    }
}
