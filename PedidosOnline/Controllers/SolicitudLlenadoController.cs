using PedidosOnline.Models;
using PedidosOnline.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PedidosOnline.Controllers
{
    public class SolicitudLlenadoController : Controller
    {
        // GET: SolicitudLlenado
        public ActionResult Index()
        {
            return View();
        }

        PedidosOnlineEntities db = new PedidosOnlineEntities();

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
        public ActionResult SolicitudLlenados()
        {
            List<SolicitudLlenado> Lista = db.SolicitudLlenado.ToList();
            ViewBag.solicitudes = Lista;
            return View(Lista);
        }

        [CheckSessionOut]
        public ActionResult SolicitudLlenado(int? RowID)
        {
            ViewBag.empaques = db.Opcion.Where(pe => pe.Agrupacion.Nombre == "EMPAQUE").ToList();
            if (RowID != null || RowID > 0)
            {
                SolicitudLlenado solicitud = db.SolicitudLlenado.Where(c => c.RowID == RowID).FirstOrDefault();
                return View(solicitud);
            }
            else
            {
                return View(new SolicitudLlenado());
            }
        }

        public JsonResult Solicitud_Buscar_Vehiculo()
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

        public JsonResult Contrato_Buscar_Todos()
        {

            List<Contratos> data = new List<Contratos>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Contratos> Lista = (from listado in db.Contrato
                                     where (listado.Titulo.Contains(terms))
                                     select new Contratos()
                                     {
                                         RowID = listado.RowID,
                                         Titulo = listado.Titulo,
                                         label = listado.Titulo,
                                         RowIDPro = listado.ProformaID,
                                     }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [CheckSessionOutAttribute]
        public JsonResult RegistrarSolicitudLlenado(FormCollection form, int RowID, int RowIDCon, int RowIDDes)
        {
            String mensaje = "";
            SolicitudLlenado ObjSolicitud = new SolicitudLlenado();
            try
            {
                if (RowID == 0)
                {
                    form = DeSerialize(form);
                    ObjSolicitud.ContratoID = RowIDCon;
                    ObjSolicitud.DestinoID = RowIDDes;
                    ObjSolicitud.FechaDescargue = Convert.ToDateTime(form["fechaD"]);
                    ObjSolicitud.BKK = form["BKK"];
                    ObjSolicitud.EmpaqueID = Convert.ToInt32(form["empaque"]);
                    ObjSolicitud.FechaCreacion = UtilTool.GetDateTime();
                    ObjSolicitud.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SolicitudLlenado.Add(ObjSolicitud);
                    db.SaveChanges();

                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    //Actualizar el plantilla
                    ObjSolicitud = db.SolicitudLlenado.Where(le => le.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    ObjSolicitud.ContratoID = RowIDCon;
                    ObjSolicitud.DestinoID = RowIDDes;
                    ObjSolicitud.FechaDescargue = Convert.ToDateTime(form["fechaD"]);
                    ObjSolicitud.BKK = form["BKK"];
                    ObjSolicitud.EmpaqueID = Convert.ToInt32(form["empaque"]);
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

            int rowid = ObjSolicitud.RowID ;
            return Json(rowid, JsonRequestBehavior.AllowGet);
        }
    

    [CheckSessionOut]
        public ActionResult ModalVehiculo(int? RowID, int RowID1)
        {
            ViewBag.empresa = null;
            ViewBag.conductor = null;
            ViewBag.empresas = db.Compañia.ToList();
            ViewBag.solicitud = RowID1;
            if (RowID != null && RowID > 0)
            {
                VehiculoSolicitudLlenado solicitud = db.VehiculoSolicitudLlenado.Where(c => c.RowID == RowID).FirstOrDefault();
                ViewBag.empresa = solicitud.Compañia.Nombre;
                ViewBag.conductor= solicitud.Tercero.ContactoERP.Identificacion;
                return View(solicitud);
            }
            else
            {
                return View(new VehiculoSolicitudLlenado());
            }
        }

        [HttpPost]
        public JsonResult Vehiculo(FormCollection form, int RowIDCon, int RowIDVeh, int RowID)
        {
            form = DeSerialize(form);

            ViewBag.error = "";
            int rowid_vehiculo = 0;
            try { rowid_vehiculo = int.Parse(form["rowid"]); }
            catch { }

            try
            {
                VehiculoSolicitudLlenado vehiculo = db.VehiculoSolicitudLlenado.Where(f => f.RowID == rowid_vehiculo).FirstOrDefault();

                if (vehiculo == null)//insertar
                {
                    vehiculo = new VehiculoSolicitudLlenado();
                    
                }
                else//modificar
                {
                    //contacto.usuario_modificacion = ((s_usuario)Session["curUser"]).nombre_usuario;
                    //contacto.fecha_modificacion = UtilTool.GetDateTime();
                }

                vehiculo.VehiculoID = RowIDVeh;
                vehiculo.FechaCargue = Convert.ToDateTime(form["fechaC"]);
                vehiculo.Remolque = form["remolque"];
                vehiculo.EmpresaTransporteID = Convert.ToInt32(form["empresaT"]);
                vehiculo.TerceroID = RowIDCon;
                vehiculo.SolicitudLlenadoID = RowID;


                if (rowid_vehiculo == 0)
                   db.VehiculoSolicitudLlenado.Add(vehiculo);

                db.SaveChanges();
                rowid_vehiculo = vehiculo.RowID;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = e.Message.ToString();
                return Json("Se presento un problema al momento de almacenar la información del vehiculo");
            }

            return Json(rowid_vehiculo);
        }
        
       public JsonResult Buscar_Destinos()
        {
            List<Ciudades> data = new List<Ciudades>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Ciudades> Lista = (from listado in db.Ciudad
                                  where (listado.Nombre.Contains(terms))
                                    select new Ciudades()
                                    {
                                        RowID = listado.RowID,
                                        label = listado.Nombre,
                                    }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public JsonResult Buscar_Conductor()
        {
            List<Terceros> data = new List<Terceros>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Terceros> Lista = (from listado in db.Tercero
                                    where (listado.ContactoERP.Identificacion.Contains(terms))
                                    select new Terceros()
                                    {
                                        RowID = listado.RowID,
                                        RazonSocial = listado.ContactoERP.Nombre,
                                        Direccion = listado.ContactoERP.Direccion1,
                                        Telefono = listado.ContactoERP.Telefono2,
                                        Nit = listado.ContactoERP.Identificacion,
                                        label = listado.ContactoERP.Identificacion,
                                    }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        
        public string VehiculosSolicitud(int RowID)
        {
            string result = "";
            
            List<VehiculoSolicitudLlenado> itemsC = db.VehiculoSolicitudLlenado.Where(c => c.SolicitudLlenadoID == RowID).ToList();
           
            foreach (var item in itemsC)
            {
                result += "<tr>" +
                         "<td >" + item.RowID + " </td >" +
                         "<td > " + item.Vehiculo.Placa + " </td >" +
                         "<td > " + item.FechaCargue + " </td >" +
                         "<td > " + item.Remolque + " </td >" +
                         "<td > " + item.Compañia.Nombre + " </td >" +
                         "<td > " + item.Tercero.ContactoERP.Nombre + " </td >" +
                   "</tr>";

            }
            return result;
        }

    }
}
