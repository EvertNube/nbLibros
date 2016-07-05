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
                    SerieLote = x.SerieLote ?? "",
                    Cantidad = x.Cantidad,
                    UnidadMedida = x.UnidadMedida ?? "",
                    FechaInicial = x.FechaInicial,
                    FechaFin = x.FechaFin,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    UsuarioCreacion = x.UsuarioCreacion,
                    IdEmpresa = x.IdEmpresa,
                    nForma = x.FormaMovimientoInv.Nombre ?? "",
                    nItem = x.Item.Codigo + " - " + x.Item.Nombre,
                    nTipo = x.FormaMovimientoInv.TipoMovimientoInv.Nombre,
                    nUsuario = x.Usuario.Nombre
                }).ToList();
                return result;
            }
        }
        public List<MovimientoInvDTO> getMovimientoInvsEnEmpresaPorTipo(int idEmpresa, int tipo)
        {
            using (var context = getContext())
            {
                var lstStockLotes = context.SP_Get_StockLotes_En_Empresa(idEmpresa).ToList();

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
                    IdEmpresa = x.IdEmpresa,
                    nForma = x.FormaMovimientoInv.Nombre,
                    nItem = x.Item.Codigo + " - " + x.Item.Nombre,
                    nTipo = x.FormaMovimientoInv.TipoMovimientoInv.Nombre,
                    nUsuario = x.Usuario.Nombre
                }).ToList();

                foreach (var item in result)
                {
                    item.StockLote = lstStockLotes.Where(x => x.SerieLote == item.SerieLote).SingleOrDefault().StockLote.GetValueOrDefault();
                }

                return result;
            }
        }
        public int get_Stock_De_Lote_En_Empresa(int idEmpresa, string lote)
        {
            using (var context = getContext())
            {
                var result = context.SP_Get_StockLote_De_Lote_En_Empresa(idEmpresa, lote).SingleOrDefault();

                if (result != null) return result.StockLote.GetValueOrDefault();

                return 0;
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
        public bool delete(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.MovimientoInv.Where(x => x.IdMovimientoInv == id).SingleOrDefault();
                    context.MovimientoInv.Remove(row);
                    context.SaveChanges();
                    return true;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public List<TipoMovimientoInvDTO> getTipoMovimientoInv()
        {
            using (var context = getContext())
            {
                var result = context.TipoMovimientoInv.Select(x => new TipoMovimientoInvDTO
                {
                    IdTipoMovimientoInv = x.IdTipoMovimientoInv,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }
        public List<FormaMovimientoInvDTO> getFormaMovimientoInvPorTipo(int tipo)
        {
            using (var context = getContext())
            {
                var result = context.FormaMovimientoInv.Where(x => x.IdTipoMovimientoInv == tipo).Select(x => new FormaMovimientoInvDTO
                {
                    IdFormaMovimientoInv = x.IdFormaMovimientoInv,
                    IdTipoMovimientoInv = x.IdTipoMovimientoInv,
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
        public List<Select2DTO_B> getLotesEnEmpresa(int idEmpresa, int conIdItem = 0, string conLote = null)
        {
            using (var context = getContext())
            {
                if (conIdItem == 0 && conLote == null)
                {
                    var result = context.SP_Get_StockLotes_En_Empresa(idEmpresa).Where(x => x.StockLote.GetValueOrDefault() > 0).Select(x => new Select2DTO_B
                    {
                        text = x.SerieLote
                    }).ToList();
                    return result;
                }
                else
                {
                    var lstStockLotes = context.SP_Get_StockLotes_En_Empresa(idEmpresa).ToList();

                    var result = (from mov in context.MovimientoInv
                                  join form in context.FormaMovimientoInv on mov.IdFormaMovimientoInv equals form.IdFormaMovimientoInv
                                  //join stk in context.SP_Get_StockLotes_En_Empresa(idEmpresa).ToList() on mov.SerieLote equals stk.SerieLote
                                  where mov.IdEmpresa == idEmpresa && mov.IdItem == conIdItem && form.IdTipoMovimientoInv == 1
                                  select new MovimientoInvDTO { SerieLote = mov.SerieLote }).ToList();

                    foreach (var item in result)
                    {
                        item.StockLote = lstStockLotes.Where(x => x.SerieLote == item.SerieLote).SingleOrDefault().StockLote.GetValueOrDefault();
                    }

                    List<Select2DTO_B> lista = result.Where(x => x.StockLote > 0).Select(x => new Select2DTO_B { text = x.SerieLote }).Distinct().ToList();

                    if(!lista.Any(x => x.text == conLote)) { lista.Add(new Select2DTO_B() { text = conLote }); }
                    return lista;
                }
            }
        }

        public List<Select2DTO_B> getLotes_PorItem_EnEmpresa(int idEmpresa, int idItem)
        {
            using (var context = getContext())
            {
                var lstStockLotes = context.SP_Get_StockLotes_En_Empresa(idEmpresa).ToList();

                var result = (from mov in context.MovimientoInv
                              join form in context.FormaMovimientoInv on mov.IdFormaMovimientoInv equals form.IdFormaMovimientoInv
                              where mov.IdEmpresa == idEmpresa && mov.IdItem == idItem && form.IdTipoMovimientoInv == 1
                              select new MovimientoInvDTO { SerieLote = mov.SerieLote }).ToList();

                foreach (var item in result)
                {
                    item.StockLote = lstStockLotes.Where(x => x.SerieLote == item.SerieLote).SingleOrDefault().StockLote.GetValueOrDefault();
                }

                List<Select2DTO_B> lista = result.Where(x => x.StockLote > 0).Select(x => new Select2DTO_B { text = x.SerieLote }).Distinct().ToList();
                return lista;
            }
        }

        public List<UbicacionDTO> getUbicacionesEnEmpresa(int idEmpresa, string conLote = null)
        {
            using (var context = getContext())
            {
                if (conLote == null)
                {
                    var result = context.Ubicacion.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new UbicacionDTO
                    {
                        IdUbicacion = x.IdUbicacion,
                        Nombre = x.Nombre
                    }).OrderBy(x => x.Nombre).ToList();
                    return result;
                }
                else
                {
                    var result = (from mov in context.MovimientoInv
                                  join ubi in context.Ubicacion on mov.IdUbicacion equals ubi.IdUbicacion
                                  join form in context.FormaMovimientoInv on mov.IdFormaMovimientoInv equals form.IdFormaMovimientoInv
                                  where mov.IdEmpresa == idEmpresa && mov.SerieLote == conLote && form.IdTipoMovimientoInv == 1
                                  select new UbicacionDTO
                                  {
                                      IdUbicacion = mov.IdUbicacion ?? 0,
                                      Nombre = ubi.Nombre
                                  }).Distinct().ToList();
                    return result;
                }

            }
        }

        /*public List<UbicacionDTO> getUbicaciones_EnLote_EnEmpresa(int idEmpresa, string serieLote)
        {
            using (var context = getContext())
            {
                var result = (from mov in context.MovimientoInv
                              join ubi in context.Ubicacion on mov.IdUbicacion equals ubi.IdUbicacion
                              join form in context.FormaMovimientoInv on mov.IdFormaMovimientoInv equals form.IdFormaMovimientoInv
                              where mov.IdEmpresa == idEmpresa && mov.SerieLote == serieLote && form.IdTipoMovimientoInv == 1
                              select new UbicacionDTO
                              {
                                  IdUbicacion = mov.IdUbicacion ?? 0,
                                  Nombre = ubi.Nombre
                              }).Distinct().ToList();
                return result;
            }
        }*/

        public List<ItemDTO> getItemsEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Item.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new ItemDTO
                {
                    IdItem = x.IdItem,
                    Codigo = x.Codigo + " - " + x.Nombre
                }).OrderBy(x => x.Codigo).ToList();
                return result;
            }
        }

        public List<ItemDTO> getItemsEnEmpresa_PorTipoMov(int idEmpresa, int idTipo)
        {
            using (var context = getContext())
            {
                if (idTipo == 1)
                {
                    var result = context.Item.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new ItemDTO
                    {
                        IdItem = x.IdItem,
                        Codigo = x.Codigo + " - " + x.Nombre,
                        Precio = x.Precio ?? 0,
                        UnidadMedida = x.UnidadMedida
                    }).OrderBy(x => x.Codigo).ToList();
                    return result;
                }
                else
                {
                    var result = (from mov in context.MovimientoInv
                                   join fmv in context.FormaMovimientoInv on mov.IdFormaMovimientoInv equals fmv.IdFormaMovimientoInv
                                   join itm in context.Item on mov.IdItem equals itm.IdItem
                                   where mov.IdEmpresa == idEmpresa && fmv.IdTipoMovimientoInv == 1
                                   select new ItemDTO
                                   {
                                       IdItem = mov.IdItem,
                                       Codigo = itm.Codigo + " - " + itm.Nombre,
                                       Precio = itm.Precio ?? 0,
                                       UnidadMedida = itm.UnidadMedida
                                   }).Distinct().ToList();
                    return result;
                }
            }
        }
    }
}
