using NubeBooks.Core.BL;
using NubeBooks.Core.DTO;
using NubeBooks.Core.Logistics.BL;
using NubeBooks.Core.Logistics.DTO;
using NubeBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace NubeBooks.Areas.Proformas.Controllers
{
    public class ProformasController : Controller
    {
        // GET: Proformas/Proformas
        #region Variables y constructor
        private bool currentUser()
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["User"] != null) { return true; }
            else { return false; }
        }
        protected Navbar navbar { get; set; }
        private UsuarioDTO getCurrentUser()
        {
            if (this.currentUser())
            {
                return (UsuarioDTO)System.Web.HttpContext.Current.Session["User"];
            }
            return null;
        }
        private void MenuNavBarSelected(int num, int? subNum = null)
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
        public ProformasController()
        {
            UsuarioDTO user = getCurrentUser();
            if (user != null)
            {
                this.navbar = new Navbar();
                ViewBag.currentUser = user;
                ViewBag.NombreEmpresa = user.nombreEmpresa;
                ViewBag.Title = "";

                ViewBag.EsAdmin = isAdministrator();
                ViewBag.EsSuperAdmin = isSuperAdministrator();
                ViewBag.EsUsuarioInterno = isUsuarioInterno();
                ViewBag.EsUsuarioExterno = isUsuarioExterno();
                ViewBag.IdRol = user.IdRol;

                EmpresaBL empBL = new EmpresaBL();
                ViewBag.Empresas = empBL.getEmpresasViewBag();
            }
            else { ViewBag.EsAdmin = false; ViewBag.EsSuperAdmin = false; }
            CultureInfo[] cultures = { new CultureInfo("es-PE") };
        }
        #endregion
        public ActionResult Index()
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar", "Admin", new { Area = string.Empty }); }
            ViewBag.Title += "Proformas";
            MenuNavBarSelected(12);

            UsuarioDTO currentUser = getCurrentUser();
            var lista = new ProformaBL().getProformasEnEmpresa(currentUser.IdEmpresa);

            return View(lista);
        }
        public ActionResult Proforma(int? id)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar", "Admin", new { Area = string.Empty }); }
            ViewBag.Title += "Proforma";
            MenuNavBarSelected(12);

            UsuarioDTO user = getCurrentUser();


            EntidadResponsableBL entBL = new EntidadResponsableBL();
            ViewBag.lstClientes = entBL.getEntidadesResponsablesActivasPorTipoEnEmpresa(user.IdEmpresa, 1);
            EmpresaBL empBL = new EmpresaBL();
            ViewBag.lstMonedas = empBL.getListaMonedas();
            MovimientoInvBL movItmBL = new MovimientoInvBL();
            ViewBag.lstItems = movItmBL.getItemsEnEmpresa_PorTipoMov(user.IdEmpresa, 1);


            var objSent = TempData["Proforma"];
            if (objSent != null) { TempData["Proforma"] = null; return View(objSent); }

            ProformaDTO obj;
            if(id != null && id > 0)
            {
                obj = new ProformaBL().getProformaId((int)id);

                return View(obj);
            }
            obj = new ProformaDTO();
            obj.IdEmpresa = user.IdEmpresa;
            obj.FechaProforma = DateTime.Now;

            return View(obj);
        }
    }
}