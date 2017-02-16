using PedidosOnline.Models;
using PedidosOnline.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PedidosOnline.Controllers
{
    public class SolicitudTransporteController : Controller
    {
        // GET: SolicitudTransporte
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
        public ActionResult SolicitudTransportes()
        {
            List<SolicitudTransporte> Lista = db.SolicitudTransporte.ToList();
            ViewBag.transportes = Lista;
            return View(Lista);
        }

        [CheckSessionOut]
        public ActionResult SolicitudTransporte(int? RowID)
        {
            ViewBag.plantas = db.Planta.ToList();
            List<string> tipo = new List<string>();
            tipo.Add("Térmico");
            tipo.Add("Acerado - Serpentín");
            tipo.Add("Lamina Negra");
            ViewBag.tipo = tipo;
            List<string> tipo1 = new List<string>();
            tipo1.Add("Aluminio");
            tipo1.Add("Plancha");
            tipo1.Add("Carrocería");
            ViewBag.tipo1 = tipo1;
            if (RowID != null || RowID > 0)
            {
                ViewBag.tipoV = db.TipoVehiculo.Where(t => t.SolicitudTransporteID == RowID).ToList();
                SolicitudTransporte solicitud = db.SolicitudTransporte.Where(c => c.RowID == RowID).FirstOrDefault();
                return View(solicitud);
            }
            else
            {
                ViewBag.tipoV = null;
                return View(new SolicitudTransporte());
            }
        }

        [ValidateInput(false)]
        public JsonResult Datos_Contrato(string titulo)
        {
            Contrato contrato = db.Contrato.Where(p => p.Titulo == titulo).FirstOrDefault();
            int RowID = contrato.RowID;
            string Titulo = contrato.Titulo;
            int RowIDPro = contrato.ProformaID;
            var data = new { RowID = RowID, Titulo = Titulo, RowIDPro = RowIDPro };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public JsonResult Datos_Proveedor(string proveedor)
        {
            //Tercero tercero = db.Tercero.Where(t => t.RazonSocial == proveedor).FirstOrDefault();
            ContactoERP contacto = db.ContactoERP.Where(p => p.Nombre == proveedor).FirstOrDefault();
            int RowID = contacto.RowID;
            string Nit = contacto.Identificacion;
            string Direccion = contacto.Direccion1;
            var data = new { RowID = RowID, Nit = Nit, Direccion= Direccion };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Solicitud_Buscar_Proveedor()
        {
            List<Terceros> data = new List<Terceros>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Terceros> Lista = (from listado in db.Tercero
                                    where (listado.ContactoERP.Nombre.Contains(terms))
                                    select new Terceros()
                                    {
                                        RowID = listado.RowID,
                                        RazonSocial = listado.ContactoERP.Nombre,
                                        Direccion = listado.ContactoERP.Direccion1,
                                        Nit = listado.ContactoERP.Identificacion,
                                        label = listado.ContactoERP.Nombre,
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
                                     }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public string ItemsContrato(int RowID)
        {
            string result = "";
            Contrato contrato = db.Contrato.Where(c => c.RowID == RowID).FirstOrDefault();
            Proforma proforma = db.Proforma.Where(p => p.RowID == contrato.ProformaID).FirstOrDefault();
            List<CalculadoraItems> itemsC = db.CalculadoraItems.Where(c => c.CalculadoraID == proforma.CalculadoraID).ToList();
            double subtotal = 0;
            foreach (var item in itemsC)
            {
                subtotal = Convert.ToDouble(item.PVE) * Convert.ToDouble(item.CantidadTonelada);
                result += "<tr>" +
                         "<td >" + item.Item.Referencia + " </td >" +
                         "<td > " + item.Item.Descripcion + " </td >" +
                         "<td > " + item.CantidadTonelada + " </td >" +
                         "<td > " + item.PVE + " </td >" +
                         "<td > " + item.Opcion.Descripcion + " </td >" +
                         "<td > " + item.CantidadTonelada + " </td >" +
                         "<td > " + subtotal + " </td >" +
                   "</tr>";

            }
            return result;
        }


        [CheckSessionOutAttribute]
        public JsonResult RegistrarSolicitudTransporte(FormCollection form, int RowID, int RowIDCon, int RowIDPro, string tipoT)
        {
            String mensaje = "";
            SolicitudTransporte ObjSolicitud = new SolicitudTransporte();
            try
            {
                if (RowID == 0)
                {
                    form = DeSerialize(form);
                    ObjSolicitud.ContratoID = RowIDCon;
                    ObjSolicitud.ProveedorID = RowIDPro;
                    ObjSolicitud.FechaCargue = Convert.ToDateTime(form["fechaC"]);
                    ObjSolicitud.FechaEntrega = Convert.ToDateTime(form["fechaE"]);
                    ObjSolicitud.RequisitosCargue = form["requisitosC"];
                    ObjSolicitud.RequisitosDescargue = form["requisitosD"];
                    //ObjSolicitud.OpcionID = Convert.ToInt32(form["periodoE"]);
                    ObjSolicitud.Flete = form["flete"];
                    ObjSolicitud.PlantaCargueID = Convert.ToInt32(form["plantaC"]);
                    ObjSolicitud.PlantaDescargueID = Convert.ToInt32(form["plantaD"]);
                    int cantidad = 0;
                    if(form["cantidadV"] != "")
                    cantidad = Convert.ToInt32(form["cantidadV"]);
                    ObjSolicitud.Cantidad = cantidad;
                    ObjSolicitud.FechaCreacion = UtilTool.GetDateTime();
                    ObjSolicitud.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SolicitudTransporte.Add(ObjSolicitud);
                    db.SaveChanges();

                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    //Actualizar el plantilla
                    ObjSolicitud = db.SolicitudTransporte.Where(le => le.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    ObjSolicitud.ContratoID = RowIDCon;
                    ObjSolicitud.ProveedorID = RowIDPro;
                    ObjSolicitud.FechaCargue = Convert.ToDateTime(form["fechaC"]);
                    ObjSolicitud.FechaEntrega = Convert.ToDateTime(form["fechaE"]);
                    ObjSolicitud.RequisitosCargue = form["requisitosC"];
                    ObjSolicitud.RequisitosDescargue = form["requisitosD"];
                    //ObjSolicitud.OpcionID = Convert.ToInt32(form["periodoE"]);
                    ObjSolicitud.Flete = form["flete"];
                    ObjSolicitud.PlantaCargueID = Convert.ToInt32(form["plantaC"]);
                    ObjSolicitud.PlantaDescargueID = Convert.ToInt32(form["plantaD"]);
                    ObjSolicitud.Cantidad = Convert.ToInt32(form["cantidadV"]);
                    ObjSolicitud.FechaCreacion = UtilTool.GetDateTime();
                    ObjSolicitud.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    ObjSolicitud.FechaModificacion = UtilTool.GetDateTime();
                    ObjSolicitud.UsuarioModificacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SaveChanges();
                    mensaje = "Se ha actualizado correctamente";
                }
                List<TipoVehiculo> tiposV = db.TipoVehiculo.Where(t => t.SolicitudTransporteID == ObjSolicitud.RowID).ToList();
                db.TipoVehiculo.RemoveRange(tiposV);
                db.SaveChanges();
                for (int i=0; i< tipoT.Split(',').Length-1; i++)
                {
                    TipoVehiculo tipo = new TipoVehiculo();
                    tipo.SolicitudTransporteID = ObjSolicitud.RowID;
                    tipo.Nombre = tipoT.Split(',')[i];
                    db.TipoVehiculo.Add(tipo);
                    db.SaveChanges();
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
