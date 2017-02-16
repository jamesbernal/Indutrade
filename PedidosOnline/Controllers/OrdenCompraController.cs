using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Models;
using PedidosOnline.Utilidades;

namespace PedidosOnline.Controllers
{
    public class OrdenCompraController : Controller
    {
        PedidosOnlineEntities db = new PedidosOnlineEntities();
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
        public JsonResult BuscarTercero(string term)
        {
            var result = (from reg in db.Tercero.Where(f => f.RazonSocial.Contains(term) || f.Identificacion.Contains(term))
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
        public JsonResult BuscarTerceroSucursal(string term)
        {
            var result = (from reg in db.Sucursal.Where(f => f.Tercero.RazonSocial.Contains(term) || f.Tercero.Identificacion.Contains(term) || f.Nombre.Contains(term))
                          select new
                          {
                              label = reg.Tercero.RazonSocial+"-"+reg.Nombre,
                              nit = reg.Tercero.Identificacion,
                              rowid = reg.RowID,
                              ciudad = reg.ContactoERP.Ciudad.Nombre + "," + reg.ContactoERP.Ciudad.Departamento.Pais.Nombre,
                              telefono = reg.ContactoERP.Telefono1,
                              direccion = reg.ContactoERP.Direccion1,
                              iva=""
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TerceroInformacion(int rowid)
        {
            var result = (from reg in db.Tercero.Where(f =>f.RowID==rowid)
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
        public JsonResult SucursalesTerceroInformacion(int? rowid)
        {
            var result = (from reg in db.Sucursal.Where(f => f.RowID==rowid)
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
        public JsonResult Contrato(string term)
        {
            List<Contrato> lista = db.Contrato.Where(f => f.Titulo.Contains(term)).ToList();
            var result = (from reg in lista.ToList()
                          select new
                          {
                              label = reg.RowID+"-"+reg.Titulo,
                              rowid = reg.RowID
                          });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string SucursalesTercero(int rowid)
        { 
            
            Sucursal sucursal = db.Sucursal.Where(f => f.RowID == rowid).FirstOrDefault();
            string resultado = "<option value=''>-Seleccionar-</option>";
            foreach (Sucursal item in db.Sucursal.Where(f=>f.TerceroID==sucursal.TerceroID).ToList())
            {
                resultado += "<option value='"+item.RowID+"'>"+item.Nombre+"</option>";
            }
            return resultado;
        }
        public string ItemsOrdenCompra(int rowid)
        {
            string result = "";
            List<OrdenCompraItem> lista = db.OrdenCompraItem.Where(f => f.OrdenCompraID == rowid).ToList();
            foreach (var item in lista)
            {
                result += "<tr><td>"+item.Item.Descripcion+"</td><td>"+item.Item.Unidad+ "</td><td><input type='number' step='any' onkeyup='CalcularCampos(" + item.RowID + ")'  id='cant" + item.RowID+"' value='" + item.Cantidad+ "' /></td><td><input type='number' step='any'  id='valor" + item.RowID + "' value='" + item.ValorUnitario+"'  onkeyup='CalcularCampos("+item.RowID+")'  /></td><td><span id='valorbase"+item.RowID+"'>"+item.ValorBase+ "</span></td><td><span id='valorimpuesto"+item.RowID+"'>" + item.ValorImpuesto+ "</span></td><td><span id='valortotal"+item.RowID+"'>" + item.ValorTotal + "</span></td></tr>";
            }
            result += "<tr><td></td><td>Total:</td><td><span id='totalcantidad'>" + lista.Sum(f=>f.Cantidad) + "</span></td><td><span id='totalunitario'>" + lista.Sum(f=>f.ValorUnitario) + "</span></td><td><span id='totalbase'>" + lista.Sum(f=>f.ValorBase) + "</span></td><td><span id='totalimpuesto'>" + lista.Sum(f=>f.ValorImpuesto) + "</span></td><td><span id='totaltotal'>" + lista.Sum(f=>f.ValorTotal) + "</span></td></tr>";
            return result;
        }

        public string GuardarOrdenCompra()
        {
            OrdenCompra reg = new Models.OrdenCompra();
            int rowid = int.Parse(Request.Params["rowid"]);
            if (rowid>0)
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
                    OCItem.UsuarioCreacion= ((Usuario)Session["CurUser"]).NombreUsuario;
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
            
            double ValorUnitario= double.Parse(Request.Params["valorUnitario"]);
            OrdenCompraItem reg = db.OrdenCompraItem.Where(f => f.RowID == rowid).FirstOrDefault();
            if (reg==null)
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
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public JsonResult Totales(int? rowid)
        {
            List<OrdenCompraItem> reg = db.OrdenCompraItem.Where(f => f.OrdenCompraID == rowid).ToList();
            var data = new
            {
                Cantidad=reg.Sum(f=>f.Cantidad),
                Valorunitario= reg.Sum(f => f.ValorUnitario),
                valortotal=  reg.Sum(f => f.ValorTotal),
                valorbase=reg.Sum(f => f.ValorBase),
                valorimpuesto=reg.Sum(f => f.ValorImpuesto),

            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }
}