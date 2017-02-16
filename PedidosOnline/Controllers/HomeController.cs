using System.Web.Mvc;
using PedidosOnline.Models;

namespace PedidosOnline.Controllers
{

    public class HomeController : Controller
    {

        PedidosOnlineEntities _db = new PedidosOnlineEntities();
        [CheckSessionOut]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Config()
        {
            return View();
        }

        public ActionResult Grafica_Inicio()
        {
            return View();
        }

        public ActionResult GraficaCartera()
        {
            return View();
        }

        public ActionResult Pagina_No_Disponible()
        {
            return View();
        }
    }
}