using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Models;
using PedidosOnline.Utilidades;

namespace PedidosOnline.Controllers
{
    public class ProformaController : Controller
    {
        // GET: Proforma
        public ActionResult Index()
        {
            return View();
        }
        PedidosOnlineEntities db = new PedidosOnlineEntities();
        [CheckSessionOut]
        public ActionResult Proformas()
        {
            List<Proforma> Lista = (from reg in db.Proforma.Where(p => p.Opcion1.Codigo == "EXPORTACION").ToList()
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
        public JsonResult BuscarTercero(string term)
        {
            var result = (from reg in db.Tercero.Where(f => f.RazonSocial.Contains(term))
                          select new
                          { label = reg.RazonSocial, nit = reg.Identificacion, rowid = reg.RowID, ciudad = reg.ContactoERP.Ciudad.Nombre + "," + reg.ContactoERP.Ciudad.Departamento.Pais.Nombre, telefono = reg.ContactoERP.Telefono1, direccion = reg.ContactoERP.Direccion1 });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InfoTercero(int rowid)
        {
            var result = (from reg in db.Tercero.Where(f => f.RowID == rowid)
                          select new
                          { label = reg.RazonSocial, nit = reg.Identificacion, rowid = reg.RowID, ciudad = reg.ContactoERP.Ciudad.Nombre + "," + reg.ContactoERP.Ciudad.Departamento.Pais.Nombre, telefono = reg.ContactoERP.Telefono1, direccion = reg.ContactoERP.Direccion1 });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
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
            reg.TipoProformaID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "EXPORTACION").FirstOrDefault().RowID;
            reg.FechaCreacion = DateTime.Now;
            if (reg.RowID == 0)
            {
                db.Proforma.Add(reg);
            }

            db.SaveChanges();
            return reg.RowID.ToString();
        }
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
        public ActionResult ProformaImportacion(int? rowid)
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

        public ActionResult ProformasImportacion()
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
        public ActionResult RecursoProformaImportacion(int rowid_proformaimportacion)
        {
            return View();
        }

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
    }
}