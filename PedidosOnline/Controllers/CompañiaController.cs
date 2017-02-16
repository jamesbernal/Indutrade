using PedidosOnline.Controllers;
using PedidosOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErpPortal.WebApp.Controllers
{
    public class CompañiaController : Controller
    {
        // GET: Compañia
        PedidosOnlineEntities db = new PedidosOnlineEntities();
        [CheckSessionOut]
        public ActionResult ListadoCompañia()
        {
            ViewBag.listacompañia = db.Compañia.ToList();
            return View();
        }
        [CheckSessionOut]
        public ActionResult compañiadeT(int? RowCompañia)
        {
            Compañia dtCompaia = db.Compañia.Where(f => f.RowID == RowCompañia).FirstOrDefault();
            return View(dtCompaia);
        }

    }
}