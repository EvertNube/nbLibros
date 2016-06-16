using NubeBooks.Core.Logistics.DTO;
using NubeBooks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.BL
{
    public class ServicioBL : Base
    {
        public List<ServicioDTO> getServiciosEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Servicio.Where(x => x.IdEmpresa == idEmpresa).Select(x => new ServicioDTO
                {
                    IdServicio = x.IdServicio,
                    IdMoneda = x.IdMoneda,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Precio = x.Precio,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    simboloMoneda = x.Moneda.Simbolo,
                    nMoneda = x.Moneda.Nombre
                }).ToList();
                return result;
            }
        }
        public List<ServicioDTO> getServiciosActivosEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Servicio.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new ServicioDTO
                {
                    IdServicio = x.IdServicio,
                    IdMoneda = x.IdMoneda,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Precio = x.Precio,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    simboloMoneda = x.Moneda.Simbolo,
                    nMoneda = x.Moneda.Nombre
                }).ToList();
                return result;
            }
        }
        public List<ServicioDTO> getServiciosEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Servicio.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new ServicioDTO
                {
                    IdServicio = x.IdServicio,
                    IdMoneda = x.IdMoneda,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Precio = x.Precio,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    simboloMoneda = x.Moneda.Simbolo
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public ServicioDTO getServicioEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Servicio.Where(x => x.IdEmpresa == idEmpresa && x.IdServicio == id)
                    .Select(x => new ServicioDTO
                    {
                        IdServicio = x.IdServicio,
                        IdMoneda = x.IdMoneda,
                        Codigo = x.Codigo,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        Estado = x.Estado,
                        IdEmpresa = x.IdEmpresa,
                        Precio = x.Precio,
                        simboloMoneda = x.Moneda.Simbolo
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(ServicioDTO Servicio)
        {
            using (var context = getContext())
            {
                try
                {
                    Servicio nuevo = new Servicio();
                    nuevo.IdMoneda = Servicio.IdMoneda;
                    nuevo.Codigo = Servicio.Codigo;
                    nuevo.Nombre = Servicio.Nombre;
                    nuevo.Precio = Servicio.Precio;
                    nuevo.Descripcion = Servicio.Descripcion;
                    nuevo.Estado = true;
                    nuevo.IdEmpresa = Servicio.IdEmpresa;
                    context.Servicio.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(ServicioDTO Servicio)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Servicio.Where(x => x.IdServicio == Servicio.IdServicio).SingleOrDefault();
                    row.IdMoneda = Servicio.IdMoneda;
                    row.Codigo = Servicio.Codigo;
                    row.Nombre = Servicio.Nombre;
                    row.Precio = Servicio.Precio;
                    row.Descripcion = Servicio.Descripcion;
                    row.Estado = Servicio.Estado;
                    row.IdEmpresa = Servicio.IdEmpresa;
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
