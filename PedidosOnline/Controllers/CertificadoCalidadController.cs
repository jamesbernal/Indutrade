using PedidosOnline.Models;
using PedidosOnline.Utilidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PedidosOnline.Controllers
{
    public class CertificadoCalidadController : Controller
    {
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
        public ActionResult ListaCertificadoCalidad()
        {
            ViewBag.listadocalidad = db.CertificadoCalidad.ToList();
            return View();
        }
        public ActionResult CertificadoCalidad( int? Rowid_certificado)
        {
            ViewBag.listaciudades = db.Ciudad.ToList();
            if (Rowid_certificado == null)
            {

                return View(new CertificadoCalidad());
            }
            else
            {
                return View(db.CertificadoCalidad.Where(ep => ep.RowID == Rowid_certificado).FirstOrDefault());
            }
        }
        public JsonResult Buscar_MatricesBL()
        {
            List<MatricesBL> data = new List<MatricesBL>();
            string terms = Request.Params["term"].Trim().ToUpper();

            List<MatricesBL> Lista = (from listado in db.MatrizBL
                                      where (listado.NumeroReserva.Contains(terms)) 
                                      select new MatricesBL()
                                      {
                                          RowID = listado.RowID,
                                          expedidor = listado.Contrato.Proforma.Tercero.RazonSocial,
                                          label = listado.NumeroReserva,
                                      }).Distinct().OrderBy(f => f.label).ToList();
            data.AddRange(Lista.ToList());

            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
       
        public JsonResult Buscar_Producto(string term)
        {
            List<Item> lista = db.Item.Where(f => f.Descripcion.Contains(term)).ToList();
            var result = (from reg in lista.ToList()
                          select new
                          {
                              label = reg.RowID + "-" + reg.Descripcion,
                              rowid = reg.RowID
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult guardar_certificado(FormCollection formulario, int Rowid_certificado)
        {
            formulario = DeSerialize(formulario);
            CertificadoCalidad ObjCertificado = new CertificadoCalidad();

                if (Rowid_certificado == 0)
                {
                    ObjCertificado.MatrizBLID = int.Parse(formulario["documento_mtz"]);
                    ObjCertificado.FechaRevision = DateTime.Parse(formulario["fecha_revision"]);
                    ObjCertificado.Cliente = formulario["cliente"];
                    ObjCertificado.CiudadID = int.Parse(formulario["ciudad"]);
                    ObjCertificado.Direccion = formulario["direccion"];
                    ObjCertificado.ProductoID = int.Parse(formulario["producto_id"]);
                    ObjCertificado.Cantidad = int.Parse(formulario["cantidad"]);
                    ObjCertificado.Codigo = formulario["codigo"];
                    ObjCertificado.Fecha_Elaboracion_Producto = DateTime.Parse(formulario["fecha_elaboracion"]);
                    ObjCertificado.Fecha_Vencimiento_Producto = DateTime.Parse(formulario["fecha_vencimiento"]);
                    ObjCertificado.FechaCreacion = UtilTool.GetDateTime();
                    ObjCertificado.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.CertificadoCalidad.Add(ObjCertificado);
                    db.SaveChanges();
                }
                else
                {
                    ObjCertificado = db.CertificadoCalidad.Where(le => le.RowID == Rowid_certificado).FirstOrDefault();
                    ObjCertificado.MatrizBLID = int.Parse(formulario["id_documentobl"]);
                    ObjCertificado.FechaRevision = DateTime.Parse(formulario["fecha_revision"]);
                    ObjCertificado.Cliente = (formulario["cliente"]);
                    ObjCertificado.CiudadID = int.Parse(formulario["ciudad"]);
                    ObjCertificado.Direccion = formulario["direccion"];
                    ObjCertificado.ProductoID = int.Parse(formulario["producto"]);
                    ObjCertificado.Cantidad = int.Parse(formulario["cantidad"]);
                    ObjCertificado.Codigo = formulario["cantidad"];
                    ObjCertificado.Fecha_Elaboracion_Producto = DateTime.Parse(formulario["fecha_elaboracion"]);
                    ObjCertificado.Fecha_Vencimiento_Producto = DateTime.Parse(formulario["fecha_vencimiento"]);
                    ObjCertificado.FechaModificacion = UtilTool.GetDateTime();
                    ObjCertificado.UsuarioModificacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SaveChanges();
                }
            int rowid = ObjCertificado.RowID;

            return Json(rowid, JsonRequestBehavior.AllowGet);
        }

    }
}