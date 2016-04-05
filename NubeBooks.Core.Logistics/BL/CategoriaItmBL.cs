using NubeBooks.Core.Logistics.DTO;
using NubeBooks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.BL
{
    public class CategoriaItmBL : Base
    {
        public List<CategoriaItmDTO> getCategoriaItmsEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.CategoriaItm.Where(x => x.IdEmpresa == idEmpresa).Select(x => new CategoriaItmDTO
                {
                    IdCategoriaItm = x.IdCategoriaItm,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).ToList();
                return result;
            }
        }
        public List<CategoriaItmDTO> getCategoriaItmsActivasEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.CategoriaItm.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new CategoriaItmDTO
                {
                    IdCategoriaItm = x.IdCategoriaItm,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).ToList();
                return result;
            }
        }
        public List<CategoriaItmDTO> getCategoriaItmsEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.CategoriaItm.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new CategoriaItmDTO
                {
                    IdCategoriaItm = x.IdCategoriaItm,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public CategoriaItmDTO getCategoriaItmEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.CategoriaItm.Where(x => x.IdEmpresa == idEmpresa && x.IdCategoriaItm == id)
                    .Select(x => new CategoriaItmDTO
                    {
                        IdCategoriaItm = x.IdCategoriaItm,
                        Nombre = x.Nombre,
                        Estado = x.Estado,
                        IdEmpresa = x.IdEmpresa
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(CategoriaItmDTO CategoriaItm)
        {
            using (var context = getContext())
            {
                try
                {
                    CategoriaItm nuevo = new CategoriaItm();
                    nuevo.Nombre = CategoriaItm.Nombre;
                    nuevo.Estado = true;
                    nuevo.IdEmpresa = CategoriaItm.IdEmpresa;
                    context.CategoriaItm.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(CategoriaItmDTO CategoriaItm)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.CategoriaItm.Where(x => x.IdCategoriaItm == CategoriaItm.IdCategoriaItm).SingleOrDefault();
                    row.Nombre = CategoriaItm.Nombre;
                    row.Estado = CategoriaItm.Estado;
                    row.IdEmpresa = CategoriaItm.IdEmpresa;
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
