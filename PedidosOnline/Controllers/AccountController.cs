using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PedidosOnline.Models;
using PedidosOnline.Utilidades;

namespace PedidosOnline.Controllers
{
    public partial class AccountController : Controller
    {
        PedidosOnlineEntities db = new PedidosOnlineEntities();

        public ActionResult RecuperarCuenta()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            if (Session["curUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (Session["curUser"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            //reset the User Session
            ResetCurSession();

            if (ModelState.IsValid)
            {
                if (ValidateLogin(model.p, model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    return RedirectToAction("Grafica_Inicio", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "El usuario o la contraseña ingresados son incorrectos.");
            return View(model);

        }

        public ActionResult CloseSession()
        {
            ResetCurSession();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private bool ValidateLogin(bool sesion_guardada, string username, string passwd, bool persistCookie)
        {
            string cryptPasswd = UtilTool.CryptPasswd(passwd, Constantes.CryptString);
            
            Usuario User = db.Usuario.FirstOrDefault(f => f.NombreUsuario == username && f.Contraseña == cryptPasswd);
            List<Modulo>  ModulosMenu = new List<Modulo>();
            List<Menu> MenuUsuario = new List<Menu>();
            if (User != null)
            {
                if (User.Rol.Nombre != "Administrador")
                {
                    List<RolMenu> menuxRol = db.RolMenu.Where(f => f.Rol.RowID == User.Rol.RowID && f.Menu.Activo == true && f.Activo == true).ToList();//agregar condicion "activo" para rolmenu
                    foreach (var item in menuxRol)
                    {
                        MenuUsuario.Add(item.Menu);

                        if (ModulosMenu.Where(f => f.RowID == item.Menu.Modulo.RowID).Count() == 0)
                        {
                            ModulosMenu.Add(item.Menu.Modulo);
                        }
                    }
                }
                else
                {
                    ModulosMenu = db.Modulo.Where(f => f.Activo == true).ToList();
                    MenuUsuario = db.Menu.Where(f => f.Activo == true).ToList();
                }
                Session["curUser"] = User;
                Session["Modulo"] = ModulosMenu;
                Session["Menu"] = MenuUsuario;
                Session.Timeout = 10;
                return true;
            }

            return false;
        }

        private void ResetCurSession()
        {
            Session.Clear();
            Session.RemoveAll();
        }
    }
}

