using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class EmpresaBL : Base
    {
        public List<EmpresaDTO> getEmpresas()
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Select(x => new EmpresaDTO
                {
                    IdEmpresa = x.IdEmpresa,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Descripcion = x.Descripcion,
                    TipoCambio = x.TipoCambio,
                    IdPeriodo = x.IdPeriodo
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<EmpresaDTO> getEmpresasActivas()
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Where(x => x.Estado).Select(x => new EmpresaDTO
                {
                    IdEmpresa = x.IdEmpresa,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Descripcion = x.Descripcion,
                    TipoCambio = x.TipoCambio,
                    IdPeriodo = x.IdPeriodo,
                    IdMoneda = x.IdMoneda,
                    SimboloMoneda = x.Moneda.Simbolo,
                    TotalSoles = x.TotalSoles,
                    TotalDolares = x.TotalDolares,
                    TotalSolesOld = x.TotalSolesOld,
                    TotalDolaresOld = x.TotalDolaresOld,
                    FechaConciliacion = x.FechaConciliacion
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public EmpresaDTO getEmpresa(int id)
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Where(x => x.IdEmpresa == id)
                    .Select(r => new EmpresaDTO
                    {
                        IdEmpresa = r.IdEmpresa,
                        Nombre = r.Nombre,
                        Estado = r.Estado,
                        Descripcion = r.Descripcion,
                        TipoCambio = r.TipoCambio,
                        IdPeriodo = r.IdPeriodo,
                        IdMoneda = r.IdMoneda,
                        SimboloMoneda = r.Moneda.Simbolo,
                        TotalSoles = r.TotalSoles,
                        TotalDolares = r.TotalDolares,
                        TotalSolesOld = r.TotalSolesOld,
                        TotalDolaresOld = r.TotalDolaresOld,
                        FechaConciliacion = r.FechaConciliacion
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(EmpresaDTO Empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    Empresa nuevo = new Empresa();
                    nuevo.Nombre = Empresa.Nombre;
                    nuevo.Estado = true;
                    nuevo.Descripcion = Empresa.Descripcion;
                    nuevo.TipoCambio = Empresa.TipoCambio == 0 ? 1 : Empresa.TipoCambio;
                    nuevo.IdPeriodo = Empresa.IdPeriodo;
                    context.Empresa.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(EmpresaDTO Empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Empresa.Where(x => x.IdEmpresa == Empresa.IdEmpresa).SingleOrDefault();
                    row.Nombre = Empresa.Nombre;
                    row.Estado = Empresa.Estado;
                    row.Descripcion = Empresa.Descripcion;
                    row.TipoCambio = Empresa.TipoCambio;
                    row.IdPeriodo = Empresa.IdPeriodo;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool updateTipoCambio(EmpresaDTO Empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Empresa.Where(x => x.IdEmpresa == Empresa.IdEmpresa).SingleOrDefault();
                    row.TipoCambio = Empresa.TipoCambio;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool updatePeriodo(EmpresaDTO Empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Empresa.Where(x => x.IdEmpresa == Empresa.IdEmpresa).SingleOrDefault();
                    row.IdPeriodo = Empresa.IdPeriodo;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool updateMontosSolesDolares(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    context.SP_ActualizarMontos_Empresa(id);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<EmpresaDTO> getEmpresasViewBag()
        {
            using (var context = getContext())
            {
                var lista = context.Empresa.Where(x => x.Estado).Select(x => new EmpresaDTO
                {
                    IdEmpresa = x.IdEmpresa,
                    Nombre = x.Nombre
                }).ToList();
                return lista;
            }
        }

        public List<LiquidezDTO> getLiquidezEnEmpresaPorMoneda(int idEmpresa, int moneda)
        {
            using (var context = getContext())
            {
                DateTime today = new DateTime();
                today = DateTime.Now;
                //Ultimo dia del mes y dia inicial del mes actual
                DateTime firstDayMonth = new DateTime(today.Year, today.Month, 1);
                DateTime lastDayMonth = firstDayMonth.AddMonths(1).AddDays(-1);
                //Ultimo dia inicial de hace un año atras (YA - Year Ago)
                DateTime firstDayMonthYA = new DateTime(today.Year - 1, today.Month + 1, 1);

                //Conseguir todos movimientos desde el mes actual hasta 11 meses atras (12 meses)
                var lista = from mov in context.Movimiento
                            join lib in context.CuentaBancaria on mov.IdCuentaBancaria equals lib.IdCuentaBancaria
                            where lib.IdEmpresa == idEmpresa && lib.IdMoneda == moneda && mov.Estado == true && (mov.FechaCreacion <= lastDayMonth && mov.FechaCreacion >= firstDayMonthYA)
                            select new MovimientoDTO
                            {
                                IdMovimiento = mov.IdMovimiento,
                                Fecha = mov.Fecha,
                                FechaCreacion = mov.FechaCreacion,
                                Monto = mov.Monto,
                                MontoSinIGV = mov.MontoSinIGV,
                                TipoCambio = mov.TipoCambio
                            };

                List<MovimientoDTO> listMovs = lista.OrderBy(x => x.FechaCreacion).ToList();

                //Obtener Liquidez por cada mes segun la moneda que corresponda
                return new List<LiquidezDTO>();
            }
        }
    }
}
