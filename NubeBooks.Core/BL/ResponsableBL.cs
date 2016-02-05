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
    public class ResponsableBL : Base
    {
        public List<ResponsableDTO> getResponsablesEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Responsable.Where(x => x.IdEmpresa == idEmpresa).Select(x => new ResponsableDTO
                {
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public List<ResponsableDTO> getResponsablesEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Responsable.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new ResponsableDTO
                {
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public ResponsableDTO getResponsableEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Responsable.Where(x => x.IdResponsable == id && x.IdEmpresa == idEmpresa)
                    .Select(r => new ResponsableDTO
                    {
                        IdResponsable = r.IdResponsable,
                        Nombre = r.Nombre,
                        Estado = r.Estado,
                        Descripcion = r.Descripcion,
                        IdEmpresa = r.IdEmpresa
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(ResponsableDTO Responsable)
        {
            using (var context = getContext())
            {
                try
                {
                    Responsable nuevo = new Responsable();
                    nuevo.Nombre = Responsable.Nombre;
                    nuevo.Descripcion = Responsable.Descripcion;
                    nuevo.Estado = true;
                    nuevo.IdEmpresa = Responsable.IdEmpresa;
                    context.Responsable.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(ResponsableDTO Responsable)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Responsable.Where(x => x.IdResponsable == Responsable.IdResponsable).SingleOrDefault();
                    row.Nombre = Responsable.Nombre;
                    row.Descripcion = Responsable.Descripcion;
                    row.Estado = Responsable.Estado;
                    row.IdEmpresa = Responsable.IdEmpresa;
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
