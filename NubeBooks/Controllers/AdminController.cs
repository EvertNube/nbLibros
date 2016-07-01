using NubeBooks.Core.BL;
using NubeBooks.Core.DTO;
using NubeBooks.Core.Logistics.BL;
using NubeBooks.Core.Logistics.DTO;
using NubeBooks.Helpers;
using NubeBooks.Helpers.Razor;
using NubeBooks.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Data;
using System.Text.RegularExpressions;

namespace NubeBooks.Controllers
{
    public class AdminController : Controller
    {
        protected Navbar navbar { get; set; }
        private bool currentUser()
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["User"] != null) { return true; }
            else { return false; }
        }
        private UsuarioDTO getCurrentUser()
        {
            if (this.currentUser())
            {
                return (UsuarioDTO)System.Web.HttpContext.Current.Session["User"];
            }
            return null;
        }
        private bool isSuperAdministrator()
        {
            if (getCurrentUser().IdRol == 1) return true;
            return false;
        }
        private bool isAdministrator()
        {
            if (getCurrentUser().IdRol <= 2) return true;
            return false;
        }
        private bool isUsuarioInterno()
        {
            if (getCurrentUser().IdRol == 3) return true;
            return false;
        }
        private bool isUsuarioExterno()
        {
            if (getCurrentUser().IdRol == 4) return true;
            return false;
        }
        private void createResponseMessage(string status, string message = "", string status_field = "status", string message_field = "message")
        {
            TempData[status_field] = status;
            if (!String.IsNullOrWhiteSpace(message))
            {
                TempData[message_field] = message;
            }
        }

        public AdminController()
        {
            UsuarioDTO user = getCurrentUser();
            if (user != null)
            {
                this.navbar = new Navbar();
                ViewBag.currentUser = user;
                ViewBag.NombreEmpresa = user.nombreEmpresa;
                //ViewBag.Title = "NubeBooks";

                ViewBag.EsAdmin = isAdministrator();
                ViewBag.EsSuperAdmin = isSuperAdministrator();
                ViewBag.EsUsuarioInterno = isUsuarioInterno();
                ViewBag.EsUsuarioExterno = isUsuarioExterno();
                ViewBag.IdRol = user.IdRol;

                EmpresaBL empBL = new EmpresaBL();
                ViewBag.Empresas = empBL.getEmpresasViewBag();
            }
            else { ViewBag.EsAdmin = false; ViewBag.EsSuperAdmin = false; }
        }

        public ActionResult Ingresar()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string UserName, string codigoEmpresa)
        {
            UsuariosBL objBL = new UsuariosBL();

            UsuarioDTO usuario = new UsuarioDTO() { Cuenta = UserName, Email = UserName, codigoEmpresa = codigoEmpresa };
            usuario = objBL.getUserByAcountOrEmail(usuario);

            if (usuario == null)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_RECOVERY_PASSWORD);
                return RedirectToAction("ForgotPassword", "Admin");
            }
            else
            {
                //if (objBL.recoverPasswordNew(usuario))
                usuario = objBL.generateTokenRecoverPassword(usuario);
                if (usuario.Token != null)
                {
                    string link = "<a href='" + this.Url.Action("ResetPassword", "Admin", new { rt = usuario.Token, emp = usuario.codigoEmpresa }, this.Request.Url.Scheme) + "'>Reset Password</a>";
                    objBL.SendMailResetPassword(usuario, link);
                    createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_MESSAGE_FOR_RECOVERY_PASSWORD);
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, "<strong>Hubo un error al recuperar la contraseña.</strong>");
                    return RedirectToAction("ForgotPassword", "Admin");
                }
            }

            return RedirectToAction("Ingresar", "Admin");
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt, string emp)
        {
            if (rt == null || emp == null)
            {
                return RedirectToAction("Ingresar", "Admin");
            }

            ResetPasswordDTO obj = new ResetPasswordDTO();
            obj.rt = rt;
            obj.emp = emp;
            return View(obj);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordDTO obj)
        {
            if (obj.rt == null || obj.emp == null || obj.Password == null || obj.Password == "")
            {
                return RedirectToAction("Ingresar", "Admin");
            }
            if (obj.Password != obj.ConfirmPassword)
            {
                createResponseMessage(CONSTANTES.ERROR, "<strong>Las contraseñas ingresadas tienen que coincidir.</strong>");
                return View();
            }
            UsuariosBL objBL = new UsuariosBL();
            UsuarioDTO usuario = new UsuarioDTO() { Token = obj.rt, codigoEmpresa = obj.emp, Pass = obj.Password };

            if (objBL.resetPasswordByTokenAndEmp(usuario))
            {
                createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_PASSWORD_CHANGE);
                return RedirectToAction("Ingresar", "Admin");
            }
            createResponseMessage(CONSTANTES.ERROR, "<strong>Usted no puede realizar esta acción o hubo un error al intentar cambiar la contraseña.</strong>");
            return View();
        }


        [HttpPost]
        public ActionResult Login(UsuarioDTO user)
        {
            UsuariosBL usuariosBL = new UsuariosBL();
            if (usuariosBL.isValidUser(user))
            {
                System.Web.HttpContext.Current.Session["User"] = usuariosBL.getUsuarioByCuenta(user);
                return RedirectToAction("Index");
            }
            createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_LOGIN);
            return RedirectToAction("Ingresar");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Ingresar");
        }

        public ActionResult Formulario()
        { return View(); }

        public ActionResult Index()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            MenuNavBarSelected(0);

            UsuarioDTO user = getCurrentUser();

            EmpresaBL objBL = new EmpresaBL();

            EmpresaDTO empresaOld = objBL.getEmpresa(user.IdEmpresa);
            EmpresaDTO empresa = objBL.getEmpresa(user.IdEmpresa);
            ViewBag.FechaConciliacion = empresa.FechaConciliacion.GetValueOrDefault().ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture("en-GB"));
            ViewBag.TotalSoles = empresa.TotalSoles.GetValueOrDefault();
            ViewBag.TotalDolares = empresa.TotalDolares.GetValueOrDefault();
            ViewBag.TotalSolesOld = empresa.TotalSolesOld.GetValueOrDefault();
            ViewBag.TotalDolaresOld = empresa.TotalDolaresOld.GetValueOrDefault();

            if (empresa.IdMoneda == 1)
            { ViewBag.TotalConsolidado = empresa.TotalSoles.GetValueOrDefault() + empresa.TotalDolares.GetValueOrDefault() * empresa.TipoCambio; }
            else
            { ViewBag.TotalConsolidado = empresa.TotalDolares.GetValueOrDefault() + empresa.TotalSoles.GetValueOrDefault() / empresa.TipoCambio; }

            ViewBag.TipoCambio = empresa.TipoCambio;
            ViewBag.TipoMoneda = empresa.IdMoneda;
            ViewBag.sMoneda = empresa.SimboloMoneda;
            //Liquidez
            ViewBag.lstLiquidezSoles = objBL.getLiquidezEnEmpresaPorMoneda(user.IdEmpresa, 1);
            ViewBag.lstLiquidezDolares = objBL.getLiquidezEnEmpresaPorMoneda(user.IdEmpresa, 2);
            //Rentabilidad
            ViewBag.lstRentabilidad = objBL.getRentabilidadEnEmpresaSegunMoneda(user.IdEmpresa, empresa.IdMoneda);
            //Facturacion y Variacion Porcentual
            List<LiquidezDTO> lista1 = objBL.getFacturacionEnEmpresaPorMoneda(user.IdEmpresa, empresa.IdMoneda, DateTime.Now);
            List<LiquidezDTO> lista2 = objBL.getFacturacionEnEmpresaPorMoneda(user.IdEmpresa, empresa.IdMoneda, DateTime.Now.AddYears(-1));
            ViewBag.lstFacturacion = lista1; ViewBag.lstFacturacionLastYear = lista2;
            ViewBag.lstVPorcentual = objBL.getVariacionPorcentual_12Meses(lista1, lista2);
            //Ejecucion de Presupuesto
            ViewBag.EjecucionIngresos = objBL.getEjecucionDePresupuestoEnEmpresa(user.IdEmpresa, empresa.IdMoneda, empresa.IdPeriodo.GetValueOrDefault(), 1);
            ViewBag.EjecucionEgresos = objBL.getEjecucionDePresupuestoEnEmpresa(user.IdEmpresa, empresa.IdMoneda, empresa.IdPeriodo.GetValueOrDefault(), 2);
            //Cuentas por Cobrar y Cuentas por Pagar
            ViewBag.CuentasXCobrar_Soles = objBL.Get_CuentasPorCobrar(empresa.IdEmpresa, 1);
            ViewBag.CuentasXCobrar_Dolares = objBL.Get_CuentasPorCobrar(empresa.IdEmpresa, 2);
            ViewBag.CuentasXPagar_Soles = objBL.Get_CuentasPorPagar(empresa.IdEmpresa, 1);
            ViewBag.CuentasXPagar_Dolares = objBL.Get_CuentasPorPagar(empresa.IdEmpresa, 2);
            //Cartera Morosa - dias vencidos
            ComprobanteBL cmpBL = new ComprobanteBL();
            List<ComprobanteDTO> lstCarteraMorosa = cmpBL.getComprobantesIngresosYEgresosEnEmpresa_diasVencidos(empresa.IdEmpresa, 1);
            ViewBag.CarteraM_Soles = cmpBL.CarteraMorosa_porMoneda(1, lstCarteraMorosa);
            ViewBag.CarteraM_Dolares = cmpBL.CarteraMorosa_porMoneda(2, lstCarteraMorosa);
            ViewBag.CarteraM_Soles_Count = cmpBL.CarteraMorosa_Count_porMoneda(1, lstCarteraMorosa);
            ViewBag.CarteraM_Dolares_Count = cmpBL.CarteraMorosa_Count_porMoneda(2, lstCarteraMorosa);

            //Principales clientes y proveedores
            ViewBag.top5Clientes = objBL.getTop5Clientes(user.IdEmpresa, empresa.IdPeriodo.GetValueOrDefault());
            ViewBag.top5Proveedores = objBL.getTop5Proveedores(user.IdEmpresa, empresa.IdPeriodo.GetValueOrDefault());
            //Ingresos por Area
            ViewBag.topIngAreas = objBL.getTopIngArea(user.IdEmpresa, empresa.IdPeriodo.GetValueOrDefault());
            ViewBag.topEgrAreas = objBL.getTopEgrArea(user.IdEmpresa, empresa.IdPeriodo.GetValueOrDefault());
            //Contador de Movimientos y Comprobantes en los meses
            //ViewBag.contMovimientos = objBL.getContadorDeMovimientos(user.IdEmpresa);
            //ViewBag.contComprobantes = objBL.getContadorDeComprobantes(user.IdEmpresa);

            return View();
        }

        public ActionResult Empresa()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            MenuNavBarSelected(0);
            UsuarioDTO currentUser = getCurrentUser();

            EmpresaBL objBL = new EmpresaBL();
            ViewBag.lstMonedas = objBL.getListaMonedas();

            var objSent = TempData["Empresa"];
            if (objSent != null) { TempData["Empresa"] = null; return View(objSent); }
            
            EmpresaDTO obj = objBL.getEmpresaBasic(getCurrentUser().IdEmpresa);
            if (obj == null) return RedirectToAction("Index");
            return View(obj);
        }

        [HttpPost]
        public ActionResult AddEmpresa(EmpresaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                EmpresaBL objBL = new EmpresaBL();
                if (dto.IdEmpresa == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Index");
                    }
                }
                else if (dto.IdEmpresa != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdEmpresa != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Empresa"] = dto;
            return RedirectToAction("Empresa");
        }

        public ActionResult Libros(int? idTipoCuenta = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Libros";
            MenuNavBarSelected(1);

            UsuarioDTO miUsuario = getCurrentUser();

            CuentaBancariaBL objBL = new CuentaBancariaBL();
            ViewBag.lstTipoCuentas = objBL.getTipoDeCuentas();
            ViewBag.idTipoCuenta = idTipoCuenta;
            List<CuentaBancariaDTO> listaLibros = new List<CuentaBancariaDTO>();

            if (miUsuario.IdEmpresa != 0)
            {
                EmpresaBL empBL = new EmpresaBL();
                Decimal miTipoCambio = empBL.getEmpresa((int)miUsuario.IdEmpresa).TipoCambio;

                listaLibros = objBL.getCuentasBancariasEnEmpresa(miUsuario.IdEmpresa);
                ViewBag.TotalSoles = DameTotalSoles(listaLibros);
                ViewBag.TotalDolares = DameTotalDolares(listaLibros);

                ViewBag.TotalConsolidado = DameTotalConsolidado(listaLibros, miTipoCambio);
                ViewBag.TipoCambio = miTipoCambio;
            }
            else
            {
                ViewBag.TotalSoles = 0.0;
                ViewBag.TotalDolares = 0.0;
                ViewBag.TotalConsolidado = 0.0;
                ViewBag.TipoCambio = 1.0;
            }

            return View("Libros", listaLibros);
        }

        public ActionResult LibrosBancarios(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Bancarios";
            MenuNavBarSelected(2, 0);

            UsuarioDTO user = getCurrentUser();
            CuentaBancariaBL objBL = new CuentaBancariaBL();
            List<CuentaBancariaDTO> lista = new List<CuentaBancariaDTO>();
            ViewBag.vbInactivos = inactivos;

            if (user.IdEmpresa != 0)
            {
                if (!inactivos)
                { lista = objBL.getCuentasBancariasActivasPorTipoEnEmpresa(user.IdEmpresa, 1); }
                else
                { lista = objBL.getCuentasBancariasPorTipoEnEmpresa(user.IdEmpresa, 1); }
            }
            return View(lista);
        }
        public ActionResult LibrosAdministrativos(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Administrativos";
            MenuNavBarSelected(2, 1);

            UsuarioDTO user = getCurrentUser();
            CuentaBancariaBL objBL = new CuentaBancariaBL();
            List<CuentaBancariaDTO> lista = new List<CuentaBancariaDTO>();
            ViewBag.vbInactivos = inactivos;

            if (user.IdEmpresa != 0)
            {
                if (!inactivos)
                { lista = objBL.getCuentasBancariasActivasPorTipoEnEmpresa(user.IdEmpresa, 2); }
                else
                { lista = objBL.getCuentasBancariasPorTipoEnEmpresa(user.IdEmpresa, 2); }
            }
            return View(lista);
        }

        //public ActionResult Libro(int? id = null, int? idTipoCuenta = null, string sortOrder = null, string currentFilter = null, string searchString = null, int? page = null)
        public ActionResult Libro(int? id = null, int? idTipoCuenta = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Libro";

            int tipoCuenta = 2;
            if (idTipoCuenta != null) { tipoCuenta = idTipoCuenta.GetValueOrDefault(); }
            MenuNavBarSelected(2, tipoCuenta - 1);

            UsuarioDTO miUsuario = getCurrentUser();

            CuentaBancariaBL objBL = new CuentaBancariaBL();
            ViewBag.IdCuentaBancaria = id;
            ViewBag.Monedas = objBL.getMonedasBag(false);
            var objSent = TempData["Libro"];
            if (objSent != null) { TempData["Libro"] = null; return View(objSent); }

            CuentaBancariaDTO obj;
            if (id != null && id != 0)
            {
                //Actualizar Saldo Disponible
                objBL.updateSaldos((int)id);
                obj = objBL.getCuentaBancariaEnEmpresa(miUsuario.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Index");
                if (obj.IdEmpresa != miUsuario.IdEmpresa) return RedirectToAction("Index");

                //obj.listaMovimientoPL = BusquedaYPaginado_Movimiento(obj.listaMovimiento, sortOrder, currentFilter, searchString, page);
                return View(obj);
            }

            obj = new CuentaBancariaDTO();
            obj.FechaConciliacion = DateTime.Now;
            obj.IdEmpresa = miUsuario.IdEmpresa;
            if (idTipoCuenta != null && idTipoCuenta != 0) obj.IdTipoCuenta = idTipoCuenta.GetValueOrDefault();

            return View(obj);
        }

        private IPagedList<MovimientoDTO> BusquedaYPaginado_Movimiento(IList<MovimientoDTO> lista, string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (!String.IsNullOrEmpty(searchString))
            { searchString = searchString.ToLower(); }
            ViewBag.CurrentSort = sortOrder;

            ViewBag.vbFecha = sortOrder == "Fecha" ? "Fecha_desc" : "Fecha";
            ViewBag.vbTipo = sortOrder == "Tipo" ? "Tipo_desc" : "Tipo";
            ViewBag.vbDetalle = sortOrder == "Detalle" ? "Detalle_desc" : "Detalle";
            ViewBag.vbMonto = sortOrder == "Monto" ? "Monto_desc" : "Monto";
            ViewBag.vbCategoria = sortOrder == "Categoria" ? "Categoria_desc" : "Categoria";
            ViewBag.vbEntidad = sortOrder == "Entidad" ? "Entidad_desc" : "Entidad";
            ViewBag.vbDocumento = sortOrder == "Documento" ? "Documento_desc" : "Documento";
            ViewBag.vbUsuario = sortOrder == "Usuario" ? "Usuario_desc" : "Usuario";
            ViewBag.vbEstado = sortOrder == "Estado" ? "Estado_desc" : "Estado";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            string tipoDato = "cadena";
            DateTime pTiempo;
            if (DateTime.TryParse(searchString, out pTiempo))
            {
                tipoDato = "tiempo";
                pTiempo = Convert.ToDateTime(searchString);
            }

            Decimal pDecimal;
            if (Decimal.TryParse(searchString, out pDecimal))
            {
                tipoDato = "numerico";
                pDecimal = Convert.ToDecimal(searchString);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                IList<MovimientoDTO> listaP;
                listaP = lista.Where(s => (s.NroOperacion.ToLower() ?? "").Contains(searchString)
                        || (s.NombreCategoria.ToLower() ?? "").Contains(searchString)
                        || (s.NombreEntidadR.ToLower() ?? "").Contains(searchString)
                        || (s.NumeroDocumento.ToLower() ?? "").Contains(searchString)
                        || (s.NombreUsuario.ToLower() ?? "").Contains(searchString)
                        ).ToList();

                switch (tipoDato)
                {
                    case "tiempo":
                        lista = lista.Where(s => DateTime.Compare(s.Fecha, pTiempo) <= 0).ToList();
                        lista = lista.Union(listaP).ToList();
                        break;
                    case "numerico":
                        lista = lista.Where(s => s.Monto.ToString().Contains(pDecimal.ToString())).ToList();
                        lista = lista.Union(listaP).ToList();
                        break;
                    default:
                        lista = listaP;
                        break;
                }
            }

            switch (sortOrder)
            {
                case "Fecha":
                    lista = lista.OrderBy(s => s.Fecha).ToList();
                    break;
                case "Tipo":
                    lista = lista.OrderBy(s => s.IdTipoMovimiento).ToList();
                    break;
                case "Detalle":
                    lista = lista.OrderBy(s => s.NroOperacion).ToList();
                    break;
                case "Monto":
                    lista = lista.OrderBy(s => s.Monto).ToList();
                    break;
                case "Categoria":
                    lista = lista.OrderBy(s => s.NombreCategoria).ToList();
                    break;
                case "Entidad":
                    lista = lista.OrderBy(s => s.NombreEntidadR).ToList();
                    break;
                case "Documento":
                    lista = lista.OrderBy(s => s.NumeroDocumento).ToList();
                    break;
                case "Usuario":
                    lista = lista.OrderBy(s => s.NombreUsuario).ToList();
                    break;
                case "Estado":
                    lista = lista.OrderBy(s => s.IdEstadoMovimiento).ToList();
                    break;
                case "Fecha_desc":
                    lista = lista.OrderByDescending(s => s.Fecha).ToList();
                    break;
                case "Tipo_desc":
                    lista = lista.OrderByDescending(s => s.IdTipoMovimiento).ToList();
                    break;
                case "Detalle_desc":
                    lista = lista.OrderByDescending(s => s.NroOperacion).ToList();
                    break;
                case "Monto_desc":
                    lista = lista.OrderByDescending(s => s.Monto).ToList();
                    break;
                case "Categoria_desc":
                    lista = lista.OrderByDescending(s => s.NombreCategoria).ToList();
                    break;
                case "Entidad_desc":
                    lista = lista.OrderByDescending(s => s.NombreEntidadR).ToList();
                    break;
                case "Documento_desc":
                    lista = lista.OrderByDescending(s => s.NumeroDocumento).ToList();
                    break;
                case "Usuario_desc":
                    lista = lista.OrderByDescending(s => s.NombreUsuario).ToList();
                    break;
                case "Estado_desc":
                    lista = lista.OrderByDescending(s => s.IdEstadoMovimiento).ToList();
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            return lista.ToPagedList(pageNumber, pageSize);
        }
        [HttpPost]
        public ActionResult AddLibro(CuentaBancariaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Libros", "Admin"); }
            try
            {
                string sTipoLibro = dto.IdTipoCuenta == 1 ? "Bancarios" : "Administrativos";
                int TipoCuenta = 1; //Por defecto tipo de comprobante Ingreso
                if (dto != null) { TipoCuenta = dto.IdTipoCuenta.GetValueOrDefault(); }

                CuentaBancariaBL objBL = new CuentaBancariaBL();
                if (dto.IdCuentaBancaria == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Libros" + sTipoLibro, "Admin");
                    }
                }
                else if (dto.IdCuentaBancaria != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Libros" + sTipoLibro, "Admin");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdCuentaBancaria != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Libro"] = dto;
            return RedirectToAction("Libro");
        }
        public ActionResult Categorias()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }

            ViewBag.Title = "Partidas de Presupuesto";

            MenuNavBarSelected(8, 0);
            EmpresaBL empBL = new EmpresaBL();

            UsuarioDTO miUsuario = getCurrentUser();
            EmpresaDTO empresa = empBL.getEmpresa(miUsuario.IdEmpresa);
            ViewBag.IdPeriodo = empresa.IdPeriodo.GetValueOrDefault();
            ViewBag.SimboloMoneda = empresa.SimboloMoneda;

            CategoriaBL objBL = new CategoriaBL();
            ViewBag.Periodos = objBL.GetPeriodosEnEmpresaViewBag(miUsuario.IdEmpresa);
            List<CategoriaDTO> listaCategorias = new List<CategoriaDTO>();
            if (empresa.IdEmpresa > 0 && empresa.IdPeriodo > 0)
            {
                //listaCategorias = objBL.getCategoriasPorPeriodo_ArbolEnEmpresa(empresa.IdEmpresa, (int)empresa.IdPeriodo);
                listaCategorias = objBL.getCategoriasTreeEnEmpresa(empresa.IdEmpresa);
            }

            return View(listaCategorias);
        }
        public ActionResult Categoria(int? id = null, int? idPadre = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }

            ViewBag.Title = "Categoría";

            MenuNavBarSelected(8, 0);
            UsuarioDTO miUsuario = getCurrentUser();
            EmpresaDTO empresa = (new EmpresaBL()).getEmpresa(miUsuario.IdEmpresa);

            CategoriaBL objBL = new CategoriaBL();
            ViewBag.IdCategoria = id;

            ViewBag.Categorias = CategoriasBucle(empresa.IdEmpresa, (int)empresa.IdPeriodo, null, null);

            ViewBag.NombreCategoria = "Sin Categoría";
            var objSent = TempData["Categoria"];
            if (objSent != null) { TempData["Categoria"] = null; return View(objSent); }

            CategoriaDTO obj;
            if (id != null || id == 0)
            {
                if (idPadre != null)
                {
                    CategoriaDTO objp = new CategoriaDTO();

                    objp.IdCategoria = 0;
                    objp.IdCategoriaPadre = idPadre;
                    objp.Orden = objBL.getUltimoHijo(idPadre.GetValueOrDefault());
                    objp.IdEmpresa = miUsuario.IdEmpresa;

                    ViewBag.NombreCategoria = objBL.getNombreCategoria(objp.IdCategoriaPadre.GetValueOrDefault());
                    return View(objp);
                }
                obj = objBL.getCategoria((int)id);
                if (obj == null) return RedirectToAction("Categorias");
                if (obj.IdEmpresa != miUsuario.IdEmpresa) return RedirectToAction("Categorias");

                ViewBag.NombreCategoria = objBL.getNombreCategoria(obj.IdCategoriaPadre.GetValueOrDefault());
                return View(obj);
            }
            obj = new CategoriaDTO();
            obj.IdEmpresa = miUsuario.IdEmpresa;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddCategoria(CategoriaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }

            try
            {
                CategoriaBL objBL = new CategoriaBL();
                if (dto.IdCategoria == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Categorias");
                    }
                }
                else if (dto.IdCategoria != 0)
                {
                    EmpresaBL empBL = new EmpresaBL();
                    int vPeriodo = empBL.getEmpresa(getCurrentUser().IdEmpresa).IdPeriodo.GetValueOrDefault();

                    if (objBL.update(dto, vPeriodo))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Categorias");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdCategoria != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Categoria"] = dto;
            return RedirectToAction("Categoria");
        }
        public ActionResult Movimientos()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }

            ViewBag.Title = "Movimientos";

            MovimientoBL objBL = new MovimientoBL();
            return View(objBL.getMovimientos());
        }
        public ActionResult Movimiento(int? id = null, int? idLibro = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Movimiento";
            MenuNavBarSelected(1);
            UsuarioDTO miUsuario = getCurrentUser();

            EmpresaBL empBL = new EmpresaBL();
            EmpresaDTO empresa = empBL.getEmpresa(miUsuario.IdEmpresa);

            ViewBag.SimboloMoneda = empresa.SimboloMoneda;

            MovimientoBL objBL = new MovimientoBL();
            ViewBag.IdMovimiento = id;
            ViewBag.EstadosMovimientos = objBL.getEstadosMovimientos(false);

            CuentaBancariaBL objCuentaBL = new CuentaBancariaBL();
            CuentaBancariaDTO objLibro = objCuentaBL.getCuentaBancariaSoloEnEmpresa(miUsuario.IdEmpresa, idLibro.GetValueOrDefault());
            if (objLibro == null) { return RedirectToAction("Index", "Admin"); }

            ViewBag.IdTipoCuenta = objLibro.IdTipoCuenta;
            ViewBag.lstTipoMovs = objBL.getTiposMovimientos();
            ViewBag.lstFormaMovs = ViewBag.IdTipoCuenta == 1 ? objBL.getListaFormaDeMovimientos() : objBL.getListaFormaDeMovimientosBasic();

            EntidadResponsableBL entBL = new EntidadResponsableBL();
            ViewBag.TipoEntidades = entBL.getTipoDeEntidades();
            ViewBag.EntidadesResponsables = objBL.getEntidadesResponsablesEnEmpresa(miUsuario.IdEmpresa, false);

            ViewBag.lstTiposDeDocumento = objBL.getListaTiposDeDocumentoVB(true);
            ViewBag.NombreCategoria = "Sin Categoría";
            ViewBag.Categorias = CategoriasBucle(empresa.IdEmpresa, (int)empresa.IdPeriodo, null, null);
            ViewBag.Comprobantes = objBL.getComprobantesPendientesEnEmpresa(miUsuario.IdEmpresa);

            var objSent = TempData["Movimiento"];
            if (objSent != null) { TempData["Movimiento"] = null; return View(objSent); }
            if (id == 0 && idLibro != null)
            {
                MovimientoDTO nuevo = new MovimientoDTO();
                nuevo.IdCuentaBancaria = (int)idLibro;
                nuevo.Fecha = DateTime.Now;
                nuevo.TipoCambio = (new EmpresaBL()).getEmpresa(miUsuario.IdEmpresa).TipoCambio;
                nuevo.NumeroDocumento = null;
                //nuevo.Comentario = "No existe comentario";
                nuevo.Estado = true;
                nuevo.UsuarioCreacion = miUsuario.IdUsuario;
                nuevo.FechaCreacion = DateTime.Now;
                return View(nuevo);
            }
            else
            {
                if (id != null)
                {
                    MovimientoDTO obj = objBL.getMovimiento((int)id);
                    if (obj == null) return RedirectToAction("Libro", "Admin", new { id = objLibro.IdCuentaBancaria });
                    if (obj.IdCuentaBancaria != objLibro.IdCuentaBancaria) return RedirectToAction("Libro", "Admin", new { id = objLibro.IdCuentaBancaria });

                    CuentaBancariaDTO objLibroMov = objCuentaBL.getCuentaBancariaEnEmpresa(miUsuario.IdEmpresa, obj.IdCuentaBancaria);
                    if (objLibroMov == null) return RedirectToAction("Index", "Admin");
                    if (objLibroMov.IdEmpresa != miUsuario.IdEmpresa) return RedirectToAction("Index", "Admin");

                    obj.UsuarioCreacion = miUsuario.IdUsuario;
                    ViewBag.NombreCategoria = objBL.getNombreCategoria(obj.IdCategoria.GetValueOrDefault());
                    return View(obj);
                }
            }
            return View();
        }
        public ActionResult AddMovimiento(MovimientoDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Libro", new { id = dto.IdCuentaBancaria }); }
            try
            {
                MovimientoBL objBL = new MovimientoBL();
                MovimientoDTO dtoAnterior = objBL.getMovimiento(dto.IdMovimiento);
                if (dto.IdComprobante != null && dto.cmpMontoPendiente != null)
                {
                    if (Decimal.Round(dto.cmpMontoPendiente.GetValueOrDefault(), 0) < 0)
                    {
                        createResponseMessage(CONSTANTES.ERROR, "<strong>Error.</strong> No se puede pagar un monto mayor al monto pendiente");
                        if (dto.IdComprobante == dtoAnterior.IdComprobante)
                        { dto.Monto = dto.IdMovimiento != 0 ? dtoAnterior.Monto : 0; }
                        else { dto.Monto = 0; }
                        TempData["Movimiento"] = dto;
                        return RedirectToAction("Movimiento", new { id = 0, idLibro = dto.IdCuentaBancaria });
                    }
                }
                if (dto.IdMovimiento == 0)
                {
                    if (objBL.add(dto))
                    {
                        if (dto.IdComprobante.GetValueOrDefault() != 0)
                        {
                            if (Decimal.Round(dto.cmpMontoPendiente.GetValueOrDefault(), 0) == 0) { ActualizarEjecucionComprobante(dto.IdComprobante.GetValueOrDefault(), true, dto.Fecha); }
                            else { ActualizarEjecucionComprobante(dto.IdComprobante.GetValueOrDefault(), dto.cmpCancelado, dto.Fecha); }
                        }
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Libro", new { id = dto.IdCuentaBancaria, page = TempData["PagMovs"] });
                    }
                }
                else if (dto.IdMovimiento != 0)
                {
                    if (objBL.update(dto))
                    {
                        if (dto.IdComprobante.GetValueOrDefault() != 0)
                        {
                            if (Decimal.Round(dto.cmpMontoPendiente.GetValueOrDefault(), 0) == 0) { ActualizarEjecucionComprobante(dto.IdComprobante.GetValueOrDefault(), true, dto.Fecha); }
                            else { ActualizarEjecucionComprobante(dto.IdComprobante.GetValueOrDefault(), dto.cmpCancelado, dto.Fecha); }
                        }
                        //Si en la actualizacion se cambio el IdComprobante
                        if (dtoAnterior.IdComprobante != null && dtoAnterior.IdComprobante != dto.IdComprobante)
                        { ActualizarEjecucionComprobante(dtoAnterior.IdComprobante.GetValueOrDefault(), false, dto.Fecha); }

                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Libro", new { id = dto.IdCuentaBancaria, page = TempData["PagMovs"] });
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdMovimiento != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Movimiento"] = dto;
            return RedirectToAction("Movimiento", "Admin", new { id = 0, idLibro = dto.IdCuentaBancaria });
        }

        [HttpPost]
        public ActionResult DeleteMovimiento(int id, int idCuentaBancaria)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Libro", "Admin", new { id = idCuentaBancaria }); }

            try
            {
                MovimientoBL objBL = new MovimientoBL();
                if (objBL.delete(id))
                {
                    createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_DELETE);
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_DELETE);
                }
            }
            catch (Exception e)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_NO_DELETE);
                throw;
            }
            return RedirectToAction("Libro", "Admin", new { id = idCuentaBancaria });
        }
        public ActionResult Usuarios(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Usuarios";
            MenuNavBarSelected(9);

            UsuarioDTO currentUser = getCurrentUser();
            UsuariosBL usuariosBL = new UsuariosBL();
            List<UsuarioDTO> listaUsuarios = new List<UsuarioDTO>();
            ViewBag.vbInactivos = inactivos;

            if (currentUser.IdEmpresa > 0)
            {
                if (!inactivos)
                { listaUsuarios = usuariosBL.getUsuariosActivosEnEmpresa(currentUser.IdEmpresa, currentUser.IdRol); }
                else
                { listaUsuarios = usuariosBL.getUsuariosEnEmpresa(currentUser.IdEmpresa, currentUser.IdRol); }
            }

            return View(listaUsuarios);
        }
        public ActionResult Usuario(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Usuario";
            MenuNavBarSelected(9);

            UsuarioDTO currentUser = getCurrentUser();
            UsuariosBL usuariosBL = new UsuariosBL();

            if (!this.isAdministrator() && id != currentUser.IdUsuario) { return RedirectToAction("Index"); }
            if (!this.isSuperAdministrator() && usuariosBL.getUsuario(id.GetValueOrDefault()).IdRol == 1) { return RedirectToAction("Usuarios"); }

            //ViewBag.vbRls = usuariosBL.getAllRolesViewBag(false);
            ViewBag.vbRls = usuariosBL.getRolesDown(currentUser.IdRol);

            var objSent = TempData["Usuario"];
            if (objSent != null) { TempData["Usuario"] = null; return View(objSent); }
            UsuarioDTO usuario;
            if (id != null)
            {
                usuario = usuariosBL.getUsuarioEnEmpresa(currentUser.IdEmpresa, id.GetValueOrDefault());
                if (usuario == null) return RedirectToAction("Usuarios");
                if (usuario.IdEmpresa != currentUser.IdEmpresa) return RedirectToAction("Usuarios");

                return View(usuario);
            }
            usuario = new UsuarioDTO();
            usuario.IdEmpresa = currentUser.IdEmpresa;
            return View(usuario);
        }
        [HttpPost]
        public ActionResult AddUser(UsuarioDTO user, string passUser = "", string passChange = "")
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            UsuarioDTO currentUser = getCurrentUser();
            if (!this.isAdministrator() && user.IdUsuario != currentUser.IdUsuario) { return RedirectToAction("Index"); }
            if (user.IdRol == 1 && !this.isSuperAdministrator()) { return RedirectToAction("Index"); }
            try
            {
                UsuariosBL usuariosBL = new UsuariosBL();

                if (user.IdUsuario == 0 && usuariosBL.validateUsuario(user))
                {
                    if (!this.isSuperAdministrator()) { return RedirectToAction("Index"); }

                    usuariosBL.add(user);
                    createResponseMessage(CONSTANTES.SUCCESS);
                    return RedirectToAction("Usuarios");
                }
                else if (user.IdUsuario != 0 && usuariosBL.validateUsuarioNoDuplicado(user))
                {
                    if (usuariosBL.update(user, passUser, passChange, this.getCurrentUser()))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        if (user.IdUsuario == this.getCurrentUser().IdUsuario)
                        {
                            System.Web.HttpContext.Current.Session["User"] = usuariosBL.getUsuario(user.IdUsuario);
                            if (!this.getCurrentUser().Active) System.Web.HttpContext.Current.Session["User"] = null;
                        }
                        return RedirectToAction("Usuarios");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE + "<br>Si está intentando actualizar la contraseña, verifique que ha ingresado la contraseña actual correctamente.");
                        TempData["Usuario"] = user;
                        return RedirectToAction("Usuario", new { id = user.IdUsuario });
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_DUPLICATE_USER);
                }
            }
            catch
            {
                if (user.IdUsuario != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_ACCOUNT);
            }
            TempData["Usuario"] = user;
            return RedirectToAction("Usuario");
        }
        public ActionResult Entidades(int? idTipoEntidad = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            //if (!isAdministrator()) { return RedirectToAction("Index"); }
            //Solo usuario Externo no tiene acceso a esto
            if (isUsuarioExterno()) { return RedirectToAction("Index"); }

            ViewBag.Title = "Entidades";
            MenuNavBarSelected(4, 3);
            UsuarioDTO currentUser = getCurrentUser();

            EntidadResponsableBL objBL = new EntidadResponsableBL();
            ViewBag.idTipoEntidad = idTipoEntidad;
            List<EntidadResponsableDTO> listaEntidades = new List<EntidadResponsableDTO>();
            ViewBag.lstTipoEntidades = objBL.getTipoDeEntidades();

            if (currentUser.IdEmpresa > 0)
            {
                listaEntidades = objBL.getEntidadResponsablesEnEmpresa(currentUser.IdEmpresa);
            }
            return View(listaEntidades);
        }
        public ActionResult EntidadesClientes(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (isUsuarioExterno()) { return RedirectToAction("Index"); }

            ViewBag.Title = "Clientes";
            MenuNavBarSelected(6, 0);

            UsuarioDTO user = getCurrentUser();

            EntidadResponsableBL objBL = new EntidadResponsableBL();
            List<EntidadResponsableDTO> lista = new List<EntidadResponsableDTO>();

            ViewBag.vbInactivos = inactivos;

            if (user.IdEmpresa > 0)
            { 
                if (!inactivos)
                { lista = objBL.getEntidadesResponsablesActivasPorTipoEnEmpresa(user.IdEmpresa, 1); }
                else
                { lista = objBL.getEntidadesResponsablesPorTipoEnEmpresa(user.IdEmpresa, 1); }
            }

            return View(lista);
        }
        public ActionResult EntidadesProveedores(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (isUsuarioExterno()) { return RedirectToAction("Index"); }

            ViewBag.Title = "Proveedores";
            MenuNavBarSelected(6, 1);

            UsuarioDTO user = getCurrentUser();

            EntidadResponsableBL objBL = new EntidadResponsableBL();
            List<EntidadResponsableDTO> lista = new List<EntidadResponsableDTO>();

            ViewBag.vbInactivos = inactivos;

            if (user.IdEmpresa > 0)
            { 
                if (!inactivos)
                { lista = objBL.getEntidadesResponsablesActivasPorTipoEnEmpresa(user.IdEmpresa, 2); }
                else
                { lista = objBL.getEntidadesResponsablesPorTipoEnEmpresa(user.IdEmpresa, 2); }
            }

            return View(lista);
        }
        public ActionResult Entidad(int? id = null, int? idTipoEntidad = null, bool inactivosC = false, bool inactivosP = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (isUsuarioExterno()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Entidad";

            int tipoEntidad = 1;
            if (idTipoEntidad != null) { tipoEntidad = idTipoEntidad.GetValueOrDefault(); }
            MenuNavBarSelected(6, tipoEntidad - 1);

            UsuarioDTO user = getCurrentUser();

            EntidadResponsableBL objBL = new EntidadResponsableBL();
            ViewBag.TipoIdentificacion = objBL.getTiposDeIdentificaciones();
            ViewBag.lstComprobantes = objBL.getComprobantes_ConEntidad(user.IdEmpresa, id.GetValueOrDefault());

            ResponsableBL resBL = new ResponsableBL();
            ViewBag.lstResponsables = resBL.getResponsablesActivosEnEmpresa(user.IdEmpresa);

            ViewBag.vbInactivosC = inactivosC;
            ViewBag.vbInactivosP = inactivosP;

            var objSent = TempData["Entidad"];
            if (objSent != null) { TempData["Entidad"] = null; return View(objSent); }

            EntidadResponsableDTO obj;
            if (id != null && id != 0)
            {
                //obj = objBL.getEntidadResponsableEnEmpresa((int)user.IdEmpresa, (int)id);
                obj = objBL.getEntidadResponsableEnEmpresa_Only((int)user.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Entidades");
                if (obj.IdEmpresa != user.IdEmpresa) return RedirectToAction("Entidades");
                //Contactos
                if (!inactivosC) { ViewBag.lstContactos = objBL.getContactosActivos_EnEntidadResponsable((int)obj.IdEntidadResponsable); }
                else { ViewBag.lstContactos = objBL.getContactos_EntidadResponsableEnEmpresa((int)obj.IdEntidadResponsable); }
                //Proyectos
                if (!inactivosP) { ViewBag.lstProyectos = objBL.getProyectosActivos_EntidadResponsableEnEmpresa((int)obj.IdEntidadResponsable); }
                else { ViewBag.lstProyectos = objBL.getProyectos_EntidadResponsableEnEmpresa((int)obj.IdEntidadResponsable); }

                return View(obj);
            }
            obj = new EntidadResponsableDTO();
            obj.IdEntidadResponsable = 0;
            obj.TipoPersona = 1;
            obj.IdEmpresa = user.IdEmpresa;
            if (idTipoEntidad != null && idTipoEntidad != 0) obj.IdTipoEntidad = idTipoEntidad;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddEntidad(EntidadResponsableDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                string sTipoEntidad = dto.IdTipoEntidad == 1 ? "Clientes" : "Proveedores";
                int TipoEntidad = 1; //Por defecto tipo de comprobante Ingreso
                if (dto != null) { TipoEntidad = dto.IdTipoEntidad.GetValueOrDefault(); }
                EntidadResponsableBL objBL = new EntidadResponsableBL();
                if (dto.IdEntidadResponsable == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Entidades" + sTipoEntidad, "Admin");
                    }
                }
                else if (dto.IdEntidadResponsable != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Entidades" + sTipoEntidad, "Admin");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdEntidadResponsable != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Entidad"] = dto;
            return RedirectToAction("Entidad");
        }
        public ActionResult Comprobantes(int? idTipoComprobante = null, string sortOrder = null, string currentFilter = null, string searchString = null, int? page = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Comprobantes";
            MenuNavBarSelected(2);

            UsuarioDTO currentUser = getCurrentUser();

            ComprobanteBL objBL = new ComprobanteBL();
            List<ComprobanteDTO> listaIngresos = new List<ComprobanteDTO>();
            List<ComprobanteDTO> listaEgresos = new List<ComprobanteDTO>();
            ViewBag.lstTipoComprobantes = objBL.getTipoDeComprobantes();
            ViewBag.idTipoComprobante = idTipoComprobante;

            if (currentUser.IdEmpresa > 0)
            {
                listaIngresos = objBL.getComprobantesEnEmpresaPorTipo(currentUser.IdEmpresa, 1);
                listaEgresos = objBL.getComprobantesEnEmpresaPorTipo(currentUser.IdEmpresa, 2);
                List<IPagedList<ComprobanteDTO>> matrix = new List<IPagedList<ComprobanteDTO>>();
                matrix.Add(BusquedaYPaginado_Comprobantes(listaIngresos, sortOrder, currentFilter, searchString, page));
                matrix.Add(BusquedaYPaginado_Comprobantes(listaEgresos, sortOrder, currentFilter, searchString, page));
                return View(matrix);
            }
            return View();
        }
        public ActionResult ComprobantesIngreso()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Comprobantes de Ingreso";

            MenuNavBarSelected(3, 0);

            UsuarioDTO user = getCurrentUser();

            ComprobanteBL objBL = new ComprobanteBL();
            int tipo = 1; //Ingresos
            ViewBag.idTipoComprobante = tipo;

            if (user.IdEmpresa > 0)
            {
                List<ComprobanteDTO> lista = objBL.getComprobantesEnEmpresaPorTipo(user.IdEmpresa, tipo);
                return View(lista);
            }
            return View();
        }
        public ActionResult ComprobantesEgreso()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Comprobantes de Egreso";

            MenuNavBarSelected(3, 1);

            UsuarioDTO user = getCurrentUser();

            ComprobanteBL objBL = new ComprobanteBL();
            int tipo = 2; //Egresos
            ViewBag.idTipoComprobante = tipo;

            if (user.IdEmpresa > 0)
            {
                List<ComprobanteDTO> lista = objBL.getComprobantesEnEmpresaPorTipo(user.IdEmpresa, tipo);
                return View(lista);
            }
            return View();
        }
        public ActionResult ComprobantesAnulados()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Comprobantes Anulados";

            MenuNavBarSelected(3, 2);

            UsuarioDTO user = getCurrentUser();

            ComprobanteBL objBL = new ComprobanteBL();
            //int tipo = 3; //Anulados
            //ViewBag.idTipoComprobante = tipo;

            if (user.IdEmpresa > 0)
            {
                List<ComprobanteDTO> lista = objBL.getComprobantesEnEmpresaPorTipo(user.IdEmpresa, 3);
                List<ComprobanteDTO> lista2 = objBL.getComprobantesEnEmpresaPorTipo(user.IdEmpresa, 4);
                lista.AddRange(lista2);
                return View(lista);
            }
            return View();
        }
        private IPagedList<ComprobanteDTO> BusquedaYPaginado_Comprobantes(IList<ComprobanteDTO> lista, string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (!String.IsNullOrEmpty(searchString))
            { searchString = searchString.ToLower(); }
            ViewBag.CurrentSort = sortOrder;

            ViewBag.vbFecha = sortOrder == "Fecha" ? "Fecha_desc" : "Fecha";
            ViewBag.vbDocumento = sortOrder == "Documento" ? "Documento_desc" : "Documento";
            ViewBag.vbNumero = sortOrder == "Numero" ? "Numero_desc" : "Numero";
            ViewBag.vbEntidad = sortOrder == "Entidad" ? "Entidad_desc" : "Entidad";
            ViewBag.vbProyecto = sortOrder == "Proyecto" ? "Proyecto_desc" : "Proyecto";
            ViewBag.vbMontoSinIGV = sortOrder == "MontoSinIGV" ? "MontoSinIGV_desc" : "MontoSinIGV";
            ViewBag.vbCategoria = sortOrder == "Categoria" ? "Categoria_desc" : "Categoria";
            ViewBag.vbFechaFin = sortOrder == "FechaFin" ? "FechaFin_desc" : "FechaFin";
            ViewBag.vbUsuario = sortOrder == "Usuario" ? "Usuario_desc" : "Usuario";
            ViewBag.vbEstado = sortOrder == "Estado" ? "Estado_desc" : "Estado";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            string tipoDato = "cadena";
            DateTime pTiempo;
            if (DateTime.TryParse(searchString, out pTiempo))
            {
                tipoDato = "tiempo";
                pTiempo = Convert.ToDateTime(searchString);
            }

            Decimal pDecimal;
            if (Decimal.TryParse(searchString, out pDecimal))
            {
                tipoDato = "numerico";
                pDecimal = Convert.ToDecimal(searchString);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                IList<ComprobanteDTO> listaP;
                listaP = lista.Where(s => (s.NombreTipoDocumento.ToLower() ?? "").Contains(searchString)
                        || (s.NombreCategoria.ToLower() ?? "").Contains(searchString)
                        || (s.NroDocumento.ToLower() ?? "").Contains(searchString)
                        || (s.NombreEntidad.ToLower() ?? "").Contains(searchString)
                        || (s.NombreUsuario.ToLower() ?? "").Contains(searchString)
                        || (s.NombreProyecto.ToLower() ?? "").Contains(searchString)
                        ).ToList();

                switch (tipoDato)
                {
                    case "tiempo":
                        lista = lista.Where(s => DateTime.Compare(s.FechaEmision, pTiempo) <= 0 || DateTime.Compare(s.FechaConclusion.GetValueOrDefault(), pTiempo) <= 0).ToList();
                        lista = lista.Union(listaP).ToList();
                        break;
                    case "numerico":
                        lista = lista.Where(s => s.MontoSinIGV.ToString().Contains(pDecimal.ToString())).ToList();
                        lista = lista.Union(listaP).ToList();
                        break;
                    default:
                        lista = listaP;
                        break;
                }
            }

            switch (sortOrder)
            {
                case "Documento":
                    lista = lista.OrderBy(s => s.NombreTipoDocumento).ToList();
                    break;
                case "Numero":
                    lista = lista.OrderBy(s => s.NroDocumento).ToList();
                    break;
                case "Categoria":
                    lista = lista.OrderBy(s => s.NombreCategoria).ToList();
                    break;
                case "Entidad":
                    lista = lista.OrderBy(s => s.NombreEntidad).ToList();
                    break;
                case "MontoSinIGV":
                    lista = lista.OrderBy(s => s.MontoSinIGV).ToList();
                    break;
                case "Usuario":
                    lista = lista.OrderBy(s => s.NombreUsuario).ToList();
                    break;
                case "Fecha":
                    lista = lista.OrderBy(s => s.FechaEmision).ToList();
                    break;
                case "FechaFin":
                    lista = lista.OrderBy(s => s.FechaConclusion).ToList();
                    break;
                case "Estado":
                    lista = lista.OrderBy(s => s.Ejecutado).ToList();
                    break;
                case "Documento_desc":
                    lista = lista.OrderByDescending(s => s.NombreTipoDocumento).ToList();
                    break;
                case "Numero_desc":
                    lista = lista.OrderByDescending(s => s.NroDocumento).ToList();
                    break;
                case "Categoria_desc":
                    lista = lista.OrderByDescending(s => s.NombreCategoria).ToList();
                    break;
                case "Entidad_desc":
                    lista = lista.OrderByDescending(s => s.NombreEntidad).ToList();
                    break;
                case "MontoSinIGV_desc":
                    lista = lista.OrderByDescending(s => s.MontoSinIGV).ToList();
                    break;
                case "Usuario_desc":
                    lista = lista.OrderByDescending(s => s.NombreUsuario).ToList();
                    break;
                case "Fecha_desc":
                    lista = lista.OrderByDescending(s => s.FechaEmision).ToList();
                    break;
                case "FechaFin_desc":
                    lista = lista.OrderByDescending(s => s.FechaConclusion).ToList();
                    break;
                case "Estado_desc":
                    lista = lista.OrderByDescending(s => s.Ejecutado).ToList();
                    break;
                default:
                    lista = lista.OrderByDescending(s => s.FechaEmision).ToList();
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            return lista.ToPagedList(pageNumber, pageSize);
        }
        public ActionResult Comprobante(int? id = null, int? idTipoComprobante = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Comprobante";

            int tipoComprobante = 1;
            if (idTipoComprobante != null) { tipoComprobante = idTipoComprobante.GetValueOrDefault(); }
            MenuNavBarSelected(3, tipoComprobante - 1);
            //MenuNavBarSelected(2);
            UsuarioDTO currentUser = getCurrentUser();
            EmpresaDTO empresa = (new EmpresaBL()).getEmpresa(currentUser.IdEmpresa);

            ComprobanteBL objBL = new ComprobanteBL();
            ViewBag.lstTipoDocumento = objBL.getTipoDeDocumentos();
            ViewBag.lstClientes = objBL.getListaClientesEnEmpresa(currentUser.IdEmpresa);
            ViewBag.lstProveedores = objBL.getListaProveedoresEnEmpresa(currentUser.IdEmpresa);
            ViewBag.lstMonedas = objBL.getListaMonedas();
            ViewBag.lstAreas = objBL.getListaAreasEnEmpresa(currentUser.IdEmpresa, false);
            ViewBag.lstResponsables = objBL.getListaResponsablesEnEmpresa(currentUser.IdEmpresa);
            ViewBag.lstHonorarios = objBL.getListaHonorariosEnEmpresa(currentUser.IdEmpresa);
            ViewBag.Proyectos = new List<ProyectoDTO>();
            ViewBag.Categorias = CategoriasBucle(empresa.IdEmpresa, (int)empresa.IdPeriodo, null, null);

            //Lista de Movimientos Asociados al comprobante
            ViewBag.lstMovimientos = objBL.getMovimientos_AsocComprobante(empresa.IdEmpresa, id.GetValueOrDefault());

            var objSent = TempData["Comprobante"];
            if (objSent != null) { TempData["Comprobante"] = null; return View(objSent); }

            ComprobanteDTO obj;
            if (id != null && id != 0)
            {
                obj = objBL.getComprobanteEnEmpresa((int)currentUser.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Comprobantes");
                if (obj.IdEmpresa != currentUser.IdEmpresa) return RedirectToAction("Comprobantes");
                obj.UsuarioCreacion = currentUser.IdUsuario;
                ViewBag.Montos = obj.lstMontos;

                //Movimientos asociados
                MovimientoBL objMov = new MovimientoBL();
                ViewBag.Movimientos = objMov.getMovimientos_Asoc_Comprobante(obj.IdComprobante);

                return View(obj);
            }
            obj = new ComprobanteDTO();
            obj.IdEmpresa = currentUser.IdEmpresa;
            obj.TipoCambio = (new EmpresaBL()).getEmpresa(currentUser.IdEmpresa).TipoCambio;
            obj.UsuarioCreacion = currentUser.IdUsuario;
            obj.FechaEmision = DateTime.Now;

            if (idTipoComprobante != null && idTipoComprobante != 0) obj.IdTipoComprobante = idTipoComprobante.GetValueOrDefault();
            return View(obj);
        }
        [HttpPost]
        public ActionResult AddComprobante(ComprobanteDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Comprobantes", "Admin"); }
            try
            {
                string sTipoComprobante = dto.IdTipoComprobante == 1 ? "Ingreso" : "Egreso";
                int TipoComprobante = 1; //Por defecto tipo de comprobante Ingreso
                if (dto != null)
                {
                    TipoComprobante = dto.IdTipoComprobante;
                    dto.lstMontos = (List<AreaPorComprobanteDTO>)TempData["AreasXMontos"] ?? new List<AreaPorComprobanteDTO>();
                }

                ComprobanteBL objBL = new ComprobanteBL();
                int nDocRepetido = objBL.repeatedNroDocumento(dto.IdEmpresa, dto.IdComprobante, dto.NroDocumento);

                if (dto.IdComprobante == 0)
                {
                    //Si el numero de documento se repite en los ingresos no permitir el registro
                    if (nDocRepetido > 0 && dto.IdTipoComprobante == 1)
                    {
                        switch (nDocRepetido)
                        {
                            case 1:
                                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_DOCUMENTO_INGRESO_REPETIDO_1);
                                break;
                            case 3:
                                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_DOCUMENTO_INGRESO_REPETIDO_3);
                                break;
                        }
                    }
                    else if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Comprobantes" + sTipoComprobante, "Admin");
                    }
                }
                else if (dto.IdComprobante != 0)
                {
                    //Si el numero de documento se repite en los ingresos no permitir el registro
                    if (nDocRepetido > 0 && dto.IdTipoComprobante == 1)
                    {
                        switch (nDocRepetido)
                        {
                            case 1:
                                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_DOCUMENTO_INGRESO_REPETIDO_1);
                                break;
                            case 3:
                                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_DOCUMENTO_INGRESO_REPETIDO_3);
                                break;
                        }
                    }
                    else if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Comprobantes" + sTipoComprobante, "Admin");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdComprobante != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Comprobante"] = dto;
            return RedirectToAction("Comprobante", "Admin", new { id = dto.IdComprobante, idTipoComprobante = dto.IdTipoComprobante });
        }
        [HttpPost]
        public ActionResult DeleteComprobante(int id)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Comprobantes", "Admin"); }

            ComprobanteDTO dto;
            try
            {
                ComprobanteBL objBL = new ComprobanteBL();
                dto = objBL.getComprobanteEnEmpresa(getCurrentUser().IdEmpresa, id);
                if (objBL.delete(id))
                {
                    createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_DELETE);
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_DELETE);
                }
            }
            catch (Exception e)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_NO_DELETE);
                throw;
            }
            string cadena = "Ingreso";
            if (dto != null) { cadena = dto.IdTipoComprobante == 1 ? "Ingreso" : "Egreso"; }
            return RedirectToAction("Comprobantes" + cadena, "Admin");
        }

        [HttpPost]
        public ActionResult DeleteCategoria(int id)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Categorias", "Admin"); }

            return View();
        }

        [HttpPost]
        public ActionResult AnularComprobante(int id, string comentario)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Comprobantes", "Admin"); }

            ComprobanteDTO dto;
            try
            {
                ComprobanteBL objBL = new ComprobanteBL();
                dto = objBL.getComprobanteEnEmpresa(getCurrentUser().IdEmpresa, id);
                if (objBL.ban(id, comentario))
                {
                    createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_BAN);
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_BAN);
                }
            }
            catch (Exception e)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_NO_BAN);
                throw;
            }
            string cadena = "Ingreso";
            if (dto != null) { cadena = dto.IdTipoComprobante == 1 ? "Ingreso" : "Egreso"; }
            return RedirectToAction("Comprobantes" + cadena, "Admin");
        }
        [HttpPost]
        public ActionResult RestablecerComprobante(int id)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Comprobantes", "Admin"); }

            ComprobanteDTO dto;
            try
            {
                ComprobanteBL objBL = new ComprobanteBL();
                dto = objBL.getComprobanteEnEmpresa(getCurrentUser().IdEmpresa, id);
                if (objBL.unban(id))
                {
                    createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_UNBAN);
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UNBAN);
                }
            }
            catch (Exception e)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_NO_UNBAN);
                throw;
            }
            return RedirectToAction("ComprobantesAnulados", "Admin");
        }
        public ActionResult InventariosIngreso()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Inventarios de Ingreso";

            MenuNavBarSelected(9, 0);

            UsuarioDTO user = getCurrentUser();

            MovimientoInvBL objBL = new MovimientoInvBL();
            int tipo = 1;
            ViewBag.idTipoInventario = tipo;

            if (user.IdEmpresa > 0)
            {
                List<MovimientoInvDTO> lista = objBL.getMovimientoInvsEnEmpresaPorTipo(user.IdEmpresa, tipo);
                return View(lista);
            }
            return View();
        }
        public ActionResult InventariosEgreso()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Inventarios de Egreso";

            MenuNavBarSelected(9, 1);

            UsuarioDTO user = getCurrentUser();

            MovimientoInvBL objBL = new MovimientoInvBL();
            int tipo = 2;
            ViewBag.idTipoInventario = tipo;

            if (user.IdEmpresa > 0)
            {
                List<MovimientoInvDTO> lista = objBL.getMovimientoInvsEnEmpresaPorTipo(user.IdEmpresa, tipo);
                return View(lista);
            }
            return View();
        }
        public ActionResult MovimientoInv(int? id = null, int? idTipo = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ViewBag.Title = "Movimiento de Inventario";

            int tipo = 1;
            if (idTipo != null) { tipo = idTipo.GetValueOrDefault(); }
            MenuNavBarSelected(9, tipo - 1);

            UsuarioDTO user = getCurrentUser();

            MovimientoInvBL objBL = new MovimientoInvBL();

            ViewBag.lstTipoMovimientoInv = objBL.getTipoMovimientoInv();
            ViewBag.lstFormaMovimiento = objBL.getFormaMovimientoInvPorTipo(tipo);
            //ViewBag.lstItems = objBL.getItemsEnEmpresa(user.IdEmpresa);
            ViewBag.lstItems = objBL.getItemsEnEmpresa_PorTipoMov(user.IdEmpresa, (int)idTipo);
            ViewBag.lstProveedores = objBL.getProveedoresEnEmpresa(user.IdEmpresa);
            //Lotes de salida Lista
            ViewBag.lstLotes = objBL.getLotesEnEmpresa(user.IdEmpresa);
            //Ubicaciones
            ViewBag.lstUbicaciones = objBL.getUbicacionesEnEmpresa(user.IdEmpresa);

            var objSent = TempData["MovimientoInv"];
            if (objSent != null) { TempData["MovimientoInv"] = null; return View(objSent); }

            MovimientoInvDTO obj;
            if (id != null && id != 0)
            {
                obj = objBL.getMovimientoInvEnEmpresa((int)user.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("MovimientoInvs");
                if (obj.IdEmpresa != user.IdEmpresa) return RedirectToAction("MovimientoInvs");
                obj.UsuarioCreacion = user.IdUsuario;

                //lstLotes.Add(new Select2DTO_B() { text = obj.SerieLote });
                ViewBag.lstLotes = objBL.getLotesEnEmpresa(user.IdEmpresa, obj.IdItem, obj.SerieLote);
                
                if (idTipo.GetValueOrDefault() == 1) { ViewBag.lstUbicaciones = objBL.getUbicacionesEnEmpresa(user.IdEmpresa); }
                else if (idTipo.GetValueOrDefault() == 2) { ViewBag.lstUbicaciones = objBL.getUbicacionesEnEmpresa(user.IdEmpresa, obj.SerieLote); }

                return View(obj);
            }
            obj = new MovimientoInvDTO();
            obj.IdTipoMovimientoInv = tipo;
            obj.IdEmpresa = user.IdEmpresa;
            obj.UsuarioCreacion = user.IdUsuario;
            obj.FechaInicial = DateTime.Now;

            //if (idTipo.GetValueOrDefault() == 1) { ViewBag.lstUbicaciones = objBL.getUbicacionesEnEmpresa(user.IdEmpresa); }
            //else if (idTipo.GetValueOrDefault() == 2) { ViewBag.lstUbicaciones = new List<Select2DTO_B>(); }

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddMovimientoInv(MovimientoInvDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("MovimientoInvs", "Admin"); }
            try
            {
                string sTipoMovimientoInv = dto.IdTipoMovimientoInv == 1 ? "Ingreso" : "Egreso";

                MovimientoInvBL objBL = new MovimientoInvBL();
                if (dto.IdMovimientoInv == 0)
                {
                    //Verificar que el stock no sea negativo
                    if(dto.IdTipoMovimientoInv == 2)
                    {
                        int sumaLote = objBL.get_Stock_De_Lote_En_Empresa(dto.IdEmpresa, dto.SerieLote) + (dto.Cantidad * -1);
                        if (sumaLote < 0)
                        {
                            createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_ITEMS_LIMIT);
                            return RedirectToAction("Inventarios" + sTipoMovimientoInv, "Admin");
                        }
                        else if (objBL.add(dto))
                        {
                            createResponseMessage(CONSTANTES.SUCCESS);
                            return RedirectToAction("Inventarios" + sTipoMovimientoInv, "Admin");
                        }
                    } else if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Inventarios" + sTipoMovimientoInv, "Admin");
                    }
                }
                else if (dto.IdMovimientoInv != 0)
                {
                    if (dto.IdTipoMovimientoInv == 2)
                    {
                        int sumaLote = objBL.get_Stock_De_Lote_En_Empresa(dto.IdEmpresa, dto.SerieLote) + (dto.Cantidad * -1);
                        if (sumaLote < 0)
                        {
                            createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_ITEMS_LIMIT);
                            return RedirectToAction("Inventarios" + sTipoMovimientoInv, "Admin");
                        }
                        else if (objBL.update(dto))
                        {
                            createResponseMessage(CONSTANTES.SUCCESS);
                            return RedirectToAction("Inventarios" + sTipoMovimientoInv, "Admin");
                        }
                    } else if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Inventarios" + sTipoMovimientoInv, "Admin");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdMovimientoInv != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["MovimientoInv"] = dto;
            return RedirectToAction("MovimientoInv");
        }
        [HttpPost]
        public ActionResult DeleteMovimientoInv(int id)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("MovimientoInvs", "Admin"); }

            MovimientoInvDTO dto;
            try
            {
                MovimientoInvBL objBL = new MovimientoInvBL();
                dto = objBL.getMovimientoInvEnEmpresa(getCurrentUser().IdEmpresa, id);
                if (objBL.delete(id))
                {
                    createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_DELETE);
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_DELETE);
                }
            }
            catch (Exception e)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_NO_DELETE);
                throw;
            }
            string cadena = "Ingreso";
            if (dto != null) { cadena = dto.IdTipoMovimientoInv == 1 ? "Ingreso" : "Egreso"; }
            return RedirectToAction("Inventarios" + cadena, "Admin");
        }
        public ActionResult Items(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Items";
            MenuNavBarSelected(10, 0);
            UsuarioDTO user = getCurrentUser();

            ItemBL objBL = new ItemBL();
            List<ItemDTO> listaItems = new List<ItemDTO>();

            ViewBag.vbInactivos = inactivos;

            if (user.IdEmpresa > 0)
            {
                if (!inactivos)
                { listaItems = objBL.getItemsActivosEnEmpresa(user.IdEmpresa); }
                else
                { listaItems = objBL.getItemsEnEmpresa(user.IdEmpresa); }
            }
            return View(listaItems);
        }
        public ActionResult Item(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Item";
            MenuNavBarSelected(10, 0);

            UsuarioDTO user = getCurrentUser();

            ItemBL objBL = new ItemBL();
            ViewBag.lstCategoriaItm = objBL.getCategoriasEnEmpresa(user.IdEmpresa);

            MonedaBL monedaBL = new MonedaBL();
            ViewBag.lstMonedas = monedaBL.getListaMonedas();

            var objSent = TempData["Item"];
            if (objSent != null) { TempData["Item"] = null; return View(objSent); }

            ItemDTO obj;
            if (id != null && id != 0)
            {
                obj = objBL.getItemEnEmpresa((int)user.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Items");
                if (obj.IdEmpresa != user.IdEmpresa) return RedirectToAction("Items");
                return View(obj);
            }
            obj = new ItemDTO();
            obj.IdEmpresa = user.IdEmpresa;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddItem(ItemDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            try
            {
                ItemBL objBL = new ItemBL();
                if (dto.IdItem == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Items");
                    }
                }
                else if (dto.IdItem != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Items");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdItem != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Item"] = dto;
            return RedirectToAction("Item");
        }
        public ActionResult Servicios(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Servicios";
            MenuNavBarSelected(11);
            UsuarioDTO user = getCurrentUser();

            ServicioBL objBL = new ServicioBL();
            List<ServicioDTO> listaServicios = new List<ServicioDTO>();

            ViewBag.vbInactivos = inactivos;

            if (user.IdEmpresa > 0)
            {
                if (!inactivos)
                { listaServicios = objBL.getServiciosActivosEnEmpresa(user.IdEmpresa); }
                else
                { listaServicios = objBL.getServiciosEnEmpresa(user.IdEmpresa); }
            }
            return View(listaServicios);
        }
        public ActionResult Servicio(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Servicio";
            MenuNavBarSelected(11);

            UsuarioDTO user = getCurrentUser();

            ServicioBL objBL = new ServicioBL();

            MonedaBL monedaBL = new MonedaBL();
            ViewBag.lstMonedas = monedaBL.getListaMonedas();

            var objSent = TempData["Servicio"];
            if (objSent != null) { TempData["Servicio"] = null; return View(objSent); }

            ServicioDTO obj;
            if (id != null && id != 0)
            {
                obj = objBL.getServicioEnEmpresa((int)user.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Servicios");
                if (obj.IdEmpresa != user.IdEmpresa) return RedirectToAction("Servicios");
                return View(obj);
            }
            obj = new ServicioDTO();
            obj.IdEmpresa = user.IdEmpresa;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddServicio(ServicioDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            try
            {
                ServicioBL objBL = new ServicioBL();
                if (dto.IdServicio == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Servicios");
                    }
                }
                else if (dto.IdServicio != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Servicios");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdServicio != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Servicio"] = dto;
            return RedirectToAction("Servicio");
        }
        public ActionResult CategoriaItms(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Categorias de Items";
            MenuNavBarSelected(10, 1);
            UsuarioDTO user = getCurrentUser();

            CategoriaItmBL objBL = new CategoriaItmBL();
            List<CategoriaItmDTO> listaCategoriaItms = new List<CategoriaItmDTO>();
            ViewBag.vbInactivos = inactivos;

            if (user.IdEmpresa > 0)
            {
                listaCategoriaItms = objBL.getCategoriaItmsEnEmpresa(user.IdEmpresa);
                if (!inactivos)
                { listaCategoriaItms = objBL.getCategoriaItmsActivasEnEmpresa(user.IdEmpresa); }
                else
                { listaCategoriaItms = objBL.getCategoriaItmsEnEmpresa(user.IdEmpresa); }
            }
            return View(listaCategoriaItms);
        }
        public ActionResult CategoriaItm(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Categoría de Items";
            MenuNavBarSelected(10, 1);

            UsuarioDTO user = getCurrentUser();

            CategoriaItmBL objBL = new CategoriaItmBL();

            var objSent = TempData["CategoriaItm"];
            if (objSent != null) { TempData["CategoriaItm"] = null; return View(objSent); }

            CategoriaItmDTO obj;
            if (id != null && id != 0)
            {
                obj = objBL.getCategoriaItmEnEmpresa((int)user.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("CategoriaItms");
                if (obj.IdEmpresa != user.IdEmpresa) return RedirectToAction("CategoriaItms");
                return View(obj);
            }
            obj = new CategoriaItmDTO();
            obj.IdEmpresa = user.IdEmpresa;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddCategoriaItm(CategoriaItmDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            try
            {
                CategoriaItmBL objBL = new CategoriaItmBL();
                if (dto.IdCategoriaItm == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("CategoriaItms");
                    }
                }
                else if (dto.IdCategoriaItm != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("CategoriaItms");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdCategoriaItm != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["CategoriaItm"] = dto;
            return RedirectToAction("CategoriaItm");
        }
        public ActionResult Ubicaciones(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Ubicaciones";
            MenuNavBarSelected(10, 2);
            UsuarioDTO user = getCurrentUser();

            UbicacionBL objBL = new UbicacionBL();
            List<UbicacionDTO> listaUbicacions = new List<UbicacionDTO>();
            ViewBag.vbInactivos = inactivos;

            if (user.IdEmpresa > 0)
            {
                listaUbicacions = objBL.getUbicacionsEnEmpresa(user.IdEmpresa);
                if (!inactivos)
                { listaUbicacions = objBL.getUbicacionsActivasEnEmpresa(user.IdEmpresa); }
                else
                { listaUbicacions = objBL.getUbicacionsEnEmpresa(user.IdEmpresa); }
            }
            return View(listaUbicacions);
        }
        public ActionResult Ubicacion(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Ubicacion";
            MenuNavBarSelected(10, 2);

            UsuarioDTO currentUser = getCurrentUser();

            UbicacionBL objBL = new UbicacionBL();

            var objSent = TempData["Ubicacion"];
            if (objSent != null) { TempData["Ubicacion"] = null; return View(objSent); }

            UbicacionDTO obj;
            if (id != null && id != 0)
            {
                obj = objBL.getUbicacionEnEmpresa((int)currentUser.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Ubicaciones");
                if (obj.IdEmpresa != currentUser.IdEmpresa) return RedirectToAction("Ubicaciones");
                return View(obj);
            }
            obj = new UbicacionDTO();
            obj.IdEmpresa = currentUser.IdEmpresa;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddUbicacion(UbicacionDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            try
            {
                UbicacionBL objBL = new UbicacionBL();
                if (dto.IdUbicacion == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Ubicaciones");
                    }
                }
                else if (dto.IdUbicacion != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Ubicaciones");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdUbicacion != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Ubicacion"] = dto;
            return RedirectToAction("Ubicacion");
        }
        public ActionResult Areas(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Areas";
            MenuNavBarSelected(4);
            UsuarioDTO currentUser = getCurrentUser();

            AreaBL objBL = new AreaBL();
            List<AreaDTO> listaAreas = new List<AreaDTO>();

            ViewBag.vbInactivos = inactivos;

            if (currentUser.IdEmpresa > 0)
            {
                if (!inactivos)
                { listaAreas = objBL.getAreasActivasEnEmpresa(currentUser.IdEmpresa); }
                else
                { listaAreas = objBL.getAreasEnEmpresa(currentUser.IdEmpresa); }
            }
            return View(listaAreas);
        }
        public ActionResult Area(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Área";
            MenuNavBarSelected(4);

            UsuarioDTO currentUser = getCurrentUser();

            AreaBL objBL = new AreaBL();

            var objSent = TempData["Area"];
            if (objSent != null) { TempData["Area"] = null; return View(objSent); }

            AreaDTO obj;
            if (id != null)
            {
                obj = objBL.getAreaEnEmpresa((int)currentUser.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Areas");
                if (obj.IdEmpresa != currentUser.IdEmpresa) return RedirectToAction("Areas");
                return View(obj);
            }
            obj = new AreaDTO();
            obj.IdEmpresa = currentUser.IdEmpresa;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddArea(AreaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            try
            {
                AreaBL objBL = new AreaBL();
                if (dto.IdArea == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Areas");
                    }
                }
                else if (dto.IdArea != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Areas");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdArea != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Area"] = dto;
            return RedirectToAction("Area");
        }
        public ActionResult Responsables(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Consultores";
            MenuNavBarSelected(5);

            UsuarioDTO currentUser = getCurrentUser();
            ResponsableBL objBL = new ResponsableBL();
            List<ResponsableDTO> listaResponsables = new List<ResponsableDTO>();

            ViewBag.vbInactivos = inactivos;

            if (currentUser.IdEmpresa > 0)
            {
                listaResponsables = objBL.getResponsablesEnEmpresa(currentUser.IdEmpresa);
                if (!inactivos)
                { listaResponsables = objBL.getResponsablesActivosEnEmpresa(currentUser.IdEmpresa); }
                else
                { listaResponsables = objBL.getResponsablesEnEmpresa(currentUser.IdEmpresa); }
            }
            return View(listaResponsables);
        }


        public ActionResult Responsable(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Responsable";
            MenuNavBarSelected(5);

            UsuarioDTO currentUser = getCurrentUser();

            ResponsableBL objBL = new ResponsableBL();
            ViewBag.lstComprobantes = objBL.getComprobantes_ConResponsable(currentUser.IdEmpresa, id.GetValueOrDefault());

            var objSent = TempData["Responsable"];
            if (objSent != null) { TempData["Responsable"] = null; return View(objSent); }

            ResponsableDTO obj;
            if (id != null)
            {
                obj = objBL.getResponsableEnEmpresa((int)currentUser.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Responsables");
                if (obj.IdEmpresa != currentUser.IdEmpresa) return RedirectToAction("Responsables");

                //ViewBag.lstComprobantes = objBL.getComprobantes_ConResponsable(currentUser.IdEmpresa, id.GetValueOrDefault());

                return View(obj);
            }
            obj = new ResponsableDTO();
            obj.IdEmpresa = currentUser.IdEmpresa;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddResponsable(ResponsableDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                ResponsableBL objBL = new ResponsableBL();
                if (dto.IdResponsable == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Responsables");
                    }
                }
                else if (dto.IdResponsable != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Responsables");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdResponsable != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Responsable"] = dto;
            return RedirectToAction("Responsable");
        }

        public ActionResult Honorarios(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Formas de Pago";
            MenuNavBarSelected(7);

            UsuarioDTO currentUser = getCurrentUser();

            HonorarioBL objBL = new HonorarioBL();
            List<HonorarioDTO> listaHonorarios = new List<HonorarioDTO>();

            ViewBag.vbInactivos = inactivos;

            if (currentUser.IdEmpresa > 0)
            {
                if (!inactivos)
                { listaHonorarios = objBL.getHonorariosActivosEnEmpresa(currentUser.IdEmpresa); }
                else
                { listaHonorarios = objBL.getHonorariosEnEmpresa(currentUser.IdEmpresa); }
            }
            return View(listaHonorarios);
        }

        public ActionResult Honorario(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Forma de Pago";
            MenuNavBarSelected(7);

            UsuarioDTO currentUser = getCurrentUser();

            HonorarioBL objBL = new HonorarioBL();

            var objSent = TempData["Honorario"];
            if (objSent != null) { TempData["Honorario"] = null; return View(objSent); }

            HonorarioDTO obj;
            if (id != null)
            {
                obj = objBL.getHonorarioEnEmpresa((int)currentUser.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Honorarios");
                if (obj.IdEmpresa != currentUser.IdEmpresa) return RedirectToAction("Honorarios");
                return View(obj);
            }
            obj = new HonorarioDTO();
            obj.IdEmpresa = currentUser.IdEmpresa;

            return View(obj);
        }
        [HttpPost]
        public ActionResult AddHonorario(HonorarioDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                HonorarioBL objBL = new HonorarioBL();
                if (dto.IdHonorario == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Honorarios");
                    }
                }
                else if (dto.IdHonorario != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Honorarios");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }

                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdHonorario != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Honorario"] = dto;
            return RedirectToAction("Honorario");
        }

        public ActionResult Proyectos()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ProyectoBL objBL = new ProyectoBL();
            return View(objBL.getProyectos());
        }
        public ActionResult Proyecto(int? id = null, int? idEntidad = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            //if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Proyecto";
            MenuNavBarSelected(6);
            UsuarioDTO user = getCurrentUser();

            ProyectoBL objBL = new ProyectoBL();
            ViewBag.IdProyecto = id;
            ViewBag.lstComprobantes = objBL.getComprobantes_ConProyecto(user.IdEmpresa, id.GetValueOrDefault());

            ResponsableBL resBL = new ResponsableBL();
            ViewBag.lstResponsables = resBL.getResponsablesActivosEnEmpresa(user.IdEmpresa);

            EntidadResponsableBL objEntidadBL = new EntidadResponsableBL();
            EntidadResponsableDTO objEntidad = objEntidadBL.getEntidadResponsableEnEmpresa(user.IdEmpresa, idEntidad.GetValueOrDefault());
            if (objEntidad == null) { return RedirectToAction("EntidadesClientes", "Admin"); }

            var objSent = TempData["Proyecto"];
            if (objSent != null) { TempData["Proyecto"] = null; return View(objSent); }
            if (id == 0 && idEntidad != null)
            {
                ProyectoDTO nuevo = new ProyectoDTO();
                nuevo.IdEntidadResponsable = (int)idEntidad;
                nuevo.Estado = true;
                return View(nuevo);
            }
            else
            {
                if (id != null)
                {
                    ProyectoDTO obj = objBL.getProyecto((int)id);

                    if (obj == null) return RedirectToAction("Entidad", "Admin", new { id = objEntidad.IdEntidadResponsable });
                    if (obj.IdEntidadResponsable != objEntidad.IdEntidadResponsable) return RedirectToAction("Entidad", "Admin", new { id = objEntidad.IdEntidadResponsable });

                    EntidadResponsableDTO objEntidadProy = objEntidadBL.getEntidadResponsableEnEmpresa(user.IdEmpresa, obj.IdEntidadResponsable);
                    if (objEntidadProy == null) return RedirectToAction("Entidades", "Admin");
                    if (objEntidadProy.IdEmpresa != user.IdEmpresa) return RedirectToAction("Entidades", "Admin");

                    return View(obj);
                }
            }
            return View();
        }
        public ActionResult AddProyecto(ProyectoDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                ProyectoBL objBL = new ProyectoBL();
                if (dto.IdProyecto == 0)
                {
                    if (objBL.add(dto))
                    {
                        //objBL.ActualizarSaldos(dto.IdCuentaBancaria);
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Entidad", new { id = dto.IdEntidadResponsable });
                    }
                }
                else if (dto.IdProyecto != 0)
                {
                    if (objBL.update(dto))
                    {
                        //objBL.ActualizarSaldos(dto.IdCuentaBancaria);
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Entidad", new { id = dto.IdEntidadResponsable });
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdProyecto != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Proyecto"] = dto;
            return RedirectToAction("Proyecto");
        }
        public ActionResult Contactos()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            ContactoBL objBL = new ContactoBL();
            return View(objBL.getContactos());
        }
        public ActionResult Contacto(int? id = null, int? idEntidad = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            //if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Contacto";
            MenuNavBarSelected(6);
            UsuarioDTO miUsuario = getCurrentUser();

            ContactoBL objBL = new ContactoBL();
            ViewBag.IdContacto = id;

            EntidadResponsableBL objEntidadBL = new EntidadResponsableBL();
            EntidadResponsableDTO objEntidad = objEntidadBL.getEntidadResponsableEnEmpresa(miUsuario.IdEmpresa, idEntidad.GetValueOrDefault());
            if (objEntidad == null) { return RedirectToAction("Entidades", "Admin"); }

            var objSent = TempData["Contacto"];
            if (objSent != null) { TempData["Contacto"] = null; return View(objSent); }
            if (id == 0 && idEntidad != null)
            {
                ContactoDTO nuevo = new ContactoDTO();
                nuevo.IdEntidadResponsable = (int)idEntidad;
                nuevo.Estado = true;
                return View(nuevo);
            }
            else if (id != null)
            {
                ContactoDTO obj = objBL.getContacto((int)id);

                if (obj == null) return RedirectToAction("Entidad", "Admin", new { id = objEntidad.IdEntidadResponsable });
                if (obj.IdEntidadResponsable != objEntidad.IdEntidadResponsable) return RedirectToAction("Entidad", "Admin", new { id = objEntidad.IdEntidadResponsable });

                EntidadResponsableDTO objEntidadProy = objEntidadBL.getEntidadResponsableEnEmpresa(miUsuario.IdEmpresa, obj.IdEntidadResponsable);
                if (objEntidadProy == null) return RedirectToAction("Entidades", "Admin");
                if (objEntidadProy.IdEmpresa != miUsuario.IdEmpresa) return RedirectToAction("Entidades", "Admin");

                return View(obj);
            }
            return View();
        }
        public ActionResult AddContacto(ContactoDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                ContactoBL objBL = new ContactoBL();
                if (dto.IdContacto == 0)
                {
                    if (objBL.add(dto))
                    {
                        //objBL.ActualizarSaldos(dto.IdCuentaBancaria);
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Entidad", new { id = dto.IdEntidadResponsable });
                    }
                }
                else if (dto.IdContacto != 0)
                {
                    if (objBL.update(dto))
                    {
                        //objBL.ActualizarSaldos(dto.IdCuentaBancaria);
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Entidad", new { id = dto.IdEntidadResponsable });
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdContacto != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Contacto"] = dto;
            return RedirectToAction("Contacto");
        }

        public ActionResult Periodos(bool inactivos = false)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Periodos";
            MenuNavBarSelected(8, 1);

            UsuarioDTO currentUser = getCurrentUser();

            PeriodoBL objBL = new PeriodoBL();
            List<PeriodoDTO> listaPeriodos = new List<PeriodoDTO>();
            ViewBag.vbInactivos = inactivos;

            if (currentUser.IdEmpresa > 0)
            {
                if (!inactivos)
                { listaPeriodos = objBL.getPeriodosActivosEnEmpresa(currentUser.IdEmpresa); }
                else
                { listaPeriodos = objBL.getPeriodosEnEmpresa(currentUser.IdEmpresa); }
            }
            return View(listaPeriodos);
        }

        public ActionResult Periodo(int? id = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            if (!this.isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title = "Periodo";
            MenuNavBarSelected(8, 1);

            UsuarioDTO currentUser = getCurrentUser();

            PeriodoBL objBL = new PeriodoBL();

            var objSent = TempData["Periodo"];
            if (objSent != null) { TempData["Periodo"] = null; return View(objSent); }

            PeriodoDTO obj;
            if (id != null)
            {
                obj = objBL.getPeriodoEnEmpresa((int)currentUser.IdEmpresa, (int)id);
                if (obj == null) return RedirectToAction("Periodos");
                if (obj.IdEmpresa != currentUser.IdEmpresa) return RedirectToAction("Periodos");
                return View(obj);
            }
            obj = new PeriodoDTO();
            obj.IdEmpresa = currentUser.IdEmpresa;

            int dyear = DateTime.Now.Year;
            obj.FechaInicio = new DateTime(dyear, 1, 1);
            obj.FechaFin = new DateTime(dyear, 12, 31);
            return View(obj);
        }

        [HttpPost]
        public ActionResult AddPeriodo(PeriodoDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            try
            {
                PeriodoBL objBL = new PeriodoBL();
                if (dto.IdPeriodo == 0)
                {
                    if (objBL.add(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Periodos");
                    }
                }
                else if (dto.IdPeriodo != 0)
                {
                    if (objBL.update(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Periodos");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }
                }
                else
                {
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                }
            }
            catch (Exception e)
            {
                if (dto.IdPeriodo != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Periodo"] = dto;
            return RedirectToAction("Periodo");
        }

        #region APIs adicionales
        public JsonResult CategoriasJson()
        {
            EmpresaDTO empresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);
            
            CategoriaBL objBL = new CategoriaBL();
            var listaCat = CategoriasBucle(empresa.IdEmpresa, (int)empresa.IdPeriodo, null, null);

            return Json(new { listaCat }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPeriodos()
        {
            PeriodoBL periodoBL = new PeriodoBL();
            var periodos = periodoBL.getPeriodosEnEmpresa(getCurrentUser().IdEmpresa);
            return Json(periodos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetComprobantes(int idMovimiento, int idEntidad, int idTipoDoc)
        {
            ComprobanteBL objBL = new ComprobanteBL();

            //var listaComp = objBL.getComprobantesPorEntXTDoc(getCurrentUser().IdEmpresa, idEntidad, idTipoDoc);
            List<Select2DTO_B> lista = objBL.getComprobantes_EntidadXDocumento_Pendientes(getCurrentUser().IdEmpresa, idEntidad, idTipoDoc);
            if(idMovimiento > 0)
            {
                MovimientoDTO mov = (new MovimientoBL()).getMovimiento(idMovimiento);
                if(mov != null && mov.IdComprobante != null && mov.IdComprobante > 0 && mov.IdEntidadResponsable == idEntidad && mov.IdTipoDocumento == idTipoDoc)
                {
                    ComprobanteDTO item = objBL.getComprobanteEnEmpresa(getCurrentUser().IdEmpresa, mov.IdComprobante.GetValueOrDefault());
                    Select2DTO_B aux = new Select2DTO_B() { id = item.IdComprobante, text = item.NroDocumento, ejecutado = item.Ejecutado };
                    lista.Insert(0, aux);
                }
                
            }
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLotes_EnItem(int idItem)
        {
            MovimientoInvBL objBL = new MovimientoInvBL();

            List<Select2DTO_B> lista = objBL.getLotes_PorItem_EnEmpresa(getCurrentUser().IdEmpresa, idItem);
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUbicaciones_EnLote(string serieLote)
        {
            MovimientoInvBL objBL = new MovimientoInvBL();

            List<UbicacionDTO> lista = objBL.getUbicacionesEnEmpresa(getCurrentUser().IdEmpresa, serieLote);
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProyectos(int idEntidad)
        {
            ProyectoBL objBL = new ProyectoBL();
            var listaProyectos = objBL.getProyectosPorEntidad(idEntidad);
            return Json(new { listaProyectos }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Buscar_Elementos(string texto, int page = 0)
        {
            texto = texto.ToLower();
            EmpresaDTO empresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            MovimientoBL movimientoBL = new MovimientoBL();
            ComprobanteBL comprobanteBL = new ComprobanteBL();
            MovimientoInvBL movimientoInvBL = new MovimientoInvBL();

            List<MovimientoDTO> lstMovimientos = movimientoBL.getMovimientos_EnEmpresa(empresa.IdEmpresa);
            List<ComprobanteDTO> lstComprobantes = comprobanteBL.getComprobantesEnEmpresa(empresa.IdEmpresa);
            List<MovimientoInvDTO> lstMovimientoInv = movimientoInvBL.getMovimientoInvsEnEmpresa(empresa.IdEmpresa);

            List<sItem> ListaS1, ListaS2, ListaS3;

            ListaS1 = lstMovimientos.Where(x => 
                            (x.NroOperacion.ToLower() ?? "").Contains(texto)||
                            (x.Monto.ToString()).Contains(texto)||
                            (x.NombreCategoria.ToLower() ?? "").Contains(texto)||
                            x.nTipoDocumento.ToLower().Contains(texto)||
                            (x.NumeroDocumento.ToLower() ?? "").Contains(texto)||
                            x.NombreEntidadR.ToLower().Contains(texto)
                            ).Select(x => new sItem {
                                id = x.IdMovimiento,
                                nombre = x.NroOperacion,
                                monto = x.Monto,
                                idElemento = 1,
                                elemento = "Movimiento",
                                tipo = x.IdCuentaBancaria.ToString(), //LibroBancario
                                simboloMoneda = x.SimboloMoneda,
                                s1 = x.NombreCategoria,
                                s2 = x.nTipoDocumento,
                                s3 = x.NumeroDocumento,
                                s4 = x.NombreEntidadR
                            }).ToList();

            ListaS2 = lstComprobantes.Where(x => 
                            (x.NroDocumento).Contains(texto)||
                            (x.Monto.ToString()).Contains(texto)||
                            x.NombreTipoDocumento.ToLower().Contains(texto)||
                            x.NombreEntidad.ToLower().Contains(texto)
                            ).Select(x => new sItem
                            {
                                id = x.IdComprobante,
                                nombre = x.NroDocumento,
                                monto = x.Monto,
                                idElemento = 2,
                                elemento = "Comprobante",
                                tipo = x.IdTipoComprobante.ToString(),
                                simboloMoneda = x.SimboloMoneda,
                                s1 = x.NombreTipoDocumento,
                                s2 = x.NombreEntidad
                            }).ToList();

            ListaS3 = lstMovimientoInv.Where(x => 
                            (x.NroDocumento.ToLower()).Contains(texto)||
                            (x.Cantidad.ToString()).Contains(texto)||
                            x.nItem.ToLower().Contains(texto)||
                            x.UnidadMedida.ToLower().Contains(texto)||
                            x.nForma.ToLower().Contains(texto)||
                            x.SerieLote.ToLower().Contains(texto)
                            ).Select(x => new sItem
                            {
                                id = x.IdMovimientoInv,
                                nombre = x.NroDocumento,
                                monto = x.Cantidad,
                                idElemento = 3,
                                elemento = "Movimiento Inv.",
                                tipo = x.IdTipoMovimientoInv.ToString(),
                                s1 = x.nItem,
                                s2 = x.UnidadMedida,
                                s3 = x.nForma,
                                s4 = x.SerieLote
                            }).ToList();

            ListaS1.AddRange(ListaS2);
            ListaS1.AddRange(ListaS3);

            InfoSItem elem = new InfoSItem();
            elem.total_count = ListaS1.Count();
            //elem.items = ListaS1.Skip(5 * page).Take(5).ToList();
            elem.items = ListaS1;

            return Json(new { elem }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActualizarEstadoEnMovimiento(int idMovimiento)
        {
            Decimal SaldoBancario = 0;
            if (!this.currentUser() || isUsuarioExterno()) return Json(new { SaldoBancario }, JsonRequestBehavior.AllowGet);

            UsuarioDTO miUsuario = getCurrentUser();
            MovimientoBL obj = new MovimientoBL();
            obj.ActualizarEstadoMovimiento(idMovimiento);
            SaldoBancario = obj.ActualizarSaldoBancarioEnMovimiento_Decimal(idMovimiento);
            //ActualizarSaldoBancarioEnMovimiento(Movimiento.IdMovimiento);
            return Json(new { SaldoBancario }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ActualizarPeriodo(int idPeriodo)
        {
            if (!this.currentUser() || !isAdministrator()) { return "false"; }

            EmpresaBL objBL = new EmpresaBL();
            UsuarioDTO miUsuario = getCurrentUser();
            EmpresaDTO obj = new EmpresaDTO() { IdEmpresa = miUsuario.IdEmpresa, IdPeriodo = idPeriodo };
            if (objBL.updatePeriodo(obj))
            {
                return "true";
            }
            return "false";
        }

        [HttpPost]
        public string GetUnidadDeMedidaEnItem(int idItem)
        {
            if (!this.currentUser() || !isAdministrator()) { return "-"; }

            ItemBL objBL = new ItemBL();
            ItemDTO obj = objBL.getItemEnEmpresa(getCurrentUser().IdEmpresa, idItem);
            if (obj != null) { return obj.UnidadMedida; }
            return "-";
        }

        [HttpPost]
        public JsonResult ActualizarPresupuesto(int idCategoria, Decimal Monto)
        {
            if (!this.currentUser() || !isAdministrator()) { return Json(false, JsonRequestBehavior.AllowGet); }

            CategoriaBL objBL = new CategoriaBL();
            EmpresaBL empBL = new EmpresaBL();
            int pPeriodo = empBL.getEmpresa(getCurrentUser().IdEmpresa).IdPeriodo.GetValueOrDefault();
            if (pPeriodo == 0) { return Json(false, JsonRequestBehavior.AllowGet); }

            CategoriaPorPeriodoDTO dto = new CategoriaPorPeriodoDTO() { IdCategoria = idCategoria, IdPeriodo = pPeriodo, Monto = Monto };
            objBL.updatePresupuesto(dto);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarComprobante(int idComprobante, int idCuentaBancaria)
        {
            ComprobanteBL objBL = new ComprobanteBL();
            var comprobante = objBL.getComprobanteEjecutadoEnEmpresa(idComprobante, idCuentaBancaria, getCurrentUser().IdEmpresa);
            return Json(new { comprobante }, JsonRequestBehavior.AllowGet);
        }

        public List<Select2DTO> CategoriasBucle(int idEmpresa, int idPeriodo, int? id = null, IList<CategoriaDTO> lista = null)
        {
            var listaCat = lista;
            if (id == null && lista == null)
            {
                CategoriaBL objBL = new CategoriaBL();
                listaCat = objBL.getCategoriasTreeEnEmpresa(getCurrentUser().IdEmpresa);
                //listaCat = objBL.getCategoriasPorPeriodo_ArbolEnEmpresa(idEmpresa, idPeriodo);
            }
            List<Select2DTO> selectTree = new List<Select2DTO>();

            foreach (var item in listaCat)
            {
                if (item.Estado)
                {
                    Select2DTO selectItem = new Select2DTO();
                    selectItem.id = item.IdCategoria;
                    selectItem.text = item.Nombre;
                    
                    if (item.Hijos != null && item.Hijos.Count > 0)
                    {
                        selectItem.children = CategoriasBucle(idEmpresa, idPeriodo, item.IdCategoria, item.Hijos);
                    }
                    selectTree.Add(selectItem);
                }
            }
            return selectTree;
        }

        #endregion
        public ActionResult ReportesPresupuestos(int? message = null)
        {
            if(!this.currentUser()) { return RedirectToAction("Ingresar"); }

            if (getCurrentUser().IdRol == 3) { return RedirectToAction("Ingresar"); }

            ViewBag.Title = "Reportes de Presupuesto";
            MenuNavBarSelected(1, 0);

            EmpresaDTO empresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            CuentaBancariaBL objBL = new CuentaBancariaBL();
            ViewBag.Categorias = CategoriasBucle(empresa.IdEmpresa, empresa.IdPeriodo.GetValueOrDefault(), null, null);
            PeriodoBL periodoBL = new PeriodoBL();
            ViewBag.Periodos = periodoBL.getPeriodosActivosEnEmpresa(getCurrentUser().IdEmpresa);

            if (message != null)
            {
                switch (message)
                {
                    case 1:
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_MESSAGE);
                        break;
                    case 2:
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_REPORTE_NO_MOVS);
                        break;
                }
            }

            return View();
        }

        public ActionResult ReportesGestion(int? message = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }

            //if (getCurrentUser().IdRol == 3) { return RedirectToAction("Ingresar"); }

            ViewBag.Title = "Reportes de Gestión";
            MenuNavBarSelected(1, 1);

            if (message != null)
            {
                switch (message)
                {
                    case 1:
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_MESSAGE);
                        break;
                    case 2:
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_REPORTE_NO_MOVS);
                        break;
                }
            }

            return View();
        }

        public ActionResult ReportesInventarios(int? message = null)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar"); }

            //if (getCurrentUser().IdRol == 3) { return RedirectToAction("Ingresar"); }

            ViewBag.Title = "Reportes de Inventarios";
            MenuNavBarSelected(1, 2);

            ItemBL objBL = new ItemBL();
            ViewBag.lstItems = objBL.getItemsEnEmpresa(getCurrentUser().IdEmpresa);

            if (message != null)
            {
                switch (message)
                {
                    case 1:
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_MESSAGE);
                        break;
                    case 2:
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_REPORTE_NO_MOVS);
                        break;
                }
            }

            return View();
        }

        #region Reportes
        public ActionResult GenerarRep_AvanceDePresupuesto(DateTime FechaInicio, DateTime FechaFin, int periodo)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesPresupuestos", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);
            PeriodoDTO objPeriodo = (new PeriodoBL()).getPeriodoEnEmpresa(objEmpresa.IdEmpresa, periodo);

            ReportesBL repBL = new ReportesBL();
            List<CategoriaDTO> lstCatsMontos = repBL.AvanceDePresupuesto(objEmpresa.IdEmpresa, FechaInicio, FechaFin);

            //Sumatoria de Presupuestos de Padres y armado de Arbol
            List<CategoriaDTO> lstCats = repBL.getCategoriasTreeEnEmpresa(lstCatsMontos, objEmpresa.IdEmpresa);
            //Arbol de presupuestos
            CategoriaBL catBL = new CategoriaBL();
            //List<CategoriaDTO> arbolPresupuestos = repBL.getCategoriasPresupuestosTreeEnEmpresa(objEmpresa.IdEmpresa, 0);
            List<CategoriaDTO> arbolPresupuestos = repBL.getCategoriasPresupuestosTree_PorPeriodo_EnEmpresa(objEmpresa.IdEmpresa, 0, periodo);

            if (lstCats == null)
                return RedirectToAction("ReportesPresupuestos", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Nivel");
            dt.Columns.Add("Partida de Presupuesto");
            dt.Columns.Add("MONTO SIN IGV");
            dt.Columns.Add("PRESUPUESTO SIN IGV");
            dt.Columns.Add("EJECUCION DEL PRESUPUESTO");

            //Suma de Padres de Nivel 0
            //Decimal SumaPadres0 = lstCatsMontos.Where(x => x.IdCategoriaPadre == null).Sum(x => x.Presupuesto.GetValueOrDefault());
            //Decimal SumaPresupuesto = arbolPresupuestos.Sum(x => x.Presupuesto.GetValueOrDefault());

            /*System.Data.DataRow auxRow = dt.NewRow();
            auxRow[0] = ""; auxRow[1] = ""; auxRow[2] = SumaPadres0.ToString("N2", CultureInfo.InvariantCulture); auxRow[3] = SumaPresupuesto.ToString("N2", CultureInfo.InvariantCulture); auxRow[4] = (SumaPresupuesto == 0) ? "0.00%" : (SumaPresupuesto / SumaPresupuesto).ToString("P2", CultureInfo.InvariantCulture);
            dt.Rows.Add(auxRow);*/

            foreach (var obj in arbolPresupuestos)
            {
                PintarArbolPadre(obj, lstCatsMontos, objEmpresa, dt);
            }

            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, "Ejecucion de Presupuesto - Empresa:" + objEmpresa.Nombre);
                //Cabecera principal
                //AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 1, "FECHAS: " + FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString());
                AddWhiteHeader(gv, 2, "PERIODO: " + objPeriodo.Nombre + " (" + objPeriodo.FechaInicio.ToShortDateString() + " - " + objPeriodo.FechaFin.ToShortDateString() + ")");
                AddWhiteHeader(gv, 3, "MONEDA: (" + objEmpresa.SimboloMoneda + ")");

                PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + "EjecucionDePresupuesto_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
            return RedirectToAction("ReportesPresupuestos", new { message = 2 });
        }
        public ActionResult GenerarRep_EgresosPorAreas(DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesGestion", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<AreaDTO> lstAreasMontos = repBL.getEgresosAreasEnEmpresa(objEmpresa.IdEmpresa, FechaInicio, FechaFin);

            if (lstAreasMontos == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Áreas");
            dt.Columns.Add("Proveedores");
            dt.Columns.Add("Monto con IGV");
            dt.Columns.Add("Monto sin IGV");
            dt.Columns.Add("Porcentaje");

            Decimal SumaTotal = lstAreasMontos.Sum(x => x.SumaClientes);
            Decimal SumaTotal_SinIGV = lstAreasMontos.Sum(x => x.SumaClientes_SinIGV);

            foreach (var obj in lstAreasMontos)
            {
                if (obj.SumaClientes != 0)
                { PintarAreas(obj, SumaTotal, dt); }
            }

            System.Data.DataRow rowFinal = dt.NewRow();
            rowFinal[0] = "TOTAL";
            rowFinal[2] = SumaTotal.ToString("N2", CultureInfo.InvariantCulture);
            rowFinal[3] = SumaTotal_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(rowFinal);

            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, "Gastos por &aacute;reas - Empresa:" + objEmpresa.Nombre);
                //Cabecera principal
                AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 2, "PERIODO: " + FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + FechaFin.GetValueOrDefault().ToShortDateString());
                AddWhiteHeader(gv, 3, "Moneda: (" + objEmpresa.SimboloMoneda + ")");

                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + "GastosPorAreas_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
            return RedirectToAction("ReportesGestion", new { message = 2 });
        }
        public ActionResult GenerarRep_FacturacionPorAreas(DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesGestion", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<AreaDTO> lstAreasMontos = repBL.getAreasEnEmpresa(objEmpresa.IdEmpresa, FechaInicio, FechaFin);

            if (lstAreasMontos == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Áreas");
            dt.Columns.Add("Clientes");
            dt.Columns.Add("Monto con IGV");
            dt.Columns.Add("Monto sin IGV");
            dt.Columns.Add("Porcentaje");

            Decimal SumaTotal = lstAreasMontos.Sum(x => x.SumaClientes);
            Decimal SumaTotal_SinIGV = lstAreasMontos.Sum(x => x.SumaClientes_SinIGV);

            foreach (var obj in lstAreasMontos)
            {
                if (obj.SumaClientes != 0)
                { PintarAreas(obj, SumaTotal, dt); }
            }

            System.Data.DataRow rowFinal = dt.NewRow();
            rowFinal[0] = "TOTAL";
            rowFinal[2] = SumaTotal.ToString("N2", CultureInfo.InvariantCulture);
            rowFinal[3] = SumaTotal_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(rowFinal);

            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, "Ventas por &aacute;reas - Empresa:" + objEmpresa.Nombre);
                //Cabecera principal
                AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 2, "PERIODO: " + FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + FechaFin.GetValueOrDefault().ToShortDateString());
                AddWhiteHeader(gv, 3, "Moneda: (" + objEmpresa.SimboloMoneda + ")");

                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + "VentasPorAreas_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
            return RedirectToAction("ReportesGestion", new { message = 2 });
        }
        public ActionResult GenerarRep_IngresosEgresosPorAreas(DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesGestion", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<AreaDTO> lstAreasIE = repBL.getIngresosEgresosAreas(objEmpresa.IdEmpresa, FechaInicio, FechaFin);

            if (lstAreasIE == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Áreas");
            //dt.Columns.Add("V/G");
            dt.Columns.Add("Movimiento");
            dt.Columns.Add("Monto Con IGV");
            dt.Columns.Add("Monto Sin IGV");

            foreach (var obj in lstAreasIE)
            {
                if (!(obj.Ingresos == 0 && obj.Egresos == 0))
                { PintarAreasIE(obj, dt); }
            }

            GenerarPdf(dt, "Ventas y Gastos por &Aacute;reas", "VentasYGastosPorAreas", objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesGestion", new { message = 2 });
        }
        public ActionResult GenerarRep_FacturacionPorCliente(DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesGestion", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<EntidadResponsableR_DTO> lstClientes = repBL.getFacturacionPorClientes(objEmpresa.IdEmpresa, FechaInicio, FechaFin);

            if (lstClientes == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Clientes");
            dt.Columns.Add("Monto con IGV");
            dt.Columns.Add("Monto sin IGV");
            dt.Columns.Add("Porcentaje");

            Decimal SumaTotal = lstClientes.Sum(x => x.Monto);
            Decimal SumaTotal_SinIGV = lstClientes.Sum(x => x.MontoSinIGV);

            foreach (var obj in lstClientes)
            {
                if (obj.Monto != 0)
                {
                    System.Data.DataRow row = dt.NewRow();
                    row["Clientes"] = obj.Nombre;
                    row["Monto con IGV"] = obj.Monto;
                    row["Monto sin IGV"] = obj.MontoSinIGV;
                    Decimal porcentaje = SumaTotal == 0 ? 0 : obj.Monto / SumaTotal;
                    row["Porcentaje"] = porcentaje.ToString("P2", CultureInfo.InvariantCulture);
                    dt.Rows.Add(row);
                }
            }

            System.Data.DataRow rowFinal = dt.NewRow();
            rowFinal["Clientes"] = "TOTAL";
            rowFinal["Monto con IGV"] = SumaTotal.ToString("N2", CultureInfo.InvariantCulture);
            rowFinal["Monto sin IGV"] = SumaTotal_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(rowFinal);

            GenerarPdf(dt, "Ventas por Clientes", "VentasPorClientes", objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesGestion", new { message = 2 });
        }
        public ActionResult GenerarRep_GastosPorProveedor(DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesGestion", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<EntidadResponsableR_DTO> lstProveedores = repBL.getGastosPorProveedores(objEmpresa.IdEmpresa, FechaInicio, FechaFin);

            if (lstProveedores == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Proveedores");
            dt.Columns.Add("Monto con IGV");
            dt.Columns.Add("Monto sin IGV");
            dt.Columns.Add("Porcentaje");

            Decimal SumaTotal = lstProveedores.Sum(x => x.Monto);
            Decimal SumaTotal_SinIGV = lstProveedores.Sum(x => x.MontoSinIGV);

            foreach (var obj in lstProveedores)
            {
                if (obj.Monto != 0)
                {
                    System.Data.DataRow row = dt.NewRow();
                    row["Proveedores"] = obj.Nombre;
                    row["Monto con IGV"] = obj.Monto;
                    row["Monto sin IGV"] = obj.MontoSinIGV;
                    Decimal porcentaje = SumaTotal == 0 ? 0 : obj.Monto / SumaTotal;
                    row["Porcentaje"] = porcentaje.ToString("P2", CultureInfo.InvariantCulture);
                    dt.Rows.Add(row);
                }
            }

            System.Data.DataRow rowFinal = dt.NewRow();
            rowFinal["Proveedores"] = "TOTAL";
            rowFinal["Monto con IGV"] = SumaTotal.ToString("N2", CultureInfo.InvariantCulture);
            rowFinal["Monto sin IGV"] = SumaTotal_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(rowFinal);

            GenerarPdf(dt, "Gastos por Proveedores", "GastosPorProveedores", objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesGestion", new { message = 2 });
        }
        public ActionResult GenerarRep_FacturacionPorVendedor(DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesGestion", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<ResponsableDTO> lstVendedores = repBL.getFacturacionPorVendedores(objEmpresa.IdEmpresa, FechaInicio, FechaFin);

            if (lstVendedores == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Consultores");
            dt.Columns.Add("Monto con IGV");
            dt.Columns.Add("Monto sin IGV");
            dt.Columns.Add("Porcentaje");

            Decimal SumaTotal = lstVendedores.Sum(x => x.Monto);
            Decimal SumaTotal_SinIGV = lstVendedores.Sum(x => x.MontoSinIGV);

            foreach (var obj in lstVendedores)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Consultores"] = obj.Nombre;
                row["Monto con IGV"] = obj.Monto;
                row["Monto sin IGV"] = obj.MontoSinIGV;
                Decimal porcentaje = SumaTotal == 0 ? 0 : obj.Monto / SumaTotal;
                row["Porcentaje"] = porcentaje.ToString("P2", CultureInfo.InvariantCulture);
                dt.Rows.Add(row);
            }

            System.Data.DataRow rowFinal = dt.NewRow();
            rowFinal["Consultores"] = "TOTAL";
            rowFinal["Monto con IGV"] = SumaTotal.ToString("N2", CultureInfo.InvariantCulture);
            rowFinal["Monto sin IGV"] = SumaTotal_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(rowFinal);

            GenerarPdf(dt, "Ventas por Consultores", "VentasPorConsultores", objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesGestion", new { message = 2 });
        }
        public ActionResult GenerarRep_DocumentosIngresoYEgresoPagadosYPorCobrar(int IdTipoComprobante, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesGestion", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();

            List<ComprobanteDTO> lstComprobantes = repBL.getComprobantesIngresosYEgresosEnEmpresa(objEmpresa.IdEmpresa, IdTipoComprobante, FechaInicio.GetValueOrDefault(), FechaFin.GetValueOrDefault());
            //List<ComprobanteDTO> lstPagados = lstComprobantes.Where(x => x.Ejecutado).OrderBy(x => x.FechaEmision).ToList();
            //List<ComprobanteDTO> lstPorCobrar = lstComprobantes.Where(x => !x.Ejecutado).OrderBy(x => x.FechaEmision).ToList();
            if (lstComprobantes == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            string Entidad = IdTipoComprobante == 1 ? "Cliente" : "Entidad";
            string FechaEjecucion = IdTipoComprobante == 1 ? "Fecha Estimada de Cobro" : "Fecha Estimada de Pago";
            int neleCols = 13;

            dt.Columns.Add("Numero");
            dt.Columns.Add("Documento");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Status");
            dt.Columns.Add(Entidad);
            if (IdTipoComprobante == 1)
            { dt.Columns.Add("Proyecto"); neleCols = 14; }
            dt.Columns.Add("Moneda");
            dt.Columns.Add("Monto Sin IGV");
            dt.Columns.Add("Monto Total");
            dt.Columns.Add("Partida de Presupuesto");
            dt.Columns.Add("Monto Pendiente");
            dt.Columns.Add("Modalidad");
            dt.Columns.Add(FechaEjecucion);
            dt.Columns.Add("Fecha Cancelación");
            dt.Columns.Add("Dias transcurridos Emisión - Cancelación");
            dt.Columns.Add("Dias Vencidos");
            dt.Columns.Add("Comentarios");

            List<bool> Ejecutados = new List<bool>() { true, false };
            DateTime FechaActual = DateTime.Now;


            foreach (var elem in Ejecutados)
            {
                List<ComprobanteDTO> lista = lstComprobantes.Where(x => x.Ejecutado == elem).OrderBy(x => x.FechaEmision).ToList();

                foreach (var obj in lista)
                {
                    System.Data.DataRow row = dt.NewRow();
                    row["Numero"] = obj.NroDocumento;
                    row["Documento"] = obj.NombreTipoDocumento;
                    //row["Fecha"] = obj.FechaEmision.ToString("d", CultureInfo.CreateSpecificCulture("es-PE"));
                    row["Fecha"] = obj.FechaEmision.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                    row["Status"] = obj.Ejecutado ? "Cancelado" : "Pendiente";
                    row[Entidad] = obj.NombreEntidad;
                    if (IdTipoComprobante == 1)
                    { row["Proyecto"] = obj.NombreProyecto; }
                    row["Moneda"] = obj.SimboloMoneda;
                    row["Monto Sin IGV"] = obj.MontoSinIGV.ToString("N2", CultureInfo.InvariantCulture);
                    row["Monto Total"] = obj.Monto.ToString("N2", CultureInfo.InvariantCulture);
                    row["Partida de Presupuesto"] = obj.NombreCategoria;
                    row["Monto Pendiente"] = obj.Ejecutado ? "0.00" : obj.MontoIncompleto.ToString("N2", CultureInfo.InvariantCulture);
                    row["Modalidad"] = obj.nHonorario;
                    row[FechaEjecucion] = obj.FechaConclusion != null ? obj.FechaConclusion.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                    row["Fecha Cancelación"] = obj.FechaPago != null ? obj.FechaPago.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                    //Dias transcurridos Emisión - Cancelación
                    row[neleCols] = obj.FechaPago != null ? (obj.Ejecutado ? obj.FechaPago.GetValueOrDefault().Subtract(obj.FechaEmision).Days.ToString() : "-") : "-";
                    row["Dias Vencidos"] = obj.Ejecutado ? "0" : obj.FechaConclusion != null ? (FechaActual - obj.FechaConclusion.GetValueOrDefault()).Days.ToString() : "N/A";
                    row["Comentarios"] = obj.Comentario;
                    dt.Rows.Add(row);
                }
                /*if (elem != Ejecutados.Last())
                {
                    DataRow space = dt.NewRow();
                    dt.Rows.Add(space);
                }*/
            }

            string titulo = IdTipoComprobante == 1 ? "Documentos Cobrados y por Cobrar" : "Documentos Pagados y por Pagar";
            string nombreFile = IdTipoComprobante == 1 ? "DocumentosCobrados_y_porCobrar" : "DocumentosPagados_y_porPagar";

            GenerarPdf2(dt, titulo, nombreFile, objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesGestion", new { message = 2 });
        }
        public ActionResult GenerarRep_DetalleIngresosYGastosPorPartidaDePresupuesto(int IdCategoria, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null || IdCategoria == 0)
            {
                return RedirectToAction("ReportesPresupuestos", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();

            CategoriaR_DTO catArbol = repBL.getDetalleIngresosYGastos_PorPartidaDePresupuesto(IdCategoria, objEmpresa.IdEmpresa, FechaInicio.GetValueOrDefault(), FechaFin.GetValueOrDefault());

            if (catArbol == null)
                return RedirectToAction("ReportesPresupuestos", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Nivel");
            dt.Columns.Add("Partida");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Entidad");
            dt.Columns.Add("Documento");
            dt.Columns.Add("# Documento");
            dt.Columns.Add("Moneda");
            dt.Columns.Add("Monto Sin IGV");
            dt.Columns.Add("Area(s)");
            dt.Columns.Add("Comentario");

            DataRow rowP = dt.NewRow();
            rowP["Nivel"] = catArbol.Nivel;
            rowP["Partida"] = catArbol.Nombre;
            dt.Rows.Add(rowP);
            foreach (var obj in catArbol.Comprobantes)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Fecha"] = obj.Fecha.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                row["Entidad"] = obj.NombreEntidad;
                row["Documento"] = obj.NombreDocumento;
                row["# Documento"] = obj.NroDocumento;
                row["Moneda"] = obj.Moneda;
                row["Monto Sin IGV"] = obj.MontoSinIGV.ToString("N2", CultureInfo.InvariantCulture);
                row["Area(s)"] = obj.Areas;
                row["Comentario"] = obj.Comentario;

                dt.Rows.Add(row);
            }
            PintarGastoPorPartidaPresupuesto(catArbol.Hijos.ToList(), dt);

            GenerarPdf2(dt, "Movimientos por Partida de Presupuesto", "Movimientos_porPartidaDePresupuesto", objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesPresupuestos", new { message = 2 });
        }
        public ActionResult GenerarRep_FacturacionPorHonorarios(DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesGestion", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<HonorarioDTO> lstHonorariosMontos = repBL.getHonorariosEnEmpresa(objEmpresa.IdEmpresa, FechaInicio, FechaFin);

            if (lstHonorariosMontos == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Modalidad");
            dt.Columns.Add("Monto con IGV");
            dt.Columns.Add("Monto sin IGV");
            dt.Columns.Add("Porcentaje");

            Decimal SumaTotal = lstHonorariosMontos.Sum(x => x.Monto);

            foreach (var obj in lstHonorariosMontos)
            {
                DataRow row = dt.NewRow();
                row["Modalidad"] = obj.Nombre;
                row["Monto con IGV"] = obj.Monto.ToString("N2", CultureInfo.InvariantCulture);
                row["Monto sin IGV"] = obj.Monto_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
                Decimal porcentaje = SumaTotal == 0 ? 0 : obj.Monto / SumaTotal;
                obj.Porcentaje = porcentaje;
                row["Porcentaje"] = porcentaje.ToString("P2", CultureInfo.InvariantCulture);
                dt.Rows.Add(row);
            }

            System.Data.DataRow rowFinal = dt.NewRow();
            rowFinal[0] = "TOTAL";
            rowFinal[1] = SumaTotal.ToString("N2", CultureInfo.InvariantCulture);
            rowFinal[2] = lstHonorariosMontos.Sum(x => x.Porcentaje).ToString("P2", CultureInfo.InvariantCulture);
            dt.Rows.Add(rowFinal);

            GenerarPdf(dt, "Ventas por Modalidad de Pago", "VentasPorModalidadDePago", objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesGestion", new { message = 2 });
        }

        public ActionResult GenerarRep_Ventas_Gastos_Por_Mes(int year)
        {
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<LiquidezDTO> lista = repBL.getGestionMensual(objEmpresa.IdEmpresa, year);

            if (lista == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Mes");
            dt.Columns.Add("Detalle");
            dt.Columns.Add("Monto Con IGV");
            dt.Columns.Add("Monto Sin IGV");

            //int mesActual = DateTime.Now.Month;

            foreach (var obj in lista)
            {
                PintarGestionPorMesIE(obj, dt);
            }

            GenerarPdf6(dt, "Ventas-Gastos por Mes", "Ventas-Gastos_por_Mes", objEmpresa, Response);

            return RedirectToAction("ReportesGestion", new { message = 2 });
        }

        public ActionResult GenerarRep_Ventas_Por_Mes(int year)
        {
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<LiquidezDTO> lista = repBL.getGestionMensual(objEmpresa.IdEmpresa, year);

            if (lista == null)
                return RedirectToAction("ReportesGestion", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Mes");
            dt.Columns.Add("Monto Con IGV");
            dt.Columns.Add("Monto Sin IGV");

            //int mesActual = DateTime.Now.Month;

            foreach (var obj in lista)
            {
                DataRow row = dt.NewRow();
                row["Mes"] = obj.nombreMes;
                row["Monto Con IGV"] = obj.Ingreso.ToString("N2", CultureInfo.InvariantCulture);
                row["Monto Sin IGV"] = obj.Ingreso_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
                dt.Rows.Add(row);
            }

            DataRow rowFinal = dt.NewRow();
            rowFinal["Mes"] = "TOTAL";
            rowFinal["Monto Con IGV"] = lista.Sum(x => x.Ingreso).ToString("N2", CultureInfo.InvariantCulture);
            rowFinal["Monto Sin IGV"] = lista.Sum(x => x.Ingreso_SinIGV).ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(rowFinal);

            GenerarPdf6(dt, "Ventas por Mes", "Ventas_por_Mes", objEmpresa, Response);

            return RedirectToAction("ReportesGestion", new { message = 2 });
        }

        public ActionResult GenerarRep_Movimiento_De_Inventarios(int? idItem, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesInventarios", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);
            
            Reportes_InventariosBL repBL = new Reportes_InventariosBL();
            ItemBL itemBL = new ItemBL();
            ItemDTO item = itemBL.getItemEnEmpresa(objEmpresa.IdEmpresa, idItem.GetValueOrDefault());
            List<MovimientoInvDTO> lstMovs = repBL.Get_Reporte_De_Movimientos_De_Inventarios(idItem.GetValueOrDefault(), objEmpresa.IdEmpresa, FechaInicio.GetValueOrDefault(), FechaFin.GetValueOrDefault());

            if (lstMovs == null)
                return RedirectToAction("ReportesInventarios", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Fecha");
            dt.Columns.Add("Movimiento");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Numero Documento");
            dt.Columns.Add("Guia de Remision");
            dt.Columns.Add("Proveedor");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("Unid Med");
            dt.Columns.Add("Ubicacion");
            dt.Columns.Add("Lote");
            dt.Columns.Add("Stock por Lote");
            dt.Columns.Add("Vencimiento");
            dt.Columns.Add("Comentario");
            dt.Columns.Add("Usuario");

            foreach (var obj in lstMovs)
            {
                DataRow row = dt.NewRow();
                row["Fecha"] = obj.FechaInicial.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                row["Movimiento"] = obj.nTipo;
                row["Tipo"] = obj.nForma;
                row["Numero Documento"] = obj.NroDocumento;
                row["Guia de Remision"] = obj.GuiaRemision;
                row["Proveedor"] = obj.nEntidadResponsable;
                row["Cantidad"] = obj.Cantidad;
                row["Unid Med"] = obj.UnidadMedida;
                row["Ubicacion"] = obj.nUbicacion;
                row["Lote"] = obj.SerieLote;
                row["Stock por Lote"] = obj.StockLote;
                row["Vencimiento"] = obj.FechaFin != null ? obj.FechaFin.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                row["Comentario"] = obj.Comentario;
                row["Usuario"] = obj.nUsuario;
                dt.Rows.Add(row);
            }

            GenerarPdf3(dt, "Movimiento por item", "MovimientoPorItem", item.Codigo, item.Nombre, objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesInventarios", new { message = 2 });
        }
        public ActionResult GenerarRep_Inventarios(DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                return RedirectToAction("ReportesInventarios", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ItemBL itemBL = new ItemBL();
            List<ItemDTO> lstItems = itemBL.getItemsEnEmpresa(objEmpresa.IdEmpresa);

            Reportes_InventariosBL repBL = new Reportes_InventariosBL();
            List<MovimientoInvDTO> lstInventarios = repBL.Get_Reporte_De_Inventarios(objEmpresa.IdEmpresa, FechaInicio.GetValueOrDefault(), FechaFin.GetValueOrDefault());

            if (lstInventarios == null)
                return RedirectToAction("ReportesInventarios", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Código");
            dt.Columns.Add("Item");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Movimiento");
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Numero Documento");
            dt.Columns.Add("Guia de Remision");
            dt.Columns.Add("Entidad");
            dt.Columns.Add("Categoría");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("Unid Med");
            dt.Columns.Add("Ubicacion");
            dt.Columns.Add("Lote");
            dt.Columns.Add("Stock por Lote");
            dt.Columns.Add("Stock por Item");
            dt.Columns.Add("Vencimiento");
            dt.Columns.Add("Comentario");
            dt.Columns.Add("Usuario");

            foreach (var item in lstItems)
            {
                DataRow row = dt.NewRow();
                row["Código"] = item.Codigo;
                row["Item"] = item.Nombre;
                dt.Rows.Add(row);

                List<MovimientoInvDTO> movsItm = lstInventarios.Where(x => x.nItemCodigo == item.Codigo).ToList();
                foreach (var mov in movsItm)
                {
                    DataRow row1 = dt.NewRow();
                    row1["Fecha"] = mov.FechaInicial.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                    //row1["Movimiento"] = mov.IdTipoMovimientoInv == 1 ? "Ingreso" : "Egreso";
                    row1["Movimiento"] = mov.nTipo;
                    row1["Tipo"] = mov.nForma;
                    row1["Numero Documento"] = mov.NroDocumento;
                    row1["Guia de Remision"] = mov.GuiaRemision;
                    row1["Entidad"] = mov.nEntidadResponsable;
                    row1["Categoría"] = mov.nCategoria;
                    row1["Cantidad"] = mov.Cantidad;
                    row1["Unid Med"] = mov.UnidadMedida;
                    row1["Ubicacion"] = mov.nUbicacion;
                    row1["Lote"] = mov.SerieLote;
                    row1["Stock por Lote"] = mov.StockLote;
                    row1["Stock por Item"] = mov.SaldoItem;
                    row1["Vencimiento"] = mov.FechaFin != null ? mov.FechaFin.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                    row1["Comentario"] = mov.Comentario;
                    row1["Usuario"] = mov.nUsuario;
                    dt.Rows.Add(row1);
                }
            }

            /*foreach (var obj in lstInventarios)
            {
                DataRow row = dt.NewRow();
                row["Código"] = obj.nItemCodigo;
                row["Item"] = obj.nItem;
                row["Categoría"] = obj.nCategoria;
                row["Lote"] = obj.SerieLote;
                row["Vencimiento"] = obj.FechaFin != null ? obj.FechaFin.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                row["Stock por Lote"] = obj.StockLote;
                row["Saldo Por Item"] = obj.SaldoItem;
                row["Ubicación"] = obj.nUbicacion;
                dt.Rows.Add(row);
            }*/

            GenerarPdf4(dt, "Movimientos de todos los items", "MovimientosDeTodosLosItems", objEmpresa, FechaInicio, FechaFin, Response);

            return RedirectToAction("ReportesInventarios", new { message = 2 });
        }
        public ActionResult GenerarRep_Items_Por_Vencimiento(DateTime? rFechaFin)
        {
            if (rFechaFin == null)
            {
                return RedirectToAction("ReportesInventarios", new { message = 1 });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            Reportes_InventariosBL repBL = new Reportes_InventariosBL();
            List<MovimientoInvDTO> lstInventarios = repBL.Get_Reporte_Items_Por_Vencimiento(objEmpresa.IdEmpresa, rFechaFin.GetValueOrDefault());

            if (lstInventarios == null)
                return RedirectToAction("ReportesInventarios", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Código");
            dt.Columns.Add("Item");
            dt.Columns.Add("Unidad Medida");
            dt.Columns.Add("Categoría");
            dt.Columns.Add("Lote");
            dt.Columns.Add("Vencimiento");
            dt.Columns.Add("Stock Por Lote");
            dt.Columns.Add("Stock Por Item");
            dt.Columns.Add("Ubicación");

            foreach (var obj in lstInventarios)
            {
                DataRow row = dt.NewRow();
                row["Código"] = obj.nItemCodigo;
                row["Item"] = obj.nItem;
                row["Unidad Medida"] = obj.UnidadMedida;
                row["Categoría"] = obj.nCategoria;
                row["Lote"] = obj.SerieLote;
                row["Vencimiento"] = obj.FechaFin != null ? obj.FechaFin.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                row["Stock Por Lote"] = obj.StockLote;
                row["Stock Por Item"] = obj.SaldoItem;
                row["Ubicación"] = obj.nUbicacion;
                dt.Rows.Add(row);
            }

            GenerarPdf7(dt, "Stock de Items con Fecha de Vencimiento", "StockDeItemsConFechaDeVencimiento", objEmpresa, rFechaFin, Response);

            return RedirectToAction("ReportesInventarios", new { message = 2 });
        }
        public ActionResult GenerarRep_Stock_De_Items(DateTime? FechaFin)
        {
            if (FechaFin == null)
            {
                return RedirectToAction("ReportesInventarios", new { message = 1 });
            }

            //FechaFin = DateTime.Now;

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            Reportes_InventariosBL repBL = new Reportes_InventariosBL();
            List<ItemDTO> lista = repBL.Get_Reporte_Stock_De_Items(objEmpresa.IdEmpresa, FechaFin.GetValueOrDefault());


            if (lista == null)
                return RedirectToAction("ReportesInventarios", new { message = 2 });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Código");
            dt.Columns.Add("Item");
            dt.Columns.Add("Unidad Medida");
            dt.Columns.Add("Categoría");
            dt.Columns.Add("Stock por Item");

            foreach (var obj in lista)
            {
                DataRow row = dt.NewRow();
                row["Código"] = obj.Codigo;
                row["Item"] = obj.Nombre;
                row["Unidad Medida"] = obj.UnidadMedida;
                row["Categoría"] = obj.nCategoriaItem;
                row["Stock por Item"] = obj.SaldoItem;
                dt.Rows.Add(row);
            }

            GenerarPdf7(dt, "Stock Por Items", "StockPorItems", objEmpresa, FechaFin, Response);

            return RedirectToAction("ReportesInventarios", new { message = 2 });
        }

        private static void GenerarPdf(DataTable dt, string titulo, string nombreDoc, EmpresaDTO objEmpresa, DateTime? FechaInicio, DateTime? FechaFin, HttpResponseBase Response)
        {
            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, titulo + " - Empresa:" + objEmpresa.Nombre);
                //Cabecera principal
                AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 2, "PERIODO: " + FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + FechaFin.GetValueOrDefault().ToShortDateString());
                AddWhiteHeader(gv, 3, "Moneda: (" + objEmpresa.SimboloMoneda + ")");

                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreDoc + "_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
        }
        private static void GenerarPdf2(DataTable dt, string titulo, string nombreDoc, EmpresaDTO objEmpresa, DateTime? FechaInicio, DateTime? FechaFin, HttpResponseBase Response)
        {
            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, titulo);
                //Cabecera principal
                AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 2, "PERIODO: " + FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + FechaFin.GetValueOrDefault().ToShortDateString());

                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreDoc + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
        }
        private static void GenerarPdf3(DataTable dt, string titulo, string nombreDoc, string codigo, string item, EmpresaDTO objEmpresa, DateTime? FechaInicio, DateTime? FechaFin, HttpResponseBase Response)
        {
            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, titulo);
                //Cabecera principal
                AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 2, "PERIODO: " + FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + FechaFin.GetValueOrDefault().ToShortDateString());
                AddWhiteHeader(gv, 3, "CODIGO: " + codigo);
                AddWhiteHeader(gv, 4, "ITEM: " + item);

                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreDoc + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
        }
        private static void GenerarPdf4(DataTable dt, string titulo, string nombreDoc, EmpresaDTO objEmpresa, DateTime? FechaInicio, DateTime? FechaFin, HttpResponseBase Response)
        {
            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, titulo + " - Empresa:" + objEmpresa.Nombre);
                //Cabecera principal
                AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 2, "PERIODO: " + FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + FechaFin.GetValueOrDefault().ToShortDateString());

                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreDoc + "_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
        }
        private static void GenerarPdf5(DataTable dt, string titulo, string nombreDoc, EmpresaDTO objEmpresa, HttpResponseBase Response)
        {
            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, titulo + " - Empresa:" + objEmpresa.Nombre);
                //Cabecera principal
                //AddWhiteHeader(gv, 1, "");
                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreDoc + "_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
        }
        private static void GenerarPdf6(DataTable dt, string titulo, string nombreDoc, EmpresaDTO objEmpresa, HttpResponseBase Response)
        {
            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, titulo + " - Empresa:" + objEmpresa.Nombre);
                //Cabecera principal
                //AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 1, "A&ntilde;o: " + DateTime.Now.Year);
                AddWhiteHeader(gv, 2, "Moneda: (" + objEmpresa.SimboloMoneda + ")");
                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreDoc + "_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
        }
        private static void GenerarPdf7(DataTable dt, string titulo, string nombreDoc, EmpresaDTO objEmpresa, DateTime? FechaFin, HttpResponseBase Response)
        {
            GridView gv = new GridView();

            gv.DataSource = dt;
            gv.AllowPaging = false;
            gv.DataBind();

            if (dt.Rows.Count > 0)
            {
                PintarCabeceraTabla(gv);
                //PintarIntercaladoCategorias(gv);

                AddSuperHeader(gv, titulo + " - Empresa:" + objEmpresa.Nombre);
                //Cabecera principal
                AddWhiteHeader(gv, 1, "");
                AddWhiteHeader(gv, 2, "FECHA LIMITE: " + FechaFin.GetValueOrDefault().ToShortDateString());

                //PintarCategorias(gv);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreDoc + "_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
        }
        private static void PintarGastoPorPartidaPresupuesto(List<CategoriaR_DTO> lista, DataTable dt)
        {
            foreach (var obj in lista)
            {
                DataRow row1 = dt.NewRow();
                row1["Nivel"] = obj.Nivel;
                row1["Partida"] = obj.Nombre;
                dt.Rows.Add(row1);
                foreach (var com in obj.Comprobantes)
                {
                    DataRow row2 = dt.NewRow();
                    row2["Fecha"] = com.Fecha.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                    row2["Entidad"] = com.NombreEntidad;
                    row2["Documento"] = com.NombreDocumento;
                    row2["# Documento"] = com.NroDocumento;
                    row2["Moneda"] = com.Moneda;
                    row2["Monto Sin IGV"] = com.MontoSinIGV.ToString("N2", CultureInfo.InvariantCulture);
                    row2["Area(s)"] = com.Areas;
                    row2["Comentario"] = com.Comentario;

                    dt.Rows.Add(row2);
                }
                PintarGastoPorPartidaPresupuesto(obj.Hijos.ToList(), dt);
            }
        }
        private static void PintarArbolPadre(CategoriaDTO obj, List<CategoriaDTO> lstCatMontos, EmpresaDTO objEmpresa, System.Data.DataTable dt)
        {
            System.Data.DataRow row = dt.NewRow();
            row["Nivel"] = obj.Nivel;
            row["Partida de Presupuesto"] = obj.Nombre;
            Decimal pMonto = lstCatMontos.SingleOrDefault(x => x.IdCategoria == obj.IdCategoria).Presupuesto.GetValueOrDefault();
            row["MONTO SIN IGV"] = pMonto.ToString("N2", CultureInfo.InvariantCulture);
            row["PRESUPUESTO SIN IGV"] = obj.Presupuesto.GetValueOrDefault().ToString("N2", CultureInfo.InvariantCulture);
            Decimal porcentaje = obj.Presupuesto.GetValueOrDefault() != 0 ? pMonto / obj.Presupuesto.GetValueOrDefault() : 0;
            row["EJECUCION DEL PRESUPUESTO"] = Math.Abs(porcentaje).ToString("P2", CultureInfo.InvariantCulture);
            dt.Rows.Add(row);
            foreach (var hijo in obj.Hijos)
            {
                PintarArbolPadre(hijo, lstCatMontos, objEmpresa, dt);
            }
        }
        private static void PintarAreas(AreaDTO obj, Decimal SumaTotal, System.Data.DataTable dt)
        {
            System.Data.DataRow row1 = dt.NewRow();
            row1[0] = obj.Nombre;
            row1["Monto con IGV"] = obj.SumaClientes.ToString("N2", CultureInfo.InvariantCulture);
            row1["Monto sin IGV"] = obj.SumaClientes_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            Decimal porcentaje = SumaTotal == 0 ? 0 : obj.SumaClientes / SumaTotal;
            row1["Porcentaje"] = porcentaje.ToString("P2", CultureInfo.InvariantCulture);
            dt.Rows.Add(row1);

            foreach (var item in obj.lstClientes)
            {
                if (item.Monto != 0)
                {
                    System.Data.DataRow row2 = dt.NewRow();
                    row2[1] = item.Nombre;
                    row2["Monto con IGV"] = item.Monto.ToString("N2", CultureInfo.InvariantCulture);
                    row2["Monto sin IGV"] = item.MontoSinIGV.ToString("N2", CultureInfo.InvariantCulture);
                    dt.Rows.Add(row2);
                }
            }
        }
        private static void PintarAreasIE(AreaDTO obj, System.Data.DataTable dt)
        {
            DataRow row1 = dt.NewRow();
            row1[0] = obj.Nombre.ToUpper();
            dt.Rows.Add(row1);
            DataRow row2 = dt.NewRow();
            row2[1] = "VENTAS";
            row2[2] = obj.Ingresos.ToString("N2", CultureInfo.InvariantCulture);
            row2[3] = obj.Ingresos_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(row2);
            DataRow row3 = dt.NewRow();
            row3[1] = "GASTOS";
            row3[2] = obj.Egresos.ToString("N2", CultureInfo.InvariantCulture);
            row3[3] = obj.Egresos_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(row3);
            DataRow row4 = dt.NewRow();
            row4[0] = "NETO";
            row4[2] = (obj.Ingresos + obj.Egresos).ToString("N2", CultureInfo.InvariantCulture);
            row4[3] = (obj.Ingresos_SinIGV + obj.Egresos_SinIGV).ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(row4);
        }
        private static void PintarGestionPorMesIE(LiquidezDTO obj, System.Data.DataTable dt)
        {
            DataRow row1 = dt.NewRow();
            row1[0] = obj.nombreMes;
            dt.Rows.Add(row1);
            DataRow row2 = dt.NewRow();
            row2[1] = "VENTAS";
            row2[2] = obj.Ingreso.ToString("N2", CultureInfo.InvariantCulture);
            row2[3] = obj.Ingreso_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(row2);
            DataRow row3 = dt.NewRow();
            row3[1] = "GASTOS";
            row3[2] = obj.Egreso.ToString("N2", CultureInfo.InvariantCulture);
            row3[3] = obj.Egreso_SinIGV.ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(row3);
            DataRow row4 = dt.NewRow();
            row4[1] = "NETO";
            row4[2] = (obj.Ingreso + obj.Egreso).ToString("N2", CultureInfo.InvariantCulture);
            row4[3] = (obj.Ingreso_SinIGV + obj.Egreso_SinIGV).ToString("N2", CultureInfo.InvariantCulture);
            dt.Rows.Add(row4);
        }
        #endregion

        #region Exportar Detalles
        public ActionResult ExportarLibros(int idTipoCuenta, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_FILE_DETAIL);
                return RedirectToAction("Libros", "Admin");
            }

            string tipo = idTipoCuenta == 1 ? "Bancarios" : "Administrativos";
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<CuentaBancariaDTO> lstCuentas = repBL.getCuentasBancariasEnEmpresa(objEmpresa.IdEmpresa, idTipoCuenta, FechaInicio.GetValueOrDefault(), FechaFin.GetValueOrDefault());

            if (lstCuentas == null || lstCuentas.Count == 0)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_EMPTY);
                return RedirectToAction("Libros" + tipo, "Admin");
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Nombre");
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Moneda");
            string miSaldo = idTipoCuenta == 1 ? "Saldo Disponible" : "Saldo Total";
            dt.Columns.Add(miSaldo);
            if (idTipoCuenta == 1) { dt.Columns.Add("Saldo Bancario"); }
            dt.Columns.Add("Estado");

            foreach (var obj in lstCuentas)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Nombre"] = obj.NombreCuenta;
                row["Fecha"] = obj.FechaConciliacion.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                row["Moneda"] = obj.SimboloMoneda;
                row[3] = obj.SaldoDisponible.ToString("N2", CultureInfo.InvariantCulture);
                if (idTipoCuenta == 1) { row["Saldo Bancario"] = obj.SaldoBancario.ToString("N2", CultureInfo.CreateSpecificCulture("en-GB")); }
                row["Estado"] = obj.Estado ? "Activo" : "Inactivo";
                dt.Rows.Add(row);
            }

            GenerarPdf(dt, "Detalle de Libros", "DetalleLibros", objEmpresa, FechaInicio, FechaFin, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Libros" + tipo, "Admin");
        }

        public ActionResult ExportarMovimientos(int idLibro, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_FILE_DETAIL);
                return RedirectToAction("Libros", "Admin");
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            CuentaBancariaDTO CuentaBancaria = repBL.getCuentaBancaria(idLibro, FechaInicio.GetValueOrDefault(), FechaFin.GetValueOrDefault());

            if (CuentaBancaria == null || CuentaBancaria.listaMovimiento.Count == 0)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_EMPTY);
                return RedirectToAction("Libros", "Admin");
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Fecha");
            dt.Columns.Add("Movimiento");
            dt.Columns.Add("Detalle");
            dt.Columns.Add("Moneda");
            dt.Columns.Add("Monto");
            dt.Columns.Add("Partida de Presupuesto");
            dt.Columns.Add("Entidad");
            dt.Columns.Add("Numero de documento");
            dt.Columns.Add("Estado");
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Activo");
            dt.Columns.Add("Comentario");

            foreach (var obj in CuentaBancaria.listaMovimiento)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Fecha"] = obj.Fecha.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")); ;
                row["Movimiento"] = obj.IdTipoMovimiento == 1 ? "Entrada" : "Salida";
                row["Detalle"] = obj.NroOperacion;
                row["Moneda"] = CuentaBancaria.SimboloMoneda;
                row["Monto"] = obj.IdTipoMovimiento == 1 ? obj.Monto.ToString("N2", CultureInfo.InvariantCulture) : (-1 * obj.Monto).ToString("N2", CultureInfo.InvariantCulture);
                row["Partida de Presupuesto"] = obj.NombreCategoria == null ? "N/A" : obj.NombreCategoria;
                row["Entidad"] = obj.NombreEntidadR;
                row["Numero de documento"] = obj.NumeroDocumento != null ? obj.NumeroDocumento : obj.NumeroDocumento2 != null ? obj.NumeroDocumento2 : "N/A";
                row["Estado"] = obj.IdEstadoMovimiento == 1 ? "Pendiente" : "Realizado";
                row["Usuario"] = obj.NombreUsuario;
                row["Activo"] = obj.Estado ? "Activo" : "Inactivo";
                row["Comentario"] = obj.Comentario;
                dt.Rows.Add(row);
            }

            GenerarPdf(dt, "Detalle de Movimientos en la Cuenta - " + CuentaBancaria.NombreCuenta, "DetalleMovimientosEnLibro", objEmpresa, FechaInicio, FechaFin, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Libros", "Admin");
        }

        public ActionResult ExportarComprobantes(int idTipoComprobante, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_FILE_DETAIL);
                return RedirectToAction("Index", "Admin");
            }

            string sTipoComprobante = idTipoComprobante == 1 ? "Ingreso" : "Egreso";
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<ComprobanteDTO> lstComprobantes = repBL.getComprobantesEnEmpresa(objEmpresa.IdEmpresa, idTipoComprobante, FechaInicio.GetValueOrDefault(), FechaFin.GetValueOrDefault());

            if (lstComprobantes == null || lstComprobantes.Count == 0)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_EMPTY);
                return RedirectToAction("Comprobantes", "Admin");
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            var rFechaFin = idTipoComprobante == 1 ? "Fecha de Cobro" : "Fecha de Pago";
            var strEntidad = idTipoComprobante == 1 ? "Cliente" : "Proveedor";

            dt.Columns.Add("Fecha");
            dt.Columns.Add("Documento");
            dt.Columns.Add("Numero");
            dt.Columns.Add(strEntidad);
            if (idTipoComprobante == 1)
            { dt.Columns.Add("Proyecto"); }
            dt.Columns.Add("Moneda");
            dt.Columns.Add("Monto Sin IGV");
            dt.Columns.Add("Monto Total (MN)");
            dt.Columns.Add("Monto Total (ME)");
            dt.Columns.Add("Tipo Cambio");
            dt.Columns.Add("Partida de Presupuesto");
            dt.Columns.Add(rFechaFin);
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Estado");
            dt.Columns.Add("Status");
            dt.Columns.Add("Comentario");

            foreach (var obj in lstComprobantes)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Fecha"] = obj.FechaEmision.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                row["Documento"] = obj.NombreTipoDocumento;
                row["Numero"] = obj.NroDocumento;
                row[strEntidad] = obj.NombreEntidad;
                if (idTipoComprobante == 1) { row["Proyecto"] = obj.NombreProyecto; }
                row["Moneda"] = obj.SimboloMoneda;
                row["Monto Sin IGV"] = obj.MontoSinIGV.ToString("N2", CultureInfo.InvariantCulture);
                row["Monto Total (MN)"] = obj.IdMoneda == 1 ? obj.Monto.ToString("N2", CultureInfo.InvariantCulture) : "";
                row["Monto Total (ME)"] = obj.IdMoneda != 1 ? obj.Monto.ToString("N2", CultureInfo.InvariantCulture) : "";
                row["Tipo Cambio"] = obj.TipoCambio.ToString("N2", CultureInfo.InvariantCulture);
                row["Partida de Presupuesto"] = obj.NombreCategoria;
                row[rFechaFin] = obj.FechaConclusion != null ? obj.FechaConclusion.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                row["Usuario"] = obj.NombreUsuario;
                row["Estado"] = obj.Estado ? "Activo" : "Inactivo";
                row["Status"] = obj.Ejecutado ? "Cancelado" : "Pendiente";
                row["Comentario"] = obj.Comentario;
                dt.Rows.Add(row);
            }

            GenerarPdf(dt, "Detalle de Comprobantes", "DetalleComprobantes", objEmpresa, FechaInicio, FechaFin, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Comprobantes" + sTipoComprobante, "Admin");
        }

        public ActionResult ExportarComprobantesAnulados(DateTime FechaInicio, DateTime FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_FILE_DETAIL);
                return RedirectToAction("Index", "Admin");
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<ComprobanteDTO> listaTipo3 = repBL.getComprobantesEnEmpresa(objEmpresa.IdEmpresa, 3, FechaInicio, FechaFin);
            List<ComprobanteDTO> listaTipo4 = repBL.getComprobantesEnEmpresa(objEmpresa.IdEmpresa, 4, FechaInicio, FechaFin);

            listaTipo3.AddRange(listaTipo4);

            if (listaTipo3 == null || listaTipo3.Count == 0)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_EMPTY);
                return RedirectToAction("ComprobantesAnulados", "Admin");
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            var rFechaFin = "Fecha de Cobro / Pago";
            var strEntidad = "Cliente / Proveedor";

            dt.Columns.Add("Fecha");
            dt.Columns.Add("Documento");
            dt.Columns.Add("Numero");
            dt.Columns.Add(strEntidad);
            /*if (idTipoComprobante == 3)
            { dt.Columns.Add("Proyecto"); }*/
            dt.Columns.Add("Moneda");
            dt.Columns.Add("Monto Sin IGV");
            dt.Columns.Add("Monto Total (MN)");
            dt.Columns.Add("Monto Total (ME)");
            dt.Columns.Add("Tipo Cambio");
            dt.Columns.Add("Partida de Presupuesto");
            dt.Columns.Add(rFechaFin);
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Estado");
            dt.Columns.Add("Status");
            dt.Columns.Add("Comentario");

            foreach (var obj in listaTipo3)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Fecha"] = obj.FechaEmision.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                row["Documento"] = obj.NombreTipoDocumento;
                row["Numero"] = obj.NroDocumento;
                row[strEntidad] = obj.NombreEntidad;
                /*if (idTipoComprobante == 1) { row["Proyecto"] = obj.NombreProyecto; }*/
                row["Moneda"] = obj.SimboloMoneda;
                row["Monto Sin IGV"] = obj.MontoSinIGV.ToString("N2", CultureInfo.InvariantCulture);
                row["Monto Total (MN)"] = obj.IdMoneda == 1 ? obj.Monto.ToString("N2", CultureInfo.InvariantCulture) : "";
                row["Monto Total (ME)"] = obj.IdMoneda != 1 ? obj.Monto.ToString("N2", CultureInfo.InvariantCulture) : "";
                row["Tipo Cambio"] = obj.TipoCambio.ToString("N2", CultureInfo.InvariantCulture);
                row["Partida de Presupuesto"] = obj.NombreCategoria;
                row[rFechaFin] = obj.FechaConclusion != null ? obj.FechaConclusion.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                row["Usuario"] = obj.NombreUsuario;
                row["Estado"] = obj.Estado ? "Activo" : "Inactivo";
                row["Status"] = obj.Ejecutado ? "Cancelado" : "Pendiente";
                row["Comentario"] = obj.Comentario;
                dt.Rows.Add(row);
            }

            GenerarPdf(dt, "Detalle de Comprobantes Anulados", "DetalleComprobantesAnulados", objEmpresa, FechaInicio, FechaFin, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("ComprobantesAnulados", "Admin");
        }

        public ActionResult ExportarClientesOProveedores(int tipo)
        {
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            EntidadResponsableBL objBL = new EntidadResponsableBL();
            List<EntidadResponsableDTO> lista = objBL.getEntidadesResponsablesPorTipoEnEmpresa(objEmpresa.IdEmpresa, tipo);

            if (lista == null || lista.Count == 0)
            {
                //createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_EMPTY);
                return RedirectToAction("Comprobantes", "Admin");
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Tipo");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Nombre Comercial");
            dt.Columns.Add("Tipo de Identificación");
            dt.Columns.Add("Identificación");
            dt.Columns.Add("Telefono 1");
            dt.Columns.Add("Telefono 2");
            dt.Columns.Add("Dirección");
            dt.Columns.Add("Crédito (Días)");
            dt.Columns.Add("Consultor");
            if(tipo == 2)
            {
                dt.Columns.Add("Detracción");
                dt.Columns.Add("Tipo Bien o Servicio");
                dt.Columns.Add("Banco");
                dt.Columns.Add("Cuenta Soles");
                dt.Columns.Add("Cuenta Dólares");
            }
            dt.Columns.Add("Comentario");
            dt.Columns.Add("Estado");

            foreach (var obj in lista)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Tipo"] = obj.TipoPersona == 1 ? "Jurídica" : "Natural";
                row["Nombre"] = obj.Nombre;
                row["Nombre Comercial"] = obj.NombreComercial;
                row["Tipo de Identificación"] = obj.TipoIdentificacion;
                row["Identificación"] = obj.NroIdentificacion;
                row["Telefono 1"] = obj.Telefono1;
                row["Telefono 2"] = obj.Telefono2;
                row["Dirección"] = obj.Direccion;
                row["Crédito (Días)"] = obj.Credito;
                row["Consultor"] = obj.nResponsable;
                if (tipo == 2)
                {
                    row["Detracción"] = obj.Detraccion;
                    row["Tipo Bien o Servicio"] = obj.Tipo;
                    row["Banco"] = obj.Banco;
                    row["Cuenta Soles"] = obj.CuentaSoles;
                    row["Cuenta Dólares"] = obj.CuentaDolares;
                }
                row["Comentario"] = obj.Comentario;
                row["Estado"] = obj.Estado ? "Activo" : "Inactivo";
                dt.Rows.Add(row);
            }

            string sCadena = tipo == 1 ? "Clientes" : "Proveedores";

            GenerarPdf5(dt, "Detalle de " + sCadena, "DetalleDe" + sCadena, objEmpresa, Response);

            //createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Entidades" + sCadena, "Admin");

        }

        public ActionResult ExportarMovimientosInv(int idTipo, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_FILE_DETAIL);
                return RedirectToAction("Index", "Admin");
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            Reportes_InventariosBL objBL = new Reportes_InventariosBL();
            List<MovimientoInvDTO> lstMovsInv = objBL.getMovimientoInvsEnEmpresaPorTipo(objEmpresa.IdEmpresa, idTipo, FechaInicio.GetValueOrDefault(), FechaFin.GetValueOrDefault());
            string sTipo = idTipo == 1 ? "Ingreso" : "Egreso";

            if (lstMovsInv == null || lstMovsInv.Count == 0)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_EMPTY);
                return RedirectToAction("Inventarios" + sTipo, "Admin");
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Fecha");
            dt.Columns.Add("Movimiento");
            dt.Columns.Add("Documento");
            dt.Columns.Add("Cod-Item");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("U. Med");
            dt.Columns.Add("Lote");
            dt.Columns.Add("Stock Lote");
            if (idTipo == 1)
            {
                dt.Columns.Add("Guia de Remision");
                dt.Columns.Add("Vencimiento");
                dt.Columns.Add("Proveedor");
            }
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Ubicacion");
            dt.Columns.Add("Comentario");

            foreach (var obj in lstMovsInv)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Fecha"] = obj.FechaInicial.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                row["Movimiento"] = obj.nForma;
                row["Documento"] = obj.NroDocumento;
                row["Cod-Item"] = obj.nItem;
                row["Cantidad"] = obj.Cantidad;
                row["U. Med"] = obj.UnidadMedida;
                row["Lote"] = obj.SerieLote;
                row["Stock Lote"] = obj.StockLote;
                if (idTipo == 1)
                {
                    row["Guia de Remision"] = obj.GuiaRemision;
                    row["Vencimiento"] = obj.FechaFin != null ? obj.FechaFin.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "-";
                    row["Proveedor"] = obj.nEntidadResponsable;
                }
                row["Usuario"] = obj.nUsuario;
                row["Ubicacion"] = obj.nUbicacion;
                row["Comentario"] = obj.Comentario;
                dt.Rows.Add(row);
            }

            GenerarPdf4(dt, "Detalle de Inventarios de " + sTipo, "DetalleInventarios" + sTipo, objEmpresa, FechaInicio, FechaFin, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Inventarios" + sTipo, "Admin");
        }

        public ActionResult ExportarComprobantesAsociados_EnEntidad(int idEntidad, DateTime FechaInicio, DateTime FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_FILE_DETAIL);
                return RedirectToAction("Entidad", "Admin", new { id = idEntidad });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<ComprobanteDTO> lista = repBL.getComprobantes_ConEntidad(objEmpresa.IdEmpresa, idEntidad, FechaInicio, FechaFin);

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Fecha");
            dt.Columns.Add("Modalidad");
            dt.Columns.Add("Documento");
            dt.Columns.Add("Numero");
            dt.Columns.Add("Proyecto");
            dt.Columns.Add("Monto Total");

            foreach (var obj in lista)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Fecha"] = obj.FechaEmision.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                row["Modalidad"] = obj.IdTipoComprobante == 1 ? "Ingreso" : "Egreso";
                row["Documento"] = obj.NombreTipoDocumento;
                row["Numero"] = obj.NroDocumento;
                row["Proyecto"] = obj.NombreProyecto;
                row["Monto Total"] = obj.Monto.ToString("N2", CultureInfo.InvariantCulture);
                dt.Rows.Add(row);
            }
            GenerarPdf(dt, "Comprobantes Asociados a Entidad", "ComprobantesAsociados_A_Entidad", objEmpresa, FechaInicio, FechaFin, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Entidad", "Admin", new { id = idEntidad });
        }

        public ActionResult ExportarComprobantesAsociados_EnProyecto(int idProyecto, int idEntidadResponsable, DateTime FechaInicio, DateTime FechaFin)
        {
            if (FechaInicio == null || FechaFin == null)
            {
                createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_FILE_DETAIL);
                return RedirectToAction("Proyecto", "Admin", new { id = idProyecto, idEntidad = idEntidadResponsable });
            }

            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ReportesBL repBL = new ReportesBL();
            List<ComprobanteDTO> lista = repBL.getComprobantes_ConProyecto(objEmpresa.IdEmpresa, idProyecto, FechaInicio, FechaFin);

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Fecha");
            dt.Columns.Add("Modalidad");
            dt.Columns.Add("Documento");
            dt.Columns.Add("Numero");
            dt.Columns.Add("Proyecto");
            dt.Columns.Add("Monto Total");

            foreach (var obj in lista)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Fecha"] = obj.FechaEmision.ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB"));
                row["Modalidad"] = obj.IdTipoComprobante == 1 ? "Ingreso" : "Egreso";
                row["Documento"] = obj.NombreTipoDocumento;
                row["Numero"] = obj.NroDocumento;
                row["Proyecto"] = obj.NombreProyecto;
                row["Monto Total"] = obj.Monto.ToString("N2", CultureInfo.InvariantCulture);
                dt.Rows.Add(row);
            }
            GenerarPdf(dt, "Comprobantes Asociados a Proyecto", "ComprobantesAsociados_A_Proyecto", objEmpresa, FechaInicio, FechaFin, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Proyecto", "Admin", new { id = idProyecto, idEntidad = idEntidadResponsable });
        }

        public ActionResult ExportarItems()
        {
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ItemBL objBL = new ItemBL();
            List<ItemDTO> lista = objBL.getItemsEnEmpresa(getCurrentUser().IdEmpresa);

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Nombre");
            dt.Columns.Add("Código");
            dt.Columns.Add("Descripción");
            dt.Columns.Add("Unidad de Medida");
            dt.Columns.Add("Categoría");
            dt.Columns.Add("Moneda");
            dt.Columns.Add("Precio");
            dt.Columns.Add("Estado");

            foreach (var item in lista)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Nombre"] = item.Nombre;
                row["Código"] = item.Codigo;
                row["Descripción"] = item.Descripcion;
                row["Unidad de Medida"] = item.UnidadMedida;
                row["Categoría"] = item.nCategoriaItem;
                row["Moneda"] = item.nMoneda;
                row["Precio"] = item.Precio.GetValueOrDefault().ToString("N2", CultureInfo.InvariantCulture);
                row["Estado"] = item.Estado ? "Activo" : "Inactivo";
                dt.Rows.Add(row);
            }
            
            GenerarPdf5(dt, "Detalle de Items", "Detalle_de_Items", objEmpresa, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Items", "Admin");
        }

        public ActionResult ExportarCategoriaItms()
        {
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            CategoriaItmBL objBL = new CategoriaItmBL();
            List<CategoriaItmDTO> lista = objBL.getCategoriaItmsEnEmpresa(getCurrentUser().IdEmpresa);

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Nombre");
            dt.Columns.Add("Estado");

            foreach (var item in lista)
            {
                DataRow row = dt.NewRow();
                row["Nombre"] = item.Nombre;
                row["Estado"] = item.Estado ? "Activo" : "Inactivo";
                dt.Rows.Add(row);
            }

            GenerarPdf5(dt, "Detalle de Categorias de Items", "Detalle_de_CategoriaItems", objEmpresa, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Items", "Admin");
        }

        public ActionResult ExportarUbicaciones()
        {
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            UbicacionBL objBL = new UbicacionBL();
            List<UbicacionDTO> lista = objBL.getUbicacionsEnEmpresa(getCurrentUser().IdEmpresa);

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Nombre");
            dt.Columns.Add("Estado");

            foreach (var item in lista)
            {
                DataRow row = dt.NewRow();
                row["Nombre"] = item.Nombre;
                row["Estado"] = item.Estado ? "Activo" : "Inactivo";
                dt.Rows.Add(row);
            }

            GenerarPdf5(dt, "Detalle de Ubicaciones", "Detalle_de_Ubicaciones", objEmpresa, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Items", "Admin");
        }

        public ActionResult ExportarServicios()
        {
            EmpresaDTO objEmpresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ServicioBL objBL = new ServicioBL();
            List<ServicioDTO> lista = objBL.getServiciosEnEmpresa(getCurrentUser().IdEmpresa);

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Nombre");
            dt.Columns.Add("Código");
            dt.Columns.Add("Descripción");
            dt.Columns.Add("Moneda");
            dt.Columns.Add("Precio");
            dt.Columns.Add("Estado");

            foreach (var Servicio in lista)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Nombre"] = Servicio.Nombre;
                row["Código"] = Servicio.Codigo;
                row["Descripción"] = Servicio.Descripcion;
                row["Moneda"] = Servicio.nMoneda;
                row["Precio"] = Servicio.Precio.GetValueOrDefault().ToString("N2", CultureInfo.InvariantCulture);
                row["Estado"] = Servicio.Estado ? "Activo" : "Inactivo";
                dt.Rows.Add(row);
            }

            GenerarPdf5(dt, "Detalle de Servicios", "Detalle_de_Servicios", objEmpresa, Response);

            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Servicios", "Admin");
        }

        #endregion
        private static void AddSuperHeader(GridView gridView, string text = null)
        {
            var myTable = (Table)gridView.Controls[0];
            var myNewRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            myNewRow.Cells.Add(MakeCell(text, gridView.HeaderRow.Cells.Count));//gridView.Columns.Count
            myNewRow.Cells[0].Style.Add("background-color", "#cbcfd6");
            myTable.Rows.AddAt(0, myNewRow);
            //myTable.EnableViewState = false;
        }
        private static void AddHeader(GridView gridView, int index, string text = null)
        {
            var myTable = (Table)gridView.Controls[0];
            var myNewRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            myNewRow.Cells.Add(MakeCell(text, gridView.HeaderRow.Cells.Count));//gridView.Columns.Count
            myNewRow.Cells[0].Style.Add("background-color", "#cbcfd6");
            myNewRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            myTable.Rows.AddAt(index, myNewRow);
            //myTable.EnableViewState = false;
        }
        private static void AddWhiteHeader(GridView gridView, int index, string text = null)
        {
            var myTable = (Table)gridView.Controls[0];
            var myNewRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            myNewRow.Cells.Add(MakeCell(text, gridView.HeaderRow.Cells.Count));//gridView.Columns.Count
            myNewRow.Cells[0].Style.Add("background-color", "#ffffff");
            myNewRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            myTable.Rows.AddAt(index, myNewRow);
        }
        private static void PintarCabeceraTabla(GridView gridView)
        {
            var myTable = (Table)gridView.Controls[0];
            foreach (GridViewRow row in myTable.Rows)
            {
                if (row.Cells.Count >= 3)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Text = row.Cells[i].Text.ToUpper();
                        row.Cells[i].BackColor = System.Drawing.Color.FromArgb(189, 195, 199);
                        row.Cells[i].Font.Bold = true;
                    }
                    break;
                }
            }
        }
        private static void PintarIntercaladoCategorias(GridView gridView)
        {
            var myTable = (Table)gridView.Controls[0];

            //Colores
            System.Drawing.Color colorPadre = new System.Drawing.Color();
            colorPadre = System.Drawing.Color.FromArgb(255, 255, 255);
            System.Drawing.Color colorSub = new System.Drawing.Color();
            colorSub = System.Drawing.Color.FromArgb(255, 255, 255);

            bool blancoActive = true;
            bool blancoActive2 = true;
            string cadenaPadre = "";
            string cadenaSub = "";

            int contPadre = 0;
            foreach (GridViewRow row in myTable.Rows)
            {
                if (cadenaPadre != row.Cells[0].Text)
                {
                    if (!blancoActive)
                    {
                        colorPadre = System.Drawing.Color.FromArgb(229, 231, 235);
                        blancoActive = true;
                    }
                    else
                    {
                        colorPadre = System.Drawing.Color.FromArgb(255, 255, 255);
                        blancoActive = false;
                    }
                    cadenaPadre = row.Cells[0].Text;
                }
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (i == 0)
                    {
                        row.Cells[i].BackColor = colorPadre;
                    }
                    else
                    {
                        if (i == 1)
                        {
                            if (cadenaSub != row.Cells[i].Text)
                            {
                                if (!blancoActive2)
                                {
                                    colorSub = System.Drawing.Color.FromArgb(229, 231, 235);
                                    blancoActive2 = true;
                                }
                                else
                                {
                                    colorSub = System.Drawing.Color.FromArgb(255, 255, 255);
                                    blancoActive2 = false;
                                }
                                cadenaSub = row.Cells[i].Text;
                            }
                        }
                        row.Cells[i].BackColor = colorSub;
                    }
                }
                contPadre++;
            }
        }
        private static void PintarColumnaNegrita(GridView gridView, int columna, bool intercalado)
        {
            var myTable = (Table)gridView.Controls[0];
            bool impar = false;
            foreach (GridViewRow row in myTable.Rows)
            {
                if (row.Cells[columna].Text != null)
                {
                    row.Cells[columna].Font.Bold = true;
                }
                if (intercalado)
                {
                    if (impar)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            row.Cells[i].BackColor = System.Drawing.Color.FromArgb(229, 231, 235);
                        }
                        impar = false;
                    }
                    else
                    {
                        impar = true;
                    }
                }
            }
        }
        private static void PintarCategorias(GridView gridView)
        {
            var myTable = (Table)gridView.Controls[0];

            foreach (GridViewRow row in myTable.Rows)
            {
                if (row.Cells.Count >= 2)
                {
                    string cadena0 = row.Cells[0].Text;
                    string cadena1 = row.Cells[1].Text;
                    if (cadena0 == "TOTAL :")
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            row.Cells[i].BackColor = System.Drawing.Color.FromArgb(71, 229, 199);
                            row.Cells[i].Font.Bold = true;
                        }
                    }
                    if (cadena1 == "TOTAL :")
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (i == 0)
                            {
                                row.Cells[i].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                                row.Cells[i].Font.Bold = true;
                            }
                            else
                            {
                                row.Cells[i].BackColor = System.Drawing.Color.FromArgb(95, 174, 227);
                                row.Cells[i].Font.Bold = true;
                            }
                        }
                    }
                }
            }
        }
        private static TableHeaderCell MakeCell(string text = null, int span = 1)
        {
            return new TableHeaderCell() { ColumnSpan = span, Text = text ?? string.Empty, CssClass = "table-header" };
        }
        private static Decimal DameTotalSoles(List<CuentaBancariaDTO> listaLibros)
        {
            Decimal Total = 0;
            foreach (var libro in listaLibros)
            {
                if (libro.IdMoneda.GetValueOrDefault() == 1 && libro.Estado && libro.IdTipoCuenta == 1) //Soles
                {
                    Total += libro.SaldoDisponible;
                }
            }
            return Total;
        }
        private static Decimal DameTotalDolares(List<CuentaBancariaDTO> listaLibros)
        {
            Decimal Total = 0;
            foreach (var libro in listaLibros)
            {
                if (libro.IdMoneda.GetValueOrDefault() == 2 && libro.Estado && libro.IdTipoCuenta == 1) //Dolares
                {
                    Total += libro.SaldoDisponible;
                }
            }
            return Total;
        }
        private static Decimal DameTotalConsolidado(List<CuentaBancariaDTO> listaLibros, Decimal TipoCambio)
        {
            Decimal Total = 0;

            foreach (var libro in listaLibros)
            {
                if (libro.IdMoneda.GetValueOrDefault() == 1 && libro.Estado && libro.IdTipoCuenta == 1) //Soles
                {
                    Total += libro.SaldoDisponible;
                }
            }
            foreach (var libro in listaLibros)
            {
                if (libro.IdMoneda.GetValueOrDefault() == 2 && libro.Estado && libro.IdTipoCuenta == 1) //Dolares
                {
                    Total += libro.SaldoDisponible * TipoCambio;
                }
            }

            return Total;
        }

        [HttpPost]
        public string CambiarEmpresaSuperAdmin(int idEmpresa)
        {
            if (!this.currentUser() || !isSuperAdministrator()) { return "false"; }

            UsuariosBL objBL = new UsuariosBL();
            if (objBL.actualizarEmpresaSuperAdmin(getCurrentUser().IdUsuario, idEmpresa))
            {
                System.Web.HttpContext.Current.Session["User"] = objBL.getUsuario(getCurrentUser().IdUsuario);
                return "true";
            }
            return "false";
        }

        [HttpPost]
        public string ActualizarTipoCambio(Decimal tipoCambio)
        {
            if (!this.currentUser() || !isAdministrator()) { return "false"; }

            EmpresaBL objBL = new EmpresaBL();
            UsuarioDTO miUsuario = getCurrentUser();
            EmpresaDTO obj = new EmpresaDTO() { IdEmpresa = miUsuario.IdEmpresa, TipoCambio = tipoCambio };
            if (objBL.updateTipoCambio(obj))
            {
                return "true";
            }
            return "false";
        }

        [HttpPost]
        public ActionResult PasslstAreasXMontos(List<AreaPorComprobanteDTO> lista)
        {
            TempData["AreasXMontos"] = lista;
            return Json(new { success = true, mensaje = "Si funciona" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ActualizarEjecucionComprobante(int idComprobante, bool ejecutado, DateTime fecha)
        {
            if (!this.currentUser() || isUsuarioExterno()) { return "false"; }

            ComprobanteBL objBL = new ComprobanteBL();
            if (objBL.actualizarEjecutado(idComprobante, ejecutado, fecha, getCurrentUser().IdEmpresa))
            { return "true"; }
            return "false";
        }

        public void MenuNavBarSelected(int num, int? subNum = null)
        {
            navbar.clearAll();
            navbar.lstOptions[num].cadena = "active";

            if (subNum != null)
            {
                //Limpiar Activos del ultimo elemento
                navbar.lstOptions[num].lstOptions[subNum.GetValueOrDefault()].cadena = "active";
            }
            ViewBag.navbar = navbar;
        }

        #region Proformas
        public ActionResult BuscarProforma()
        {
            //UsuarioDTO currentUser = getCurrentUser();
            //var lista = new NubeBooks.Core.BL.ProformaBL().getProformaEnEmpresa(1);

            //return View(lista);
            return View();
        }
        public ActionResult Proforma(Int32 id)
        {
            /*var lista = new NubeBooks.Core.BL.ProformaBL().getProformaId(id); 
            return View(lista);*/
            return View();
        }
        public ActionResult NuevaProforma()
        {
            UsuarioDTO currentUser = getCurrentUser();
            ComprobanteBL objBL = new ComprobanteBL();
            ViewBag.lstClientes = new SelectList(objBL.getListaClientesEnEmpresa(currentUser.IdEmpresa),"IdEntidadResponsable", "Nombre");
            ViewBag.lstResponsables = new SelectList(objBL.getListaResponsablesEnEmpresa(currentUser.IdEmpresa), "IdResponsable", "Nombre");
            ViewBag.lstUbicacion = new SelectList(new UbicacionBL().getUbicacionsActivasEnEmpresa(currentUser.IdEmpresa), "IdUbicacion", "Nombre");
            ViewBag.lstItem = new SelectList(new ItemBL().getItemsActivosEnEmpresa(currentUser.IdEmpresa), "IdItem", "Nombre");

            return View();
        }
        public ActionResult SaveProforma()
        {
            /*UsuarioDTO currentUser = getCurrentUser();
            ProformaDTO Proforma = new ProformaDTO();
            TryUpdateModel(Proforma);

            Proforma.IdEmpresa = currentUser.IdEmpresa;
            if(Proforma.FechaRegistro==null){ Proforma.FechaRegistro = DateTime.Now; }
            var lista = new ProformaBL().SaveProforma(Proforma);
            return Json(lista, JsonRequestBehavior.AllowGet);*/
            return View();
        }
        #endregion
    }
}
