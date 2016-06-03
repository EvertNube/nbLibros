using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//required for sql function access
using System.Data.Entity.Core.Objects.DataClasses;

namespace NubeBooks.Core.BL
{
    public class MovimientoBL : Base
    {
        public List<MovimientoDTO> getMovimientos_EnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                List<int> listaCuentas = context.CuentaBancaria.Where(x => x.IdEmpresa == idEmpresa).Select(x => x.IdCuentaBancaria).ToList();

                var result = context.Movimiento.Where(x => listaCuentas.Contains(x.IdCuentaBancaria)).Select(x => new MovimientoDTO
                {
                    IdMovimiento = x.IdMovimiento,
                    IdCuentaBancaria = x.IdCuentaBancaria,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoMovimiento = x.FormaMovimiento.IdTipoMovimiento,
                    IdFormaMovimiento = x.IdFormaMovimiento,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdCategoria = x.IdCategoria,
                    IdEstadoMovimiento = x.IdEstadoMovimiento,
                    NroOperacion = x.NroOperacion ?? "",
                    Fecha = x.Fecha,
                    Monto = x.Monto,
                    nTipoDocumento = x.TipoDocumento.Nombre ?? "",
                    NumeroDocumento = x.IdComprobante != null ? x.Comprobante.NroDocumento : x.NumeroDocumento ?? "",
                    TipoCambio = x.TipoCambio,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaCreacion = x.FechaCreacion,
                    MontoSinIGV = x.MontoSinIGV,
                    IdComprobante = x.IdComprobante,
                    NombreCategoria = x.Categoria.Nombre ?? "",
                    NombreEntidadR = x.EntidadResponsable.Nombre ?? "",
                    NombreUsuario = x.Usuario.Cuenta,
                    SaldoBancario = x.SaldoBancario,
                    SimboloMoneda = x.CuentaBancaria.Moneda.Simbolo
                }).ToList();
                return result;
            }
        }

        public List<MovimientoDTO> getMovimientos()
        {
            using (var context = getContext())
            {
                var result = context.Movimiento.Select(x => new MovimientoDTO
                {
                    IdMovimiento = x.IdMovimiento,
                    IdCuentaBancaria = x.IdCuentaBancaria,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoMovimiento = x.FormaMovimiento.IdTipoMovimiento,
                    IdFormaMovimiento = x.IdFormaMovimiento,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdCategoria = x.IdCategoria,
                    IdEstadoMovimiento = x.IdEstadoMovimiento,
                    NroOperacion = x.NroOperacion,
                    Fecha = x.Fecha,
                    Monto = x.Monto,
                    TipoCambio = x.TipoCambio,
                    NumeroDocumento = x.IdComprobante != null ? x.Comprobante.NroDocumento : x.NumeroDocumento,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaCreacion = x.FechaCreacion,
                    MontoSinIGV = x.MontoSinIGV,
                    IdComprobante = x.IdComprobante
                }).OrderBy(x => x.Fecha).ToList();
                return result;
            }
        }
        public MovimientoDTO getMovimiento(int id)
        {
            using (var context = getContext())
            {
                var result = context.Movimiento.Where(x => x.IdMovimiento == id)
                    .Select(r => new MovimientoDTO
                    {
                        IdMovimiento = r.IdMovimiento,
                        IdCuentaBancaria = r.IdCuentaBancaria,
                        IdEntidadResponsable = r.IdEntidadResponsable,
                        IdTipoMovimiento = r.FormaMovimiento.IdTipoMovimiento,
                        IdFormaMovimiento = r.IdFormaMovimiento,
                        IdTipoDocumento = r.IdTipoDocumento,
                        IdCategoria = r.IdCategoria,
                        IdEstadoMovimiento = r.IdEstadoMovimiento,
                        NroOperacion = r.NroOperacion,
                        Fecha = r.Fecha,
                        Monto = r.Monto,
                        TipoCambio = r.TipoCambio,
                        NumeroDocumento = r.IdComprobante != null ? r.Comprobante.NroDocumento : r.NumeroDocumento,
                        Comentario = r.Comentario,
                        Estado = r.Estado,
                        UsuarioCreacion = r.UsuarioCreacion,
                        FechaCreacion = r.FechaCreacion,
                        MontoSinIGV = r.MontoSinIGV,
                        IdComprobante = r.IdComprobante
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(MovimientoDTO Movimiento)
        {
            using (var context = getContext())
            {
                try
                {
                    Movimiento nuevo = new Movimiento();
                    nuevo.IdCuentaBancaria = Movimiento.IdCuentaBancaria;
                    nuevo.IdEntidadResponsable = Movimiento.IdEntidadResponsable;
                    nuevo.IdFormaMovimiento = Movimiento.IdFormaMovimiento;
                    nuevo.IdTipoDocumento = (Movimiento.IdTipoDocumento != 0 && Movimiento.IdTipoDocumento != null ) ? Movimiento.IdTipoDocumento : null;
                    //El IdCategoria sera 1 porque la Categoria con Id = 1 es No tiene categoria
                    //nuevo.IdCategoria = (Movimiento.IdCategoria != 0 && Movimiento.IdCategoria != null) ? Movimiento.IdCategoria : 1;
                    nuevo.IdCategoria = Movimiento.IdCategoria;
                    nuevo.IdEstadoMovimiento = Movimiento.IdEstadoMovimiento;
                    nuevo.NroOperacion = Movimiento.NroOperacion;
                    //Hora Actual
                    DateTime Hora = DateTime.Now;
                    Movimiento.FechaCreacion = Movimiento.FechaCreacion + new TimeSpan(Hora.Hour, Hora.Minute, Hora.Second);
                    Movimiento.Fecha = Movimiento.Fecha + new TimeSpan(Hora.Hour, Hora.Minute, Hora.Second);
                    //--------
                    nuevo.Fecha = Convert.ToDateTime(Movimiento.Fecha.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    nuevo.Monto = Movimiento.Monto;
                    nuevo.TipoCambio = Movimiento.TipoCambio;
                    nuevo.NumeroDocumento = Movimiento.NumeroDocumento;
                    nuevo.Comentario = Movimiento.Comentario;
                    nuevo.Estado = true;
                    nuevo.UsuarioCreacion = Movimiento.UsuarioCreacion;
                    //nuevo.FechaCreacion = Movimiento.FechaCreacion;
                    nuevo.FechaCreacion = Convert.ToDateTime(Movimiento.FechaCreacion.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    nuevo.MontoSinIGV = Movimiento.MontoSinIGV ?? 0;
                    nuevo.IdComprobante = Movimiento.IdComprobante == 0 ? null : Movimiento.IdComprobante;
                    context.Movimiento.Add(nuevo);
                    context.SaveChanges();
                    
                    //Actualizar saldos del Libro
                    ActualizarSaldos(Movimiento.IdCuentaBancaria);
                    //Actualizar saldo Bancario en Movimiento
                    ActualizarSaldoBancarioEnMovimiento(nuevo.IdMovimiento);
                    //Actualizar Fecha de Ultima Fecha de Conciliacion en la Empresa
                    //ActualizarFechaConciliacionEnEmpresa(Movimiento.IdCuentaBancaria);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(MovimientoDTO Movimiento)
        {
            using (var context = getContext())
            {
                try
                {
                    var datoRow = context.Movimiento.Where(x => x.IdMovimiento == Movimiento.IdMovimiento).SingleOrDefault();
                    datoRow.IdCuentaBancaria = Movimiento.IdCuentaBancaria;
                    datoRow.IdEntidadResponsable = Movimiento.IdEntidadResponsable;
                    datoRow.IdFormaMovimiento = Movimiento.IdFormaMovimiento;
                    datoRow.IdTipoDocumento = Movimiento.IdTipoDocumento == 0 ? null : Movimiento.IdTipoDocumento;
                    //El IdCategoria sera 1 porque la Categoria con Id = 1 es No tiene categoria
                    //datoRow.IdCategoria = (Movimiento.IdCategoria != 0 && Movimiento.IdCategoria != null) ? Movimiento.IdCategoria : 1;
                    datoRow.IdCategoria = Movimiento.IdCategoria;
                    datoRow.IdEstadoMovimiento = Movimiento.IdEstadoMovimiento;
                    datoRow.NroOperacion = Movimiento.NroOperacion;
                    //Hora Actual
                    DateTime Hora = DateTime.Now;
                    Movimiento.Fecha = Movimiento.Fecha + new TimeSpan(Hora.Hour, Hora.Minute, Hora.Second);
                    //--------
                    datoRow.Fecha = Convert.ToDateTime(Movimiento.Fecha.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    datoRow.Monto = Movimiento.Monto;
                    datoRow.TipoCambio = Movimiento.TipoCambio;
                    datoRow.NumeroDocumento = Movimiento.NumeroDocumento;
                    datoRow.Comentario = Movimiento.Comentario;
                    datoRow.Estado = Movimiento.Estado;
                    datoRow.UsuarioCreacion = Movimiento.UsuarioCreacion;
                    //datoRow.FechaCreacion = Movimiento.FechaCreacion;
                    datoRow.FechaCreacion = Convert.ToDateTime(Movimiento.FechaCreacion.ToString("yyyy-MM-dd hh:mm:ss tt"));
                    datoRow.MontoSinIGV = Movimiento.MontoSinIGV ?? 0;
                    datoRow.IdComprobante = Movimiento.IdComprobante == 0 ? null : Movimiento.IdComprobante;
                    context.SaveChanges();
                    //Actualizar saldos del Libro
                    ActualizarSaldos(Movimiento.IdCuentaBancaria);
                    //Actualizar saldo Bancario en Movimiento
                    //ActualizarSaldoBancarioEnMovimiento(Movimiento.IdMovimiento);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool delete(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Movimiento.Where(x => x.IdMovimiento == id).SingleOrDefault();
                    //Si el movimiento esta ligado a un comprobante le ponemos el estado EJECUTADO = FALSE
                    if(row.IdComprobante != null && row.IdComprobante != 0)
                    {
                        var row2 = context.Comprobante.Where(x => x.IdComprobante == row.IdComprobante).SingleOrDefault();
                        row2.FechaPago = null;
                        row2.Ejecutado = false;
                    }
                    int idCuenta = row.IdCuentaBancaria;
                    context.Movimiento.Remove(row);
                    context.SaveChanges();
                    ActualizarSaldos(idCuenta);
                    return true;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public void ActualizarEstadoMovimiento(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    var dataRow = context.Movimiento.Where(x => x.IdMovimiento == id).SingleOrDefault();
                    dataRow.IdEstadoMovimiento = dataRow.IdEstadoMovimiento == 1 ? 2 : 1;
                    context.SaveChanges();
                    ActualizarSaldos(dataRow.IdCuentaBancaria);
                    //return ActualizarSaldoBancarioEnMovimiento_Decimal(dataRow.IdMovimiento);
                }
                catch (Exception e)
                {
                    throw e;
                }
                
            }
        }

        public IList<TipoMovimientoDTO> getTiposMovimientos(bool AsSelectList = false)
        {
            TipoMovimientoBL oBL = new TipoMovimientoBL();
            if (!AsSelectList)
            {
                //return oBL.getTiposMovimientos();
                var lista = oBL.getTiposMovimientos().ToList();
                return lista;
            }
            else
            {
                var lista = oBL.getTiposMovimientos();
                lista.Insert(0, new TipoMovimientoDTO() { IdTipoMovimiento = 0, Nombre = "Seleccione el Tipo de Mov." });
                return lista;
            }
        }

        public IList<EstadoMovimientoDTO> getEstadosMovimientos(bool AsSelectList = false)
        {
            EstadoMovimientoBL oBL = new EstadoMovimientoBL();
            if (!AsSelectList)
                return oBL.getEstadosMovimientos();
            else
            {
                var lista = oBL.getEstadosMovimientos();
                lista.Insert(0, new EstadoMovimientoDTO() { IdEstadoMovimiento = 0, Nombre = "Seleccione el Estado del Mov." });
                return lista;
            }
        }

        public IList<EntidadResponsableDTO> getEntidadesResponsablesEnEmpresa(int idEmpresa, bool AsSelectList = false)
        {
            EntidadResponsableBL oBL = new EntidadResponsableBL();
            if (!AsSelectList)
                return oBL.getEntidadResponsablesEnEmpresaViewBag(idEmpresa);
            else
            {
                var lista = oBL.getEntidadResponsablesEnEmpresaViewBag(idEmpresa);
                lista.Insert(0, new EntidadResponsableDTO() { IdEntidadResponsable = null, Nombre = "Seleccione la Entidad Responsable" });
                return lista;
            }
        }

        public IList<EntidadResponsableDTO> getEntidadesResponsables(bool AsSelectList = false)
        {
            EntidadResponsableBL oBL = new EntidadResponsableBL();
            if (!AsSelectList)
                return oBL.getEntidadResponsablesViewBag();
            else
            {
                var lista = oBL.getEntidadResponsablesViewBag();
                lista.Insert(0, new EntidadResponsableDTO() { IdEntidadResponsable = 0, Nombre = "Seleccione la Entidad Responsable" });
                return lista;
            }
        }

        public string getNombreCategoria(int id)
        {
            if(id != 0)
            { 
            CategoriaBL oBL = new CategoriaBL();
            return oBL.getCategoria(id).Nombre;
            }
            return "Sin Categoría";
        }

        public void ActualizarSaldos(int idCuentaB)
        {
            if (idCuentaB != 0)
            {
                CuentaBancariaBL oBL = new CuentaBancariaBL();
                oBL.updateSaldos(idCuentaB);
            }
        }

        public void ActualizarFechaConciliacionEnEmpresa(int idCuentaB)
        {
            using (var context = getContext())
            {
                try
                {
                    var cuenta = context.CuentaBancaria.Where(x => x.IdCuentaBancaria == idCuentaB).SingleOrDefault();
                    var empresa = context.Empresa.Where(x => x.IdEmpresa == cuenta.IdEmpresa).SingleOrDefault();
                    //La fecha de conciliacion siempre tiene que ser mayor
                    //if (empresa.FechaConciliacion < Fecha) { empresa.FechaConciliacion = Convert.ToDateTime(Fecha.ToString("yyyy-MM-dd hh:mm:ss tt")); }
                    if (empresa.FechaConciliacion < cuenta.FechaConciliacion) { empresa.FechaConciliacion = cuenta.FechaConciliacion; }

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void ActualizarSaldoBancarioEnMovimiento(int idMovimiento)
        {
            using (var context = getContext())
            {
                try
                {
                    var obj = context.Movimiento.Where(x => x.IdMovimiento == idMovimiento).SingleOrDefault();
                    obj.SaldoBancario = obj.CuentaBancaria.SaldoBancario;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public Decimal ActualizarSaldoBancarioEnMovimiento_Decimal(int idMovimiento)
        {
            using (var context = getContext())
            {
                try
                {
                    var obj = context.Movimiento.Where(x => x.IdMovimiento == idMovimiento).SingleOrDefault();
                    obj.SaldoBancario = obj.CuentaBancaria.SaldoBancario;
                    context.SaveChanges();
                    return obj.SaldoBancario.GetValueOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public IList<MovimientoDTO> getReporteDetalleLibro(int? IdCuentaB, DateTime? FechaInicio, DateTime? FechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_GetReporteDetalleMovimientos(IdCuentaB, FechaInicio, FechaFin).Select(x => new MovimientoDTO
                {
                    IdMovimiento = x.IdMovimiento,
                    IdCuentaBancaria = x.IdCuentaBancaria,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoMovimiento = x.IdTipoMovimiento,
                    IdCategoria = x.IdCategoria,
                    IdEstadoMovimiento = x.IdEstadoMovimiento,
                    NroOperacion = x.NroOperacion,
                    Fecha = x.Fecha,
                    Monto = x.Monto,
                    NumeroDocumento = x.NumeroDocumento,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaCreacion = x.FechaCreacion,
                    NombreEntidadR = x.EntidadResNombre,
                    NombreCategoria = x.CategoriaNombre,
                    NombreUsuario = x.UsuarioNombre
                }).ToList();
                return result;
            }
        }
        public List<TipoEntidadDTO> getListaTipoEntidades()
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
        public List<FormaMovimientoDTO> getListaFormaDeMovimientos()
        {
            using (var context = getContext())
            {
                var result = context.FormaMovimiento.Select(x => new FormaMovimientoDTO
                {
                    IdFormaMovimiento = x.IdFormaMovimiento,
                    IdTipoMovimiento = x.IdTipoMovimiento,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    NombreTipo = x.TipoMovimiento.Nombre
                }).ToList();
                return result;
            }
        }

        public List<Select2DTO> Select2_lstFormaDeMovimientos()
        {
            using (var context = getContext())
            {
                var result = context.TipoMovimiento.AsEnumerable().Select(x => new Select2DTO
                {
                    id = x.IdTipoMovimiento,
                    text = x.Nombre,
                    disabled = "true",
                    children = x.FormaMovimiento.Select(z => new Select2DTO
                    {
                        id = z.IdFormaMovimiento,
                        text = z.Nombre
                    }).ToList()
                }).ToList();
                
                return result;
            }
        }

        public List<FormaMovimientoDTO> getListaFormaDeMovimientosBasic()
        {
            List<FormaMovimientoDTO> lista = new List<FormaMovimientoDTO>();

            lista.Add(new FormaMovimientoDTO() { IdFormaMovimiento = 4, IdTipoMovimiento = 1, Nombre = "Ingreso", Estado = true, NombreTipo = "Entrada" });
            lista.Add(new FormaMovimientoDTO() { IdFormaMovimiento = 8, IdTipoMovimiento = 2, Nombre = "Egreso", Estado = true, NombreTipo = "Salida" });

            return lista;
        }

        public List<TipoDocumentoDTO> getListaTiposDeDocumento()
        {
            using (var context = getContext())
            {
                var result = context.TipoDocumento.Select(x => new TipoDocumentoDTO 
                {
                    IdTipoDocumento = x.IdTipoDocumento,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }

        public IList<TipoDocumentoDTO> getListaTiposDeDocumentoVB(bool AsSelectList = false)
        {
            if (!AsSelectList)
                return getListaTiposDeDocumento();
            else
            {
                var lista = getListaTiposDeDocumento();
                lista.Insert(0, new TipoDocumentoDTO() { IdTipoDocumento = 0, Nombre = "Seleccione un tipo" });
                return lista;
            }
        }

        public List<Select2DTO_B> getComprobantesPendientesEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && !x.Ejecutado && x.Estado).Select(x => new Select2DTO_B
                    {
                        id = x.IdComprobante,
                        text = x.NroDocumento
                    }).ToList();
                return result;
            }
        }
        public List<MovimientoDTO> getMovimientos_Asoc_Comprobante(int idComprobante)
        {
            using (var context = getContext())
            {
                var result = context.Movimiento.Where(x => x.IdComprobante == idComprobante).Select(x => new MovimientoDTO
                {
                    IdMovimiento = x.IdMovimiento,
                    IdCuentaBancaria = x.IdCuentaBancaria,
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    IdTipoMovimiento = x.FormaMovimiento.IdTipoMovimiento,
                    IdFormaMovimiento = x.IdFormaMovimiento,
                    IdTipoDocumento = x.IdTipoDocumento,
                    IdCategoria = x.IdCategoria,
                    IdEstadoMovimiento = x.IdEstadoMovimiento,
                    NroOperacion = x.NroOperacion ?? "",
                    Fecha = x.Fecha,
                    Monto = x.Monto,
                    nTipoDocumento = x.TipoDocumento.Nombre ?? "",
                    NumeroDocumento = x.IdComprobante != null ? x.Comprobante.NroDocumento : x.NumeroDocumento ?? "",
                    TipoCambio = x.TipoCambio,
                    Comentario = x.Comentario,
                    Estado = x.Estado,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaCreacion = x.FechaCreacion,
                    MontoSinIGV = x.MontoSinIGV,
                    IdComprobante = x.IdComprobante,
                    NombreCategoria = x.Categoria.Nombre ?? "",
                    NombreEntidadR = x.EntidadResponsable.Nombre ?? "",
                    NombreUsuario = x.Usuario.Cuenta,
                    SaldoBancario = x.SaldoBancario
                }).ToList();
                return result;
            }
        }
    }
}
