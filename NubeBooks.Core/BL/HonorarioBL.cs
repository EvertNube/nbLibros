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
    public class HonorarioBL : Base
    {
        public List<HonorarioDTO> getHonorariosEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Honorario.Where(x => x.IdEmpresa == idEmpresa).Select(x => new HonorarioDTO
                {
                    IdHonorario = x.IdHonorario,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).ToList();
                return result;
            }
        }
        public List<HonorarioDTO> getHonorariosActivosEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Honorario.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new HonorarioDTO
                {
                    IdHonorario = x.IdHonorario,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).ToList();
                return result;
            }
        }
        public List<HonorarioDTO> getHonorariosEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Honorario.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new HonorarioDTO
                {
                    IdHonorario = x.IdHonorario,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public HonorarioDTO getHonorarioEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Honorario.Where(x => x.IdEmpresa == idEmpresa && x.IdHonorario == id)
                    .Select(x => new HonorarioDTO
                    {
                        IdHonorario = x.IdHonorario,
                        Nombre = x.Nombre,
                        Estado = x.Estado,
                        IdEmpresa = x.IdEmpresa
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(HonorarioDTO Honorario)
        {
            using (var context = getContext())
            {
                try
                {
                    Honorario nuevo = new Honorario();
                    nuevo.Nombre = Honorario.Nombre;
                    nuevo.Estado = true;
                    nuevo.IdEmpresa = Honorario.IdEmpresa;
                    context.Honorario.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(HonorarioDTO Honorario)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Honorario.Where(x => x.IdHonorario == Honorario.IdHonorario).SingleOrDefault();
                    row.Nombre = Honorario.Nombre;
                    row.Estado = Honorario.Estado;
                    row.IdEmpresa = Honorario.IdEmpresa;
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
