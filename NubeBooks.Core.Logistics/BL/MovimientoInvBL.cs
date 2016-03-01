using NubeBooks.Core.Logistics.DTO;
using NubeBooks.Core.DTO;
using NubeBooks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.BL
{
    public class MovimientoInvBL : Base
    {
        public List<MovimientoInvDTO> getMovimientoInvsEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.MovimientoInv.Where(x => x.IdEmpresa == idEmpresa).Select(x => new MovimientoInvDTO
                {
                    IdMovimientoInv = x.IdMovimientoInv,
                    IdFormaMovimientoInv = x.IdFormaMovimientoInv,
                    IdTipoMovimientoInv = x.FormaMovimientoInv.IdTipoMovimientoInv,
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
                    IdEmpresa = x.IdEmpresa
                }).ToList();
                return result;
            }
        }
        public List<MovimientoInvDTO> getMovimientoInvsEnEmpresaPorTipo(int idEmpresa, int tipo)
        {
            using (var context = getContext())
            {
                var result = context.MovimientoInv.Where(x => x.IdEmpresa == idEmpresa && x.FormaMovimientoInv.IdTipoMovimientoInv == tipo).Select(x => new MovimientoInvDTO
                {
                    IdMovimientoInv = x.IdMovimientoInv,
                    IdFormaMovimientoInv = x.IdFormaMovimientoInv,
                    IdTipoMovimientoInv = x.FormaMovimientoInv.IdTipoMovimientoInv,
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
                    IdEmpresa = x.IdEmpresa
                }).ToList();
                return result;
            }
        }
        public List<MovimientoInvDTO> getMovimientoInvsEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.MovimientoInv.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new MovimientoInvDTO
                {
                    IdMovimientoInv = x.IdMovimientoInv,
                    IdFormaMovimientoInv = x.IdFormaMovimientoInv,
                    IdTipoMovimientoInv = x.FormaMovimientoInv.IdTipoMovimientoInv,
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
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.NroDocumento).ToList();
                return result;
            }
        }
        public MovimientoInvDTO getMovimientoInvEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.MovimientoInv.Where(x => x.IdEmpresa == idEmpresa && x.IdMovimientoInv == id)
                    .Select(x => new MovimientoInvDTO
                    {
                        IdMovimientoInv = x.IdMovimientoInv,
                        IdFormaMovimientoInv = x.IdFormaMovimientoInv,
                        IdTipoMovimientoInv = x.FormaMovimientoInv.IdTipoMovimientoInv,
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
                        IdEmpresa = x.IdEmpresa
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(MovimientoInvDTO MovimientoInv)
        {
            using (var context = getContext())
            {
                try
                {
                    MovimientoInv nuevo = new MovimientoInv();
                    nuevo.IdFormaMovimientoInv = MovimientoInv.IdFormaMovimientoInv;
                    nuevo.IdItem = MovimientoInv.IdItem;
                    nuevo.IdEntidadResponsable = MovimientoInv.IdEntidadResponsable;
                    nuevo.IdUbicacion = MovimientoInv.IdUbicacion;
                    nuevo.NroDocumento = MovimientoInv.NroDocumento;
                    nuevo.GuiaRemision = MovimientoInv.GuiaRemision;
                    nuevo.SerieLote = MovimientoInv.SerieLote;
                    nuevo.Cantidad = MovimientoInv.Cantidad;
                    nuevo.UnidadMedida = MovimientoInv.UnidadMedida;
                    nuevo.FechaInicial = MovimientoInv.FechaInicial;
                    nuevo.FechaFin = MovimientoInv.FechaFin;
                    nuevo.Comentario = MovimientoInv.Comentario;
                    nuevo.Estado = true;
                    nuevo.UsuarioCreacion = MovimientoInv.UsuarioCreacion;
                    nuevo.IdEmpresa = MovimientoInv.IdEmpresa;
                    context.MovimientoInv.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(MovimientoInvDTO MovimientoInv)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.MovimientoInv.Where(x => x.IdMovimientoInv == MovimientoInv.IdMovimientoInv).SingleOrDefault();
                    row.IdFormaMovimientoInv = MovimientoInv.IdFormaMovimientoInv;
                    row.IdItem = MovimientoInv.IdItem;
                    row.IdEntidadResponsable = MovimientoInv.IdEntidadResponsable;
                    row.IdUbicacion = MovimientoInv.IdUbicacion;
                    row.NroDocumento = MovimientoInv.NroDocumento;
                    row.GuiaRemision = MovimientoInv.GuiaRemision;
                    row.SerieLote = MovimientoInv.SerieLote;
                    row.Cantidad = MovimientoInv.Cantidad;
                    row.UnidadMedida = MovimientoInv.UnidadMedida;
                    row.FechaInicial = MovimientoInv.FechaInicial;
                    row.FechaFin = MovimientoInv.FechaFin;
                    row.Comentario = MovimientoInv.Comentario;
                    row.Estado = MovimientoInv.Estado;
                    row.UsuarioCreacion = MovimientoInv.UsuarioCreacion;
                    row.IdEmpresa = MovimientoInv.IdEmpresa;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public List<FormaMovimientoInvDTO> getFormaMovimientoInvPorTipo(int tipo)
        {
            using (var context = getContext())
            {
                var result = context.FormaMovimientoInv.Where(x => x.IdTipoMovimientoInv == tipo).Select(x => new FormaMovimientoInvDTO
                {
                    IdFormaMovimientoInv = x.IdFormaMovimientoInv,
                    nTipoMovimientoInv = x.TipoMovimientoInv.Nombre,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public List<EntidadResponsableDTO> getProveedoresEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdTipoEntidad == 2 && x.IdEmpresa == idEmpresa && x.Estado).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public List<UbicacionDTO> getUbicacionesEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Ubicacion.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new UbicacionDTO
                {
                    IdUbicacion = x.IdUbicacion,
                    Nombre = x.Nombre
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
    }
}
