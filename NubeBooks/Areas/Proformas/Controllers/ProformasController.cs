using NubeBooks.Core.BL;
using NubeBooks.Core.DTO;
using NubeBooks.Core.Logistics.BL;
using NubeBooks.Core.Logistics.DTO;
using NubeBooks.Models;
using NubeBooks.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Mvc;
using System.Globalization;
using System.Data;

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
        private void createResponseMessage(string status, string message = "", string status_field = "status", string message_field = "message")
        {
            TempData[status_field] = status;
            if (!String.IsNullOrWhiteSpace(message))
            {
                TempData[message_field] = message;
            }
        }

        #region reportes
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

        private static void AddSuperHeader(GridView gridView, string text = null)
        {
            var myTable = (Table)gridView.Controls[0];
            var myNewRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            myNewRow.Cells.Add(MakeCell(text, gridView.HeaderRow.Cells.Count));//gridView.Columns.Count
            myNewRow.Cells[0].Style.Add("background-color", "#cbcfd6");
            myTable.Rows.AddAt(0, myNewRow);
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

        private static TableHeaderCell MakeCell(string text = null, int span = 1)
        {
            return new TableHeaderCell() { ColumnSpan = span, Text = text ?? string.Empty, CssClass = "table-header" };
        }
        #endregion

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
            ViewBag.lstMonedas = empBL.getListaMonedasAll();
            MovimientoInvBL movItmBL = new MovimientoInvBL();
            ViewBag.lstItems = movItmBL.getItemsEnEmpresa_PorTipoMov(user.IdEmpresa, 1);
            CuentaBancariaBL cbBL = new CuentaBancariaBL();
            ViewBag.lstCuentasBancarias = cbBL.getCuentasBancariasActivasPorTipoEnEmpresa(user.IdEmpresa, 1);
            ViewBag.lstContactos = new List<ContactoDTO>();

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
            obj.DetalleProforma = new List<DetalleProformaDTO>();

            return View(obj);
        }

        public ActionResult ProformaDetalle(int id)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar", "Admin", new { Area = string.Empty }); }
            ViewBag.Title += "Detalle Proforma";
            MenuNavBarSelected(12);

            var obj = new ProformaBL().getProformaId(id);

            return View(obj);
        }

        [HttpPost]
        public ActionResult AddProforma(ProformaDTO dto)
        {
            if (!this.currentUser()) { return RedirectToAction("Ingresar", "Admin", new { Area = string.Empty }); }
            if (getCurrentUser().IdRol == 4) { return RedirectToAction("Index", "Proformas"); }
            try
            {
                if(dto != null)
                {
                    dto.DetalleProforma = (List<DetalleProformaDTO>)TempData["lstDetalleProforma"] ?? new List<DetalleProformaDTO>();
                }

                ProformaBL objBL = new ProformaBL();

                if (dto.IdProforma == 0)
                {
                    if (objBL.SaveProforma(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Index", "Proformas");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
                    }
                }
                else if (dto.IdProforma > 0)
                {
                    if (objBL.SaveProforma(dto))
                    {
                        createResponseMessage(CONSTANTES.SUCCESS);
                        return RedirectToAction("Index", "Proformas");
                    }
                    else
                    {
                        createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                    }
                }
            }
            catch(Exception e)
            {
                if (dto.IdProforma != 0)
                    createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_UPDATE_MESSAGE);
                else createResponseMessage(CONSTANTES.ERROR, CONSTANTES.ERROR_INSERT_MESSAGE);
            }
            TempData["Proforma"] = dto;
            return RedirectToAction("Proforma", "Proformas", new { id = dto.IdProforma });
        }

        [HttpPost]
        public ActionResult PasslstItems(List<DetalleProformaDTO> lista)
        {
            TempData["lstDetalleProforma"] = lista;
            return Json(new { success = true, mensaje = "Si funciona" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetContactos(int idEntidad)
        {
            EntidadResponsableBL objBL = new EntidadResponsableBL();
            var lista = objBL.getContactosActivos_EnEntidadResponsable(idEntidad);
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportarProformas()
        {
            EmpresaDTO empresa = (new EmpresaBL()).getEmpresa(getCurrentUser().IdEmpresa);

            ProformaBL objBL = new ProformaBL();
            List<ProformaDTO> lista = objBL.getProformasExportarEnEmpresa(empresa.IdEmpresa);

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Clear();

            dt.Columns.Add("Codigo de Proforma");
            dt.Columns.Add("Nombre Cliente");
            dt.Columns.Add("Atención");
            dt.Columns.Add("Cargo");
            dt.Columns.Add("Email");
            dt.Columns.Add("Telefono");
            dt.Columns.Add("Celular");
            dt.Columns.Add("Fecha Registro");
            dt.Columns.Add("Fecha Proforma");
            dt.Columns.Add("Validez");
            dt.Columns.Add("Método de Pago");
            dt.Columns.Add("Fecha de Entrega");
            dt.Columns.Add("Lugar de Entrega");
            dt.Columns.Add("Sub Total");
            dt.Columns.Add("IGV");
            dt.Columns.Add("Total");
            dt.Columns.Add("Cuenta Bancaria");
            dt.Columns.Add("Estado");
            dt.Columns.Add("Observaciones de la Proforma");
            dt.Columns.Add("Comentarios adicionales");

            foreach (var item in lista)
            {
                System.Data.DataRow row = dt.NewRow();
                row["Codigo de Proforma"] = item.CodigoProforma;
                row["Nombre Cliente"] = item.EntidadResponsable.Nombre;
                row["Atención"] = item.Contacto.Nombre;
                row["Cargo"] = item.Contacto.Cargo;
                row["Email"] = item.Contacto.Email;
                row["Telefono"] = item.Contacto.Telefono;
                row["Celular"] = item.Contacto.Celular;
                row["Fecha Registro"] = item.FechaRegistro != null ? item.FechaRegistro.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "";
                row["Fecha Proforma"] = item.FechaProforma != null ? item.FechaProforma.GetValueOrDefault().ToString("dd/MMM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")) : "";
                row["Validez"] = item.ValidezOferta + "día(s)";
                row["Método de Pago"] = item.MetodoPago;
                row["Fecha de Entrega"] = item.FechaEntrega;
                row["Lugar de Entrega"] = item.LugarEntrega;
                row["Sub Total"] = item.DetalleProforma.Sum(x => x.MontoTotal).GetValueOrDefault().ToString("N2", CultureInfo.InvariantCulture);
                row["IGV"] = item.DetalleProforma.Sum(x => x.Igv).GetValueOrDefault().ToString("N2", CultureInfo.InvariantCulture);
                row["Total"] = item.DetalleProforma.Sum(x => x.MontoTotal + x.Igv).GetValueOrDefault().ToString("N2", CultureInfo.InvariantCulture);
                row["Cuenta Bancaria"] = item.NombreCuentaBancaria;
                row["Estado"] = item.Estado == 1 ? "Pendiente" : (item.Estado == 2 ? "Aprobada" : "Rechazada");
                row["Observaciones de la Proforma"] = item.ComentarioProforma;
                row["Comentarios adicionales"] = item.ComentarioAdiccional;
                dt.Rows.Add(row);
            }

            GenerarProformaPdf(dt, "Detalle de Proformas", "Detalle_de_Proformas", empresa, Response);
            createResponseMessage(CONSTANTES.SUCCESS, CONSTANTES.SUCCESS_FILE);
            return RedirectToAction("Index", "Proformas"); ;
        }

        private static void GenerarProformaPdf(DataTable dt, string titulo, string nombreDoc, EmpresaDTO objEmpresa, HttpResponseBase Response)
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
                AddWhiteHeader(gv, 1, "RUC: " + objEmpresa.RUC);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreDoc + "_" + objEmpresa.Nombre + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";

                System.IO.StringWriter sw = new System.IO.StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                htw.Close();
                sw.Close();
            }
        }

        
    }
}