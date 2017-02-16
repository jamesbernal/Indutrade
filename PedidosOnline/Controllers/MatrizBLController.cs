using PedidosOnline.Models;
using PedidosOnline.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PedidosOnline.Controllers
{
    public class MatrizBLController : Controller
    {
        // GET: MatrizBL
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
        public ActionResult ListaMatrizBL()
        {
            List<MatrizBL> lista = db.MatrizBL.OrderByDescending(f => f.RowID).ToList();
            ViewBag.matrices = lista.ToList();
            return View(lista);
        }

        [CheckSessionOut]
        public ActionResult MatrizBL(int? rowid)
        {
            if (rowid != null || rowid > 0)
            {
               MatrizBL matrizbl = db.MatrizBL.Where(m => m.RowID == rowid).FirstOrDefault();
                return View(matrizbl);
            }
            else
            { 
            return View(new MatrizBL());
            }
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
        
       public JsonResult Buscar_Empresas()
        {

            List<Terceros> data = new List<Terceros>();
            string terms = Request.Params["term"].Trim().ToUpper();
            List<Terceros> Lista = (from listado in db.Tercero
                                    where (listado.RazonSocial.Contains(terms))
                                     select new Terceros()
                                     {
                                         RowID = listado.RowID,
                                         Nit = listado.Identificacion,
                                         Direccion = listado.ContactoERP.Direccion1,
                                         Ciudad = listado.ContactoERP.Ciudad.Nombre,
                                         Telefono = listado.ContactoERP.Telefono1,
                                         label = listado.RazonSocial,
                                     }).Distinct().OrderBy(f => f.label).ToList();//.Take(15);
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }


        [ValidateInput(false)]
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

        [ValidateInput(false)]
        public JsonResult Datos_Empresa(string titulo)
        {
            Tercero empresa = db.Tercero.Where(p => p.RazonSocial == titulo).FirstOrDefault();
            int RowID = empresa.RowID;
            string Nit = empresa.Identificacion;
            string Telefono = empresa.ContactoERP.Telefono1;
            string Direccion = empresa.ContactoERP.Direccion1;
            string RazonSocial = empresa.RazonSocial;
            string Ciudad = empresa.ContactoERP.Ciudad.Nombre;
            var data = new { RowID = RowID, Titulo = RazonSocial, Nit = Nit, Telefono = Telefono, Direccion = Direccion, Ciudad= Ciudad };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [CheckSessionOutAttribute]
        public JsonResult RegistrarMatrizBL(FormCollection form, int RowID, int RowIDCon, int RowIDE)
        {
            String mensaje = "";
            MatrizBL ObjMatriz = new MatrizBL();
            try
            {
                if (RowID == 0)
                {
                    form = DeSerialize(form);
                    ObjMatriz.ContratoID = RowIDCon;
                    ObjMatriz.EmpresaID = RowIDE;
                    ObjMatriz.Fecha = Convert.ToDateTime(form["fecha"]);
                    ObjMatriz.NumeroReserva = form["numReserva"];
                    ObjMatriz.Expedidor = form["expedidor"];
                    ObjMatriz.Consignee = form["consignee"];
                    ObjMatriz.Notificacion = form["notificacion"];
                    try { ObjMatriz.NCM = Convert.ToInt32(form["ncm"]); } catch { }
                    ObjMatriz.FechaCreacion = UtilTool.GetDateTime();
                    ObjMatriz.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.MatrizBL.Add(ObjMatriz);
                    db.SaveChanges();

                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    //Actualizar el plantilla
                    ObjMatriz = db.MatrizBL.Where(le => le.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    ObjMatriz.ContratoID = RowIDCon;
                    ObjMatriz.EmpresaID = RowIDE;
                    ObjMatriz.Fecha = Convert.ToDateTime(form["fecha"]);
                    ObjMatriz.NumeroReserva = form["numReserva"];
                    ObjMatriz.Expedidor = form["expedidor"];
                    ObjMatriz.Consignee = form["consignee"];
                    ObjMatriz.Notificacion = form["notificacion"];
                    try { ObjMatriz.NCM = Convert.ToInt32(form["ncm"]); } catch { }
                    ObjMatriz.FechaModificacion = UtilTool.GetDateTime();
                    ObjMatriz.UsuarioModificacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SaveChanges();
                    mensaje = "Se ha actualizado correctamente";
                }

            }
            catch (Exception e)
            {
                mensaje = "No se ha podido guardar los datos, error : " + e.Message;

            }

            int rowid= ObjMatriz.RowID;
            return Json(rowid , JsonRequestBehavior.AllowGet);
        }

    }
}
