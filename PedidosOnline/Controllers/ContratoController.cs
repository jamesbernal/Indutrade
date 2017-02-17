using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Models;
using PedidosOnline.Utilidades;

namespace PedidosOnline.Controllers
{
    public class ContratoController : Controller
    {

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

        // GET: Proforma
        public ActionResult Index()
        {
            return View();
        }
        PedidosOnlineEntities db = new PedidosOnlineEntities();

        [CheckSessionOut]
        public ActionResult Contratos()
        {
            int rowid = rowid_tipo("EXPORTACION");
            List<Contrato> Lista = db.Contrato.Where(c => c.TipoContratoID == rowid).ToList();
            ViewBag.contratos = Lista;
            return View(Lista);
        }

        [CheckSessionOut]
        public ActionResult Contrato(int? RowID)
        {
            ViewBag.periodoE = db.Opcion.Where(pe => pe.Agrupacion.Nombre == "PERIODO EMBARQUE").ToList();
            ViewBag.transporte = db.Opcion.Where(pe => pe.Agrupacion.Nombre == "TRANSPORTE").ToList();
            if (RowID != null || RowID > 0)
            {
                Contrato contrato = db.Contrato.Where(c => c.RowID == RowID).FirstOrDefault();
                return View(contrato);
            }
            else
            {
                return View(new Contrato());
            }

        }

        public ActionResult RecursosContrato()
        {
            return View();
        }

        public JsonResult GuardarRecursosContrato()
        {
            RecursosContrato objrecurso = new RecursosContrato();
            HttpFileCollectionBase archivos;
            var nombre = Request.Params["codigo_archivo"].Split(',');
            int contador_inser = 0;
            int rowid_proforma = int.Parse(Request.Params["rowid_proforma"]);

            ViewBag.recursos = db.RecursosProforma.Where(rp => rp.RowID == rowid_proforma).ToList();

            string tipo_respuesta = "";
            string respuesta = "";
            string ruta_archivo;
            if (Request.Files.Count == 0)
            {
                respuesta = "No hay archivo para subir, Verifique la informacón";
                tipo_respuesta = "error";
                return Json(new { respuesta = respuesta, tipo_respuesta = tipo_respuesta });
            }
            else
            {
                try
                {
                    foreach (string item in Request.Files)
                    {
                        objrecurso.Codigo = nombre[contador_inser];
                        objrecurso.ContratoID = rowid_proforma;
                        HttpPostedFileBase file = Request.Files[contador_inser];
                        ruta_archivo = rowid_proforma + nombre[contador_inser] + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/RepositorioArchivos/ArchivosContrato/" + ruta_archivo));
                        ruta_archivo = "/RepositorioArchivos/ArchivosContrato/" + ruta_archivo;
                        objrecurso.Archivo = ruta_archivo;
                        db.RecursosContrato.Add(objrecurso);
                        db.SaveChanges();
                        contador_inser++;

                    }
                    respuesta = "Los archivos a sido almacenados";
                    tipo_respuesta = "success";
                    return Json(new { respuesta = respuesta, tipo_respuesta = tipo_respuesta });
                }
                catch (Exception ex)
                {
                    tipo_respuesta = "error";
                    return Json(new { respuesta = ex, tipo_respuesta = tipo_respuesta }, JsonRequestBehavior.AllowGet);
                }

            }


            /* = Request.Files["archivo_recurso"];*/
            return Json("");
        }


