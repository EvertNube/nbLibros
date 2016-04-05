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
    public class AreaBL : Base
    {
        public List<AreaDTO> getAreasEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Area.Where(x => x.IdEmpresa == idEmpresa).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public List<AreaDTO> getAreasActivasEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Area.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public List<AreaDTO> getAreasEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Area.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public AreaDTO getAreaEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Area.Where(x => x.IdArea == id && x.IdEmpresa == idEmpresa)
                    .Select(r => new AreaDTO
                    {
                        IdArea = r.IdArea,
                        Nombre = r.Nombre,
                        Estado = r.Estado,
                        Descripcion = r.Descripcion,
                        IdEmpresa = r.IdEmpresa
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(AreaDTO Area)
        {
            using (var context = getContext())
            {
                try
                {
                    Area nuevo = new Area();
                    nuevo.Nombre = Area.Nombre;
                    nuevo.Descripcion = Area.Descripcion;
                    nuevo.Estado = true;
                    nuevo.IdEmpresa = Area.IdEmpresa;
                    context.Area.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(AreaDTO Area)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Area.Where(x => x.IdArea == Area.IdArea).SingleOrDefault();
                    row.Nombre = Area.Nombre;
                    row.Descripcion = Area.Descripcion;
                    row.Estado = Area.Estado;
                    row.IdEmpresa = Area.IdEmpresa;
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
