using NubeBooks.Core.BL;
using NubeBooks.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NubeBooks.Areas.Proformas.Controllers
{
    public class ProformaController : Controller
    {
        // GET: Proformas/Proforma
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
        public ActionResult Index(bool inactivos = false)
        {
            //if (!this.currentUser()) { return RedirectToAction("Ingresar"); }
            //if (!isAdministrator()) { return RedirectToAction("Index"); }
            ViewBag.Title += " - Areas";
            //MenuNavBarSelected(4);
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
            //return View();
        }
    }
}