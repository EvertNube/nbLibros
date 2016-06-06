﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NubeBooks.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LibrosDBEntities : DbContext
    {
        public LibrosDBEntities()
            : base("name=LibrosDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AreaPorComprobante> AreaPorComprobante { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<CategoriaItm> CategoriaItm { get; set; }
        public virtual DbSet<CategoriaPorPeriodo> CategoriaPorPeriodo { get; set; }
        public virtual DbSet<Comprobante> Comprobante { get; set; }
        public virtual DbSet<Contacto> Contacto { get; set; }
        public virtual DbSet<CuentaBancaria> CuentaBancaria { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<EntidadResponsable> EntidadResponsable { get; set; }
        public virtual DbSet<EstadoMovimiento> EstadoMovimiento { get; set; }
        public virtual DbSet<FormaMovimiento> FormaMovimiento { get; set; }
        public virtual DbSet<FormaMovimientoInv> FormaMovimientoInv { get; set; }
        public virtual DbSet<Honorario> Honorario { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<LogUsuario> LogUsuario { get; set; }
        public virtual DbSet<Moneda> Moneda { get; set; }
        public virtual DbSet<Movimiento> Movimiento { get; set; }
        public virtual DbSet<MovimientoInv> MovimientoInv { get; set; }
        public virtual DbSet<Periodo> Periodo { get; set; }
        public virtual DbSet<Proyecto> Proyecto { get; set; }
        public virtual DbSet<Responsable> Responsable { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TipoComprobante> TipoComprobante { get; set; }
        public virtual DbSet<TipoCuenta> TipoCuenta { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<TipoEntidad> TipoEntidad { get; set; }
        public virtual DbSet<TipoIdentificacion> TipoIdentificacion { get; set; }
        public virtual DbSet<TipoMovimiento> TipoMovimiento { get; set; }
        public virtual DbSet<TipoMovimientoInv> TipoMovimientoInv { get; set; }
        public virtual DbSet<Ubicacion> Ubicacion { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
    
        public virtual int SP_ActualizarMontos(Nullable<int> idCuentaB)
        {
            var idCuentaBParameter = idCuentaB.HasValue ?
                new ObjectParameter("IdCuentaB", idCuentaB) :
                new ObjectParameter("IdCuentaB", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ActualizarMontos", idCuentaBParameter);
        }
    
        public virtual int SP_ActualizarMontos_Empresa(Nullable<int> idEmpresa)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ActualizarMontos_Empresa", idEmpresaParameter);
        }
    
        public virtual int SP_ActualizarPresupuestoPadre(Nullable<int> idCategoria, Nullable<int> idPeriodo)
        {
            var idCategoriaParameter = idCategoria.HasValue ?
                new ObjectParameter("IdCategoria", idCategoria) :
                new ObjectParameter("IdCategoria", typeof(int));
    
            var idPeriodoParameter = idPeriodo.HasValue ?
                new ObjectParameter("IdPeriodo", idPeriodo) :
                new ObjectParameter("IdPeriodo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_ActualizarPresupuestoPadre", idCategoriaParameter, idPeriodoParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<SP_Get_Arbol_Categoria_Result> SP_Get_Arbol_Categoria(Nullable<int> idCategoria)
        {
            var idCategoriaParameter = idCategoria.HasValue ?
                new ObjectParameter("IdCategoria", idCategoria) :
                new ObjectParameter("IdCategoria", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_Arbol_Categoria_Result>("SP_Get_Arbol_Categoria", idCategoriaParameter);
        }
    
        public virtual ObjectResult<SP_Get_CuentasPor_CobrarPagar_Result> SP_Get_CuentasPor_CobrarPagar(Nullable<int> idEmpresa, Nullable<int> idMoneda, Nullable<int> idTipoComprobante)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var idMonedaParameter = idMoneda.HasValue ?
                new ObjectParameter("IdMoneda", idMoneda) :
                new ObjectParameter("IdMoneda", typeof(int));
    
            var idTipoComprobanteParameter = idTipoComprobante.HasValue ?
                new ObjectParameter("IdTipoComprobante", idTipoComprobante) :
                new ObjectParameter("IdTipoComprobante", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_CuentasPor_CobrarPagar_Result>("SP_Get_CuentasPor_CobrarPagar", idEmpresaParameter, idMonedaParameter, idTipoComprobanteParameter);
        }
    
        public virtual ObjectResult<SP_Get_MontoIncompletoEnComprobante_Result> SP_Get_MontoIncompletoEnComprobante(Nullable<int> idComprobante, Nullable<int> idCuentaBancaria, Nullable<int> idEmpresa)
        {
            var idComprobanteParameter = idComprobante.HasValue ?
                new ObjectParameter("IdComprobante", idComprobante) :
                new ObjectParameter("IdComprobante", typeof(int));
    
            var idCuentaBancariaParameter = idCuentaBancaria.HasValue ?
                new ObjectParameter("IdCuentaBancaria", idCuentaBancaria) :
                new ObjectParameter("IdCuentaBancaria", typeof(int));
    
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_MontoIncompletoEnComprobante_Result>("SP_Get_MontoIncompletoEnComprobante", idComprobanteParameter, idCuentaBancariaParameter, idEmpresaParameter);
        }
    
        public virtual ObjectResult<SP_Get_Rep_De_Inventarios_Result> SP_Get_Rep_De_Inventarios(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_Rep_De_Inventarios_Result>("SP_Get_Rep_De_Inventarios", idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Get_Rep_De_Items_Por_Vencimiento_Result> SP_Get_Rep_De_Items_Por_Vencimiento(Nullable<int> idEmpresa, Nullable<System.DateTime> rFechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var rFechaFinParameter = rFechaFin.HasValue ?
                new ObjectParameter("rFechaFin", rFechaFin) :
                new ObjectParameter("rFechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_Rep_De_Items_Por_Vencimiento_Result>("SP_Get_Rep_De_Items_Por_Vencimiento", idEmpresaParameter, rFechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Get_Rep_De_Movimientos_De_Inventarios_Result> SP_Get_Rep_De_Movimientos_De_Inventarios(Nullable<int> idItem, Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idItemParameter = idItem.HasValue ?
                new ObjectParameter("IdItem", idItem) :
                new ObjectParameter("IdItem", typeof(int));
    
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_Rep_De_Movimientos_De_Inventarios_Result>("SP_Get_Rep_De_Movimientos_De_Inventarios", idItemParameter, idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Get_Rep_Stock_De_Items_Result> SP_Get_Rep_Stock_De_Items(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_Rep_Stock_De_Items_Result>("SP_Get_Rep_Stock_De_Items", idEmpresaParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Get_StockLote_De_Lote_En_Empresa_Result> SP_Get_StockLote_De_Lote_En_Empresa(Nullable<int> idEmpresa, string serieLote)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var serieLoteParameter = serieLote != null ?
                new ObjectParameter("SerieLote", serieLote) :
                new ObjectParameter("SerieLote", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_StockLote_De_Lote_En_Empresa_Result>("SP_Get_StockLote_De_Lote_En_Empresa", idEmpresaParameter, serieLoteParameter);
        }
    
        public virtual ObjectResult<SP_Get_StockLotes_En_Empresa_Result> SP_Get_StockLotes_En_Empresa(Nullable<int> idEmpresa)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Get_StockLotes_En_Empresa_Result>("SP_Get_StockLotes_En_Empresa", idEmpresaParameter);
        }
    
        public virtual ObjectResult<SP_GetReporteDetalleMovimientos_Result> SP_GetReporteDetalleMovimientos(Nullable<int> idCuentaB, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idCuentaBParameter = idCuentaB.HasValue ?
                new ObjectParameter("IdCuentaB", idCuentaB) :
                new ObjectParameter("IdCuentaB", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetReporteDetalleMovimientos_Result>("SP_GetReporteDetalleMovimientos", idCuentaBParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_GetReporteResumenCategorias_Result> SP_GetReporteResumenCategorias(Nullable<int> idCuentaB, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idCuentaBParameter = idCuentaB.HasValue ?
                new ObjectParameter("IdCuentaB", idCuentaB) :
                new ObjectParameter("IdCuentaB", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetReporteResumenCategorias_Result>("SP_GetReporteResumenCategorias", idCuentaBParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_GetReporteResumenEntidadesRes_Result> SP_GetReporteResumenEntidadesRes(Nullable<int> idCuentaB, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idCuentaBParameter = idCuentaB.HasValue ?
                new ObjectParameter("IdCuentaB", idCuentaB) :
                new ObjectParameter("IdCuentaB", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetReporteResumenEntidadesRes_Result>("SP_GetReporteResumenEntidadesRes", idCuentaBParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual ObjectResult<SP_Rep_AvanceDePresupuesto_Result> SP_Rep_AvanceDePresupuesto(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_AvanceDePresupuesto_Result>("SP_Rep_AvanceDePresupuesto", idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_DetalleGastosPorPartidaDePresupuesto_Result> SP_Rep_DetalleGastosPorPartidaDePresupuesto(Nullable<int> idCategoria, Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idCategoriaParameter = idCategoria.HasValue ?
                new ObjectParameter("IdCategoria", idCategoria) :
                new ObjectParameter("IdCategoria", typeof(int));
    
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_DetalleGastosPorPartidaDePresupuesto_Result>("SP_Rep_DetalleGastosPorPartidaDePresupuesto", idCategoriaParameter, idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_Documentos_IngYEgr_PagadosYPorCobrar_Result> SP_Rep_Documentos_IngYEgr_PagadosYPorCobrar(Nullable<int> idTipoComprobante, Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idTipoComprobanteParameter = idTipoComprobante.HasValue ?
                new ObjectParameter("IdTipoComprobante", idTipoComprobante) :
                new ObjectParameter("IdTipoComprobante", typeof(int));
    
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_Documentos_IngYEgr_PagadosYPorCobrar_Result>("SP_Rep_Documentos_IngYEgr_PagadosYPorCobrar", idTipoComprobanteParameter, idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_EgresosPorAreas_Result> SP_Rep_EgresosPorAreas(Nullable<int> idArea, Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idAreaParameter = idArea.HasValue ?
                new ObjectParameter("IdArea", idArea) :
                new ObjectParameter("IdArea", typeof(int));
    
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_EgresosPorAreas_Result>("SP_Rep_EgresosPorAreas", idAreaParameter, idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_FacturacionPorAreas_Result> SP_Rep_FacturacionPorAreas(Nullable<int> idArea, Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idAreaParameter = idArea.HasValue ?
                new ObjectParameter("IdArea", idArea) :
                new ObjectParameter("IdArea", typeof(int));
    
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_FacturacionPorAreas_Result>("SP_Rep_FacturacionPorAreas", idAreaParameter, idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_FacturacionPorCliente_Result> SP_Rep_FacturacionPorCliente(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_FacturacionPorCliente_Result>("SP_Rep_FacturacionPorCliente", idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_FacturacionPorHonorarios_Result> SP_Rep_FacturacionPorHonorarios(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_FacturacionPorHonorarios_Result>("SP_Rep_FacturacionPorHonorarios", idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_FacturacionPorVendedor_Result> SP_Rep_FacturacionPorVendedor(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_FacturacionPorVendedor_Result>("SP_Rep_FacturacionPorVendedor", idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_GastosPorProveedor_Result> SP_Rep_GastosPorProveedor(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_GastosPorProveedor_Result>("SP_Rep_GastosPorProveedor", idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_IngresosEgresos_PorMes_EnEmpresa_Result> SP_Rep_IngresosEgresos_PorMes_EnEmpresa(Nullable<int> idEmpresa, Nullable<int> anio)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var anioParameter = anio.HasValue ?
                new ObjectParameter("Anio", anio) :
                new ObjectParameter("Anio", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_IngresosEgresos_PorMes_EnEmpresa_Result>("SP_Rep_IngresosEgresos_PorMes_EnEmpresa", idEmpresaParameter, anioParameter);
        }
    
        public virtual ObjectResult<SP_Rep_IngresosEgresosPorAreaNull_Result> SP_Rep_IngresosEgresosPorAreaNull(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_IngresosEgresosPorAreaNull_Result>("SP_Rep_IngresosEgresosPorAreaNull", idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual ObjectResult<SP_Rep_IngresosEgresosPorAreas_Result> SP_Rep_IngresosEgresosPorAreas(Nullable<int> idEmpresa, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var idEmpresaParameter = idEmpresa.HasValue ?
                new ObjectParameter("IdEmpresa", idEmpresa) :
                new ObjectParameter("IdEmpresa", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Rep_IngresosEgresosPorAreas_Result>("SP_Rep_IngresosEgresosPorAreas", idEmpresaParameter, fechaInicioParameter, fechaFinParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}
