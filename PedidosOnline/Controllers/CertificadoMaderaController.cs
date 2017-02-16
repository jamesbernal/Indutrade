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
    public class CertificadoMaderaController : Controller
    {
        // GET: CertificadoMadera
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
        public ActionResult ListaCertificadoMadera()
        {
            List<CertificadoMadera> lista = db.CertificadoMadera.OrderByDescending(f => f.RowID).ToList();
            ViewBag.certificados = lista.ToList();
            return View(lista);
        }

        [CheckSessionOut]
        public ActionResult CertificadoMadera(int? rowid)
        {
            if (rowid != null || rowid > 0)
            {
                CertificadoMadera certificado = db.CertificadoMadera.Where(m => m.RowID == rowid).FirstOrDefault();
                return View(certificado);
            }
            else
            {
                return View(new CertificadoMadera());
            }
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

        [ValidateInput(false)]
        public JsonResult Datos_MatrizBL(string titulo)
        {
            MatrizBL matriz = db.MatrizBL.Where(p => p.NumeroReserva == titulo).FirstOrDefault();
            int RowID = matriz.RowID;
            string puertoC = matriz.Contrato.Proforma.Puerto.Nombre;
            string puertoD = matriz.Contrato.Proforma.Puerto1.Nombre;
            string consignatario = matriz.Consignee;
            string expedidor = matriz.Expedidor;
            decimal cantidadC = Convert.ToDecimal(db.CalculadoraItems.Where(c => c.CalculadoraID == matriz.Contrato.Proforma.CalculadoraID).Sum(d => d.NumeroContenedor));
            var data = new { RowID = RowID, puertoC = puertoC, puertoD = puertoD, consignatario = consignatario, expedidor = expedidor, cantidadC = cantidadC };
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [CheckSessionOutAttribute]
        public JsonResult RegistrarCertificadoMadera(FormCollection form, int RowID, int RowIDBL, int RowIDM, int RowIDC)
        {
            String mensaje = "";
            CertificadoMadera ObjCertificadoMadera = new CertificadoMadera();
            try
            {
                if (RowID == 0)
                {
                    form = DeSerialize(form);
                    ObjCertificadoMadera.MatrizBLID = RowIDBL;
                    ObjCertificadoMadera.MotonaveID = RowIDM;
                    ObjCertificadoMadera.CiudadID = RowIDC;
                    ObjCertificadoMadera.DiaInspeccion = Convert.ToDateTime(form["fecha"]);
                    ObjCertificadoMadera.Para = form["para"];
                    ObjCertificadoMadera.DescripcionBienes = form["descripcionB"];
                    ObjCertificadoMadera.Inspector = form["inspector"];
                    ObjCertificadoMadera.FechaCreacion = UtilTool.GetDateTime();
                    ObjCertificadoMadera.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.CertificadoMadera.Add(ObjCertificadoMadera);
                    db.SaveChanges();

                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    //Actualizar el plantilla
                    ObjCertificadoMadera = db.CertificadoMadera.Where(le => le.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    ObjCertificadoMadera.MatrizBLID = RowIDBL;
                    ObjCertificadoMadera.MotonaveID = RowIDM;
                    ObjCertificadoMadera.CiudadID = RowIDC;
                    ObjCertificadoMadera.DiaInspeccion = Convert.ToDateTime(form["fecha"]);
                    ObjCertificadoMadera.Para = form["para"];
                    ObjCertificadoMadera.DescripcionBienes = form["descripcionB"];
                    ObjCertificadoMadera.Inspector = form["inspector"];
                    ObjCertificadoMadera.FechaModificacion = UtilTool.GetDateTime();
                    ObjCertificadoMadera.UsuarioModificacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    db.SaveChanges();
                    mensaje = "Se ha actualizado correctamente";
                }

            }
            catch (Exception e)
            {
                mensaje = "No se ha podido guardar los datos, error : " + e.Message;

            }

           int rowid = ObjCertificadoMadera.RowID ;
            return Json(rowid, JsonRequestBehavior.AllowGet);
        }



    }
}
