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
    public class ProformaController : Controller
    {
        // GET: Proformas/Proforma
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
        public ProformaController()
        {
            UsuarioDTO user = getCurrentUser();
            if (user != null)
            {
                this.navbar = new Navbar();
                ViewBag.currentUser = user;
                ViewBag.NombreEmpresa = user.nombreEmpresa;
                ViewBag.Title = "NubeBooks";

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
            if (!this.currentUser()) { return RedirectToAction("Ingresar", "Admin"); }
            UsuarioDTO currentUser = getCurrentUser();
            var lista = new ProformaBL().getProformaEnEmpresa(currentUser.IdEmpresa);

            ViewBag.Title += " - Buscar Proforma";
            MenuNavBarSelected(12);
            return View(lista);
        }
        public ActionResult Proforma(Int32 id)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar", "Admin"); }
            var lista = new ProformaBL().getProformaId(id);
            ViewBag.Title += " - Proforma";
            MenuNavBarSelected(12);
            return View(lista);
        }
        public ActionResult NuevaProforma(Int32? id)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar", "Admin"); }
            UsuarioDTO currentUser = getCurrentUser();
            ProformaDTO pro = new ProformaDTO();
            
            if (id.HasValue)
            {
                pro = new ProformaBL().getProformaId(id.Value);
            }
            ViewBag.Title += " - Editar Proforma";
            MenuNavBarSelected(10);
            return View(pro);
        }
        public ActionResult GetDetalleProforma(Int32 IdProforma)
        {
            var lista = new ProformaBL().getDetalleProformaPorId(IdProforma);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveProforma(ProformaDTO Proforma)
        {
            UsuarioDTO currentUser = getCurrentUser();
            //ProformaDTO Proforma = new ProformaDTO();
            //TryUpdateModel(Proforma);

            Proforma.IdEmpresa = currentUser.IdEmpresa;
            if (Proforma.FechaRegistro == null) { Proforma.FechaRegistro = DateTime.Now; }
            var lista = new ProformaBL().SaveProforma(Proforma);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClientes()
        {
            UsuarioDTO currentUser = getCurrentUser();
            ComprobanteBL objBL = new ComprobanteBL();
            var lista = objBL.getListaClientesEnEmpresa(currentUser.IdEmpresa);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConsultor()
        {
            UsuarioDTO currentUser = getCurrentUser();
            ComprobanteBL objBL = new ComprobanteBL();
            var lista = objBL.getListaResponsablesEnEmpresa(currentUser.IdEmpresa);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItem()
        {
            UsuarioDTO currentUser = getCurrentUser();
            var lista = new Core.Logistics.BL.ItemBL().getItemsActivosEnEmpresa(currentUser.IdEmpresa);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}