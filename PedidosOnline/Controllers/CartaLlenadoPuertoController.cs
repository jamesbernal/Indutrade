using PedidosOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Utilidades;

namespace PedidosOnline.Controllers
{
    public class CartaLlenadoPuertoController : Controller
    {
        PedidosOnlineEntities db = new PedidosOnlineEntities();
        // GET: CartaSolicitudPuerto
        public ActionResult Index()
        {
            return View();
        }

        private FormCollection DeSerialize(FormCollection FormData)
        {
            FormCollection collection = new FormCollection();
            //un-encode, and add spaces back in
            string querystring = Uri.UnescapeDataString(FormData[0]).Replace("+", " ");
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

        [CheckSessionOut]
        public ActionResult CartaLlenadoPuertos()
        {
            List<CartaLlenadoPuerto> lista = db.CartaLlenadoPuerto.OrderByDescending(f => f.RowID).ToList();
            ViewBag.cartasPuerto = lista.ToList();
            return View(lista);
        }

        [CheckSessionOut]
        public ActionResult CartaLlenadoPuerto(int? rowid)
        {
            ViewBag.medidas = db.Opcion.Where(o => o.Agrupacion.Nombre == "MEDIDAS.PESO").ToList();
           if (rowid != null || rowid > 0)
            {
                CartaLlenadoPuerto carta = db.CartaLlenadoPuerto.Where(c=>c.RowID == rowid).FirstOrDefault();
                return View(carta);
            }else { 
            return View(new CartaLlenadoPuerto());
            }
        }

        public JsonResult Buscar_Vehiculos()
        {
            List<Vehiculos> data = new List<Vehiculos>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Vehiculos> Lista = (from listado in db.Vehiculo
                                     where (listado.Placa.Contains(terms))
                                     select new Vehiculos()
                                     {
                                         RowID = listado.RowID,
                                         placa = listado.Placa,
                                         label = listado.Placa,
                                     }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public JsonResult Buscar_Motonaves()
        {
            List<MotoNaves> data = new List<MotoNaves>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<MotoNaves> Lista = (from listado in db.MotoNave
                                     where (listado.Nombre.Contains(terms) /*|| (listado.NumeroViaje + "").Contains(terms)*/)
                                     select new MotoNaves()
                                     {
                                         RowID = listado.RowID,
                                         nombre = listado.Nombre,
                                         numViajes = listado.NumeroViaje.Value,
                                         label = listado.Nombre,
                                     }).Distinct().OrderBy(f => f.nombre).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public JsonResult Buscar_Puertos()
        {
            List<Puertos> data = new List<Puertos>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Puertos> Lista = (from listado in db.Puerto
                                     where (listado.Nombre.Contains(terms))
                                     select new Puertos()
                                     {
                                         RowID = listado.RowID,
                                         label = listado.Nombre,
                                     }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public JsonResult Buscar_Empresas()
        {
            List<Empresas> data = new List<Empresas>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Empresas> Lista = (from listado in db.Empresa
                                    where (listado.RazonSocial.Contains(terms))
                                   select new Empresas()
                                   {
                                       RowID = listado.RowID,
                                       label = listado.RazonSocial,
                                   }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public JsonResult Buscar_Conductores()
        {
            List<Terceros> data = new List<Terceros>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Terceros> Lista = (from listado in db.Tercero
                                    where (listado.Identificacion.Contains(terms))
                                    select new Terceros()
                                    {
                                        RowID = listado.RowID,
                                        RazonSocial = listado.RazonSocial,
                                        label = listado.Identificacion,
                                    }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        
        public JsonResult Datos_Conductor(string titulo)
        {

            Tercero conductor = db.Tercero.Where(p => p.Identificacion == titulo).FirstOrDefault();
            int RowID = conductor.RowID;
            string Identificacion = conductor.Identificacion;
            string Nombre = conductor.RazonSocial;
            var data = new { RowID = RowID, Identificacion = Identificacion, Nombre = Nombre };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [CheckSessionOutAttribute]
        public JsonResult RegistrarCartaLlenadoPuerto(FormCollection form, int RowID, int RowIDM, int RowIDT, int RowIDP, int RowIDE, int RowIDV)
        {
            String mensaje = "";
            CartaLlenadoPuerto ObjSolicitud = new CartaLlenadoPuerto();
            try
            {
                if (RowID == 0)
                {
                    form = DeSerialize(form);
                    ObjSolicitud.MotonaveID = RowIDM;
                    ObjSolicitud.TerceroID = RowIDT;
                    ObjSolicitud.PuertoID = RowIDP;
                    ObjSolicitud.EmpresaID = RowIDE;
                    ObjSolicitud.VehiculoID = RowIDV;
                    ObjSolicitud.DescripcionMercancia = form["descripcionM"];
                    try { ObjSolicitud.Peso = Convert.ToInt32(form["peso"]); } catch { }
                    try { ObjSolicitud.PorcentajeVacio = Convert.ToInt32(form["porcentajeV"]); } catch { }
                    ObjSolicitud.Importador = form["importador"];
                    ObjSolicitud.Direccion = form["direccion"];
                    ObjSolicitud.NombreSIA = form["nombreS"];
                    try { ObjSolicitud.OpcionID = Convert.ToInt32(form["medidaP"]); }catch { }
                    ObjSolicitud.FechaCreacion = UtilTool.GetDateTime();
                    ObjSolicitud.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.CartaLlenadoPuerto.Add(ObjSolicitud);
                    db.SaveChanges();

                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    //Actualizar el plantilla
                    ObjSolicitud = db.CartaLlenadoPuerto.Where(le => le.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    ObjSolicitud.MotonaveID = RowIDM;
                    ObjSolicitud.TerceroID = RowIDT;
                    ObjSolicitud.PuertoID = RowIDP;
                    ObjSolicitud.EmpresaID = RowIDE;
                    ObjSolicitud.VehiculoID = RowIDV;
                    ObjSolicitud.DescripcionMercancia = form["descripcionM"];
                    try { ObjSolicitud.Peso = Convert.ToInt32(form["peso"]); } catch { }
                    try { ObjSolicitud.PorcentajeVacio = Convert.ToInt32(form["porcentajeV"]); } catch { }
                    try { ObjSolicitud.OpcionID = Convert.ToInt32(form["medidaP"]); } catch { }
                    ObjSolicitud.Importador = form["importador"];
                    ObjSolicitud.Direccion = form["direccion"];
                    ObjSolicitud.NombreSIA = form["nombreS"];
                    ObjSolicitud.FechaModificacion = UtilTool.GetDateTime();
                    ObjSolicitud.UsuarioModificacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SaveChanges();
                    mensaje = "Se ha actualizado correctamente";
                }
            }
            catch (Exception e)
            {
                mensaje = "No se ha podido guardar los datos, error : " + e.Message;

            }

            int rowid = ObjSolicitud.RowID;
            return Json(rowid, JsonRequestBehavior.AllowGet);
        }


    }
}
