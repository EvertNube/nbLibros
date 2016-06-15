using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class ReportesBL : Base
    {
        #region Avance de Presupuesto
        public List<CategoriaDTO> AvanceDePresupuesto(int? idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_Rep_AvanceDePresupuesto(idEmpresa, fechaInicio, fechaFin).Select(x => new CategoriaDTO
                {
                    IdCategoria = x.IdCategoria,
                    Nombre = x.Nombre,
                    Orden = x.Orden,
                    Estado = x.Estado,
                    IdCategoriaPadre = x.IdCategoriaPadre,
                    IdEmpresa = x.IdEmpresa,
                    Presupuesto = x.MontoFinal
                }).ToList();
                return result;
            }
        }
        public List<CategoriaDTO> getCategoriasTreeEnEmpresa(List<CategoriaDTO> lstCatsMontos, int idEmpresa, int? id = null)
        {
            
            var result = from r in lstCatsMontos
                            where ((id == null ? r.IdCategoriaPadre == null : r.IdCategoriaPadre == id) && r.Estado && r.IdEmpresa == idEmpresa)
                            select new CategoriaDTO
                            {
                                IdCategoria = r.IdCategoria,
                                Nombre = r.Nombre,
                                Orden = r.Orden,
                                Estado = r.Estado,
                                IdCategoriaPadre = r.IdCategoriaPadre,
                                IdEmpresa = r.IdEmpresa,
                                Presupuesto = lstCatsMontos.SingleOrDefault(x => x.IdCategoria == r.IdCategoria).Presupuesto
                            };
            List<CategoriaDTO> categoriasTree = result.AsEnumerable<CategoriaDTO>().OrderBy(x => x.Orden).ToList<CategoriaDTO>();

            foreach (CategoriaDTO obj in categoriasTree)
            {
                obj.Hijos = getCategoriasTreeEnEmpresa(lstCatsMontos, idEmpresa, obj.IdCategoria);
                if(obj.Hijos.Count() > 0)
                {
                    obj.Presupuesto = lstCatsMontos.Where(x => x.IdCategoriaPadre == obj.IdCategoria).Sum(x => x.Presupuesto);
                    lstCatsMontos.SingleOrDefault(x => x.IdCategoria == obj.IdCategoria).Presupuesto = obj.Presupuesto;
                }
            }

            return categoriasTree;
        }
        public List<CategoriaDTO> getCategoriasPresupuestosTreeEnEmpresa(int idEmpresa, int nivel, int? id = null)
        {
            using (var context = getContext())
            {
                var result = from r in context.Categoria
                             where ((id == null ? r.IdCategoriaPadre == null : r.IdCategoriaPadre == id) && r.Estado && r.IdEmpresa == idEmpresa)
                             select new CategoriaDTO
                             {
                                 IdCategoria = r.IdCategoria,
                                 Nombre = r.Nombre,
                                 Orden = r.Orden,
                                 Estado = r.Estado,
                                 IdCategoriaPadre = r.IdCategoriaPadre,
                                 IdEmpresa = r.IdEmpresa,
                                 Nivel = nivel,
                                 Presupuesto = r.CategoriaPorPeriodo.Where(x => x.IdPeriodo == r.Empresa.IdPeriodo).FirstOrDefault().Monto
                             };
                List<CategoriaDTO> categoriasTree = result.AsEnumerable<CategoriaDTO>().OrderBy(x => x.Orden).ToList<CategoriaDTO>();

                foreach (CategoriaDTO obj in categoriasTree)
                {
                    obj.Hijos = getCategoriasPresupuestosTreeEnEmpresa(idEmpresa, nivel+1, obj.IdCategoria);
                }
                return categoriasTree;
            }
        }

        public List<CategoriaDTO> getCategoriasPresupuestosTree_PorPeriodo_EnEmpresa(int idEmpresa, int nivel, int periodo, int? id = null)
        {
            using (var context = getContext())
            {
                var result = from r in context.Categoria
                             where ((id == null ? r.IdCategoriaPadre == null : r.IdCategoriaPadre == id) && r.Estado && r.IdEmpresa == idEmpresa)
                             select new CategoriaDTO
                             {
                                 IdCategoria = r.IdCategoria,
                                 Nombre = r.Nombre,
                                 Orden = r.Orden,
                                 Estado = r.Estado,
                                 IdCategoriaPadre = r.IdCategoriaPadre,
                                 IdEmpresa = r.IdEmpresa,
                                 Nivel = nivel,
                                 Presupuesto = r.CategoriaPorPeriodo.Where(x => x.IdPeriodo == periodo).FirstOrDefault().Monto
                             };
                List<CategoriaDTO> categoriasTree = result.AsEnumerable<CategoriaDTO>().OrderBy(x => x.Orden).ToList<CategoriaDTO>();

                foreach (CategoriaDTO obj in categoriasTree)
                {
                    obj.Hijos = getCategoriasPresupuestosTree_PorPeriodo_EnEmpresa(idEmpresa, nivel + 1, periodo, obj.IdCategoria);
                }
                return categoriasTree;
            }
        }
        #endregion

        #region Facturacion por areas
        public List<AreaDTO> getAreasEnEmpresa(int idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.Area.AsEnumerable().Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    lstClientes = context.SP_Rep_FacturacionPorAreas(x.IdArea, x.IdEmpresa, fechaInicio, fechaFin).Select(r => new EntidadResponsableR_DTO 
                    {
                        IdEntidadResponsable = r.IdEntidadResponsable,
                        Nombre = r.Nombre,
                        Monto = r.Monto.GetValueOrDefault(),
                        MontoSinIGV = r.MontoSinIGV.GetValueOrDefault()
                    }).ToList()
                }).OrderBy(x => x.Nombre).ToList();

                foreach (var item in result)
                {
                    item.SumaClientes = item.lstClientes.Sum(x => x.Monto);
                    item.SumaClientes_SinIGV = item.lstClientes.Sum(x => x.MontoSinIGV);
                }

                return result;
            }
        }
        #endregion

        #region Egresos por areas
        public List<AreaDTO> getEgresosAreasEnEmpresa(int idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.Area.AsEnumerable().Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    lstClientes = context.SP_Rep_EgresosPorAreas(x.IdArea, x.IdEmpresa, fechaInicio, fechaFin).Select(r => new EntidadResponsableR_DTO
                    {
                        IdEntidadResponsable = r.IdEntidadResponsable,
                        Nombre = r.Nombre,
                        Monto = r.Monto.GetValueOrDefault(),
                        MontoSinIGV = r.MontoSinIGV.GetValueOrDefault()
                    }).ToList()
                }).OrderBy(x => x.Nombre).ToList();

                foreach (var item in result)
                {
                    item.SumaClientes = item.lstClientes.Sum(x => x.Monto);
                    item.SumaClientes_SinIGV = item.lstClientes.Sum(x => x.MontoSinIGV);
                }

                return result;
            }
        }
        #endregion

        #region Ingresos y Egresos por Areas
        public List<AreaDTO> getIngresosEgresosAreas(int idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_Rep_IngresosEgresosPorAreas(idEmpresa, fechaInicio, fechaFin).Select(x => new AreaDTO
                    {
                        IdArea = x.IdArea,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        Estado = x.Estado,
                        IdEmpresa = x.IdEmpresa,
                        Ingresos = x.Ingreso.GetValueOrDefault(),
                        Egresos = x.Egreso.GetValueOrDefault(),
                        Ingresos_SinIGV = x.Ingreso_SinIGV.GetValueOrDefault(),
                        Egresos_SinIGV = x.Egreso_SinIGV.GetValueOrDefault()
                    }).OrderBy(x => x.Nombre).ToList();
                
                var resultNull = context.SP_Rep_IngresosEgresosPorAreaNull(idEmpresa, fechaInicio, fechaFin).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descipcion,
                    Estado = Convert.ToBoolean(x.Estado),
                    IdEmpresa = x.IdEmpresa.GetValueOrDefault(),
                    Ingresos = x.Ingreso.GetValueOrDefault(),
                    Egresos = x.Egreso.GetValueOrDefault(),
                    Ingresos_SinIGV = x.Ingreso_SinIGV.GetValueOrDefault(),
                    Egresos_SinIGV = x.Egreso_SinIGV.GetValueOrDefault()
                }).ToList();

                foreach (var item in resultNull)
                {
                    result.Add(item);
                }

                return result;
            }
        }
        #endregion

        #region Facturación por Cliente
        public List<EntidadResponsableR_DTO> getFacturacionPorClientes(int idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_Rep_FacturacionPorCliente(idEmpresa, fechaInicio, fechaFin).Select(x => new EntidadResponsableR_DTO
                    {
                        IdEntidadResponsable = x.IdEntidadResponsable,
                        Nombre = x.Nombre,
                        Estado = x.Estado,
                        Tipo = x.Tipo,
                        IdEmpresa = x.IdEmpresa,
                        Monto = x.Monto.GetValueOrDefault(),
                        MontoSinIGV = x.MontoSinIGV.GetValueOrDefault()
                    }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        #endregion

        #region Gastos por Proveedores
        public List<EntidadResponsableR_DTO> getGastosPorProveedores(int idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_Rep_GastosPorProveedor(idEmpresa, fechaInicio, fechaFin).Select(x => new EntidadResponsableR_DTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    Monto = x.Monto.GetValueOrDefault(),
                    MontoSinIGV = x.MontoSinIGV.GetValueOrDefault()
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        #endregion

        #region Facturacion por Vendedor
        public List<ResponsableDTO> getFacturacionPorVendedores(int idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_Rep_FacturacionPorVendedor(idEmpresa, fechaInicio, fechaFin).Select(x => new ResponsableDTO
                {
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    Monto = x.Monto.GetValueOrDefault(),
                    MontoSinIGV = x.MontoSinIGV.GetValueOrDefault()
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        #endregion

        #region Documentos Pagados y Por Cobrar
        public List<ComprobanteDTO> getComprobantesIngresosYEgresosEnEmpresa(int idEmpresa, int idTipoComprobante, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = getContext())
            {
                var lstMontosIncompletos = context.SP_Rep_Documentos_IngYEgr_PagadosYPorCobrar(idTipoComprobante, idEmpresa, fechaInicio, fechaFin).Select(x => new ComprobanteDTO
                    {
                        IdComprobante = x.IdComprobante,
                        MontoIncompleto = x.MontoIncompleto.GetValueOrDefault()
                    }).ToList<ComprobanteDTO>();

                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoComprobante == idTipoComprobante && (x.FechaEmision >= fechaInicio && x.FechaEmision <= fechaFin) && x.Estado && x.IdTipoDocumento < 9).Select(x => new ComprobanteDTO
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
                }

                return lista;
            }
        }
        #endregion

        #region Facturacion Modalidad de Pago
        public List<HonorarioDTO> getHonorariosEnEmpresa(int idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_Rep_FacturacionPorHonorarios(idEmpresa, fechaInicio, fechaFin).Select(x => new HonorarioDTO
                {
                    IdHonorario = x.IdHonorario,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    Monto = x.Monto.GetValueOrDefault(),
                    Monto_SinIGV = x.Monto_SinIGV.GetValueOrDefault()
                }).OrderBy(x => x.Nombre).ToList();

                return result;
            }
        }
        #endregion

        #region Gestion Mensual en el Año
        public List<LiquidezDTO> getGestionMensual(int idEmpresa, int year)
        {
            using (var context = getContext())
            {
                List<string> meses = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                var result = context.SP_Rep_IngresosEgresos_PorMes_EnEmpresa(idEmpresa, year).Select(x => new LiquidezDTO
                {
                    Mes = x.Mes,
                    nombreMes = meses[x.Mes - 1],
                    Ingreso = x.Ingreso.GetValueOrDefault(),
                    Egreso = x.Egreso.GetValueOrDefault(),
                    Ingreso_SinIGV = x.Ingreso_SinIGV.GetValueOrDefault(),
                    Egreso_SinIGV = x.Egreso_SinIGV.GetValueOrDefault()
                }).ToList();

                return result;
            }
        }
        #endregion

        #region Exportacion de Detalles
        public List<CuentaBancariaDTO> getCuentasBancariasEnEmpresa(int idEmpresa, int idTipoCuenta, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.CuentaBancaria.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoCuenta == idTipoCuenta && x.FechaConciliacion >= fechaInicio && x.FechaConciliacion <= fechaFin).Select(x => new CuentaBancariaDTO
                {
                    IdCuentaBancaria = x.IdCuentaBancaria,
                    NombreCuenta = x.NombreCuenta,
                    FechaConciliacion = x.FechaConciliacion,
                    SaldoDisponible = x.SaldoDisponible,
                    SaldoBancario = x.SaldoBancario,
                    Estado = x.Estado,
                    SimboloMoneda = x.Moneda.Simbolo,
                    IdMoneda = x.IdMoneda,
                    IdEmpresa = x.IdEmpresa,
                    IdTipoCuenta = x.IdTipoCuenta
                }).ToList();
                return result;
            }
        }

        public CuentaBancariaDTO getCuentaBancaria(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.CuentaBancaria.Where(x => x.IdCuentaBancaria == id)
                    .Select(r => new CuentaBancariaDTO
                    {
                        IdCuentaBancaria = r.IdCuentaBancaria,
                        NombreCuenta = r.NombreCuenta,
                        FechaConciliacion = r.FechaConciliacion,
                        SaldoDisponible = r.SaldoDisponible,
                        SaldoBancario = r.SaldoBancario,
                        Estado = r.Estado,
                        IdMoneda = r.IdMoneda,
                        NombreMoneda = r.Moneda.Nombre,
                        SimboloMoneda = r.Moneda.Simbolo,
                        IdEmpresa = r.IdEmpresa,
                        IdTipoCuenta = r.IdTipoCuenta,
                        listaMovimiento = r.Movimiento.Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin).Select(x => new MovimientoDTO
                        {
                            IdMovimiento = x.IdMovimiento,
                            IdCuentaBancaria = x.IdCuentaBancaria,
                            IdEntidadResponsable = x.IdEntidadResponsable,
                            IdTipoMovimiento = x.FormaMovimiento.IdTipoMovimiento,
                            IdCategoria = x.IdCategoria,
                            IdEstadoMovimiento = x.IdEstadoMovimiento,
                            NroOperacion = x.NroOperacion,
                            Fecha = x.Fecha,
                            Monto = x.Monto,
                            NumeroDocumento = x.Comprobante.NroDocumento,
                            Comentario = x.Comentario,
                            Estado = x.Estado,
                            UsuarioCreacion = x.UsuarioCreacion,
                            FechaCreacion = x.FechaCreacion,
                            NombreEntidadR = x.EntidadResponsable.Nombre,
                            NombreCategoria = x.Categoria.Nombre,
                            NombreUsuario = x.Usuario.Cuenta,
                            NumeroDocumento2 = x.NumeroDocumento
                        }).OrderBy(x => x.Fecha).ToList()
                    }).SingleOrDefault();
                return result;
            }
        }

        public List<ComprobanteDTO> getComprobantesEnEmpresa(int idEmpresa, int idTipoComprobante, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoComprobante == idTipoComprobante && x.FechaEmision >= fechaInicio && x.FechaEmision <= fechaFin).Select(x => new ComprobanteDTO
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
                    NombreUsuario = x.Usuario.Cuenta,
                    NombreCategoria = x.Categoria.Nombre,
                    NombreProyecto = x.Proyecto.FirstOrDefault().Nombre,
                    Ejecutado = x.Ejecutado
                }).OrderBy(x => x.FechaEmision).ToList();
                return result;
            }
        }
        #endregion

        #region Detalle de Gastos por Partida de Presupuesto
        public CategoriaR_DTO getDetalleIngresosYGastos_PorPartidaDePresupuesto(int idCategoria, int idEmpresa, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = getContext())
            {
                List<CategoriaR_DTO> lstArbol = context.SP_Get_Arbol_Categoria(idCategoria).Select(x => new CategoriaR_DTO
                    {
                        IdCategoria = x.IdCategoria.GetValueOrDefault(),
                        IdCategoriaPadre = x.IdCategoriaPadre,
                        Nombre = x.Nombre,
                        Orden = x.Orden.GetValueOrDefault(),
                        Estado = x.Estado.GetValueOrDefault(),
                        IdEmpresa = x.IdEmpresa.GetValueOrDefault(),
                        Nivel = x.Nivel.GetValueOrDefault()
                    }).ToList<CategoriaR_DTO>();

                List<ComprobanteR_DTO> lstDetalle = context.SP_Rep_DetalleGastosPorPartidaDePresupuesto(idCategoria, idEmpresa, fechaInicio, fechaFin).Select(x => new ComprobanteR_DTO
                    {
                        IdCategoria = x.IdCategoria,
                        IdCategoriaPadre = x.IdCategoriaPadre,
                        NombreCategoria = x.Nombre,
                        IdComprobante = x.IdComprobante,
                        Fecha = x.FechaEmision,
                        NombreEntidad = x.NombreEntidad,
                        NombreDocumento = x.NombreDocumento,
                        NroDocumento = x.NroDocumento,
                        Moneda = x.Moneda,
                        Monto = x.Monto,
                        MontoSinIGV = x.MontoSinIGV,
                        Areas = x.Areas,
                        Comentario = x.Comentario
                    }).ToList<ComprobanteR_DTO>();

                CategoriaR_DTO result = lstArbol.Where(x => x.IdCategoria == idCategoria).SingleOrDefault();
                result.Comprobantes = lstDetalle.Where(x => x.IdCategoria == idCategoria).ToList();
                result.Hijos = GetArbolEnCategoria(idCategoria, lstArbol, lstDetalle);

                return result;
            }
        }

        public List<CategoriaR_DTO> GetArbolEnCategoria(int idCategoria, List<CategoriaR_DTO> lstArbol, List<ComprobanteR_DTO> lstDetalle)
        {
            if (idCategoria == 0)
                return null;

            List<CategoriaR_DTO> result = lstArbol.Where(x => x.IdCategoriaPadre == idCategoria).Select(x => new CategoriaR_DTO
            {
                IdCategoria = x.IdCategoria,
                Nombre = x.Nombre,
                Orden = x.Orden,
                Estado = x.Estado,
                IdCategoriaPadre = x.IdCategoriaPadre,
                IdEmpresa = x.IdEmpresa,
                Nivel = x.Nivel,
                Comprobantes = lstDetalle.Where(r => r.IdCategoria == x.IdCategoria).ToList(),
                Hijos = GetArbolEnCategoria(x.IdCategoria, lstArbol, lstDetalle)
            }).ToList();

            return result;
        }


        #endregion
        
        public List<ComprobanteDTO> getComprobantes_ConEntidad(int idEmpresa, int idEntidad, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.IdEntidadResponsable == idEntidad && x.FechaEmision >= fechaInicio && x.FechaEmision <= fechaFin).Select(x => new ComprobanteDTO
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
                }).ToList();

                return result;
            }
        }
        public List<ComprobanteDTO> getComprobantes_ConProyecto(int idEmpresa, int idProyecto, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.Proyecto.Any(r => r.IdProyecto == idProyecto) && x.FechaEmision >= fechaInicio && x.FechaEmision <= fechaFin).Select(x => new ComprobanteDTO
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
                }).ToList();

                return result;
            }
        }
    }
}
