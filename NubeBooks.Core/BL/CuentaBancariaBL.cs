using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data.Objects.SqlClient;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//required for sql function access
using System.Data.Entity.Core.Objects.DataClasses;

namespace NubeBooks.Core.BL
{
    public class CuentaBancariaBL : Base
    {
        public List<CuentaBancariaDTO> getCuentasBancariasEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.CuentaBancaria.Where(x => x.IdEmpresa == idEmpresa).Select(x => new CuentaBancariaDTO
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
        public List<CuentaBancariaDTO> getCuentasBancariasActivasEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.CuentaBancaria.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new CuentaBancariaDTO
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
        public List<CuentaBancariaDTO> getCuentasBancarias()
        {
            using (var context = getContext())
            {
                var result = context.CuentaBancaria.Select(x => new CuentaBancariaDTO
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

        public List<CuentaBancariaDTO> getCuentasBancariasViewBag()
        {
            using (var context = getContext())
            {
                var result = context.CuentaBancaria.Where(x => x.Estado).Select(x => new CuentaBancariaDTO
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

        public CuentaBancariaDTO getCuentaBancariaSoloEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.CuentaBancaria.Where(x => x.IdCuentaBancaria == id && x.IdEmpresa == idEmpresa)
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
                        IdTipoCuenta = r.IdTipoCuenta
                    }).SingleOrDefault();
                return result;
            }
        }

        public CuentaBancariaDTO getCuentaBancariaEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.CuentaBancaria.Where(x => x.IdCuentaBancaria == id && x.IdEmpresa == idEmpresa)
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
                        listaMovimiento = r.Movimiento.Select(x => new MovimientoDTO
                        {
                            IdMovimiento = x.IdMovimiento,
                            IdCuentaBancaria = x.IdCuentaBancaria,
                            IdEntidadResponsable = x.IdEntidadResponsable,
                            IdTipoMovimiento = x.FormaMovimiento.IdTipoMovimiento,
                            IdCategoria = x.IdCategoria,
                            IdEstadoMovimiento = x.IdEstadoMovimiento,
                            NroOperacion = x.NroOperacion ?? "",
                            Fecha = x.Fecha,
                            Monto = x.Monto,
                            NumeroDocumento = x.IdComprobante != null ? x.Comprobante.NroDocumento : (x.NumeroDocumento ?? ""),
                            Comentario = x.Comentario,
                            Estado = x.Estado,
                            UsuarioCreacion = x.UsuarioCreacion,
                            FechaCreacion = x.FechaCreacion,
                            NombreEntidadR = x.EntidadResponsable.Nombre,
                            NombreCategoria = x.Categoria.Nombre ?? "",
                            NombreUsuario = x.Usuario.Cuenta
                            //NumeroDocumento2 = x.NumeroDocumento
                        }).OrderByDescending(x => x.Fecha).ToList()
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(CuentaBancariaDTO CuentaBancaria)
        {
            using (var context = getContext())
            {
                try
                {
                    CuentaBancaria nuevo = new CuentaBancaria();
                    nuevo.NombreCuenta = CuentaBancaria.NombreCuenta;
                    nuevo.FechaConciliacion = Convert.ToDateTime(CuentaBancaria.FechaConciliacion.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    //nuevo.FechaConciliacion = CuentaBancaria.FechaConciliacion.ToString("yyyy-MM-dd H:mm:ss");
                    nuevo.SaldoDisponible = CuentaBancaria.SaldoDisponible;
                    nuevo.SaldoBancario = CuentaBancaria.SaldoBancario;
                    nuevo.Estado = true;
                    nuevo.IdMoneda = CuentaBancaria.IdMoneda;
                    nuevo.IdEmpresa = CuentaBancaria.IdEmpresa;
                    nuevo.IdTipoCuenta = CuentaBancaria.IdTipoCuenta;
                    context.CuentaBancaria.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(CuentaBancariaDTO CuentaBancaria)
        {
            using (var context = getContext())
            {
                try
                {
                    //var miSaldoDisponible = context.SP_GetTotalIngresos(CuentaBancaria.IdCuentaBancaria).AsQueryable().First() as Decimal?;
                    var datoRow = context.CuentaBancaria.Where(x => x.IdCuentaBancaria == CuentaBancaria.IdCuentaBancaria).SingleOrDefault();
                    datoRow.NombreCuenta = CuentaBancaria.NombreCuenta;
                    //datoRow.FechaConciliacion = CuentaBancaria.FechaConciliacion;
                    datoRow.FechaConciliacion = Convert.ToDateTime(CuentaBancaria.FechaConciliacion.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    datoRow.SaldoDisponible = CuentaBancaria.SaldoDisponible;
                    datoRow.SaldoBancario = CuentaBancaria.SaldoBancario;
                    datoRow.Estado = CuentaBancaria.Estado;
                    datoRow.IdMoneda = CuentaBancaria.IdMoneda;
                    datoRow.IdEmpresa = CuentaBancaria.IdEmpresa;
                    datoRow.IdTipoCuenta = CuentaBancaria.IdTipoCuenta;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool updateSaldos(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    context.SP_ActualizarMontos(id);
                    return true;
                }
                catch (Exception e)
                {
                    string miCadena = e.Message;
                    throw e;
                }
            }
        }

        public IList<CuentaBancariaDTO> getCuentasBancariasBag(bool AsSelectList = false)
        {
            if (!AsSelectList)
                return getCuentasBancariasViewBag();
            else
            {
                var lista = getCuentasBancariasViewBag();
                lista.Insert(0, new CuentaBancariaDTO() { IdCuentaBancaria = 0, NombreCuenta = "Seleccione un Libro" });
                return lista;
            }
        }

        public IList<CuentaBancariaDTO> getCuentasBancariasEnEmpresaBag(int idEmpresa, bool AsSelectList = false)
        {
            if (!AsSelectList)
                return getCuentasBancariasActivasEnEmpresa(idEmpresa);
            else
            {
                var lista = getCuentasBancariasActivasEnEmpresa(idEmpresa);
                lista.Insert(0, new CuentaBancariaDTO() { IdCuentaBancaria = 0, NombreCuenta = "Seleccione un Libro" });
                return lista;
            }
        }

        public List<MonedaDTO> getMonedasViewBag()
        {
            using (var context = getContext())
            {
                var result = context.Moneda.Select(x => new MonedaDTO
                {
                    IdMoneda = x.IdMoneda,
                    Nombre = x.Nombre,
                    Simbolo = x.Simbolo
                }).ToList();
                return result;
            }
        }

        public IList<MonedaDTO> getMonedasBag(bool AsSelectList = false)
        {
            if (!AsSelectList)
                return getMonedasViewBag();
            else
            {
                var lista = getMonedasViewBag();
                lista.Insert(0, new MonedaDTO() { IdMoneda = 0, Nombre = "Seleccione una Moneda" });
                return lista;
            }
        }

        public List<TipoCuentaDTO> getTipoDeCuentas()
        {
            using (var context = getContext())
            {
                var result = context.TipoCuenta.Select(x => new TipoCuentaDTO
                {
                    IdTipoCuenta = x.IdTipoCuenta,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }
    }
}
