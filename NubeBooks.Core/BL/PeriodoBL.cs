using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class PeriodoBL : Base
    {
        public List<PeriodoDTO> getPeriodosEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Periodo.Where(x => x.IdEmpresa == idEmpresa).Select(x => new PeriodoDTO
                {
                    IdPeriodo = x.IdPeriodo,
                    Nombre = x.Nombre,
                    FechaInicio = x.FechaInicio,
                    FechaFin = x.FechaFin,
                    Active = x.Active,
                    IdEmpresa = x.IdEmpresa
                }).OrderByDescending(x => x.FechaInicio).ToList();
                return result;
            }
        }
        public List<PeriodoDTO> getPeriodosActivosEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Periodo.Where(x => x.IdEmpresa == idEmpresa && x.Active).Select(x => new PeriodoDTO
                {
                    IdPeriodo = x.IdPeriodo,
                    Nombre = x.Nombre,
                    FechaInicio = x.FechaInicio,
                    FechaFin = x.FechaFin,
                    Active = x.Active,
                    IdEmpresa = x.IdEmpresa
                }).OrderByDescending(x => x.FechaInicio).ToList();
                return result;
            }
        }
        public List<PeriodoDTO> getPeriodosEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Periodo.Where(x => x.Active && x.IdEmpresa == idEmpresa).Select(x => new PeriodoDTO
                {
                    IdPeriodo = x.IdPeriodo,
                    Nombre = x.Nombre,
                    FechaInicio = x.FechaInicio,
                    FechaFin = x.FechaFin,
                    Active = x.Active,
                    IdEmpresa = x.IdEmpresa
                }).OrderByDescending(x => x.FechaInicio).ToList();
                return result;
            }
        }
        public PeriodoDTO getPeriodoEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Periodo.Where(x => x.IdEmpresa == idEmpresa && x.IdPeriodo == id)
                    .Select(x => new PeriodoDTO
                    {
                        IdPeriodo = x.IdPeriodo,
                        Nombre = x.Nombre,
                        FechaInicio = x.FechaInicio,
                        FechaFin = x.FechaFin,
                        Active = x.Active,
                        IdEmpresa = x.IdEmpresa
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(PeriodoDTO Periodo)
        {
            using (var context = getContext())
            {
                try
                {
                    Periodo nuevo = new Periodo();
                    nuevo.Nombre = Periodo.Nombre;
                    nuevo.FechaInicio = Periodo.FechaInicio;
                    nuevo.FechaFin = Periodo.FechaFin;
                    nuevo.Active = true;
                    nuevo.IdEmpresa = Periodo.IdEmpresa;
                    context.Periodo.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(PeriodoDTO Periodo)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Periodo.Where(x => x.IdPeriodo == Periodo.IdPeriodo).SingleOrDefault();
                    row.Nombre = Periodo.Nombre;
                    row.FechaInicio = Periodo.FechaInicio;
                    row.FechaFin = Periodo.FechaFin;
                    row.Active = Periodo.Active;
                    row.IdEmpresa = Periodo.IdEmpresa;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