        [CheckSessionOutAttribute]
        public JsonResult RegistrarContrato(FormCollection form, int RowID, int RowIDPro)
        {
            String mensaje = "";
            Contrato ObjContrato = new Contrato();
            try
            {
                if (RowID == 0)
                {

                    form = DeSerialize(form);
                    ObjContrato.ProformaID = RowIDPro;
                    ObjContrato.Titulo = form["titulo"];
                    ObjContrato.Fecha = Convert.ToDateTime(form["fecha"]);
                    ObjContrato.Intermediario = form["intermediario"];
                    ObjContrato.Seguro = form["seguro"];
                    ObjContrato.Calidad = form["calidad"];
                    ObjContrato.EmbarqueID = Convert.ToInt32(form["periodoE"]);
                    ObjContrato.Transporte = form["transporte"];
                    ObjContrato.Calidad = form["calidad1"];
                    ObjContrato.Calidad2 = form["calidad2"];
                    ObjContrato.Inspector = form["autoc_inspector"];
                    ObjContrato.EmbarqueParcial = form["EmbarqueP"];
                    ObjContrato.FechaCreacion = UtilTool.GetDateTime();
                    ObjContrato.TipoContratoID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "EXPORTACION").FirstOrDefault().RowID;
                    ObjContrato.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.Contrato.Add(ObjContrato);
                    db.SaveChanges();

                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    ObjContrato = db.Contrato.Where(le => le.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    ObjContrato.ProformaID = RowIDPro;
                    ObjContrato.Titulo = form["titulo"];
                    ObjContrato.Fecha = Convert.ToDateTime(form["fecha"]);
                    ObjContrato.Intermediario = form["intermediario"];
                    ObjContrato.Seguro = form["seguro"];
                    ObjContrato.Calidad = form["calidad"];
                    ObjContrato.EmbarqueID = Convert.ToInt32(form["periodoE"]);
                    ObjContrato.Transporte = form["transporte"];
                    ObjContrato.Calidad = form["calidad1"];
                    ObjContrato.Calidad2 = form["calidad2"];
                    ObjContrato.Inspector = form["autoc_inspector"];
                    ObjContrato.EmbarqueParcial = form["EmbarqueP"];
                    ObjContrato.FechaCreacion = UtilTool.GetDateTime();
                    ObjContrato.TipoContratoID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "EXPORTACION").FirstOrDefault().RowID;
                    ObjContrato.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SaveChanges();
                    mensaje = "Se ha actualizado correctamente";
                }
            }
            catch (Exception e)
            {
                mensaje = "No se ha podido guardar los datos, error : " + e.Message;

            }

            int rowid = ObjContrato.RowID;

