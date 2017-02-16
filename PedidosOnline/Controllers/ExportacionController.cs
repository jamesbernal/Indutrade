using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Models;
using PedidosOnline.Utilidades;

namespace PedidosOnline.Controllers
{
    public class ExportacionController : Controller
    {
        // GET: Exportacion

        PedidosOnlineEntities db = new PedidosOnlineEntities();
        #region ::DCD
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
        public ActionResult DCD()
        {
            List<DCD> lista = db.DCD.OrderByDescending(f => f.RowID).ToList();
            ViewBag.ListaDCD = lista.ToList();
            return View(lista);
        }
        [CheckSessionOut]
        public ActionResult DatosDCD(int? RowID)
        {
            if (RowID != null || RowID > 0)
            {
                DCD Datos = db.DCD.Where(c => c.RowID == RowID).FirstOrDefault();
                ViewBag.ListaItems = db.CalculadoraItems.Where(f => f.CalculadoraID == Datos.Contrato.Proforma.CalculadoraID).ToList();
                return View(Datos);
            }
            else
            {
                return View(new DCD());
            }
        }
        [CheckSessionOut]
        public JsonResult Buscar_Representante()
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
                                    }).Distinct().OrderBy(f => f.label).ToList();
            data.AddRange(Lista.ToList());


            var jsonResult = Json(data.OrderBy(f => f.RowID), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        [CheckSessionOut]
        public JsonResult BuscarContrato(int? RowID)
        {
            var cont = (from terceroexiste in db.Contrato.Where(f => f.RowID == RowID).ToList()
                        select new
                        {
                            nombrecomp = terceroexiste.Proforma.Tercero.RazonSocial,
                            nitcomp = terceroexiste.Proforma.Tercero.Identificacion,
                            nombreven = terceroexiste.Proforma.Tercero1.RazonSocial,
                            nitven = terceroexiste.Proforma.Tercero1.Identificacion,
                             }).FirstOrDefault();

            var jsonResult = Json(cont, JsonRequestBehavior.AllowGet);
            return Json( cont  , JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarDCD(FormCollection form, int? RowID, int? RowIDContrato)
        {
            String mensaje = "";
            DCD Objdcd = new DCD();
            try
            {
                if (RowID == 0)
                {
                    form = DeSerialize(form);
                    Objdcd.ContratoID = RowIDContrato.Value;
                    Objdcd.Año = form["ano"];
                    Objdcd.Mes = form["Mes"];
                    Objdcd.NroConvenio = form["NroConvenio"];
                    Objdcd.RepresentanteLegal = form["Reprensentanteleg"];
                    Objdcd.FechaExpedicion = form["Fecha"];
                    Objdcd.Destino = form["Destinarlo"];
                    db.DCD.Add(Objdcd);
                    db.SaveChanges();
                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    Objdcd = db.DCD.Where(f => f.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    Objdcd.Año = form["Año"];
                    Objdcd.Mes = form["Mes"];
                    Objdcd.NroConvenio = form["NroConvenio"];
                    Objdcd.RepresentanteLegal = form["Reprensentanteleg"];
                    Objdcd.FechaExpedicion = form["Fecha"];
                    Objdcd.Destino = form["Destinarlo"];
                    db.SaveChanges();
                    mensaje = "Se ha actualizado correctamente";
                }
            }
            catch (Exception e)
            {
                mensaje = "No se ha podido guardar los datos, error : " + e.Message;
            }

            int rowid = Objdcd.RowID;

            return Json(rowid, JsonRequestBehavior.AllowGet);
        }
        public string BalanceCalculadora(int? rowid)
        {
            string result = "", embalaje = "";
            Contrato cont = db.Contrato.Where(f => f.RowID == rowid).FirstOrDefault();
            if (cont != null)
            {
                List<CalculadoraItems> lista = db.CalculadoraItems.Where(f => f.CalculadoraID == cont.Proforma.CalculadoraID).ToList();
                foreach (var item in lista)
                {
                    if (item.Opcion != null)
                    {
                        embalaje = item.Opcion.Nombre;
                    }
                    result += "<tr><td>" + item.Item.Descripcion + "</td>" +
                       "<td>" + embalaje + "</td>" +
                       "<td>" + item.CantidadTonelada + "</td>" +
                       "<td>" + item.NumeroEnvio + "</td>" +
                       "<td>" + item.NumeroContenedor + "</td>" +
                       "<td>" + item.TRM + "</td>" +
                       "<td>" + item.MPPCUSDCalculado + "</td>" +
                       "<td>" + item.CTotalPUSD_usd + "</td>" +
                       "<td></td></tr>";
                }
            }
            else
            {
                result += "<tr><td></td>" +
                       "<td></td>" +
                       "<td></td>" +
                       "<td></td>" +
                       "<td></td>" +
                       "<td></td>" +
                       "<td></td>" +
                       "<td></td>" +
                       "<td></td></tr>";
            }
            return result;
        }
        #endregion

        #region FEP
        [CheckSessionOut]
        public ActionResult ListadoRemisionFEP()
        {
            List<FEP> lista = db.FEP.OrderByDescending(f => f.RowID).ToList();
            ViewBag.listaFEP = lista.ToList();
            return View(lista);
        }
        [CheckSessionOut]
        public ActionResult DatoFEP(int? RowID)
        {
            if (RowID != null || RowID > 0)
            {
                FEP Datos = db.FEP.Where(c => c.RowID == RowID).FirstOrDefault();
                ViewBag.ListaItems = db.CalculadoraItems.Where(f => f.CalculadoraID == Datos.DCD.Contrato.Proforma.CalculadoraID).ToList();
                return View(Datos);
            }
            else
            {
                return View(new FEP());
            }
        }

        public JsonResult BuscarDCD(int? RowID)
        {
            DCD data = db.DCD.Where(f => f.RowID == RowID).FirstOrDefault();
            if (data == null)
            {
                Response.StatusCode = 500;
                return Json(0, JsonRequestBehavior.AllowGet);
            }

                SolicitudTransporte slt = db.SolicitudTransporte.Where(f => f.ContratoID == data.ContratoID).FirstOrDefault();
                var cont = (from terceroexiste in db.DCD.Where(f => f.RowID == RowID).ToList()
                            select new
                            {
                                nombrecomp = terceroexiste.Contrato.Proforma.Tercero.RazonSocial,
                                nitcomp = terceroexiste.Contrato.Proforma.Tercero.Identificacion,
                                nombreven = terceroexiste.Contrato.Proforma.Tercero1.RazonSocial,
                                nitven = terceroexiste.Contrato.Proforma.Tercero1.Identificacion,
                                convenio = terceroexiste.NroConvenio,
                                fechac = slt.FechaCargue.ToString(),
                            }).FirstOrDefault();
                var jsonResult = Json(cont, JsonRequestBehavior.AllowGet);
                return Json(cont, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GuardarFEP(FormCollection form, int? RowID, int? RowID_DCD)
        {
            String mensaje = "";
            FEP Objfep = new FEP();
            try
            {
                if (RowID == 0)
                {
                    form = DeSerialize(form);
                    Objfep.DCDID = RowID_DCD;
                    Objfep.Dex = form["Dex"];
                    Objfep.NCertificadoCP = form["nCertificado"];
                    db.FEP.Add(Objfep);
                    db.SaveChanges();
                    mensaje = "Se ha ingresado correctamente";
                }
                else
                {
                    Objfep = db.FEP.Where(f => f.RowID == RowID).FirstOrDefault();
                    form = DeSerialize(form);
                    Objfep.DCDID = RowID_DCD;
                    Objfep.Dex = form["Dex"];
                    Objfep.NCertificadoCP = form["nCertificado"];
                    db.SaveChanges();
                    mensaje = "Se ha actualizado correctamente";
                }
            }
            catch (Exception e)
            {
                mensaje = "No se ha podido guardar los datos, error : " + e.Message;
            }

            int rowid = Objfep.RowID;
            return Json(rowid, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}