using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Utilidades;
using PedidosOnline.Models;

namespace PedidosOnline.Controllers
{
    public class CalculadoraController : Controller
    {
        // GET: Calculadora
        public ActionResult Index()
        {
            return View();
        }
        PedidosOnlineEntities db = new PedidosOnlineEntities();

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
            obj.AgenteNavieraID = int.Parse(Request.Params["agenciaNav"]);
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
                    Build += "<tr><td>" + item.Nombre + "</td><td>" + item.Opcion.Nombre + "</td><td><input class='currency' type='number' step='any' id='costoContenedor" + item.RowID + "' name='costoContenedor"+ item.RowID + "' style = 'background: rgba(0, 114, 199, 0.55); color:white'" + item.RowID + "'  onkeyup=\"ReCalcular()\" /></td><td><input class='currency deshabilitar'  type='number' step='any' id='costoTonelada" + item.RowID + "' name='costoTonelada" + item.RowID + "' onkeyup=\"ReCalcular()\"  /></td><td><input type='number' step='any' class='currency deshabilitar' id='costoTotal" + item.RowID + "' name='costoTotal" + item.RowID + "'    /></td><td><input type='number' step='any' id='part" + item.RowID + "' class='currency deshabilitar' name='part" + item.RowID + "'    /></td></tr>";
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
        public string BalanceCalculadora(int? rowid)
        {
            string result = "", embalaje="";
            List<CalculadoraItems> lista = db.CalculadoraItems.Where(f => f.CalculadoraID == rowid).ToList();
            foreach (var item in lista)
            {
                if(item.Opcion != null)
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
            int embalaje= 0;
            object jsondata = null;
            if(reg.Opcion != null)
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

                pvexteriorp = reg.PVE,
            };
            var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
    }
}