            return Json(rowid, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Datos_Proforma(string titulo)
        {
            Proforma proforma = db.Proforma.Where(p => p.Titulo == titulo).FirstOrDefault();
            int RowID = proforma.RowID;
            string vendedor = proforma.Tercero1.RazonSocial;
            string comprador = proforma.Tercero.RazonSocial;
            string origen = "";
            string destino = proforma.Calculadora.Ciudad.Nombre;
            string formaPago = proforma.Calculadora.Opcion.Descripcion;
            var data = new { RowID = RowID, vendedor = vendedor, comprador = comprador, origen = origen, destino = destino, formaPago = formaPago };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Contrato_Buscar_Inspector()
        {
            List<Terceros> data = new List<Terceros>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Terceros> Lista = (from listado in db.Tercero
                                    where (listado.RazonSocial.Contains(terms))
                                    select new Terceros()
                                    {
                                        RowID = listado.RowID,
                                        RazonSocial = listado.RazonSocial,
                                        label = listado.RazonSocial,

                                    }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        public JsonResult Proforma_Buscar_Todos(string tipo_proforma)
        {
            int rowid = rowid_tipo(tipo_proforma);
            List<Proformas> data = new List<Proformas>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Proformas> Lista = (from listado in db.Proforma
                                     where (listado.Titulo.Contains(terms) && listado.TipoProformaID == rowid)
                                     select new Proformas()
                                     {
                                         RowID = listado.RowID,
                                         vendedor = listado.Tercero1.RazonSocial,
                                         comprador = listado.Tercero.RazonSocial,
                                         origen = "",
                                         destino = listado.Calculadora.Ciudad.Nombre,
                                         formaPago = listado.Opcion.Descripcion,
                                         label = listado.Titulo,

                                     }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public ActionResult ContratoImportacion(int? RowID)
        {
            ViewBag.periodoE = db.Opcion.Where(pe => pe.Agrupacion.Nombre == "PERIODO EMBARQUE").ToList();
            ViewBag.transporte = db.Opcion.Where(pe => pe.Agrupacion.Nombre == "TRANSPORTE").ToList();
            if (RowID != null || RowID > 0)
            {
                Contrato contrato = db.Contrato.Where(c => c.RowID == RowID).FirstOrDefault();
                return View(contrato);
            }
            else
            {
                return View(new Contrato());
            }

        }
        public ActionResult ContratosImportacion()
        {
            int rowid = rowid_tipo("IMPORTACION");
            List<Contrato> Lista = db.Contrato.Where(c => c.TipoContratoID == rowid).ToList();
            ViewBag.contratos = Lista;
            return View(Lista);
        }
        public JsonResult RegistrarContratoImportacion(FormCollection form, int RowID, int RowIDPro)
        {
            String mensaje = "";
            Contrato ObjContrato = new Contrato();
            try
            {
                if (RowID == 0)
                {

                    form = DeSerialize(form);
                    ObjContrato.ProformaID = RowIDPro;
                    ObjContrato.Titulo = form["titulo"];
                    ObjContrato.Fecha = Convert.ToDateTime(form["fecha"]);
                    ObjContrato.Intermediario = form["intermediario"];
                    ObjContrato.Seguro = form["seguro"];
                    ObjContrato.Calidad = form["calidad"];
                    ObjContrato.EmbarqueID = Convert.ToInt32(form["periodoE"]);
                    ObjContrato.Transporte = form["transporte"];
                    ObjContrato.Calidad = form["calidad1"];
                    ObjContrato.Calidad2 = form["calidad2"];
                    ObjContrato.Inspector = form["autoc_inspector"];
                    ObjContrato.EmbarqueParcial = form["EmbarqueP"];
                    ObjContrato.FechaCreacion = UtilTool.GetDateTime();
                    ObjContrato.TipoContratoID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "EXPORTACION").FirstOrDefault().RowID;
                    ObjContrato.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.Contrato.Add(ObjContrato);
                    db.SaveChanges();
                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    ObjContrato = db.Contrato.Where(le => le.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    ObjContrato.ProformaID = RowIDPro;
                    ObjContrato.Titulo = form["titulo"];
                    ObjContrato.Fecha = Convert.ToDateTime(form["fecha"]);
                    ObjContrato.Intermediario = form["intermediario"];
                    ObjContrato.Seguro = form["seguro"];
                    ObjContrato.Calidad = form["calidad"];
                    ObjContrato.EmbarqueID = Convert.ToInt32(form["periodoE"]);
                    ObjContrato.Transporte = form["transporte"];
                    ObjContrato.Calidad = form["calidad1"];
                    ObjContrato.Calidad2 = form["calidad2"];
                    ObjContrato.Inspector = form["autoc_inspector"];
                    ObjContrato.EmbarqueParcial = form["EmbarqueP"];
                    ObjContrato.FechaCreacion = UtilTool.GetDateTime();
                    ObjContrato.TipoContratoID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "EXPORTACION").FirstOrDefault().RowID;
                    ObjContrato.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SaveChanges();
                    mensaje = "Se ha actualizado correctamente";
                }
            }
            catch (Exception e)
            {
                mensaje = "No se ha podido guardar los datos, error : " + e.Message;

            }

            int rowid = ObjContrato.RowID;

            return Json(rowid, JsonRequestBehavior.AllowGet);
        }
        public string ItemsContrato(int RowID)
        {
            string result = "";
            Proforma proforma = db.Proforma.Where(p => p.RowID == RowID).FirstOrDefault();        
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
        public int rowid_tipo(string tipo_proforma)
        {
            int rowid_tipo = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == tipo_proforma).FirstOrDefault().RowID;
            return rowid_tipo;
        }
    }
}
