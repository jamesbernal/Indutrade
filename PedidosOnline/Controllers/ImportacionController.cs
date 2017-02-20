using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Models;
using PedidosOnline.Utilidades;

namespace PedidosOnline.Controllers
{
    public class ImportacionController : Controller
    {
        PedidosOnlineEntities db = new PedidosOnlineEntities();

        #region ::::::METODOS GENERALES::::::
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
        [CheckSessionOut]
        public JsonResult Buscar_MatricesBL()
        {
            List<MatricesBL> data = new List<MatricesBL>();
            string terms = Request.Params["term"].Trim().ToUpper();

            List<MatricesBL> Lista = (from listado in db.MatrizBL
                                      where (listado.NumeroReserva.Contains(terms))
                                      select new MatricesBL()
                                      {
                                          RowID = listado.RowID,
                                          puertoC = listado.Contrato.Proforma.Puerto.Nombre,
                                          puertoD = listado.Contrato.Proforma.Puerto1.Nombre,
                                          consignatario = listado.Consignee,
                                          expedidor = listado.Expedidor,
                                          cantidadC = "",//db.CalculadoraItems.Where(c => c.CalculadoraID == listado.Contrato.Proforma.CalculadoraID).Sum(d => d.NumeroContenedor)
                                          label = listado.NumeroReserva,
                                      }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());

            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        [CheckSessionOut]
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
        [CheckSessionOutAttribute]
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
        [CheckSessionOut]
        public JsonResult BuscarTercero(string term)
        {
            var result = (from reg in db.Tercero.Where(f => f.RazonSocial.Contains(term))
                          select new
                          {
                              label = reg.RazonSocial,
                              nit = reg.Identificacion,
                              rowid = reg.RowID,
                              ciudad = reg.ContactoERP.Ciudad.Nombre + "," + reg.ContactoERP.Ciudad.Departamento.Pais.Nombre,
                              telefono = reg.ContactoERP.Telefono1,
                              direccion = reg.ContactoERP.Direccion1
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CheckSessionOut]
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
        [CheckSessionOut]
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
                                         puertoc = listado.Proforma.Puerto.Nombre,
                                         puertod = listado.Proforma.Puerto1.Nombre,
                                         label = listado.Titulo,
                                     }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        [ValidateInput(false)]
        [CheckSessionOut]
        public JsonResult Datos_Contrato(string titulo)
        {
            Contrato contrato = db.Contrato.Where(p => p.Titulo == titulo).FirstOrDefault();
            int RowID = contrato.RowID;
            string Titulo = contrato.Titulo;
            int RowIDPro = contrato.ProformaID;
            string puertoc = contrato.Proforma.Puerto.Nombre;
            string puertod = contrato.Proforma.Puerto1.Nombre;
            var data = new { RowID = RowID, Titulo = Titulo, RowIDPro = RowIDPro, puertoc = puertoc, puertod = puertod };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region :::::PROFORMA:::::
        [CheckSessionOut]
        public JsonResult InfoTercero(int rowid)
        {
            var result = (from reg in db.Tercero.Where(f => f.RowID == rowid)
                          select new
                          { label = reg.RazonSocial, nit = reg.Identificacion, rowid = reg.RowID, ciudad = reg.ContactoERP.Ciudad.Nombre + "," + reg.ContactoERP.Ciudad.Departamento.Pais.Nombre, telefono = reg.ContactoERP.Telefono1, direccion = reg.ContactoERP.Direccion1 });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CheckSessionOut]
        public string CalculadoraComprador(int? rowid)
        {
            string result = "<option value=\"\">-Seleccionar-</option>";
            List<Calculadora> lista = db.Calculadora.Where(f => f.TerceroID == rowid).ToList();
            foreach (var item in lista)
            {
                result += "<option value=\"" + item.RowID + "\">" + item.Opcion.Nombre + "-" + item.RowID + "</option>";
            }
            return result;
        }
        [CheckSessionOut]
        public string GuardarProforma()
        {
            Proforma reg = new Models.Proforma();
            int rowid = int.Parse(Request.Params["rowid"]);
            reg = db.Proforma.Where(f => f.RowID == rowid).FirstOrDefault();
            if (reg == null)
            {
                reg = new Models.Proforma();
            }
            reg.Titulo = Request.Params["encabezado"];
            reg.CompradorID = int.Parse(Request.Params["rowid_comprador"]);
            reg.VendedorID = int.Parse(Request.Params["rowid_vendedor"]);
            reg.CalculadoraID = int.Parse(Request.Params["calculadora"]);
            reg.Fecha = DateTime.Parse(Request.Params["fecha"]);
            reg.UsuarioCreacion = ((Usuario)Session["curUser"]).NombreUsuario;
            reg.TipoProformaID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "IMPORTACION").FirstOrDefault().RowID;
            reg.FechaCreacion = DateTime.Now;
            if (reg.RowID == 0)
            {
                db.Proforma.Add(reg);
            }

            db.SaveChanges();
            return reg.RowID.ToString();
        }
        [CheckSessionOut]
        public string ItemsCalculadora(int? rowid)
        {
            string result = "";
            List<CalculadoraItems> lista = db.CalculadoraItems.Where(f => f.CalculadoraID == rowid).ToList();
            foreach (var item in lista)
            {
                result += "<tr><td>" + item.Item.Referencia + "-" + item.Item.Descripcion + "</td><td>" + item.CantidadTonelada + "</td><td><input type=\"number\" id=\"UV" + item.ItemID + "\" name=\"UV" + item.ItemID + "\"  value='" + item.PVE + "' disabled /></td><td><input type=\"number\" class='total' name='TI" + item.ItemID + "' disabled value='" + item.DatosVentaUSDTotal + "' /></td></tr>";
            }
            return result;
        }
        [CheckSessionOut]
        public string GuardarProformaItems()
        {
            int rowid = int.Parse(Request.Params["rowid"]);
            Proforma reg = db.Proforma.Where(f => f.RowID == rowid).FirstOrDefault();
            reg.PuertoDescargueID = int.Parse(Request.Params["puertodescargue"]);
            reg.PuertoCargueID = int.Parse(Request.Params["puertocargue"]);
            reg.TipoPagoID = int.Parse(Request.Params["formapago"]);
            reg.NumeroContrato = Request.Params["numerocontrato"];
            reg.Observaciones = Request.Params["observaciones"];
            reg.FechaEnvio = DateTime.Parse(Request.Params["fechaenvio"]);
            reg.FechaModificacion = DateTime.Now;
            reg.UsuarioModificacion = ((Usuario)Session["curUser"]).NombreUsuario;
            db.SaveChanges();
            return "";
        }
        [CheckSessionOut]
        public ActionResult Proforma(int? rowid)
        {
            ViewBag.FormaPago = db.Opcion.Where(f => f.Agrupacion.Nombre == "PROFORMA.FORMAPAGO").ToList();
            ViewBag.Puertos = db.Puerto.ToList();
            reg_Proforma reg = new reg_Proforma();
            if (rowid > 0)
            {
                reg.Proforma = db.Proforma.Where(f => f.RowID == rowid).FirstOrDefault();
                reg.items = db.CalculadoraItems.Where(f => f.CalculadoraID == reg.Proforma.CalculadoraID).ToList();
            }
            else
            {
                reg.items = new List<CalculadoraItems>();
                reg.Proforma = new Models.Proforma();
                reg.Proforma.Tercero = new Tercero();
                reg.Proforma.Tercero1 = new Tercero();
                reg.Proforma.Calculadora = new Calculadora();
                reg.Proforma.Calculadora.Ciudad = new Ciudad();
                reg.Proforma.Calculadora.Opcion = new Opcion();
                reg.Proforma.Calculadora.Opcion1 = new Opcion();
            }

            return View(reg);
        }
        [CheckSessionOut]
        public ActionResult Proformas()
        {
            List<Proforma> Lista = (from reg in db.Proforma.Where(p => p.Opcion1.Codigo == "IMPORTACION").ToList()
                                    select new Proforma
                                    {
                                        RowID = reg.RowID,
                                        Calculadora = reg.Calculadora,
                                        Puerto = reg.Puerto == null ? new Puerto() : reg.Puerto,
                                        Puerto1 = reg.Puerto1 == null ? new Puerto() : reg.Puerto1,
                                        Opcion = reg.Opcion == null ? new Opcion() : reg.Opcion,
                                        Tercero = reg.Tercero,
                                        Tercero1 = reg.Tercero1,
                                        Titulo = reg.Titulo
                                    }).ToList();
            return View(Lista);
        }
        [CheckSessionOut]
        public ActionResult RecursoProformaImportacion(int rowid_proformaimportacion)
        {
            return View();
        }
        [CheckSessionOut]
        public JsonResult GuardarRecursoProformaImportacion()
        {
            RecursosProforma objrecurso = new RecursosProforma();
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
                        objrecurso.ProformaID = rowid_proforma;
                        HttpPostedFileBase file = Request.Files[contador_inser];
                        ruta_archivo = rowid_proforma + nombre[contador_inser] + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/RepositorioArchivos/ArchivosProformas/" + ruta_archivo));
                        ruta_archivo = "/RepositorioArchivos/ArchivosProformas/" + ruta_archivo;
                        objrecurso.Archivo = ruta_archivo;
                        db.RecursosProforma.Add(objrecurso);
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
        #endregion

        #region :::::CONTRATO:::::
        [CheckSessionOutAttribute]
        public ActionResult RecursosContrato()
        {
            return View();
        }
        [CheckSessionOutAttribute]
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
        [CheckSessionOutAttribute]
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
        [CheckSessionOutAttribute]
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
        [CheckSessionOutAttribute]
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
        [CheckSessionOutAttribute]
        public ActionResult Contratos()
        {
            List<Contrato> Lista = db.Contrato.Where(c => c.TipoContratoID== db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "IMPORTACION").FirstOrDefault().RowID).ToList();
            ViewBag.contratos = Lista;
            return View(Lista);
        }
        [CheckSessionOutAttribute]
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
                    ObjContrato.TipoContratoID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "IMPORTACION").FirstOrDefault().RowID;
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
                    ObjContrato.TipoContratoID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "IMPORTACION").FirstOrDefault().RowID;
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
        [CheckSessionOutAttribute]
        public int rowid_tipo(string tipo_proforma)
        {
            int rowid_tipo = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == tipo_proforma).FirstOrDefault().RowID;
            return rowid_tipo;
        }
        #endregion
    }
}