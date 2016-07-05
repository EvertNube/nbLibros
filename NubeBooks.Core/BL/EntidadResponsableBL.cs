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
    public class EntidadResponsableBL : Base
    {
        public List<EntidadResponsableDTO> getEntidadesResponsablesPorTipoEnEmpresa(int idEmpresa, int pTipoEntidad)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoEntidad == pTipoEntidad).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoIdentificacion = x.IdTipoIdentificacion,
                    IdTipoEntidad = x.IdTipoEntidad,
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Detraccion = x.Detraccion,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    NroIdentificacion = x.NroIdentificacion,
                    TipoIdentificacion = x.TipoIdentificacion.Nombre,
                    NombreIdentificacion = x.NroIdentificacion != null ? x.TipoIdentificacion.Nombre + " - " + x.NroIdentificacion : "N/A",
                    NombreComercial = x.NombreComercial,
                    nResponsable = x.Responsable.Nombre,
                    Direccion = x.Direccion,
                    Banco = x.Banco,
                    CuentaSoles = x.CuentaSoles,
                    CuentaDolares = x.CuentaDolares,
                    Credito = x.Credito,
                    TipoPersona = x.TipoPersona,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Email = x.Email,
                    Comentario = x.Comentario
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<EntidadResponsableDTO> getEntidadesResponsablesPorTipo_Activos_EnEmpresa(int idEmpresa, int pTipoEntidad)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoEntidad == pTipoEntidad && x.Estado).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoIdentificacion = x.IdTipoIdentificacion,
                    IdTipoEntidad = x.IdTipoEntidad,
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Detraccion = x.Detraccion,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    NroIdentificacion = x.NroIdentificacion,
                    NombreIdentificacion = x.NroIdentificacion != null ? x.TipoIdentificacion.Nombre + " - " + x.NroIdentificacion : "N/A",
                    NombreComercial = x.NombreComercial,
                    Direccion = x.Direccion,
                    Banco = x.Banco,
                    CuentaSoles = x.CuentaSoles,
                    CuentaDolares = x.CuentaDolares,
                    Credito = x.Credito,
                    TipoPersona = x.TipoPersona,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Email = x.Email,
                    Comentario = x.Comentario
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<EntidadResponsableDTO> getEntidadesResponsablesActivasPorTipoEnEmpresa(int idEmpresa, int pTipoEntidad)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdEmpresa == idEmpresa && x.IdTipoEntidad == pTipoEntidad && x.Estado).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoIdentificacion = x.IdTipoIdentificacion,
                    IdTipoEntidad = x.IdTipoEntidad,
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Detraccion = x.Detraccion,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    NroIdentificacion = x.NroIdentificacion,
                    NombreIdentificacion = x.NroIdentificacion != null ? x.TipoIdentificacion.Nombre + " - " + x.NroIdentificacion : "N/A",
                    NombreComercial = x.NombreComercial,
                    Direccion = x.Direccion,
                    Banco = x.Banco,
                    CuentaSoles = x.CuentaSoles,
                    CuentaDolares = x.CuentaDolares,
                    Credito = x.Credito ?? 0,
                    TipoPersona = x.TipoPersona,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Email = x.Email,
                    Comentario = x.Comentario
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<EntidadResponsableDTO> getEntidadResponsablesEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdEmpresa == idEmpresa).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoIdentificacion = x.IdTipoIdentificacion,
                    IdTipoEntidad = x.IdTipoEntidad,
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Detraccion = x.Detraccion,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    NroIdentificacion = x.NroIdentificacion,
                    NombreIdentificacion = x.NroIdentificacion != null ? x.TipoIdentificacion.Nombre + " - " + x.NroIdentificacion : "N/A",
                    NombreComercial = x.NombreComercial,
                    Direccion = x.Direccion,
                    Banco = x.Banco,
                    CuentaSoles = x.CuentaSoles,
                    CuentaDolares = x.CuentaDolares,
                    Credito = x.Credito,
                    TipoPersona = x.TipoPersona,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Email = x.Email,
                    Comentario = x.Comentario
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public List<EntidadResponsableDTO> getEntidadResponsables()
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoIdentificacion = x.IdTipoIdentificacion,
                    IdTipoEntidad = x.IdTipoEntidad,
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Detraccion = x.Detraccion,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    NombreIdentificacion = x.NroIdentificacion,
                    NombreComercial = x.NombreComercial,
                    Direccion = x.Direccion,
                    Banco = x.Banco,
                    CuentaSoles = x.CuentaSoles,
                    CuentaDolares = x.CuentaDolares,
                    Credito = x.Credito,
                    TipoPersona = x.TipoPersona,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Email = x.Email,
                    Comentario = x.Comentario
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<EntidadResponsableDTO> getEntidadResponsablesEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoIdentificacion = x.IdTipoIdentificacion,
                    IdTipoEntidad = x.IdTipoEntidad,
                    IdResponsable = x.IdResponsable,
                    //Nombre = x.IdTipoEntidad == 1 ? "C > " + x.Nombre : "P > " + x.Nombre,
                    Nombre = x.Nombre,
                    NombreTipoEntidad = x.TipoEntidad.Nombre,
                    Estado = x.Estado,
                    Detraccion = x.Detraccion,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    NroIdentificacion = x.NroIdentificacion,
                    NombreComercial = x.NombreComercial,
                    Direccion = x.Direccion,
                    Banco = x.Banco,
                    CuentaSoles = x.CuentaSoles,
                    CuentaDolares = x.CuentaDolares,
                    Credito = x.Credito,
                    TipoPersona = x.TipoPersona,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Email = x.Email,
                    Comentario = x.Comentario
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<EntidadResponsableDTO> getEntidadResponsablesViewBag()
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.Estado).Select(x => new EntidadResponsableDTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoIdentificacion = x.IdTipoIdentificacion,
                    IdTipoEntidad = x.IdTipoEntidad,
                    IdResponsable = x.IdResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Detraccion = x.Detraccion,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    NroIdentificacion = x.NroIdentificacion,
                    NombreComercial = x.NombreComercial,
                    Direccion = x.Direccion,
                    Banco = x.Banco,
                    CuentaSoles = x.CuentaSoles,
                    CuentaDolares = x.CuentaDolares,
                    Credito = x.Credito,
                    TipoPersona = x.TipoPersona,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Email = x.Email,
                    Comentario = x.Comentario
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public EntidadResponsableDTO getEntidadResponsableEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdEntidadResponsable == id && x.IdEmpresa == idEmpresa)
                    .Select(r => new EntidadResponsableDTO
                    {
                        IdEntidadResponsable = r.IdEntidadResponsable,
                        IdTipoIdentificacion = r.IdTipoIdentificacion,
                        IdTipoEntidad = r.IdTipoEntidad,
                        IdResponsable = r.IdResponsable,
                        Nombre = r.Nombre,
                        Estado = r.Estado,
                        Detraccion = r.Detraccion,
                        Tipo = r.Tipo,
                        IdEmpresa = r.IdEmpresa,
                        NroIdentificacion = r.NroIdentificacion,
                        NombreComercial = r.NombreComercial,
                        Direccion = r.Direccion,
                        Banco = r.Banco,
                        CuentaSoles = r.CuentaSoles,
                        CuentaDolares = r.CuentaDolares,
                        Credito = r.Credito,
                        TipoPersona = r.TipoPersona,
                        Telefono1 = r.Telefono1,
                        Telefono2 = r.Telefono2,
                        Email = r.Email,
                        Comentario = r.Comentario,
                        lstProyectos = r.Proyecto.Select(x => new ProyectoDTO {
                            IdProyecto = x.IdProyecto,
                            IdEntidadResponsable = x.IdEntidadResponsable,
                            Nombre = x.Nombre,
                            Descripcion = x.Descripcion,
                            Estado = x.Estado
                        }).ToList(),
                        lstContactos = r.Contacto.Select(x => new ContactoDTO {
                            IdContacto = x.IdContacto,
                            IdEntidadResponsable = x.IdEntidadResponsable,
                            Nombre = x.Nombre,
                            Telefono = x.Telefono,
                            Celular = x.Celular,
                            Email = x.Email,
                            Cargo = x.Cargo,
                            Estado = x.Estado
                        }).ToList()
                    }).SingleOrDefault();
                return result;
            }
        }

        public EntidadResponsableDTO getEntidadResponsableEnEmpresa_Only(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdEntidadResponsable == id && x.IdEmpresa == idEmpresa)
                    .Select(x => new EntidadResponsableDTO {
                        IdEntidadResponsable = x.IdEntidadResponsable,
                        IdTipoIdentificacion = x.IdTipoIdentificacion,
                        IdTipoEntidad = x.IdTipoEntidad,
                        IdResponsable = x.IdResponsable,
                        Nombre = x.Nombre,
                        Estado = x.Estado,
                        Detraccion = x.Detraccion,
                        Tipo = x.Tipo,
                        IdEmpresa = x.IdEmpresa,
                        NroIdentificacion = x.NroIdentificacion,
                        NombreComercial = x.NombreComercial,
                        Direccion = x.Direccion,
                        Banco = x.Banco,
                        CuentaSoles = x.CuentaSoles,
                        CuentaDolares = x.CuentaDolares,
                        Credito = x.Credito,
                        TipoPersona = x.TipoPersona,
                        Telefono1 = x.Telefono1,
                        Telefono2 = x.Telefono2,
                        Email = x.Email,
                        Comentario = x.Comentario
                    }).SingleOrDefault();

                return result;
            }
        }

        public List<ProyectoDTO> getProyectosActivos_EntidadResponsableEnEmpresa(int id)
        {
            using (var context = getContext())
            {
                var result = context.Proyecto.Where(x => x.IdEntidadResponsable == id && x.Estado).Select(x => new ProyectoDTO
                {
                    IdProyecto = x.IdProyecto,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }

        public List<ProyectoDTO> getProyectos_EntidadResponsableEnEmpresa(int id)
        {
            using (var context = getContext())
            {
                var result = context.Proyecto.Where(x => x.IdEntidadResponsable == id).Select(x => new ProyectoDTO {
                    IdProyecto = x.IdProyecto,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }

        public List<ContactoDTO> getContactosActivos_EnEntidadResponsable(int id)
        {
            using (var context = getContext())
            {
                var result = context.Contacto.Where(x => x.IdEntidadResponsable == id && x.Estado).Select(x => new ContactoDTO
                {
                    IdContacto = x.IdContacto,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Telefono = x.Telefono,
                    Celular = x.Celular,
                    Email = x.Email,
                    Cargo = x.Cargo,
                    Estado = x.Estado
                }).ToList();

                return result;
            }
        }
        public List<ContactoDTO> getContactos_EntidadResponsableEnEmpresa(int id)
        {
            using (var context = getContext())
            {
                var result = context.Contacto.Where(x => x.IdEntidadResponsable == id).Select(x => new ContactoDTO {
                    IdContacto = x.IdContacto,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Telefono = x.Telefono,
                    Celular = x.Celular,
                    Email = x.Email,
                    Cargo = x.Cargo,
                    Estado = x.Estado
                }).ToList();

                return result;
            }
        }

        public EntidadResponsableDTO getEntidadResponsable(int id)
        {
            using (var context = getContext())
            {
                var result = context.EntidadResponsable.Where(x => x.IdEntidadResponsable == id)
                    .Select(x => new EntidadResponsableDTO
                    {
                        IdEntidadResponsable = x.IdEntidadResponsable,
                        IdTipoIdentificacion = x.IdTipoIdentificacion,
                        IdTipoEntidad = x.IdTipoEntidad,
                        IdResponsable = x.IdResponsable,
                        Nombre = x.Nombre,
                        Estado = x.Estado,
                        Detraccion = x.Detraccion,
                        Tipo = x.Tipo,
                        IdEmpresa = x.IdEmpresa,
                        NroIdentificacion = x.NroIdentificacion,
                        NombreComercial = x.NombreComercial,
                        Direccion = x.Direccion,
                        Banco = x.Banco,
                        CuentaSoles = x.CuentaSoles,
                        CuentaDolares = x.CuentaDolares,
                        Credito = x.Credito,
                        TipoPersona = x.TipoPersona,
                        Telefono1 = x.Telefono1,
                        Telefono2 = x.Telefono2,
                        Email = x.Email,
                        Comentario = x.Comentario
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(EntidadResponsableDTO EntidadResponsable)
        {
            using (var context = getContext())
            {
                try
                {
                    EntidadResponsable nuevo = new EntidadResponsable();
                    nuevo.Nombre = EntidadResponsable.Nombre;
                    nuevo.IdTipoIdentificacion = EntidadResponsable.IdTipoIdentificacion;
                    nuevo.IdTipoEntidad = EntidadResponsable.IdTipoEntidad;
                    nuevo.IdResponsable = EntidadResponsable.IdResponsable;
                    nuevo.Estado = true;
                    nuevo.Detraccion = EntidadResponsable.Detraccion;
                    nuevo.Tipo = EntidadResponsable.Tipo;
                    nuevo.IdEmpresa = EntidadResponsable.IdEmpresa;
                    nuevo.NroIdentificacion = EntidadResponsable.NroIdentificacion;
                    nuevo.NombreComercial = EntidadResponsable.NombreComercial;
                    nuevo.Direccion = EntidadResponsable.Direccion;
                    nuevo.Banco = EntidadResponsable.Banco;
                    nuevo.CuentaSoles = EntidadResponsable.CuentaSoles;
                    nuevo.CuentaDolares = EntidadResponsable.CuentaDolares;
                    nuevo.Credito = EntidadResponsable.Credito;
                    nuevo.TipoPersona = EntidadResponsable.TipoPersona;
                    nuevo.Telefono1 = EntidadResponsable.Telefono1;
                    nuevo.Telefono2 = EntidadResponsable.Telefono2;
                    nuevo.Email = EntidadResponsable.Email;
                    nuevo.Comentario = EntidadResponsable.Comentario;
                    context.EntidadResponsable.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(EntidadResponsableDTO EntidadResponsable)
        {
            using (var context = getContext())
            {
                try
                {
                    var datoRow = context.EntidadResponsable.Where(x => x.IdEntidadResponsable == EntidadResponsable.IdEntidadResponsable).SingleOrDefault();
                    datoRow.Nombre = EntidadResponsable.Nombre;
                    datoRow.IdTipoIdentificacion = EntidadResponsable.IdTipoIdentificacion;
                    datoRow.IdTipoEntidad = EntidadResponsable.IdTipoEntidad;
                    datoRow.IdResponsable = EntidadResponsable.IdResponsable;
                    datoRow.Estado = EntidadResponsable.Estado;
                    datoRow.Detraccion = EntidadResponsable.Detraccion;
                    datoRow.Tipo = EntidadResponsable.Tipo;
                    datoRow.IdEmpresa = EntidadResponsable.IdEmpresa;
                    datoRow.NroIdentificacion = EntidadResponsable.NroIdentificacion;
                    datoRow.NombreComercial = EntidadResponsable.NombreComercial;
                    datoRow.Direccion = EntidadResponsable.Direccion;
                    datoRow.Banco = EntidadResponsable.Banco;
                    datoRow.CuentaSoles = EntidadResponsable.CuentaSoles;
                    datoRow.CuentaDolares = EntidadResponsable.CuentaDolares;
                    datoRow.Credito = EntidadResponsable.Credito;
                    datoRow.TipoPersona = EntidadResponsable.TipoPersona;
                    datoRow.Telefono1 = EntidadResponsable.Telefono1;
                    datoRow.Telefono2 = EntidadResponsable.Telefono2;
                    datoRow.Email = EntidadResponsable.Email;
                    datoRow.Comentario = EntidadResponsable.Comentario;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public IList<EntidadResponsableR_DTO> getReporteResumenEntidadesR(int? IdCuentaB, DateTime? FechaInicio, DateTime? FechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_GetReporteResumenEntidadesRes(IdCuentaB, FechaInicio, FechaFin).Select(x => new EntidadResponsableR_DTO
                    {
                        IdEntidadResponsable = x.IdEntidadResponsable,
                        Nombre = x.Nombre,
                        Detraccion = x.Detraccion,
                        Monto = x.MontoTotal.GetValueOrDefault(),
                        Tipo = x.Tipo
                    }).ToList();
                return result;
            }
        }

        public List<TipoEntidadDTO> getTipoDeEntidades()
        {
            using (var context = getContext())
            {
                var result = context.TipoEntidad.Select(x => new TipoEntidadDTO
                {
                    IdTipoEntidad = x.IdTipoEntidad,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }

        public List<TipoIdentificacionDTO> getTiposDeIdentificaciones()
        {
            using (var context = getContext())
            {
                var result = context.TipoIdentificacion.Select(x => new TipoIdentificacionDTO
                    {
                        IdTipoIdentificacion = x.IdTipoIdentificacion,
                        Nombre = x.Nombre,
                        Estado = x.Estado
                    }).ToList();
                return result;
            }
        }

        public List<ComprobanteDTO> getComprobantes_ConEntidad(int idEmpresa, int idEntidad)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && (x.IdEntidadResponsable == idEntidad || x.IdEntidadResponsable2 == idEntidad)).Select(x => new ComprobanteDTO
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
                    IdProyecto = x.Proyecto.FirstOrDefault().IdProyecto,
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
                    NombreProyecto = x.Proyecto.FirstOrDefault().Nombre ?? ""
                }).ToList();

                return result;
            }
        }
    }
}