using NubeBooks.Core.DTO;
using NubeBooks.Core.BL;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class ComprobanteBL : Base
    {
        public List<ComprobanteDTO> getComprobantesEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa).Select(x => new ComprobanteDTO
                {
                    IdComprobante = x.IdComprobante,
                    IdTipoComprobante = x.IdTipoComprobante,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdMoneda = x.IdMoneda,
                    IdEmpresa = x.IdEmpresa,
                    NroDocumento = x.NroDocumento,
                    Monto = x.Monto,
                    IdArea = x.IdArea,
                    IdResponsable = x.IdResponsable,
                    IdCategoria = x.IdCategoria,
                    IdProyecto = x.Proyecto.FirstOrDefault().IdProyecto,
                    FechaEmision = x.FechaEmision,
                    FechaConclusion = x.FechaConclusion,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    Ejecutado = x.Ejecutado,
                    IdHonorario = x.IdHonorario,
                    NombreEntidad = x.EntidadResponsable.Nombre ?? "",
                    NombreMoneda = x.Moneda.Nombre,
                    NombreTipoComprobante = x.TipoComprobante.Nombre,
                    NombreTipoDocumento = x.TipoDocumento.Nombre ?? "",
                    SimboloMoneda = x.Moneda.Simbolo,
                    MontoSinIGV = x.MontoSinIGV,
                    TipoCambio = x.TipoCambio,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaPago = x.FechaPago,
                    NombreUsuario = x.Usuario.Cuenta,
                    NombreCategoria = x.Categoria.Nombre ?? "",
                    NombreProyecto = x.Proyecto.FirstOrDefault().Nombre ?? ""
                }).OrderBy(x => x.NroDocumento).ToList();
                return result;
            }
        }
        public List<ComprobanteDTO> getComprobantesEnEmpresaPorTipo(int idEmpresa, int tipo)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoComprobante == tipo).Select(x => new ComprobanteDTO
                {
                    IdComprobante = x.IdComprobante,
                    IdTipoComprobante = x.IdTipoComprobante,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdMoneda = x.IdMoneda,
                    IdEmpresa = x.IdEmpresa,
                    NroDocumento = x.NroDocumento,
                    Monto = x.Monto,
                    IdArea = x.IdArea,
                    IdResponsable = x.IdResponsable,
                    IdCategoria = x.IdCategoria,
                    IdProyecto = x.Proyecto.FirstOrDefault().IdProyecto,
                    FechaEmision = x.FechaEmision,
                    FechaConclusion = x.FechaConclusion,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    Ejecutado = x.Ejecutado,
                    IdHonorario = x.IdHonorario,
                    NombreEntidad = x.EntidadResponsable.Nombre ?? "",
                    NombreMoneda = x.Moneda.Nombre,
                    NombreTipoComprobante = x.TipoComprobante.Nombre,
                    NombreTipoDocumento = x.TipoDocumento.Nombre,
                    SimboloMoneda = x.Moneda.Simbolo,
                    MontoSinIGV = x.MontoSinIGV,
                    TipoCambio = x.TipoCambio,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaPago = x.FechaPago,
                    NombreUsuario = x.Usuario.Cuenta,
                    NombreCategoria = x.Categoria.Nombre ?? "",
                    NombreProyecto = x.Proyecto.FirstOrDefault().Nombre ?? ""
                }).OrderBy(x => x.FechaEmision).ToList();
                return result;
            }
        }
        public List<ComprobanteDTO> getComprobantesEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new ComprobanteDTO
                {
                    IdComprobante = x.IdComprobante,
                    IdTipoComprobante = x.IdTipoComprobante,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdMoneda = x.IdMoneda,
                    IdEmpresa = x.IdEmpresa,
                    NroDocumento = x.NroDocumento,
                    Monto = x.Monto,
                    IdArea = x.IdArea,
                    IdResponsable = x.IdResponsable,
                    IdCategoria = x.IdCategoria,
                    IdProyecto = x.Proyecto.FirstOrDefault().IdProyecto,
                    FechaEmision = x.FechaEmision,
                    FechaConclusion = x.FechaConclusion,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    Ejecutado = x.Ejecutado,
                    IdHonorario = x.IdHonorario,
                    NombreEntidad = x.EntidadResponsable.Nombre,
                    NombreMoneda = x.Moneda.Nombre,
                    NombreTipoComprobante = x.TipoComprobante.Nombre,
                    NombreTipoDocumento = x.TipoDocumento.Nombre,
                    SimboloMoneda = x.Moneda.Simbolo,
                    MontoSinIGV = x.MontoSinIGV,
                    TipoCambio = x.TipoCambio,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaPago = x.FechaPago,
                    NombreUsuario = x.Usuario.Cuenta,
                    NombreCategoria = x.Categoria.Nombre ?? "",
                    NombreProyecto = x.Proyecto.FirstOrDefault().Nombre
                }).OrderBy(x => x.NroDocumento).ToList();
                return result;
            }
        }
        public ComprobanteDTO getComprobanteEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdComprobante == id && x.IdEmpresa == idEmpresa)
                    .Select(r => new ComprobanteDTO
                    {
                        IdComprobante = r.IdComprobante,
                        IdTipoComprobante = r.IdTipoComprobante,
                        IdTipoDocumento = r.IdTipoDocumento,
                        IdEntidadResponsable = r.IdEntidadResponsable,
                        IdEntidadResponsable2 = r.IdEntidadResponsable2,
                        IdMoneda = r.IdMoneda,
                        IdEmpresa = r.IdEmpresa,
                        NroDocumento = r.NroDocumento,
                        Monto = r.Monto,
                        IdArea = r.IdArea,
                        IdResponsable = r.IdResponsable,
                        IdCategoria = r.IdCategoria,
                        IdProyecto = r.Proyecto.FirstOrDefault().IdProyecto,
                        FechaEmision = r.FechaEmision,
                        FechaConclusion = r.FechaConclusion,
                        Comentario = r.Comentario,
                        Estado = r.Estado,
                        Ejecutado = r.Ejecutado,
                        IdHonorario = r.IdHonorario,
                        NombreEntidad = r.EntidadResponsable.Nombre,
                        NombreMoneda = r.Moneda.Nombre,
                        NombreTipoComprobante = r.TipoComprobante.Nombre,
                        NombreTipoDocumento = r.TipoDocumento.Nombre,
                        SimboloMoneda = r.Moneda.Simbolo,
                        MontoSinIGV = r.MontoSinIGV,
                        TipoCambio = r.TipoCambio,
                        UsuarioCreacion = r.UsuarioCreacion,
                        FechaPago = r.FechaPago,
                        NombreUsuario = r.Usuario.Cuenta,
                        NombreCategoria = r.Categoria.Nombre ?? "",
                        NombreProyecto = r.Proyecto.FirstOrDefault().Nombre,
                        lstMontos = r.AreaPorComprobante.Select(x => new AreaPorComprobanteDTO
                        {
                            IdArea = x.IdArea,
                            IdComprobante = x.IdComprobante,
                            Monto = x.Monto,
                            NombreArea = x.Area.Nombre
                        }).ToList()
                    }).SingleOrDefault();
                return result;
            }
        }

        public ComprobanteDTO getComprobanteEjecutadoEnEmpresa(int id, int idCuentaBancaria, int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.SP_Get_MontoIncompletoEnComprobante(id, idCuentaBancaria, idEmpresa)
                    .Select(r => new ComprobanteDTO
                    {
                        IdComprobante = r.IdComprobante,
                        IdTipoComprobante = r.IdTipoComprobante,
                        IdTipoDocumento = r.IdTipoDocumento,
                        IdEntidadResponsable = r.IdEntidadResponsable,
                        IdMoneda = r.IdMoneda,
                        IdEmpresa = r.IdEmpresa,
                        NroDocumento = r.NroDocumento,
                        Monto = r.Monto,
                        MontoSinIGV = r.MontoSinIGV,
                        IdArea = r.IdArea,
                        IdResponsable = r.IdResponsable,
                        IdCategoria = r.IdCategoria,
                        //IdProyecto = r.IdProyecto,
                        FechaEmision = r.FechaEmision,
                        FechaConclusion = r.FechaConclusion,
                        Comentario = r.Comentario,
                        Estado = r.Estado,
                        Ejecutado = r.Ejecutado,
                        IdHonorario = r.IdHonorario,
                        TipoCambio = r.TipoCambio,
                        UsuarioCreacion = r.UsuarioCreacion,
                        MontoIncompleto = r.MontoIncompleto.GetValueOrDefault()
                    }).SingleOrDefault();
                return result;
            }
        }

        public bool add(ComprobanteDTO Comprobante)
        {
            using (var context = getContext())
            {
                try
                {
                    Comprobante nuevo = new Comprobante();
                    nuevo.IdTipoComprobante = Comprobante.IdTipoComprobante;
                    nuevo.IdTipoDocumento = Comprobante.IdTipoDocumento;
                    nuevo.IdEntidadResponsable = Comprobante.IdEntidadResponsable;
                    nuevo.IdEntidadResponsable2 = Comprobante.IdEntidadResponsable2;
                    nuevo.IdMoneda = Comprobante.IdMoneda;
                    nuevo.IdEmpresa = Comprobante.IdEmpresa;
                    nuevo.NroDocumento = Comprobante.NroDocumento;
                    nuevo.Monto = Comprobante.Monto;
                    nuevo.IdArea = Comprobante.IdArea;
                    nuevo.IdResponsable = Comprobante.IdResponsable;
                    nuevo.IdCategoria = Comprobante.IdCategoria;
                    //nuevo.IdProyecto = Comprobante.IdProyecto;
                    if(Comprobante.IdTipoComprobante == 1 || Comprobante.IdTipoComprobante == 3)
                    { 
                        if (Comprobante.IdProyecto > 0)
                        {
                            var pProyecto = context.Proyecto.Where(x => x.IdProyecto == Comprobante.IdProyecto).FirstOrDefault();
                            nuevo.Proyecto.Add(pProyecto);
                        }
                    }
                    else if(Comprobante.IdEntidadResponsable2 > 0)
                    {
                        if (Comprobante.IdProyecto > 0)
                        {
                            var pProyecto = context.Proyecto.Where(x => x.IdProyecto == Comprobante.IdProyecto).FirstOrDefault();
                            nuevo.Proyecto.Add(pProyecto);
                        }
                    }
                    nuevo.FechaEmision = Comprobante.FechaEmision;
                    nuevo.FechaConclusion = Comprobante.FechaConclusion;
                    nuevo.Comentario = Comprobante.Comentario;
                    nuevo.Estado = true;
                    nuevo.Ejecutado = false;
                    nuevo.IdHonorario = Comprobante.IdHonorario;
                    nuevo.MontoSinIGV = Comprobante.MontoSinIGV;
                    nuevo.TipoCambio = Comprobante.TipoCambio;
                    nuevo.UsuarioCreacion = Comprobante.UsuarioCreacion;
                    
                    context.Comprobante.Add(nuevo);

                    foreach (var item in Comprobante.lstMontos)
                    {
                        AreaPorComprobante novo = new AreaPorComprobante();
                        novo.IdArea = item.IdArea;
                        novo.IdComprobante = nuevo.IdComprobante;
                        novo.Monto = item.Monto;
                        nuevo.AreaPorComprobante.Add(novo);
                    }

                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(ComprobanteDTO Comprobante)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Comprobante.Where(x => x.IdComprobante == Comprobante.IdComprobante).SingleOrDefault();
                    row.IdTipoComprobante = Comprobante.IdTipoComprobante;
                    row.IdTipoDocumento = Comprobante.IdTipoDocumento;
                    row.IdEntidadResponsable = Comprobante.IdEntidadResponsable;
                    row.IdEntidadResponsable2 = Comprobante.IdEntidadResponsable2;
                    row.IdMoneda = Comprobante.IdMoneda;
                    row.IdEmpresa = Comprobante.IdEmpresa;
                    row.NroDocumento = Comprobante.NroDocumento;
                    row.Monto = Comprobante.Monto;
                    row.IdArea = Comprobante.IdArea;
                    row.IdResponsable = Comprobante.IdResponsable;
                    row.IdCategoria = Comprobante.IdCategoria;
                    //row.IdProyecto = Comprobante.IdProyecto;
                    if (Comprobante.IdTipoComprobante == 1 || Comprobante.IdTipoComprobante == 3)
                    {
                        if (row.Proyecto.FirstOrDefault() != null && row.Proyecto.FirstOrDefault().IdProyecto != Comprobante.IdProyecto)
                        {
                            var zProyecto = row.Proyecto.FirstOrDefault();
                            row.Proyecto.Remove(zProyecto);
                            if (Comprobante.IdProyecto > 0)
                            {
                                var xProyecto = context.Proyecto.Where(x => x.IdProyecto == Comprobante.IdProyecto).FirstOrDefault();
                                row.Proyecto.Add(xProyecto);
                            }
                        }
                        else if (row.Proyecto.FirstOrDefault() == null)
                        {
                            var xProyecto = context.Proyecto.Where(x => x.IdProyecto == Comprobante.IdProyecto).FirstOrDefault();
                            row.Proyecto.Add(xProyecto);
                        }
                    }
                    else if(Comprobante.IdEntidadResponsable2 > 0)
                    {
                        if (row.Proyecto.FirstOrDefault() != null && row.Proyecto.FirstOrDefault().IdProyecto != Comprobante.IdProyecto)
                        {
                            var zProyecto = row.Proyecto.FirstOrDefault();
                            row.Proyecto.Remove(zProyecto);
                            if (Comprobante.IdProyecto > 0)
                            {
                                var xProyecto = context.Proyecto.Where(x => x.IdProyecto == Comprobante.IdProyecto).FirstOrDefault();
                                row.Proyecto.Add(xProyecto);
                            }
                        }
                        else if (row.Proyecto.FirstOrDefault() == null)
                        {
                            var xProyecto = context.Proyecto.Where(x => x.IdProyecto == Comprobante.IdProyecto).FirstOrDefault();
                            row.Proyecto.Add(xProyecto);
                        }
                    }
                        
                    row.FechaEmision = Comprobante.FechaEmision;
                    row.FechaConclusion = Comprobante.FechaConclusion;
                    row.Comentario = Comprobante.Comentario;
                    row.Estado = Comprobante.Estado;
                    row.IdHonorario = Comprobante.IdHonorario;
                    row.MontoSinIGV = Comprobante.MontoSinIGV;
                    row.TipoCambio = Comprobante.TipoCambio;
                    row.UsuarioCreacion = Comprobante.UsuarioCreacion;

                    var allmontos = from m in context.AreaPorComprobante
                                    where m.IdComprobante == row.IdComprobante
                                    select m;

                    foreach (var item in allmontos)
                    {
                        row.AreaPorComprobante.Remove(item);
                    }

                    foreach (var item in Comprobante.lstMontos)
                    {
                        AreaPorComprobante novo = new AreaPorComprobante();
                        novo.IdArea = item.IdArea;
                        novo.IdComprobante = row.IdComprobante;
                        novo.Monto = item.Monto;
                        row.AreaPorComprobante.Add(novo);
                    }

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
                    var row = context.Comprobante.Where(x => x.IdComprobante == id).SingleOrDefault();
                    //Si el comprobante esta ligado a Movimientos primero se eliminan todos los Movimientos
                    var allmovimientos = context.Movimiento.Where(x => x.IdComprobante == row.IdComprobante).ToList();
                    MovimientoBL movBL = new MovimientoBL();
                    foreach (var item in allmovimientos)
                    {
                        movBL.delete(item.IdMovimiento);
                    }
                    //Si el comprobante esta ligado a pagos por areas primero se eliminan todos sus montos AreasPorComprobantes
                    var allmontos = context.AreaPorComprobante.Where(x => x.IdComprobante == row.IdComprobante).ToList();
                    foreach (var item in allmontos)
                    {
                        row.AreaPorComprobante.Remove(item);
                    }
                    context.Comprobante.Remove(row);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int repeatedNroDocumento(int idEmpresa, int idComprobante, string NroDocumento)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && (x.IdTipoComprobante == 1 || x.IdTipoComprobante == 3) && x.NroDocumento == NroDocumento && x.IdComprobante != idComprobante).SingleOrDefault();
                    if (row != null)
                    {
                        return row.IdTipoComprobante;
                    }
                    //No existe Comprobante con ese numero
                    return 0;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

        public bool ban(int id, string comentario)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Comprobante.Where(x => x.IdComprobante == id).SingleOrDefault();
                    if (row.IdTipoComprobante < 3)
                    {
                        //Si el comprobante esta ligado a Movimientos primero se eliminan todos los Movimientos
                        var allmovimientos = context.Movimiento.Where(x => x.IdComprobante == row.IdComprobante).ToList();
                        MovimientoBL movBL = new MovimientoBL();
                        foreach (var item in allmovimientos)
                        {
                            movBL.delete(item.IdMovimiento);
                        }
                        //Anulando el Comprobante
                        row.IdTipoComprobante = row.IdTipoComprobante == 1 ? 3 : 4;
                        row.Ejecutado = false;
                        row.Comentario = comentario;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool unban(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Comprobante.Where(x => x.IdComprobante == id).SingleOrDefault();
                    //Restableciendo el Comprobante
                    if (row.IdTipoComprobante > 2)
                    {
                        row.IdTipoComprobante = row.IdTipoComprobante == 3 ? 1 : 2;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool actualizarEjecutado(int idComprobante, bool ejecutado, DateTime fecha, int idEmpresa)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Comprobante.Where(x => x.IdComprobante == idComprobante && x.IdEmpresa == idEmpresa).SingleOrDefault();
                    row.Ejecutado = ejecutado;
                    if (ejecutado) { row.FechaPago = fecha; }
                    else { row.FechaPago = null; }

                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<TipoComprobanteDTO> getTipoDeComprobantes()
        {
            using (var context = getContext())
            {
                var result = context.TipoComprobante.Select(x => new TipoComprobanteDTO
                {
                    IdTipoComprobante = x.IdTipoComprobante,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }
        public List<TipoDocumentoDTO> getTipoDeDocumentos()
        {
            using (var context = getContext())
            {
                var result = context.TipoDocumento.Select(x => new TipoDocumentoDTO
                {
                    IdTipoDocumento = x.IdTipoDocumento,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }
        public List<TipoEntidadDTO> getTipoDeEntidades()
        {
            using (var context = getContext())
            {
                var result = context.TipoEntidad.Select(x => new TipoEntidadDTO
                {
                    IdTipoEntidad = x.IdTipoEntidad,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();

                return result;
            }
        }
        public List<EntidadResponsableDTO> getListaEntidadesEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdEmpresa == idEmpresa).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }

        public List<EntidadResponsableDTO> getListaClientesEnEmpresa(int idEmpresa)
        {
            //SOLO ACTIVOS
            //Clientes Entidad Tipo 1
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdTipoEntidad == 1 && x.IdEmpresa == idEmpresa && x.Estado).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Credito = x.Credito,
                    IdResponsable = x.IdResponsable
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public List<EntidadResponsableDTO> getListaProveedoresEnEmpresa(int idEmpresa)
        {
            //SOLO ACTIVOS
            //Proveedores Entidad Tipo 2
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdTipoEntidad == 2 && x.IdEmpresa == idEmpresa && x.Estado).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Credito = x.Credito
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<MonedaDTO> getListaMonedas()
        {
            using (var context = getContext())
            {
                var result = context.Moneda.Select(x => new MonedaDTO
                {
                    IdMoneda = x.IdMoneda,
                    Nombre = x.Nombre,
                    Simbolo = x.Simbolo
                }).Take(2).ToList();
                return result;
            }
        }

        public List<AreaNDTO> getListaAreasEnEmpresa(int idEmpresa, bool esNull = true)
        {
            //SOLO ACTIVOS
            using (var context = getContext())
            {
                var result = context.Area.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new AreaNDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).OrderBy(x => x.Nombre).ToList();

                if (esNull)
                {
                    result.Insert(0, new AreaNDTO() { IdArea = null, Nombre = "Seleccione un área" });
                }
                return result;
            }
        }

        public List<ResponsableDTO> getListaResponsablesEnEmpresa(int idEmpresa)
        {
            //SOLO ACTIVOS
            using (var context = getContext())
            {
                var result = context.Responsable.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new ResponsableDTO
                {
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<Select2DTO_B> getComprobantesPorEntXTDoc(int idEmpresa, int idEntidad, int idTipoDoc)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.IdEntidadResponsable == idEntidad && x.IdTipoDocumento == idTipoDoc && x.Estado && x.IdTipoComprobante < 3).Select(x => new Select2DTO_B
                {
                    id = x.IdComprobante,
                    text = x.NroDocumento,
                    ejecutado = x.Ejecutado
                }).OrderBy(x => x.text).ToList();
                return result;
            }
        }

        public List<Select2DTO_B> getComprobantes_EntidadXDocumento_Pendientes(int idEmpresa, int idEntidad, int idTipoDoc)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.IdEntidadResponsable == idEntidad && x.IdTipoDocumento == idTipoDoc && !x.Ejecutado && x.Estado && x.IdTipoComprobante < 3).Select(x => new Select2DTO_B
                {
                    id = x.IdComprobante,
                    text = x.NroDocumento,
                    ejecutado = x.Ejecutado
                }).OrderBy(x => x.text).ToList();
                return result;
            }
        }

        public List<HonorarioDTO> getListaHonorariosEnEmpresa(int idEmpresa)
        {
            HonorarioBL objBL = new HonorarioBL();
            return objBL.getHonorariosEnEmpresaViewBag(idEmpresa);
        }

        public List<MovimientoDTO> getMovimientos_AsocComprobante(int idEmpresa, int idComprobante)
        {
            using (var context = getContext())
            {
                List<int> listaCuentas = context.CuentaBancaria.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoCuenta == 1).Select(x => x.IdCuentaBancaria).ToList();

                var result = context.Movimiento.Where(x => listaCuentas.Contains(x.IdCuentaBancaria) && x.IdComprobante == idComprobante).Select(x => new MovimientoDTO
                {
                    IdMovimiento = x.IdMovimiento,
                    IdCuentaBancaria = x.IdCuentaBancaria,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoMovimiento = x.FormaMovimiento.IdTipoMovimiento,
                    IdFormaMovimiento = x.IdFormaMovimiento,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdCategoria = x.IdCategoria,
                    IdEstadoMovimiento = x.IdEstadoMovimiento,
                    NroOperacion = x.NroOperacion ?? "",
                    Fecha = x.Fecha,
                    Monto = x.Monto,
                    nTipoDocumento = x.TipoDocumento.Nombre ?? "",
                    NumeroDocumento = x.IdComprobante != null ? x.Comprobante.NroDocumento : x.NumeroDocumento ?? "",
                    TipoCambio = x.TipoCambio,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaCreacion = x.FechaCreacion,
                    MontoSinIGV = x.MontoSinIGV,
                    IdComprobante = x.IdComprobante,
                    NombreCategoria = x.Categoria.Nombre ?? "",
                    NombreEntidadR = x.EntidadResponsable.Nombre ?? "",
                    NombreUsuario = x.Usuario.Cuenta,
                    NombreCuenta = x.CuentaBancaria.NombreCuenta,
                    SaldoBancario = x.SaldoBancario
                }).ToList();
                return result;
            }
        }

        public List<ComprobanteDTO> getComprobantesIngresosYEgresosEnEmpresa_diasVencidos(int idEmpresa, int idTipoComprobante)
        {
            using (var context = getContext())
            {
                DateTime FechaActual = DateTime.Now;

                var lstMontosIncompletos = context.SP_Rep_Documentos_IngYEgr_PagadosYPorCobrar_Total(idTipoComprobante, idEmpresa).Select(x => new ComprobanteDTO
                {
                    IdComprobante = x.IdComprobante,
                    MontoIncompleto = x.MontoIncompleto.GetValueOrDefault()
                }).ToList<ComprobanteDTO>();

                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoComprobante == idTipoComprobante && x.Estado && x.IdTipoDocumento < 9).Select(x => new ComprobanteDTO
                {
                    IdComprobante = x.IdComprobante,
                    IdTipoComprobante = x.IdTipoComprobante,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdMoneda = x.IdMoneda,
                    IdEmpresa = x.IdEmpresa,
                    NroDocumento = x.NroDocumento,
                    Monto = x.Monto,
                    IdArea = x.IdArea,
                    IdResponsable = x.IdResponsable,
                    IdCategoria = x.IdCategoria,
                    IdProyecto = x.Proyecto.FirstOrDefault().IdProyecto,
                    FechaEmision = x.FechaEmision,
                    FechaConclusion = x.FechaConclusion,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    IdHonorario = x.IdHonorario,
                    NombreEntidad = x.EntidadResponsable.Nombre,
                    NombreMoneda = x.Moneda.Nombre,
                    NombreTipoComprobante = x.TipoComprobante.Nombre,
                    NombreTipoDocumento = x.TipoDocumento.Nombre,
                    SimboloMoneda = x.Moneda.Simbolo,
                    MontoSinIGV = x.MontoSinIGV,
                    TipoCambio = x.TipoCambio,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaPago = x.FechaPago,
                    NombreUsuario = x.Usuario.Cuenta,
                    NombreCategoria = x.Categoria.Nombre,
                    NombreProyecto = x.Proyecto.FirstOrDefault().Nombre,
                    nHonorario = x.Honorario.Nombre,
                    Ejecutado = x.Ejecutado
                }).OrderBy(x => x.NroDocumento).ToList<ComprobanteDTO>();

                List<ComprobanteDTO> lista = result;

                foreach (var item in lista)
                {
                    item.MontoIncompleto = lstMontosIncompletos.SingleOrDefault(r => r.IdComprobante == item.IdComprobante).MontoIncompleto;
                    item.diasVencidos = item.Ejecutado ? 0 : (item.FechaConclusion != null) ? (FechaActual - item.FechaConclusion.GetValueOrDefault()).Days : new int?();
                }

                return lista;
            }
        }

        public List<Decimal> CarteraMorosa_porMoneda(int idMoneda, List<ComprobanteDTO> lista)
        {
            //De 0 a 30 dias
            Decimal monto1 = lista.Where(x => x.IdMoneda == idMoneda && (x.diasVencidos > 0 && x.diasVencidos < 31)).Sum(x => x.MontoIncompleto);
            Decimal monto2 = lista.Where(x => x.IdMoneda == idMoneda && (x.diasVencidos > 30 && x.diasVencidos < 91)).Sum(x => x.MontoIncompleto);
            Decimal monto3 = lista.Where(x => x.IdMoneda == idMoneda && (x.diasVencidos > 90 && x.diasVencidos < 181)).Sum(x => x.MontoIncompleto);
            Decimal monto4 = lista.Where(x => x.IdMoneda == idMoneda && (x.diasVencidos > 180)).Sum(x => x.MontoIncompleto);

            List<Decimal> lstMontos = new List<Decimal>();
            lstMontos.Add(monto1);
            lstMontos.Add(monto2);
            lstMontos.Add(monto3);
            lstMontos.Add(monto4);

            return lstMontos;
        }

        public List<int> CarteraMorosa_Count_porMoneda(int idMoneda, List<ComprobanteDTO> lista)
        {
            //De 0 a 30 dias
            int count1 = lista.Where(x => x.IdMoneda == idMoneda && (x.diasVencidos > 0 && x.diasVencidos < 31)).Count();
            int count2 = lista.Where(x => x.IdMoneda == idMoneda && (x.diasVencidos > 30 && x.diasVencidos < 91)).Count();
            int count3 = lista.Where(x => x.IdMoneda == idMoneda && (x.diasVencidos > 90 && x.diasVencidos < 181)).Count();
            int count4 = lista.Where(x => x.IdMoneda == idMoneda && (x.diasVencidos > 180)).Count();

            List<int> lstMontos = new List<int>();
            lstMontos.Add(count1);
            lstMontos.Add(count2);
            lstMontos.Add(count3);
            lstMontos.Add(count4);

            return lstMontos;
        }
    }
}
