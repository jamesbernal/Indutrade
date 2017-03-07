using PedidosOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PedidosOnline.Controllers
{
    public class MaestroController : Controller
    {
        PedidosOnlineEntities db = new PedidosOnlineEntities();
        // GET: Maestro
        public ActionResult Index()
        {
            return View();
        }


        #region General
        private FormCollection DeSerialize(FormCollection formulario)
        {
            FormCollection collection = new FormCollection();
            //un-encode, and add spaces back in
            string querystring = Uri.UnescapeDataString(formulario["formulario"]).Replace("+", " ");
            var split = querystring.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach (string s in split)
            {
                string text = s.Substring(0, s.IndexOf("="));
                string value = s.Substring(s.IndexOf("=") + 1);

                if (items.Keys.Contains(text))
                    items[text] = items[text] + "," + value;
                else
                    items.Add(text, value);
            }
            foreach (var i in items)
            {
                collection.Add(i.Key, i.Value);
            }
            return collection;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region AgenteNaviera

        [CheckSessionOutAttribute]
        public ActionResult AgenteNaviera()
        {
            ViewBag.Agente = db.AgenteNaviera.ToList();
            return View();
        }


        [CheckSessionOutAttribute]
        public JsonResult GuardarAgente(FormCollection formulario)
        {
            String respuesta = "";
            formulario = DeSerialize(formulario);
            AgenteNaviera agente = new AgenteNaviera();
            try{
                    agente.Nombre = formulario["Nombre"];
                    agente.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                agente.FechaCreacion = DateTime.Now;
                    db.AgenteNaviera.Add(agente);
                    db.SaveChanges();
                }
                catch (Exception ex)
                { return Json(new { respuesta = "Error " + ex.Message }); }
                respuesta = "Guardado Correctamente";
            
            return Json(respuesta);
        }

        public JsonResult RefrescarAgentes()
        {
            var data = (from agente in db.AgenteNaviera
                        select new
                        {
                            rowid = agente.RowID,
                            Nombre = agente.Nombre,
                            CreadoPor = agente.UsuarioCreacion,
                            ano = agente.FechaCreacion.Year,
                            mes = agente.FechaCreacion.Month,
                            dia = agente.FechaCreacion.Day


                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [CheckSessionOutAttribute]
        public JsonResult ConsultarAgente(int RowID_Agente)
        {
            String respuesta = "";
            AgenteNaviera agente = new AgenteNaviera();

            try
            {
                agente = db.AgenteNaviera.Where(f => f.RowID == RowID_Agente).FirstOrDefault();
                respuesta = "ok";
            }
            catch (Exception ex)
            { return Json(new { agente = agente.Nombre }); }
            return Json(new {  nom = agente.Nombre, row_id_a= agente.RowID});
        }

        [CheckSessionOutAttribute]
        public JsonResult ActualizarAgente(FormCollection formulario, int RowID_Agente)
        {
            String respuesta = "";
            formulario = DeSerialize(formulario);
            AgenteNaviera agente = new AgenteNaviera();
            if (RowID_Agente != 0)
            {
                try
                {
                    agente = db.AgenteNaviera.Where(f => f.RowID == RowID_Agente).FirstOrDefault();
                    agente.Nombre = formulario["Nombre"];
                    agente.UsuarioModificacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    agente.FechaModificacion = DateTime.Now;
                    db.SaveChanges();
                }
                catch (Exception ex)
                { return Json(new { respuesta = "Error " + ex.Message }); }
                respuesta = "Guardado Correctamente";
            }
            return Json(respuesta);
        }
        #endregion

        #region Puerto

        [CheckSessionOutAttribute]
        public ActionResult Puerto(int? RowID_Lista)
        {
            if (RowID_Lista == null || RowID_Lista == 0)
            {
                ViewBag.Ciudad = db.Ciudad.ToList();
                ViewBag.Departamento = db.Departamento.ToList();
                ViewBag.Pais = db.Pais.ToList();
            }
            else {
                Puerto p = db.Puerto.Where(a => a.RowID == RowID_Lista).FirstOrDefault();
                ViewBag.Pais = db.Pais.ToList();
                ViewBag.Departamento = db.Departamento.Where(f=>f.PaisID==p.Ciudad.Departamento.PaisID);
                ViewBag.Ciudad = db.Ciudad.Where(f => f.DepartamentoID == p.Ciudad.DepartamentoID);
            }
         

            if (RowID_Lista != null)
            {
                return View(db.Puerto.Where(le => le.RowID == RowID_Lista).FirstOrDefault());
            }
            else
            {
                return View(new Puerto());
            }
        }

        [CheckSessionOutAttribute]
        public ActionResult ListadoPuerto()
        {
            ViewBag.Puerto = db.Puerto.ToList();
            return View();
        }


        [CheckSessionOutAttribute]
        public JsonResult CargarDepartamento(Int32 rowid)
        {
            List<Departamento> departamentos = db.Departamento.Where(f => f.PaisID == rowid).ToList();
            ///Para formar el Json
            var query = (from Departamento in departamentos
                         select new
                         {
                             RowId = Departamento.RowID,
                             Nombre = Departamento.Nombre
                         }
            ).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [CheckSessionOutAttribute]
        public JsonResult CargarCiudad(Int32 rowid)
        {
            List<Ciudad> ciudad = db.Ciudad.Where(f => f.DepartamentoID == rowid).ToList();
            ///Para formar el Json
            var query = (from Ciudad in ciudad
                         select new
                         {
                             RowId = Ciudad.RowID,
                             Nombre = Ciudad.Nombre
                         }
            ).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }



        [CheckSessionOutAttribute]
        public JsonResult GuardarPuerto(FormCollection formulario, int? RowID_Puerto)
        {
            String respuesta = "";
            formulario = DeSerialize(formulario);
            if (RowID_Puerto == 0) { 
            Puerto puerto = new Puerto();
            try
            {
                puerto.Nombre = formulario["Nombre"];
                puerto.CiudadID = Convert.ToInt32(formulario["ciudad"]);
                puerto.UsuarioCreacion = "puerto";
                puerto.FechaCreacion = DateTime.Now;
                db.Puerto.Add(puerto);
                db.SaveChanges();
            }
            catch (Exception ex)
            { return Json(new { respuesta = "Error " + ex.Message }); }
            respuesta = "Guardado Correctamente";
        }else{
                Puerto puerto = db.Puerto.Where(f=> f.RowID == RowID_Puerto).FirstOrDefault();
                try
                {
                    puerto.Nombre = formulario["Nombre"];
                    puerto.CiudadID = Convert.ToInt32(formulario["ciudad"]);
                    puerto.UsuarioModificacion = Session["curUser"].ToString();
                    puerto.FechaModificacion = DateTime.Now;
                    db.SaveChanges();
                }
                catch (Exception ex)
                { return Json(new { respuesta = "Error " + ex.Message }); }
                respuesta = "Guardado Correctamente";
            }

            return Json(respuesta);
        }
        #endregion
        public ActionResult ListaUsuarioSistemas()
        {
            List<Usuario> lista_usuario = db.Usuario.Where(lius => lius.Activo == true).ToList();
            return View(lista_usuario);
        }
        public ActionResult CrearFirma(int? RowID_Usuario) {
            Usuario objUsuario = db.Usuario.Where(us => us.RowID == RowID_Usuario).FirstOrDefault();
            if (objUsuario == null) { 
                return View(new Usuario());
            }
            
            return View(objUsuario);
        }

    }
}