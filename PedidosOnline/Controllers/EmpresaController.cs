using PedidosOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErpPortal.WebApp.Controllers
{
    public class EmpresaController : Controller
    {
        PedidosOnlineEntities db = new PedidosOnlineEntities();
        public ActionResult EmpresaListado()
        {
            ViewBag.Listado = db.Empresa.ToList();
            return View();
        }

        public ActionResult ver(int? Rowid)
        {
            Empresa empresa = db.Empresa.Where(f => f.RowID == Rowid).FirstOrDefault();

            return View(empresa);
        }
    }
}