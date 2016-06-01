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
    public class ProyectoBL : Base
    {
        public List<ProyectoDTO> getProyectos()
        {
            using (var context = getContext())
            {
                var result = context.Proyecto.Select(x => new ProyectoDTO
                {
                    IdProyecto = x.IdProyecto,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }
        public ProyectoDTO getProyecto(int id)
        {
            using (var context = getContext())
            {
                var result = context.Proyecto.Where(x => x.IdProyecto == id)
                    .Select(x => new ProyectoDTO
                    {
                        IdProyecto = x.IdProyecto,
                        IdEntidadResponsable = x.IdEntidadResponsable,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        Estado = x.Estado,
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(ProyectoDTO Proyecto)
        {
            using (var context = getContext())
            {
                try
                {
                    Proyecto nuevo = new Proyecto();
                    nuevo.Nombre = Proyecto.Nombre;
                    nuevo.IdEntidadResponsable = Proyecto.IdEntidadResponsable;
                    nuevo.Estado = true;
                    nuevo.Descripcion = Proyecto.Descripcion;
                    context.Proyecto.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(ProyectoDTO Proyecto)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Proyecto.Where(x => x.IdProyecto == Proyecto.IdProyecto).SingleOrDefault();
                    row.IdEntidadResponsable = Proyecto.IdEntidadResponsable;
                    row.Nombre = Proyecto.Nombre;
                    row.Descripcion = Proyecto.Descripcion;
                    row.Estado = Proyecto.Estado;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<Select2DTO_B> getProyectosPorEntidad(int idEntidad, bool? esNull = false)
        {
            using (var context = getContext())
            {
                var result = context.Proyecto.Where(x => x.IdEntidadResponsable == idEntidad && x.Estado).Select(x => new Select2DTO_B
                {
                    id = x.IdProyecto,
                    text = x.Nombre
                }).ToList();

                if(esNull != null)
                {
                    result.Insert(0, new Select2DTO_B() { id = null, text = "Vacio" });
                }

                return result;
            }
        }

        public List<ComprobanteDTO> getComprobantes_ConProyecto(int idEmpresa, int idProyecto)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.IdProyecto == idProyecto).Select(x => new ComprobanteDTO
                {
                    IdComprobante = x.IdComprobante,
                    IdTipoComprobante = x.IdTipoComprobante,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdMoneda = x.IdMoneda,
                    IdEmpresa = x.IdEmpresa,
                    NroDocumento = x.NroDocumento,
                    Monto = x.Monto,
                    IdArea = x.IdArea,
                    IdResponsable = x.IdResponsable,
                    IdCategoria = x.IdCategoria,
                    IdProyecto = x.IdProyecto,
                    FechaEmision = x.FechaEmision,
                    FechaConclusion = x.FechaConclusion,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    Ejecutado = x.Ejecutado,
                    IdHonorario = x.IdHonorario,
                    NombreEntidad = x.EntidadResponsable.Nombre ?? "",
                    NombreMoneda = x.Moneda.Nombre,
                    NombreTipoComprobante = x.TipoComprobante.Nombre,
                    NombreTipoDocumento = x.TipoDocumento.Nombre,
                    SimboloMoneda = x.Moneda.Simbolo,
                    MontoSinIGV = x.MontoSinIGV,
                    TipoCambio = x.TipoCambio,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaPago = x.FechaPago,
                    NombreUsuario = x.Usuario.Cuenta,
                    NombreCategoria = x.Categoria.Nombre ?? "",
                    NombreProyecto = x.Proyecto.Nombre ?? ""
                }).ToList();

                return result;
            }
        }
    }
}
