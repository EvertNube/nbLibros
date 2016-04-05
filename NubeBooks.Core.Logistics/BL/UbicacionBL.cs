using NubeBooks.Core.Logistics.DTO;
using NubeBooks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.BL
{
    public class UbicacionBL : Base
    {
        public List<UbicacionDTO> getUbicacionsEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Ubicacion.Where(x => x.IdEmpresa == idEmpresa).Select(x => new UbicacionDTO
                {
                    IdUbicacion = x.IdUbicacion,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).ToList();
                return result;
            }
        }
        public List<UbicacionDTO> getUbicacionsActivasEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Ubicacion.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new UbicacionDTO
                {
                    IdUbicacion = x.IdUbicacion,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).ToList();
                return result;
            }
        }
        public List<UbicacionDTO> getUbicacionsEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Ubicacion.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new UbicacionDTO
                {
                    IdUbicacion = x.IdUbicacion,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public UbicacionDTO getUbicacionEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Ubicacion.Where(x => x.IdEmpresa == idEmpresa && x.IdUbicacion == id)
                    .Select(x => new UbicacionDTO
                    {
                        IdUbicacion = x.IdUbicacion,
                        Nombre = x.Nombre,
                        Estado = x.Estado,
                        IdEmpresa = x.IdEmpresa
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(UbicacionDTO Ubicacion)
        {
            using (var context = getContext())
            {
                try
                {
                    Ubicacion nuevo = new Ubicacion();
                    nuevo.Nombre = Ubicacion.Nombre;
                    nuevo.Estado = true;
                    nuevo.IdEmpresa = Ubicacion.IdEmpresa;
                    context.Ubicacion.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(UbicacionDTO Ubicacion)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Ubicacion.Where(x => x.IdUbicacion == Ubicacion.IdUbicacion).SingleOrDefault();
                    row.Nombre = Ubicacion.Nombre;
                    row.Estado = Ubicacion.Estado;
                    row.IdEmpresa = Ubicacion.IdEmpresa;
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
