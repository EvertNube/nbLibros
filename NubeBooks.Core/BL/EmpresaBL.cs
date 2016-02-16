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
    }
}
