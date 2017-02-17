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

        #region ::::::METODOS GENERALES::::::
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

        #region ::::CALCULADORA::::

        [CheckSessionOut]
        public ActionResult Calculadoras()
        {
            List<Calculadora> lista = db.Calculadora.OrderByDescending(f => f.RowID).ToList();
            return View(lista);
        }
        [CheckSessionOut]
        public ActionResult Calculadora(int? rowid)
        {
            ViewBag.Terceros = db.Tercero.ToList();
            ViewBag.Ciudad = db.Ciudad.ToList();
            ViewBag.Incoterms = db.Opcion.Where(f => f.Agrupacion.Nombre == "CALCULADORA.ITEM" && f.Activo == true).ToList();
            ViewBag.AgenciaNaviera = db.AgenteNaviera.ToList();
            ViewBag.ListaEmbalajes = db.Opcion.Where(f => f.Agrupacion.Nombre == "CALCULADORA.EMBALAJE" && f.Activo == true).ToList();

            reg_calculadora obj = new reg_calculadora();
            obj.Calculadora = new Models.Calculadora();
            if (rowid != null || rowid > 0)
            {
                obj = (from reg in db.Calculadora.Where(f => f.RowID == rowid)
                       select new reg_calculadora
                       {
                           Calculadora = reg,
                           items = db.CalculadoraItems.Where(f => f.CalculadoraID == reg.RowID).ToList()
                       }).FirstOrDefault();
            }
            return View(obj);
        }
        public string GuardarCalculadora()
        {
            Calculadora obj = new Models.Calculadora();
            int rowid = int.Parse(Request.Params["rowid"]);
            int rowid_calculadora = 0;
            if (rowid > 0)
            {
                obj = db.Calculadora.Where(f => f.RowID == rowid).FirstOrDefault();
                obj.UsuarioModificacion = "Admin";//((s_usuario)Session["curUser"]).nombre_usuario;
                obj.FechaModificacion = DateTime.Now;
            }
            else
            {
                obj.UsuarioCreacion = "Admin";//((s_usuario)Session["curUser"]).nombre_usuario;
                obj.FechaCreacion = DateTime.Now;
            }
            obj.TerceroID = int.Parse(Request.Params["cliente"]);
            obj.IncotermID = int.Parse(Request.Params["InCortems"]);
            obj.Fecha = DateTime.Parse(Request.Params["DateDetalle"]);
            obj.DestinoID = int.Parse(Request.Params["ciudadID"]);
            //obj.AgenteNavieraID = int.Parse(Request.Params["agenciaNav"]);
            obj.NombreBroker = Request.Params["NombreBroker"];
            obj.observacion = Request.Params["observa"];
            if (obj.RowID == 0)
            {
                db.Calculadora.Add(obj);
            }

            rowid_calculadora = obj.RowID;

            db.SaveChanges();
            return obj.RowID.ToString();
        }
        public string CostoLogistico()
        {
            #region TablaCostosLogisticos
            List<Costo> lista = db.Costo.Where(f => f.Opcion1.Nombre == "LOGISTICO").ToList();
            string Build = "";
            foreach (var item in lista)
            {
                if (item.Nombre.Contains("Broker"))
                    Build += "<tr><td>" + item.Nombre + "</td><td>" + item.Opcion.Nombre + "</td><td><input class='currency deshabilitar' step='any' type='number' id='costoContenedor" + item.RowID + "' name='costoContenedor" + item.RowID + "'  onkeyup=\"ReCalcular()\" /></td><td><input class='currency ' step='any' style='background: rgba(0, 114, 199, 0.55); color:white' type='number' id='costoTonelada" + item.RowID + "' name='costoTonelada" + item.RowID + "' onkeyup=\"ReCalcular()\"  /></td><td><input type='number' step='any' class='currency deshabilitar' id='costoTotal" + item.RowID + "' name='costoTotal" + item.RowID + "'    /></td><td><input type='number' step='any' id='part" + item.RowID + "' class='currency deshabilitar' name='part" + item.RowID + "'    /></td></tr>";
                else if (item.Nombre.Contains("terrestre"))
                {
                    Build += "<tr><td>" + item.Nombre + "</td><td>" + item.Opcion.Nombre + "</td><td><input class='currency deshabilitar' step='any' type='number' id='costoContenedor" + item.RowID + "' name='costoContenedor" + item.RowID + "'  onkeyup=\"ReCalcular()\" /></td><td><input class='currency ' step='any' style='background: rgba(0, 114, 199, 0.55); color:white' type='number' id='costoTonelada" + item.RowID + "' name='costoTonelada" + item.RowID + "' onkeyup=\"ReCalcular()\"  /></td><td><input type='number' step='any' class='currency deshabilitar' id='costoTotal" + item.RowID + "' name='costoTotal" + item.RowID + "'    /></td><td><input type='number' step='any' id='part" + item.RowID + "' class='currency deshabilitar' name='part" + item.RowID + "'    /></td></tr>";
                }
                else if (item.Nombre.Contains("Maritimo"))
                {
                    Build += "<tr><td>" + item.Nombre + "</td><td>" + item.Opcion.Nombre + "</td><td><input class='currency' type='number' step='any' id='costoContenedor" + item.RowID + "' name='costoContenedor" + item.RowID + "' style = 'background: rgba(0, 114, 199, 0.55); color:white'" + item.RowID + "'  onkeyup=\"ReCalcular()\" /></td><td><input class='currency deshabilitar'  type='number' step='any' id='costoTonelada" + item.RowID + "' name='costoTonelada" + item.RowID + "' onkeyup=\"ReCalcular()\"  /></td><td><input type='number' step='any' class='currency deshabilitar' id='costoTotal" + item.RowID + "' name='costoTotal" + item.RowID + "'    /></td><td><input type='number' step='any' id='part" + item.RowID + "' class='currency deshabilitar' name='part" + item.RowID + "'    /></td></tr>";
                }
                else
                {
                    Build += "<tr><td>" + item.Nombre + "</td><td>" + item.Opcion.Nombre + "</td><td><input class='currency deshabilitar' type='number' step='any' id='costoContenedor" + item.RowID + "' name='costoContenedor" + item.RowID + "'  onkeyup=\"ReCalcular()\" /></td><td><input class='currency deshabilitar' step='any' type='number' id='costoTonelada" + item.RowID + "' name='costoTonelada" + item.RowID + "' onkeyup=\"ReCalcular()\"  /></td><td><input type='number' step='any' class='currency deshabilitar' id='costoTotal" + item.RowID + "' name='costoTotal" + item.RowID + "'    /></td><td><input type='number' step='any' id='part" + item.RowID + "' class='currency deshabilitar' name='part" + item.RowID + "'    /></td></tr>";
                }

            }
            Build += "<tr><td>Total Costos</td><td></td><td><input type='number' step='any' class='currency deshabilitar' id='costoContenedorTotal' name='costoContenedorTotal'   /></td><td><input type='number' step='any' class='currency deshabilitar' id='costoToneladaTotal' name='costoToneladaTotal'    /></td><td><input type='number' id='costoTotal' name='costoTotal' step='any' class='currency deshabilitar'   /></td><td><input type='number' step='any'  class='currency deshabilitar' id='partTotal' name='partTotal'    /></td></tr>";
            return Build;
            #endregion
        }
        public string CostoVentas()
        {
            #region TablaCostosLogisticos
            List<Costo> lista = db.Costo.Where(f => f.Opcion1.Nombre == "VENTA").ToList();
            string Build = "";
            foreach (var item in lista)
            {
                Build += "<tr><td>" + item.Nombre + "</td><td>" + item.Opcion.Nombre + "</td><td><input class='currency deshabilitar'  step='any' type='number' id='costoContenedor" + item.RowID + "' name='costoContenedor" + item.RowID + "'  onkeyup=\"ReCalcular()\"    /></td><td><input type='number' step='any' class='currency deshabilitar' id='costoTonelada" + item.RowID + "' name='costoTonelada" + item.RowID + "' onkeyup=\"ReCalcular()\"     /></td><td><input class='currency deshabilitar' type='number' step='any' id='costoTotal" + item.RowID + "' name='costoTotal" + item.RowID + "'    /></td><td></td></tr>";
            }
            //Build += "<tr><td>Total Costos</td><td></td><td><input type='number' id='costoContenedorTotal' name='costoContenedorTotal'   /></td><td><input type='number' id='costoToneladaTotal' name='costoToneladaTotal'    /></td><td><input type='number' id='costoTotal' name='costoTotal'    /></td><td><input type='number' id='partTotal' name='partTotal'    /></td></tr>";
            return Build;
            #endregion
        }
        public JsonResult ItemsAutocomplete()
        {
            string term = Request.Params["term"];

            try
            {
                var data = (from item in db.Item
                                                 .Where(f => f.Descripcion.Contains(term)
                                                 || f.Referencia.Contains(term.Trim())).ToList()
                            select new
                            {
                                rowid = item.RowID,
                                nombre = item.Referencia + " " + item.Descripcion,
                                label = item.Referencia + " " + item.Descripcion
                            }).Distinct().OrderBy(f => f.label).Take(15);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public string GuardarItem()
        {
            CalculadoraItems obj = new CalculadoraItems();
            #region InformacionGeneral
            int rowid_detalle = int.Parse(Request.Params["rowid_detalle"]);
            try { obj = db.CalculadoraItems.Where(f => f.RowID == rowid_detalle).FirstOrDefault(); } catch { }
            if (obj == null)
            {
                obj = new CalculadoraItems();
            }
            try { obj.CalculadoraID = int.Parse(Request.Params["rowid_calculadora"]); } catch { }
            try { obj.ItemID = int.Parse(Request.Params["rowid_item"]); } catch { }
            try { obj.EmbalajeID = int.Parse(Request.Params["embalaje"]); } catch { }
            try { obj.AgenteNavieraID = int.Parse(Request.Params["agentenaviera"]); } catch { }
            try { obj.CantidadTonelada = double.Parse(Request.Params["cantidadton"]); } catch { }
            try { obj.NumeroEnvio = int.Parse(Request.Params["numeroenvios"]); } catch { }
            try { obj.ToneladaContenedor = decimal.Parse(Request.Params["toneladacont"]); } catch { }
            try { obj.NumeroContenedor = decimal.Parse(Request.Params["numcontenedores"]); } catch (Exception e) { }
            #endregion
            #region TRM
            try { obj.TRM = double.Parse(Request.Params["trmactual"]); }
            catch { }
            try { obj.TRMVenta = double.Parse(Request.Params["trmventa"]); }
            catch { }
            try { obj.TRMCompra = double.Parse(Request.Params["trmcompra"]); }
            catch { }
            #endregion
            #region Rentabilidad
            try { obj.PVEBRentabilidad = decimal.Parse(Request.Params["rentabilidadvalor"]); } catch { }
            try { obj.PVEBRentabilidadporcentaje = decimal.Parse(Request.Params["rentabilidadPorcentaje"]); } catch { }
            try { obj.PVEporcentaje = decimal.Parse(Request.Params["precioVentaExteriorProcentaje"]); } catch { }
            try { obj.PVE = decimal.Parse(Request.Params["precioVentaExteriorValor"]); } catch { }
            #endregion
            #region MateriaPrima
            try { obj.MPPCCOPCalculado = decimal.Parse(Request.Params["preciocompraCOP1"]); } catch { }
            try { obj.MPPCCOPValor = decimal.Parse(Request.Params["preciocompraCOP"]); } catch { }
            try { obj.MPPCUSDCalculado = decimal.Parse(Request.Params["preciocompraUSD1"]); } catch { }
            try { obj.MPPCUSDValor = decimal.Parse(Request.Params["preciocompraUSD"]); } catch { }
            #endregion
            #region CostosLogisticos
            #region Broker
            try { obj.CBrokerPARTE = decimal.Parse(Request.Params["part1"]); } catch { }
            try { obj.CBrokerUSD = decimal.Parse(Request.Params["costoTotal1"]); } catch { }
            try { obj.CBrokerUSDTON = decimal.Parse(Request.Params["costoTonelada1"]); } catch { }
            try { obj.CBrokerUSDCON = decimal.Parse(Request.Params["costoContenedor1"]); } catch { }
            #endregion
            #region CertificadoSGS
            try { obj.SGSPARTE = decimal.Parse(Request.Params["part2"]); } catch { }
            try { obj.SGSUSD = decimal.Parse(Request.Params["costoTotal2"]); } catch { }
            try { obj.SGSUSDTON = decimal.Parse(Request.Params["costoTonelada2"]); } catch { }
            try { obj.SGSUSDCON = decimal.Parse(Request.Params["costoContenedor2"]); } catch { }
            #endregion
            #region CertificadoCalidad
            try { obj.CertificadoCalidadPARTE = decimal.Parse(Request.Params["part3"]); } catch { }
            try { obj.CertificadoCalidadUSD = decimal.Parse(Request.Params["costoTotal3"]); } catch { }
            try { obj.CertificadoCalidadUSDTON = decimal.Parse(Request.Params["costoTonelada3"]); } catch { }
            try { obj.CertificadoCalidadUSDCON = decimal.Parse(Request.Params["costoContenedor3"]); } catch { }
            #endregion
            #region Trasiego
            try { obj.TrasiegoPARTE = decimal.Parse(Request.Params["part4"]); } catch { }
            try { obj.TrasiegoUSD = decimal.Parse(Request.Params["costoTotal4"]); } catch { }
            try { obj.TrasiegoUSDTON = decimal.Parse(Request.Params["costoTonelada4"]); } catch { }
            try { obj.TrasiegoUSDCON = decimal.Parse(Request.Params["costoContenedor4"]); } catch { }
            #endregion
            #region Flexitanque
            try { obj.FlexiTanquePARTE = decimal.Parse(Request.Params["part5"]); } catch { }
            try { obj.FlexiTanqueUSD = decimal.Parse(Request.Params["costoTotal5"]); } catch { }
            try { obj.FlexiTanqueUSDTON = decimal.Parse(Request.Params["costoTonelada5"]); } catch { }
            try { obj.FlexiTanqueUSDCON = decimal.Parse(Request.Params["costoContenedor5"]); } catch { }
            #endregion
            #region GastosPortuarios
            try { obj.GPortuarioPARTE = decimal.Parse(Request.Params["part6"]); } catch { }
            try { obj.GPortuarioUSD = decimal.Parse(Request.Params["costoTotal6"]); } catch { }
            try { obj.GPortuarioUSDTON = decimal.Parse(Request.Params["costoTonelada6"]); } catch { }
            try { obj.GPortuarioUSDCON = decimal.Parse(Request.Params["costoContenedor6"]); } catch { }
            #endregion
            #region MovilizacionPuerto
            try { obj.GPortuarioPARTE = decimal.Parse(Request.Params["part7"]); } catch { }
            try { obj.GPortuarioUSD = decimal.Parse(Request.Params["costoTotal7"]); } catch { }
            try { obj.GPortuarioUSDTON = decimal.Parse(Request.Params["costoTonelada7"]); } catch { }
            try { obj.GPortuarioUSDCON = decimal.Parse(Request.Params["costoContenedor7"]); } catch { }
            #endregion
            #region LlenadoFlexitanque/Isotanques
            try { obj.GPLLFlexiPARTE = decimal.Parse(Request.Params["part8"]); } catch { }
            try { obj.GPLLFlexiUSD = decimal.Parse(Request.Params["costoTotal8"]); } catch { }
            try { obj.GPLLFlexiUSDTON = decimal.Parse(Request.Params["costoTonelada8"]); } catch { }
            try { obj.GPLLFlexiUSDCON = decimal.Parse(Request.Params["costoContenedor8"]); } catch { }
            #endregion
            #region UIOPT Contenedor Lleno
            try { obj.GPUCLLPARTE = decimal.Parse(Request.Params["part9"]); } catch { }
            try { obj.GPUCLLUSD = decimal.Parse(Request.Params["costoTotal9"]); } catch { }
            try { obj.GPUCLLUSDTON = decimal.Parse(Request.Params["costoTonelada9"]); } catch { }
            try { obj.GPUCLLUSDCON = decimal.Parse(Request.Params["costoContenedor9"]); } catch { }
            #endregion
            #region Uso de Instalaciones Contenedor
            try { obj.GPUICPARTE = decimal.Parse(Request.Params["part10"]); } catch { }
            try { obj.GPUICUSD = decimal.Parse(Request.Params["costoTotal10"]); } catch { }
            try { obj.GPUCLLUSDTON = decimal.Parse(Request.Params["costoTonelada10"]); } catch { }
            try { obj.GPUCLLUSDCON = decimal.Parse(Request.Params["costoContenedor10"]); } catch { }
            #endregion
            #region Serpentin
            try { obj.SerpentinPARTE = decimal.Parse(Request.Params["part11"]); } catch { }
            try { obj.SerpentinUSD = decimal.Parse(Request.Params["costoTotal11"]); } catch { }
            try { obj.SerpentinUSDTON = decimal.Parse(Request.Params["costoTonelada11"]); } catch { }
            try { obj.SerpentinUSDCON = decimal.Parse(Request.Params["costoContenedor11"]); } catch { }
            #endregion
            #region Comision Agente Carga
            try { obj.ComisionACargaPARTE = decimal.Parse(Request.Params["part12"]); } catch { }
            try { obj.ComisionACargaUSD = decimal.Parse(Request.Params["costoTotal12"]); } catch { }
            try { obj.ComisionACargaUSDTON = decimal.Parse(Request.Params["costoTonelada12"]); } catch { }
            try { obj.ComisionACargaUSDCON = decimal.Parse(Request.Params["costoContenedor12"]); } catch { }
            #endregion
            #region Handling Fee
            try { obj.CACHFPARTE = decimal.Parse(Request.Params["part13"]); } catch { }
            try { obj.CACHFUSD = decimal.Parse(Request.Params["costoTotal13"]); } catch { }
            try { obj.CACHFUSDTON = decimal.Parse(Request.Params["costoTonelada13"]); } catch { }
            try { obj.CACHFUSDCON = decimal.Parse(Request.Params["costoContenedor13"]); } catch { }
            #endregion
            #region Doc Fee
            try { obj.CACDFPARTE = decimal.Parse(Request.Params["part14"]); } catch { }
            try { obj.CACDFUSD = decimal.Parse(Request.Params["costoTotal14"]); } catch { }
            try { obj.CACDFUSDTON = decimal.Parse(Request.Params["costoTonelada14"]); } catch { }
            try { obj.CACDFUSDCON = decimal.Parse(Request.Params["costoContenedor14"]); } catch { }
            #endregion
            #region BL Fee
            try { obj.CACBFPARTE = decimal.Parse(Request.Params["part15"]); } catch { }
            try { obj.CACBFUSD = decimal.Parse(Request.Params["costoTotal15"]); } catch { }
            try { obj.CACBFUSDTON = decimal.Parse(Request.Params["costoTonelada15"]); } catch { }
            try { obj.CACBFUSDCON = decimal.Parse(Request.Params["costoContenedor15"]); } catch { }
            #endregion
            #region Origen terminal handling
            try { obj.CACothPARTE = decimal.Parse(Request.Params["part16"]); } catch { }
            try { obj.CACothUSD = decimal.Parse(Request.Params["costoTotal16"]); } catch { }
            try { obj.CACothUSDTON = decimal.Parse(Request.Params["costoTonelada16"]); } catch { }
            try { obj.CACothUSDCON = decimal.Parse(Request.Params["costoContenedor16"]); } catch { }
            #endregion
            #region Handling logistic Fee
            try { obj.CAChlfPARTE = decimal.Parse(Request.Params["part17"]); } catch { }
            try { obj.CAChlfUSD = decimal.Parse(Request.Params["costoTotal17"]); } catch { }
            try { obj.CAChlfUSDTON = decimal.Parse(Request.Params["costoTonelada17"]); } catch { }
            try { obj.CAChlfUSDCON = decimal.Parse(Request.Params["costoContenedor17"]); } catch { }
            #endregion
            #region Dias de almacenaje
            try { obj.DiasAlmacenPARTE = decimal.Parse(Request.Params["part18"]); } catch { }
            try { obj.DiasAlmacenUSD = decimal.Parse(Request.Params["costoTotal18"]); } catch { }
            try { obj.DiasAlmacenUSDTON = decimal.Parse(Request.Params["costoTonelada18"]); } catch { }
            try { obj.DiasAlmacenUSDCON = decimal.Parse(Request.Params["costoContenedor18"]); } catch { }
            #endregion
            #region DEX y Gastos Operativos Aduana
            try { obj.DGOaPARTE = decimal.Parse(Request.Params["part19"]); } catch { }
            try { obj.DGOaUSD = decimal.Parse(Request.Params["costoTotal19"]); } catch { }
            try { obj.DGOaUSDTON = decimal.Parse(Request.Params["costoTonelada19"]); } catch { }
            try { obj.DGOaUSDCON = decimal.Parse(Request.Params["costoContenedor19"]); } catch { }
            #endregion
            #region Flete terrestre
            try { obj.FleteTerrPARTE = decimal.Parse(Request.Params["part20"]); } catch { }
            try { obj.FleteTerrUSD = decimal.Parse(Request.Params["costoTotal20"]); } catch { }
            try { obj.FleteTerrUSDTON = decimal.Parse(Request.Params["costoTonelada20"]); } catch { }
            try { obj.FleteTerrUSDCON = decimal.Parse(Request.Params["costoContenedor20"]); } catch { }
            #endregion
            #region Envio de documentos
            try { obj.EDocumentoPARTE = decimal.Parse(Request.Params["part21"]); } catch { }
            try { obj.EDocumentoUSD = decimal.Parse(Request.Params["costoTotal21"]); } catch { }
            try { obj.EDocumentoUSDTON = decimal.Parse(Request.Params["costoTonelada21"]); } catch { }
            try { obj.EDocumentoUSDCON = decimal.Parse(Request.Params["costoContenedor21"]); } catch { }
            #endregion
            #region Flete Maritimo
            try { obj.FleteMaritimoPARTE = decimal.Parse(Request.Params["part22"]); } catch { }
            try { obj.FleteMaritimoUSD = decimal.Parse(Request.Params["costoTotal22"]); } catch { }
            try { obj.FleteMaritimoUSDTON = decimal.Parse(Request.Params["costoTonelada22"]); } catch { }
            try { obj.FleteMaritimoUSDCON = decimal.Parse(Request.Params["costoContenedor22"]); } catch { }
            #endregion
            #region TotalCostos
            try { obj.CLTotalcostoPARTE = decimal.Parse(Request.Params["partMateriaPrima"]); } catch { }
            try { obj.CLTotalcostoUSD = decimal.Parse(Request.Params["costoTotalMateriaPrima"]); } catch { }
            try { obj.CLTotalcostoUSDTON = decimal.Parse(Request.Params["costoToneladaTotal"]); } catch { }
            try { obj.CLTotalcostoUSDCON = decimal.Parse(Request.Params["costoContenedorTotal"]); } catch { }
            #endregion
            #endregion
            #region Costos Materia Prima
            try { obj.CMateriaPPARTE = decimal.Parse(Request.Params["partMateriaPrima"]); } catch { }
            try { obj.CMateriaPUSD = decimal.Parse(Request.Params["costoTotalMateriaPrima"]); } catch { }
            try { obj.CMateriaPUSDTON = decimal.Parse(Request.Params["costoToneladaMateriaPrima"]); } catch { }
            try { obj.CMateriaPUSDCON = decimal.Parse(Request.Params["costoContenedorMateriaPrima"]); } catch { }
            #endregion
            #region CostoTotal
            try { obj.CTotalPUSD_usd = decimal.Parse(Request.Params["costoTotalUSDTotal"]); } catch { }
            try { obj.CTotalUSDCON_usd = decimal.Parse(Request.Params["costoTotalUSDContenedor"]); } catch { }
            try { obj.CTotalUSDTON_usd = decimal.Parse(Request.Params["costoTotalUSDTonelada"]); } catch { }
            try { obj.CTotalPUSD_cop = decimal.Parse(Request.Params["costoTotalCOPTotal"]); } catch { }
            try { obj.CTotalUSDCON_cop = decimal.Parse(Request.Params["costoTotalCOPContenedor"]); } catch { }
            try { obj.CTotalUSDTON_cop = decimal.Parse(Request.Params["costoTotalCOPTonelada"]); } catch { }
            #endregion
            #region DatosVenta
            #region Venta USD
            try { obj.DatosVentaUSDTotal = decimal.Parse(Request.Params["VentaTotalUSD"]); } catch { }
            try { obj.DatosVentaUSDPorton = decimal.Parse(Request.Params["VentaToneladaUSD"]); } catch { }
            try { obj.DatosVentaUSDPContenedor = decimal.Parse(Request.Params["VentaContenedorUSD"]); } catch { }
            #endregion
            #region Venta COP
            try { obj.DatosVentaCOPTotal = decimal.Parse(Request.Params["VentaTotalCOP"]); } catch { }
            try { obj.DatosVentaCOPPorton = decimal.Parse(Request.Params["VentaToneladaCOP"]); } catch { }
            try { obj.DatosVentaCOPPContenedor = decimal.Parse(Request.Params["VentaContenedorCOP"]); } catch { }
            #endregion
            #region Agenciamiento
            try { obj.DVAgendamientoTotal = decimal.Parse(Request.Params["costoTotal23"]); } catch { }
            try { obj.DVAgendamientoPContenedor = decimal.Parse(Request.Params["costoContenedor23"]); } catch { }
            try { obj.DVAgendamientoPorton = decimal.Parse(Request.Params["costoTonelada23"]); } catch { }
            #endregion
            #region Seguro
            try { obj.DVSeguroTotal = decimal.Parse(Request.Params["costoTotal24"]); } catch { }
            try { obj.DVSeguroPContenedor = decimal.Parse(Request.Params["costoContenedor24"]); } catch { }
            try { obj.DVSeguroPorton = decimal.Parse(Request.Params["costoTonelada24"]); } catch { }
            #endregion
            #region Costo de manipulacion en destino
            try { obj.DVCostoManipulacionTotal = decimal.Parse(Request.Params["costoTotal25"]); } catch { }
            try { obj.DVCostoManipulacionPContenedor = decimal.Parse(Request.Params["costoContenedor25"]); } catch { }
            try { obj.DVCostoManipulacionPorton = decimal.Parse(Request.Params["costoTonelada25"]); } catch { }
            #endregion
            #region Tramites aduaneros destino
            try { obj.DVTAduanerosTotal = decimal.Parse(Request.Params["costoTotal26"]); } catch { }
            try { obj.DVTAduanerosPContenedor = decimal.Parse(Request.Params["costoContenedor26"]); } catch { }
            try { obj.DVTAduanerosPorton = decimal.Parse(Request.Params["costoTonelada26"]); } catch { }
            #endregion
            #region Transporte en destino
            try { obj.DVTDestinoTotal = decimal.Parse(Request.Params["costoTotal27"]); } catch { }
            try { obj.DVTDestinoPContenedor = decimal.Parse(Request.Params["costoContenedor27"]); } catch { }
            try { obj.DVTDestinoPorton = decimal.Parse(Request.Params["costoTonelada27"]); } catch { }
            #endregion
            #region Margen % 
            try { obj.DVMargenTotal = decimal.Parse(Request.Params["costoTotal28"]); } catch { }
            try { obj.DVMargenPContenedor = decimal.Parse(Request.Params["costoContenedor28"]); } catch { }
            try { obj.DVMargenPorton = decimal.Parse(Request.Params["costoTonelada28"]); } catch { }
            #endregion
            #region Utilidad
            try { obj.DVUtilidadTotal = decimal.Parse(Request.Params["costoTotal29"]); } catch { }
            try { obj.DVUtilidadPContenedor = decimal.Parse(Request.Params["costoContenedor29"]); } catch { }
            try { obj.DVUtilidadPorton = decimal.Parse(Request.Params["costoTonelada29"]); } catch { }
            #endregion
            if (obj.RowID == 0)
            {
                obj.UsuarioCreacion = "Admin";//((s_usuario)Session["curUser"]).nombre_usuario;
                obj.FechaCreacion = DateTime.Now;
                db.CalculadoraItems.Add(obj);
            }
            else
            {
                obj.UsuarioModificacion = "Admin";//((s_usuario)Session["curUser"]).nombre_usuario;
                obj.FechaModificacion = DateTime.Now;
            }
            db.SaveChanges();
            #endregion
            return "";
        }
        public string BalanceCalculadoraI(int? rowid)
        {
            string result = "", embalaje = "";
            List<CalculadoraItems> lista = db.CalculadoraItems.Where(f => f.CalculadoraID == rowid).ToList();
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
                   "<td><a  class=\"\"  href=\"javascript:EliminarItem(" + item.RowID + ")\" ><i class=\"glyphicon glyphicon-trash\"></i></a>" +
                   "<a  class=\"\"  href=\"javascript:ActualizarItem(" + item.RowID + ")\" ><i class=\"glyphicon glyphicon-pencil\"></i></a></td></tr>";
            }
            return result;
        }
        public int Siguiente(int rowid)
        {
            Calculadora ex = db.Calculadora.Where(f => f.RowID == rowid).First();
            int rowid_calculadora = ex.RowID;
            return rowid;
        }
        public bool EliminarItem(int? rowid)
        {
            CalculadoraItems reg = db.CalculadoraItems.Where(f => f.RowID == rowid).FirstOrDefault();
            db.CalculadoraItems.Remove(reg);
            db.SaveChanges();
            return true;
        }
        public JsonResult ActualizarItem(int? rowid)
        {

            CalculadoraItems reg = db.CalculadoraItems.Where(f => f.RowID == rowid).FirstOrDefault();
            int embalaje = 0;
            object jsondata = null;
            if (reg.Opcion != null)
            {
                embalaje = reg.Opcion.RowID;
            }
            var jsonData = new
            {
                rowid = reg.RowID,
                nombre_item = reg.Item.Descripcion,
                embalaje = embalaje,
                numero_envios = reg.NumeroEnvio,
                cantidad = reg.CantidadTonelada,
                toneladaContenedor = reg.ToneladaContenedor,
                trm = reg.TRM,
                precioCompraCOP = reg.MPPCCOPValor,
                precioCompraUSD = reg.MPPCUSDValor,

                brokerTonelada = reg.CBrokerUSDTON,

                fleteTerrestreTonelada = reg.FleteTerrUSDTON,

                fleteMaritimoCon = reg.FleteMaritimoUSDCON,

                pvexteriorb = reg.PVEBRentabilidadporcentaje,
                agencia = reg.AgenteNavieraID,
                pvexteriorp = reg.PVE,
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        #endregion

        #region :::::CARTA LLENADO PUERTO ::::::::
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
                CartaLlenadoPuerto carta = db.CartaLlenadoPuerto.Where(c => c.RowID == rowid).FirstOrDefault();
                return View(carta);
            }
            else
            {
                return View(new CartaLlenadoPuerto());
            }
        }
        [CheckSessionOut]
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
        [CheckSessionOut]
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
        [CheckSessionOut]
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
        [CheckSessionOut]
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
                    try { ObjSolicitud.OpcionID = Convert.ToInt32(form["medidaP"]); } catch { }
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
        #endregion

        #region ::::::DCD::::::
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

        #region ::::::FEP::::::
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
        [CheckSessionOut]
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
        [CheckSessionOut]
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

        #region ::::::CERTIFICADO CALIDAD:::::
        [CheckSessionOut]
        public ActionResult ListaCertificadoCalidad()
        {
            ViewBag.listadocalidad = db.CertificadoCalidad.ToList();
            return View();
        }
        [CheckSessionOut]
        public ActionResult CertificadoCalidad(int? Rowid_certificado)
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
        [CheckSessionOut]
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
        [CheckSessionOut]
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
        #endregion

        #region :::::CERTIFICADO MADERA:::::
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
        [ValidateInput(false)]
        [CheckSessionOut]
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

            int rowid = ObjCertificadoMadera.RowID;
            return Json(rowid, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region :::::CONTRATO:::::
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
        public int rowid_tipo(string tipo_proforma)
        {
            int rowid_tipo = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == tipo_proforma).FirstOrDefault().RowID;
            return rowid_tipo;
        }
        #endregion

        #region :::::MATRIZ BL :::::
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
            var data = new { RowID = RowID, Titulo = RazonSocial, Nit = Nit, Telefono = Telefono, Direccion = Direccion, Ciudad = Ciudad };
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

            int rowid = ObjMatriz.RowID;
            return Json(rowid, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region :::::ORDEN COMPRA:::::
        [CheckSessionOut]
        public ActionResult OrdenCompra(int? rowid)
        {
            ViewBag.FormaPago = db.Opcion.Where(f => f.Agrupacion.Nombre == "PROFORMA.FORMAPAGO").ToList();
            OrdenCompra reg = new Models.OrdenCompra();
            if (rowid > 0)
            {
                reg = db.OrdenCompra.Where(f => f.RowID == rowid).FirstOrDefault();
            }
            else
            {
                reg.Contrato = new Models.Contrato();
                reg.Tercero = new Tercero();
                reg.Opcion = new Opcion();
                reg.Sucursal = new Sucursal();
                reg.Sucursal.Tercero = new Tercero();
                reg.Sucursal1 = new Sucursal();
                reg.Sucursal1.Tercero = new Tercero();
            }
            return View(reg);
        }
        [CheckSessionOut]
        public ActionResult OrdenCompras()
        {
            List<OrdenCompra> lista = db.OrdenCompra.ToList();
            return View(lista);
        }
        [CheckSessionOut]
        public JsonResult BuscarTerceroSucursal(string term)
        {
            var result = (from reg in db.Sucursal.Where(f => f.Tercero.RazonSocial.Contains(term) || f.Tercero.Identificacion.Contains(term) || f.Nombre.Contains(term))
                          select new
                          {
                              label = reg.Tercero.RazonSocial + "-" + reg.Nombre,
                              nit = reg.Tercero.Identificacion,
                              rowid = reg.RowID,
                              ciudad = reg.ContactoERP.Ciudad.Nombre + "," + reg.ContactoERP.Ciudad.Departamento.Pais.Nombre,
                              telefono = reg.ContactoERP.Telefono1,
                              direccion = reg.ContactoERP.Direccion1,
                              iva = ""
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CheckSessionOut]
        public JsonResult TerceroInformacion(int rowid)
        {
            var result = (from reg in db.Tercero.Where(f => f.RowID == rowid)
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
        public JsonResult SucursalesTerceroInformacion(int? rowid)
        {
            var result = (from reg in db.Sucursal.Where(f => f.RowID == rowid)
                          select new
                          {
                              label = reg.Tercero.RazonSocial + "-" + reg.Nombre,
                              nit = reg.Tercero.Identificacion,
                              rowid = reg.RowID,
                              ciudad = reg.ContactoERP.Ciudad.Nombre + "," + reg.ContactoERP.Ciudad.Departamento.Pais.Nombre,
                              telefono = reg.ContactoERP.Telefono1,
                              direccion = reg.ContactoERP.Direccion1,
                              iva = ""
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CheckSessionOut]
        public JsonResult Contrato(string term)
        {
            List<Contrato> lista = db.Contrato.Where(f => f.Titulo.Contains(term)).ToList();
            var result = (from reg in lista.ToList()
                          select new
                          {
                              label = reg.RowID + "-" + reg.Titulo,
                              rowid = reg.RowID
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CheckSessionOut]
        public string SucursalesTercero(int rowid)
        {

            Sucursal sucursal = db.Sucursal.Where(f => f.RowID == rowid).FirstOrDefault();
            string resultado = "<option value=''>-Seleccionar-</option>";
            foreach (Sucursal item in db.Sucursal.Where(f => f.TerceroID == sucursal.TerceroID).ToList())
            {
                resultado += "<option value='" + item.RowID + "'>" + item.Nombre + "</option>";
            }
            return resultado;
        }
        [CheckSessionOut]
        public string ItemsOrdenCompra(int rowid)
        {
            string result = "";
            List<OrdenCompraItem> lista = db.OrdenCompraItem.Where(f => f.OrdenCompraID == rowid).ToList();
            foreach (var item in lista)
            {
                result += "<tr><td>" + item.Item.Descripcion + "</td><td>" + item.Item.Unidad + "</td><td><input type='number' step='any' onkeyup='CalcularCampos(" + item.RowID + ")'  id='cant" + item.RowID + "' value='" + item.Cantidad + "' /></td><td><input type='number' step='any'  id='valor" + item.RowID + "' value='" + item.ValorUnitario + "'  onkeyup='CalcularCampos(" + item.RowID + ")'  /></td><td><span id='valorbase" + item.RowID + "'>" + item.ValorBase + "</span></td><td><span id='valorimpuesto" + item.RowID + "'>" + item.ValorImpuesto + "</span></td><td><span id='valortotal" + item.RowID + "'>" + item.ValorTotal + "</span></td></tr>";
            }
            result += "<tr><td></td><td>Total:</td><td><span id='totalcantidad'>" + lista.Sum(f => f.Cantidad) + "</span></td><td><span id='totalunitario'>" + lista.Sum(f => f.ValorUnitario) + "</span></td><td><span id='totalbase'>" + lista.Sum(f => f.ValorBase) + "</span></td><td><span id='totalimpuesto'>" + lista.Sum(f => f.ValorImpuesto) + "</span></td><td><span id='totaltotal'>" + lista.Sum(f => f.ValorTotal) + "</span></td></tr>";
            return result;
        }
        [CheckSessionOut]
        public string GuardarOrdenCompra()
        {
            OrdenCompra reg = new Models.OrdenCompra();
            int rowid = int.Parse(Request.Params["rowid"]);
            if (rowid > 0)
            {
                reg = db.OrdenCompra.Where(f => f.RowID == rowid).FirstOrDefault();
            }
            reg.ProveedorID = int.Parse(Request.Params["rowid_proveedor"]);
            reg.ContratoID = int.Parse(Request.Params["rowid_contrato"]);
            reg.SucursalFactura = int.Parse(Request.Params["rowid_facturacion"]);
            reg.SucursalDespacho = int.Parse(Request.Params["despacho"]);
            reg.FormaPagoID = int.Parse(Request.Params["formapago"]);
            reg.Observaciones = Request.Params["observaciones"];

            reg.Fecha = DateTime.Now;
            if (reg.RowID == 0)
            {
                reg.FechaCreacion = DateTime.Now;
                reg.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                db.OrdenCompra.Add(reg);
                db.SaveChanges();
                Contrato c = db.Contrato.Where(f => f.RowID == reg.ContratoID).FirstOrDefault();
                foreach (var item in db.CalculadoraItems.Where(f => f.CalculadoraID == reg.Contrato.Proforma.CalculadoraID))
                {
                    OrdenCompraItem OCItem = new Models.OrdenCompraItem();
                    OCItem.ItemID = item.ItemID;
                    OCItem.OrdenCompraID = reg.RowID;
                    OCItem.UnidadEmpaque = item.Item.Unidad;
                    OCItem.Cantidad = item.CantidadTonelada;
                    OCItem.ValorUnitario = double.Parse(item.MPPCUSDCalculado.ToString());
                    OCItem.ValorImpuesto = (((OCItem.Cantidad * OCItem.ValorUnitario) * item.Item.Impuesto) / 100);
                    OCItem.ValorBase = OCItem.Cantidad * OCItem.ValorUnitario;
                    OCItem.ValorTotal = OCItem.ValorBase + OCItem.ValorImpuesto;
                    OCItem.UsuarioCreacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                    OCItem.FechaCreacion = DateTime.Now;
                    db.OrdenCompraItem.Add(OCItem);
                    db.SaveChanges();
                }
            }
            else
            {
                reg.UsuarioModificacion = ((Usuario)Session["CurUser"]).NombreUsuario;
                reg.FechaModificacion = DateTime.Now;
            }

            return reg.RowID.ToString();
        }
        [CheckSessionOut]
        public JsonResult ActualizarValores()
        {
            int rowid = int.Parse(Request.Params["rowid"]);
            double cantidad = 0;
            try
            {
                cantidad = double.Parse(Request.Params["cantidad"]);
            }
            catch (Exception)
            {
                return Json(0);
            }

            double ValorUnitario = double.Parse(Request.Params["valorUnitario"]);
            OrdenCompraItem reg = db.OrdenCompraItem.Where(f => f.RowID == rowid).FirstOrDefault();
            if (reg == null)
            {
                return Json(0);
            }
            reg.Cantidad = cantidad;
            reg.ValorUnitario = ValorUnitario;
            reg.ValorImpuesto = (((reg.Cantidad * reg.ValorUnitario) * reg.Item.Impuesto) / 100);
            reg.ValorBase = reg.Cantidad * reg.ValorUnitario;
            reg.ValorTotal = reg.ValorBase + reg.ValorImpuesto;
            reg.UsuarioModificacion = ((Usuario)Session["CurUser"]).NombreUsuario;
            reg.FechaModificacion = DateTime.Now;
            db.SaveChanges();
            var data = new
            {
                reg.Cantidad,
                reg.ValorUnitario,
                reg.ValorTotal,
                reg.ValorBase,
                reg.ValorImpuesto,

            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [CheckSessionOut]
        public JsonResult Totales(int? rowid)
        {
            List<OrdenCompraItem> reg = db.OrdenCompraItem.Where(f => f.OrdenCompraID == rowid).ToList();
            var data = new
            {
                Cantidad = reg.Sum(f => f.Cantidad),
                Valorunitario = reg.Sum(f => f.ValorUnitario),
                valortotal = reg.Sum(f => f.ValorTotal),
                valorbase = reg.Sum(f => f.ValorBase),
                valorimpuesto = reg.Sum(f => f.ValorImpuesto),

            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region :::::PROFORMA:::::
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
            reg.TipoProformaID = db.Opcion.Where(op => op.Agrupacion.Nombre == "TIPOPROFORMA" && op.Codigo == "EXPORTACION").FirstOrDefault().RowID;
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
        [CheckSessionOut]
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

        #region :::::SOLICITUD DE LLENADO:::::
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
        [CheckSessionOut]
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

            int rowid = ObjSolicitud.RowID;
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
                ViewBag.conductor = solicitud.Tercero.ContactoERP.Identificacion;
                return View(solicitud);
            }
            else
            {
                return View(new VehiculoSolicitudLlenado());
            }
        }
        [HttpPost]
        [CheckSessionOut]
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
        [CheckSessionOut]
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
        [CheckSessionOut]
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
        #endregion

        #region ::::::SOLICITUD DE TRANSPORTE::::::
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
        [CheckSessionOut]
        public JsonResult Datos_Proveedor(string proveedor)
        {
            //Tercero tercero = db.Tercero.Where(t => t.RazonSocial == proveedor).FirstOrDefault();
            ContactoERP contacto = db.ContactoERP.Where(p => p.Nombre == proveedor).FirstOrDefault();
            int RowID = contacto.RowID;
            string Nit = contacto.Identificacion;
            string Direccion = contacto.Direccion1;
            var data = new { RowID = RowID, Nit = Nit, Direccion = Direccion };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [CheckSessionOut]
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
                    if (form["cantidadV"] != "")
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
                for (int i = 0; i < tipoT.Split(',').Length - 1; i++)
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
        #endregion
    }
}